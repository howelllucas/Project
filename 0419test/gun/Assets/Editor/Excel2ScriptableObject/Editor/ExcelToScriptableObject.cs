using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using Excel;
using System.Security.Cryptography;

public class ExcelToScriptableObject : EditorWindow {

	public const string SETTINGS_PATH = "ProjectSettings/ExcelToScriptableObjectSettings.asset";

	private static Regex reg_color32 = new Regex(@"^[A-Fa-f0-9]{8}$");
	private static Regex reg_color24 = new Regex(@"^[A-Fa-f0-9]{6}$");

	[MenuItem("Cheetah/Excel2ScriptableObject")]
	static void Excel2ScriptableObject() {
		ExcelToScriptableObject window = GetWindow<ExcelToScriptableObject>("Process Excel");
		window.minSize = new Vector2(540f, 320f);
		window.maxSize = new Vector2(4000f, 4000f);
	}

	static bool Process(ExcelToScriptableObjectSetting excel, bool generateCode) {
		string className = Path.GetFileNameWithoutExtension(excel.excel_name);
		if (!CheckClassName(className)) {
			string msg = string.Format("Invalid excel file '{0}', because the name of the xlsx file should be a class name...", excel.excel_name);
			EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
			return false;
		}
		Stream stream = null;
		int lastDot = excel.excel_name.LastIndexOf('.');
		string tempExcelName = excel.excel_name.Substring(0, lastDot) + "_temp" + excel.excel_name.Substring(lastDot, excel.excel_name.Length - lastDot);
		try {
			File.Copy(excel.excel_name, tempExcelName);
			stream = File.OpenRead(tempExcelName);
		} catch {
			string msg = string.Format("Fail to open '{0}' because of sharing violation. Perhaps you should close your Excel application first...", excel.excel_name);
			EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
			File.Delete(tempExcelName);
			return false;
		}
		IExcelDataReader reader = excel.excel_name.ToLower().EndsWith(".xls") ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
		DataSet data = reader.AsDataSet();
		reader.Dispose();
		stream.Close();
		File.Delete(tempExcelName);

		if (data == null) {
			string msg = string.Format("Fail to read '{0}'. It seems that it's not a proper xlsx file...", excel.excel_name);
			EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
			return false;
		}
		DataTable table = data.Tables[0];
		if (table.Rows.Count < Mathf.Max(global_configs.field_row, global_configs.type_row) + 1) {
			string msg = string.Format("Fail to parse '{0}'. The excel file should contains at least 2 lines that specify the column names and their types...", excel.excel_name);
			EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
			return false;
		}
		object[] items;
		List<string> fieldNames = new List<string>();
		List<int> fieldIndices = new List<int>();
		items = table.Rows[global_configs.field_row].ItemArray;
		for (int i = 0, imax = items.Length; i < imax; i++) {
			string fieldName = items[i].ToString().Trim();
			if (string.IsNullOrEmpty(fieldName)) { break; }
			if (!CheckFieldName(fieldName)) {
				string msg = string.Format("Fail to parse '{0}' because of invalid field name '{1}'...", excel.excel_name, fieldName);
				EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
				return false;
			}
			fieldNames.Add(fieldName);
			fieldIndices.Add(i);
		}
		if (fieldNames.Count <= 0) {
			string msg = string.Format("Fail to parse '{0}' because of no appropriate field names...", excel.excel_name);
			EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
			return false;
		}
		Dictionary<string, List<string>> enumDict = excel.treat_unknown_types_as_enum ? new Dictionary<string, List<string>>() : null;
		int firstIndex = fieldIndices[0];
		List<eFieldTypes> fieldTypes = new List<eFieldTypes>();
		List<string> fieldTypeNames = new List<string>();
		items = table.Rows[global_configs.type_row].ItemArray;
		for (int i = 0, imax = fieldNames.Count; i < imax; i++) {
			int fieldIndex = fieldIndices[i];
			string typeName = items[fieldIndex].ToString();
			eFieldTypes fieldType = GetFieldType(typeName);
			if (fieldType == eFieldTypes.Unknown) {
				if (enumDict == null) {
					string msg = string.Format("Fail to parse '{0}' because of invalid field type '{1}'...", excel.excel_name, typeName);
					EditorUtility.DisplayDialog("Excel To ScriptableObject", msg, "OK");
					return false;
				}
				List<string> enumValues = null;
				if (!enumDict.TryGetValue(typeName, out enumValues)) {
					enumValues = new List<string>();
					enumValues.Add("Default");
					enumDict.Add(typeName, enumValues);
				}
				for (int j = global_configs.data_from_row, jmax = table.Rows.Count; j < jmax; j++) {
					object[] enumObjs = table.Rows[j].ItemArray;
					string enumValue = fieldIndex < enumObjs.Length ? enumObjs[fieldIndex].ToString() : null;
					if (!enumValues.Contains(enumValue)) { enumValues.Add(enumValue); }
				}
			}
			fieldTypes.Add(fieldType);
			fieldTypeNames.Add(typeName);
		}

		List<int> indices = new List<int>();
		if (excel.generate_get_method_if_possible) {
			string err = null;
			if (fieldTypes[0] == eFieldTypes.Int) {
				SortedList<int, List<int>> ids = new SortedList<int, List<int>>();
				for (int i = global_configs.data_from_row, imax = table.Rows.Count; i < imax; i++) {
					if (EditorUtility.DisplayCancelableProgressBar("Excel", string.Format("Checking IDs or Keys... {0} / {1}", i - 1, imax - 2), (float)(i - 1) / (imax - 2))) {
						EditorUtility.ClearProgressBar();
						return false;
					}
					string idStr = table.Rows[i].ItemArray[firstIndex].ToString().Trim();
					if (string.IsNullOrEmpty(idStr)) { continue; }
					int id;
					if (!int.TryParse(idStr, out id)) {
						err = string.Format("Fail to parse '{0}' in line {1} because it seems not to be a 'int'", idStr, i);
						break;
					}
					List<int> idList;
					if (ids.TryGetValue(id, out idList)) {
						if (excel.key_to_multi_values) {
							idList.Add(i);
						} else {
							err = string.Format("ID or key:{0} appeared more once...", id);
							break;
						}
					} else {
						idList = new List<int>();
						idList.Add(i);
						ids.Add(id, idList);
					}
				}
				foreach (KeyValuePair<int, List<int>> kv in ids) {
					indices.AddRange(kv.Value);
				}
			} else if (fieldTypes[0] == eFieldTypes.String) {
				SortedList<string, List<int>> keys = new SortedList<string, List<int>>();
				for (int i = global_configs.data_from_row, imax = table.Rows.Count; i < imax; i++) {
					if (EditorUtility.DisplayCancelableProgressBar("Excel", string.Format("Checking IDs or Keys... {0} / {1}", i - 1, imax - 2), (i - 1f) / (imax - 2f))) {
						EditorUtility.ClearProgressBar();
						return false;
					}
					string key = table.Rows[i].ItemArray[firstIndex].ToString().Trim();
					if (string.IsNullOrEmpty(key)) { continue; }
					List<int> idList;
					if (keys.TryGetValue(key, out idList)) {
						if (excel.key_to_multi_values) {
							idList.Add(i);
						} else {
							err = string.Format("ID or key:{0} appeared more once...", key);
							break;
						}
					} else {
						idList = new List<int>();
						idList.Add(i);
						keys.Add(key, idList);
					}
				}
				foreach (KeyValuePair<string, List<int>> kv in keys) {
					indices.AddRange(kv.Value);
				}
			}
			if (!string.IsNullOrEmpty(err)) {
				Debug.LogError(err);
				indices.Clear();
			}
			indices.Reverse();
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();
		}

		EditorUtility.ClearProgressBar();

		if (generateCode) {
			string serializeAttribute = excel.hide_asset_properties ? "[SerializeField, HideInInspector]" : "[SerializeField]";
			StringBuilder content = new StringBuilder();
			content.AppendLine("//----------------------------------------------");
			content.AppendLine("//    Auto Generated. DO NOT edit manually!");
			content.AppendLine("//----------------------------------------------");
			content.AppendLine();
			content.AppendLine("using UnityEngine;");
			if (excel.generate_get_method_if_possible && excel.key_to_multi_values && indices.Count > 0) {
				content.AppendLine("using System.Collections.Generic;");
			}
			content.AppendLine();

			string indent = "";
			if (!string.IsNullOrEmpty(excel.name_space)) {
				content.AppendLine(string.Format("namespace {0} {{", excel.name_space));
				content.AppendLine();
				indent = "\t";
			}

			content.AppendLine(string.Format("{0}public partial class {1} : ScriptableObject {{", indent, className));
			content.AppendLine();
			if (excel.use_hash_string) {
				content.AppendLine(string.Format("{0}\t{1}", indent, serializeAttribute));
				content.AppendLine(string.Format("{0}\tprivate string[] _HashStrings;", indent));
				content.AppendLine();
				content.AppendLine(string.Format("{0}\tprivate bool mHashStringSet = false;", indent));
				content.AppendLine();
			}
			content.AppendLine(string.Format("{0}\t{1}", indent, serializeAttribute));
			content.AppendLine(string.Format("{0}\tprivate {1}Item[] _Items;", indent, className));
			if (excel.use_hash_string) {
				content.AppendLine(string.Format("{0}\t{1} {2}Item[] items {{",
					indent, excel.use_public_items_getter ? "public" : "private", className));
				content.AppendLine(string.Format("{0}\t\tget {{", indent));
				content.AppendLine(string.Format("{0}\t\t\tif (!mHashStringSet) {{", indent));
				content.AppendLine(string.Format("{0}\t\t\t\tfor (int i = 0, imax = _Items.Length; i < imax; i++) {{", indent));
				content.AppendLine(string.Format("{0}\t\t\t\t\t_Items[i].SetStrings(_HashStrings);", indent));
				content.AppendLine(string.Format("{0}\t\t\t\t}}", indent));
				content.AppendLine(string.Format("{0}\t\t\t\tmHashStringSet = true;", indent));
				content.AppendLine(string.Format("{0}\t\t\t}}", indent));
				content.AppendLine(string.Format("{0}\t\t\treturn _Items;", indent));
				content.AppendLine(string.Format("{0}\t\t}}", indent));
				content.AppendLine(string.Format("{0}\t}}", indent));
			} else {
				content.AppendLine(string.Format("{0}\t{1} {2}Item[] items {{ get {{ return _Items; }} }}",
					indent, excel.use_public_items_getter ? "public" : "private", className));
			}

			content.AppendLine();
			if (indices.Count > 0) {
				string idVarName = fieldNames[0];
				idVarName = idVarName.Substring(0, 1).ToLower() + idVarName.Substring(1, idVarName.Length - 1);
				if (excel.key_to_multi_values) {
					content.AppendLine(string.Format("{0}\tpublic List<{1}Item> Get({2} {3}) {{", indent, className,
						GetFieldTypeName(fieldTypes[0]), idVarName));
					content.AppendLine(string.Format("{0}\t\tList<{1}Item> list = new List<{1}Item>(); ", indent, className));
					content.AppendLine(string.Format("{0}\t\tGet({1}, list);", indent, idVarName));
					content.AppendLine(string.Format("{0}\t\treturn list;", indent));
					content.AppendLine(string.Format("{0}\t}}", indent));
					content.AppendLine(string.Format("{0}\tpublic int Get({2} {3}, List<{1}Item> list) {{", indent, className,
						GetFieldTypeName(fieldTypes[0]), idVarName));
					content.AppendLine(string.Format("{0}\t\tint min = 0;", indent));
					content.AppendLine(string.Format("{0}\t\tint len = items.Length;", indent));
					content.AppendLine(string.Format("{0}\t\tint max = len;", indent));
					content.AppendLine(string.Format("{0}\t\tint index = -1;", indent));
					content.AppendLine(string.Format("{0}\t\twhile (min < max) {{", indent));
					content.AppendLine(string.Format("{0}\t\t\tint i = (min + max) >> 1;", indent));
					content.AppendLine(string.Format("{0}\t\t\t{1}Item item = _Items[i];", indent, className));
					content.AppendLine(string.Format("{0}\t\t\tif (item.{1} == {2}) {{", indent, fieldNames[0], idVarName));
					content.AppendLine(string.Format("{0}\t\t\t\tindex = i;", indent));
					content.AppendLine(string.Format("{0}\t\t\t\tbreak;", indent));
					content.AppendLine(string.Format("{0}\t\t\t}}", indent));
					if (fieldTypes[0] == eFieldTypes.String) {
						content.AppendLine(string.Format("{0}\t\t\tif (string.Compare({1}, item.{2}) < 0) {{", indent, idVarName, fieldNames[0]));
					} else {
						content.AppendLine(string.Format("{0}\t\t\tif ({1} < item.{2}) {{", indent, idVarName, fieldNames[0]));
					}
					content.AppendLine(string.Format("{0}\t\t\t\tmax = i;", indent));
					content.AppendLine(string.Format("{0}\t\t\t}} else {{", indent));
					content.AppendLine(string.Format("{0}\t\t\t\tmin = i + 1;", indent));
					content.AppendLine(string.Format("{0}\t\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t\tif (index < 0) {{ return 0; }}", indent));
					content.AppendLine(string.Format("{0}\t\tint l = index;", indent));
					content.AppendLine(string.Format("{0}\t\twhile (l - 1 >= 0 && _Items[l - 1].{1} == {2}) {{ l--; }}", indent, fieldNames[0], idVarName));
					content.AppendLine(string.Format("{0}\t\tint r = index;", indent));
					content.AppendLine(string.Format("{0}\t\twhile (r + 1 < len && _Items[r + 1].{1} == {2}) {{ r++; }}", indent, fieldNames[0], idVarName));
					content.AppendLine(string.Format("{0}\t\tfor (int i = l; i <= r; i++) {{", indent));
					content.AppendLine(string.Format("{0}\t\t\tlist.Add(_Items[i]);", indent));
					content.AppendLine(string.Format("{0}\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t\treturn r - l + 1;", indent));
					content.AppendLine(string.Format("{0}\t}}", indent));
				} else {
					content.AppendLine(string.Format("{0}\tpublic {1}Item Get({2} {3}) {{", indent, className,
						GetFieldTypeName(fieldTypes[0]), idVarName));
					content.AppendLine(string.Format("{0}\t\tint min = 0;", indent));
					content.AppendLine(string.Format("{0}\t\tint max = items.Length;", indent));
					content.AppendLine(string.Format("{0}\t\twhile (min < max) {{", indent));
					content.AppendLine(string.Format("{0}\t\t\tint index = (min + max) >> 1;", indent));
					content.AppendLine(string.Format("{0}\t\t\t{1}Item item = _Items[index];", indent, className));
					content.AppendLine(string.Format("{0}\t\t\tif (item.{1} == {2}) {{ return item; }}", indent, fieldNames[0], idVarName));
					if (fieldTypes[0] == eFieldTypes.String) {
						content.AppendLine(string.Format("{0}\t\t\tif (string.Compare({1}, item.{2}) < 0) {{", indent, idVarName, fieldNames[0]));
					} else {
						content.AppendLine(string.Format("{0}\t\t\tif ({1} < item.{2}) {{", indent, idVarName, fieldNames[0]));
					}
					content.AppendLine(string.Format("{0}\t\t\t\tmax = index;", indent));
					content.AppendLine(string.Format("{0}\t\t\t}} else {{", indent));
					content.AppendLine(string.Format("{0}\t\t\t\tmin = index + 1;", indent));
					content.AppendLine(string.Format("{0}\t\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t\treturn null;", indent));
					content.AppendLine(string.Format("{0}\t}}", indent));
				}
				content.AppendLine();
			}
			content.AppendLine(string.Format("{0}}}", indent));
			content.AppendLine();

			content.AppendLine(string.Format("{0}[System.Serializable]", indent));
			content.AppendLine(string.Format("{0}public class {1}Item {{", indent, className));
			content.AppendLine();
			if (enumDict != null) {
				foreach (KeyValuePair<string, List<string>> kv in enumDict) {
					content.AppendLine(string.Format("{0}\tpublic enum {1} {{", indent, kv.Key));
					content.Append(indent);
					content.Append("\t\t");
					for (int i = 0, imax = kv.Value.Count; i < imax; i++) {
						content.Append(kv.Value[i]);
						if (i < imax - 1) {
							content.Append(", ");
						} else {
							content.AppendLine();
						}
					}
					content.AppendLine(string.Format("{0}\t}}", indent));
					content.AppendLine();
				}
			}
			if (excel.use_hash_string) {
				content.AppendLine(string.Format("{0}\tprivate string[] mHashStrings;", indent));
				content.AppendLine(string.Format("{0}\tpublic void SetStrings(string[] strings) {{ mHashStrings = strings; }}", indent));
				content.AppendLine();
			}
			for (int i = 0, imax = fieldNames.Count; i < imax; i++) {
				string fieldName = fieldNames[i];
				eFieldTypes fieldType = fieldTypes[i];
				string fieldTypeName = fieldType == eFieldTypes.Unknown && enumDict != null ? fieldTypeNames[i] : GetFieldTypeName(fieldType);
				string capitalFieldName = CapitalFirstChar(fieldName);
				content.AppendLine(string.Format("{0}\t{1}", indent, serializeAttribute));
				if (excel.use_hash_string && fieldType == eFieldTypes.String) {
					content.AppendLine(string.Format("{0}\tprivate int _{1};", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\tpublic {1} {2} {{ get {{ return mHashStrings[_{3}]; }} }}", indent, fieldTypeName, fieldName, capitalFieldName));
				} else if (excel.use_hash_string && fieldType == eFieldTypes.Strings) {
					content.AppendLine(string.Format("{0}\tprivate int[] _{1};", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\tprivate {1} _{2}_;", indent, fieldTypeName, capitalFieldName));
					content.AppendLine(string.Format("{0}\tpublic {1} {2} {{", indent, fieldTypeName, fieldName));
					content.AppendLine(string.Format("{0}\t\tget {{", indent));
					content.AppendLine(string.Format("{0}\t\t\tif (_{1}_ == null || _{1}_.Length != _{1}.Length) {{ _{1}_ = new string[_{1}.Length]; }}",
						indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t\tfor (int i = _{1}.Length - 1; i >= 0; i--) {{", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t\t\t_{1}_[i] = mHashStrings[_{1}[i]];", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t\t\treturn _{1}_;", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t}}", indent));
				} else if (excel.compress_color_into_int && fieldType == eFieldTypes.Color) {
					content.AppendLine(string.Format("{0}\tprivate int _{1};", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\tpublic {1} {2} {{", indent, fieldTypeName, fieldName));
					content.AppendLine(string.Format("{0}\t\tget {{", indent));
					content.AppendLine(string.Format("{0}\t\t\tfloat inv = 1f / 255f;", indent));
					content.AppendLine(string.Format("{0}\t\t\tColor c = Color.black;", indent));
					content.AppendLine(string.Format("{0}\t\t\tc.r = inv * ((_{1} >> 24) & 0xFF);", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t\tc.g = inv * ((_{1} >> 16) & 0xFF);", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t\tc.b = inv * ((_{1} >> 8) & 0xFF);", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t\tc.a = inv * (_{1} & 0xFF);", indent, capitalFieldName));
					content.AppendLine(string.Format("{0}\t\t\treturn c;", indent));
					content.AppendLine(string.Format("{0}\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t}}", indent));
				} else {
					content.AppendLine(string.Format("{0}\tprivate {1} _{2};", indent, fieldTypeName, capitalFieldName));
					content.AppendLine(string.Format("{0}\tpublic {1} {2} {{ get {{ return _{3}; }} }}", indent, fieldTypeName, fieldName, capitalFieldName));
				}
				content.AppendLine();
			}
			if (excel.generate_tostring_method) {
				content.AppendLine(string.Format("{0}\tpublic override string ToString() {{", indent));
				List<string> toStringFormats = new List<string>();
				List<string> toStringValues = new List<string>();
				bool toStringContainsArray = false;
				for (int i = 0, imax = fieldNames.Count; i < imax; i++) {
					string fieldName = fieldNames[i];
					eFieldTypes fieldType = fieldTypes[i];
					toStringFormats.Add(string.Format("{0}:{{{1}}}", fieldName, i));
					bool isArray = fieldType == eFieldTypes.Floats || fieldType == eFieldTypes.Ints || fieldType == eFieldTypes.Strings;
					if (isArray) {
						toStringValues.Add(string.Format("array2string({0})", fieldName));
					} else {
						toStringValues.Add(fieldName);
					}
					if (!toStringContainsArray) {
						toStringContainsArray = isArray;
					}
				}
				content.AppendLine(string.Format("{0}\t\treturn string.Format(\"[{1}Item]{{{{{2}}}}}\",",
					indent, className, string.Join(", ", toStringFormats.ToArray())));
				content.AppendLine(string.Format("{0}\t\t\t{1});", indent, string.Join(", ", toStringValues.ToArray())));
				content.AppendLine(string.Format("{0}\t}}", indent));
				content.AppendLine();
				if (toStringContainsArray) {
					content.AppendLine(string.Format("{0}\tprivate string array2string(System.Array array) {{", indent));
					content.AppendLine(string.Format("{0}\t\tint len = array.Length;", indent));
					content.AppendLine(string.Format("{0}\t\tstring[] strs = new string[len];", indent));
					content.AppendLine(string.Format("{0}\t\tfor (int i = 0; i < len; i++) {{", indent));
					content.AppendLine(string.Format("{0}\t\t\tstrs[i] = string.Format(\"{{0}}\", array.GetValue(i));", indent));
					content.AppendLine(string.Format("{0}\t\t}}", indent));
					content.AppendLine(string.Format("{0}\t\treturn string.Concat(\"[\", string.Join(\", \", strs), \"]\");", indent));
					content.AppendLine(string.Format("{0}\t}}", indent));
					content.AppendLine();
				}
			}
			if (!string.IsNullOrEmpty(excel.name_space)) {
				content.AppendLine("\t}");
				content.AppendLine();
			}
			content.AppendLine("}");

			if (!Directory.Exists(excel.script_directory)) {
				Directory.CreateDirectory(excel.script_directory);
			}
			string scriptPath = null;
			if (excel.script_directory.EndsWith("/")) {
				scriptPath = string.Concat(excel.script_directory, className, ".cs");
			} else {
				scriptPath = string.Concat(excel.script_directory, "/", className, ".cs");
			}
			string fileMD5 = null;
			MD5CryptoServiceProvider md5Calc = null;
			if (File.Exists(scriptPath)) {
				md5Calc = new MD5CryptoServiceProvider();
				try {
					using (FileStream fs = File.OpenRead(scriptPath)) {
						fileMD5 = System.BitConverter.ToString(md5Calc.ComputeHash(fs));
					}
				} catch (System.Exception e) { Debug.LogException(e); }
			}
			byte[] bytes = Encoding.UTF8.GetBytes(content.ToString());
			bool toWrite = true;
			if (!string.IsNullOrEmpty(fileMD5)) {
				if (System.BitConverter.ToString(md5Calc.ComputeHash(bytes)) == fileMD5) {
					toWrite = false;
				}
			}
			if (toWrite) { File.WriteAllBytes(scriptPath, bytes); }
		} else {
			if (indices.Count <= 0) {
				for (int i = table.Rows.Count - 1; i >= global_configs.data_from_row; i--) {
					indices.Add(i);
				}
			}

			if (!Directory.Exists(excel.asset_directory)) {
				Directory.CreateDirectory(excel.asset_directory);
			}
			AssetDatabase.Refresh();

			string assetPath = null;
			if (excel.script_directory.EndsWith("/")) {
				assetPath = string.Concat(excel.asset_directory, className, ".asset");
			} else {
				assetPath = string.Concat(excel.asset_directory, "/", className, ".asset");
			}
			ScriptableObject obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ScriptableObject)) as ScriptableObject;
			bool isAlreadyExists = true;
			if (obj == null) {
				string fullName = !string.IsNullOrEmpty(excel.name_space) ? excel.name_space + "." + className : className;
				obj = ScriptableObject.CreateInstance(fullName);
				AssetDatabase.CreateAsset(obj, assetPath);
				isAlreadyExists = false;
			}

			Dictionary<string, int> hashStrings = new Dictionary<string, int>();
			SerializedObject so = new SerializedObject(obj);

			SerializedProperty pItems = so.FindProperty("_Items");
			pItems.ClearArray();

			if (isAlreadyExists) {
				SerializedProperty pStrings = so.FindProperty("_HashStrings");
				if (pStrings != null) { pStrings.ClearArray(); }
			}

			for (int i = 0, imax = indices.Count; i < imax; i++) {
				if (EditorUtility.DisplayCancelableProgressBar("Excel", string.Format("Serializing datas... {0} / {1}", i, imax), i / (float)imax)) {
					EditorUtility.ClearProgressBar();
					return false;
				}
				pItems.InsertArrayElementAtIndex(0);
				SerializedProperty pItem = pItems.GetArrayElementAtIndex(0);
				items = table.Rows[indices[i]].ItemArray;
				int numItems = items.Length;
				for (int j = 0, jmax = fieldNames.Count; j < jmax; j++) {
					SerializedProperty pField = pItem.FindPropertyRelative("_" + CapitalFirstChar(fieldNames[j]));
					int itemIndex = fieldIndices[j];
					object item = itemIndex < numItems ? items[itemIndex] : null;
					string value = item == null ? "" : item.ToString().Trim();
					if (itemIndex == firstIndex && string.IsNullOrEmpty(value)) { continue; }
					switch (fieldTypes[j]) {
						case eFieldTypes.Int:
							int intValue;
							if (int.TryParse(value, out intValue)) {
								pField.intValue = intValue;
							} else {
								pField.intValue = 0;
							}
							break;
						case eFieldTypes.Ints:
							int[] ints = GetIntsFromString(value);
							pField.ClearArray();
							for (int k = ints.Length - 1; k >= 0; k--) {
								pField.InsertArrayElementAtIndex(0);
								pField.GetArrayElementAtIndex(0).intValue = ints[k];
							}
							break;
						case eFieldTypes.Float:
							float floatValue;
							if (float.TryParse(value, out floatValue)) {
								pField.floatValue = floatValue;
							} else {
								pField.floatValue = 0f;
							}
							break;
						case eFieldTypes.Floats:
							float[] floats = GetFloatsFromString(value);
							pField.ClearArray();
							for (int k = floats.Length - 1; k >= 0; k--) {
								pField.InsertArrayElementAtIndex(0);
								pField.GetArrayElementAtIndex(0).floatValue = floats[k];
							}
							break;
                        case eFieldTypes.Double:
                            double doubleValue;
                            if (double.TryParse(value, out doubleValue))
                            {
                                pField.doubleValue = doubleValue;
                            }
                            else
                            {
                                pField.doubleValue = 0d;
                            }
                            break;
                        case eFieldTypes.Doubles:
                            double[] doubles = GetDoublesFromString(value);
                            pField.ClearArray();
                            for (int k = doubles.Length - 1; k >= 0; k--)
                            {
                                pField.InsertArrayElementAtIndex(0);
                                pField.GetArrayElementAtIndex(0).doubleValue = doubles[k];
                            }
                            break;
                        case eFieldTypes.Vector2:
							float[] floatsV2 = GetFloatsFromString(value);
							pField.vector2Value = floatsV2.Length == 2 ? new Vector2(floatsV2[0], floatsV2[1]) : Vector2.zero;
							break;
						case eFieldTypes.Vector3:
							float[] floatsV3 = GetFloatsFromString(value);
							pField.vector3Value = floatsV3.Length == 3 ? new Vector3(floatsV3[0], floatsV3[1], floatsV3[2]) : Vector3.zero;
							break;
						case eFieldTypes.Vector4:
							float[] floatsV4 = GetFloatsFromString(value);
							pField.vector4Value = floatsV4.Length == 4 ? new Vector4(floatsV4[0], floatsV4[1], floatsV4[2], floatsV4[3]) : Vector4.zero;
							break;
						case eFieldTypes.Rect:
							float[] floatsRect = GetFloatsFromString(value);
							pField.rectValue = floatsRect.Length == 4 ? new Rect(floatsRect[0], floatsRect[1], floatsRect[2], floatsRect[3]) : new Rect();
							break;
						case eFieldTypes.Color:
							Color c = GetColorFromString(value);
							if (excel.compress_color_into_int) {
								int colorInt = 0;
								colorInt |= Mathf.RoundToInt(c.r * 255f) << 24;
								colorInt |= Mathf.RoundToInt(c.g * 255f) << 16;
								colorInt |= Mathf.RoundToInt(c.b * 255f) << 8;
								colorInt |= Mathf.RoundToInt(c.a * 255f);
								pField.intValue = colorInt;
							} else {
								pField.colorValue = c;
							}
							break;
						case eFieldTypes.String:
							if (excel.use_hash_string) {
								int stringIndex;
								if (!hashStrings.TryGetValue(value, out stringIndex)) {
									stringIndex = hashStrings.Count;
									hashStrings.Add(value, stringIndex);
								}
								pField.intValue = stringIndex;
							} else {
								pField.stringValue = value;
							}
							break;
						case eFieldTypes.Strings:
							string[] strs = GetStringsFromString(value);
							pField.ClearArray();
							if (excel.use_hash_string) {
								for (int k = strs.Length - 1; k >= 0; k--) {
									string str = strs[k];
									int stringIndex;
									if (!hashStrings.TryGetValue(str, out stringIndex)) {
										stringIndex = hashStrings.Count;
										hashStrings.Add(str, stringIndex);
									}
									pField.InsertArrayElementAtIndex(0);
									pField.GetArrayElementAtIndex(0).intValue = stringIndex;
								}
							} else {
								for (int k = strs.Length - 1; k >= 0; k--) {
									pField.InsertArrayElementAtIndex(0);
									pField.GetArrayElementAtIndex(0).stringValue = strs[k];
								}
							}
							break;
						case eFieldTypes.Unknown:
							List<string> enumValues;
							if (enumDict != null && enumDict.TryGetValue(fieldTypeNames[j], out enumValues)) {
								pField.enumValueIndex = string.IsNullOrEmpty(value) ? 0 : enumValues.IndexOf(value);
							}
							break;
					}
				}
			}
			if (excel.use_hash_string && hashStrings.Count > 0) {
				string[] strings = new string[hashStrings.Count];
				foreach (KeyValuePair<string, int> kv in hashStrings) {
					strings[kv.Value] = kv.Key;
				}
				SerializedProperty pStrings = so.FindProperty("_HashStrings");
				pStrings.ClearArray();
				int total = strings.Length;
				for (int i = strings.Length - 1; i >= 0; i--) {
					if (EditorUtility.DisplayCancelableProgressBar("Excel", string.Format("Writing hash strings ... {0} / {1}",
						total - i, total), (float)(total - i) / total)) {
						EditorUtility.ClearProgressBar();
						return false;
					}
					pStrings.InsertArrayElementAtIndex(0);
					SerializedProperty pString = pStrings.GetArrayElementAtIndex(0);
					pString.stringValue = strings[i];
				}
			}
			EditorUtility.ClearProgressBar();
			so.ApplyModifiedProperties();
		}
		return true;
	}

	private static int[] GetIntsFromString(string str) {
		str = TrimBracket(str);
		if (string.IsNullOrEmpty(str)) { return new int[0]; }
		string[] splits = str.Split(',');
		int[] ints = new int[splits.Length];
		for (int i = 0, imax = splits.Length; i < imax; i++) {
			int intValue;
			if (int.TryParse(splits[i].Trim(), out intValue)) {
				ints[i] = intValue;
			} else {
				ints[i] = 0;
			}
		}
		return ints;
	}

    private static double[] GetDoublesFromString(string str)
    {
        str = TrimBracket(str);
        if (string.IsNullOrEmpty(str)) { return new double[0]; }
        string[] splits = str.Split(',');
        double[] doubles = new double[splits.Length];
        for (int i = 0, imax = splits.Length; i < imax; i++)
        {
            double doubleVal;
            if (double.TryParse(splits[i].Trim(), out doubleVal))
            {
                doubles[i] = doubleVal;
            }
            else
            {
                doubles[i] = 0;
            }
        }
        return doubles;
    }
    private static float[] GetFloatsFromString(string str) {
		str = TrimBracket(str);
		if (string.IsNullOrEmpty(str)) { return new float[0]; }
		string[] splits = str.Split(',');
		float[] floats = new float[splits.Length];
		for (int i = 0, imax = splits.Length; i < imax; i++) {
			float floatValue;
			if (float.TryParse(splits[i].Trim(), out floatValue)) {
				floats[i] = floatValue;
			} else {
				floats[i] = 0;
			}
		}
		return floats;
	}

	private static Color GetColorFromString(string str) {
		if (string.IsNullOrEmpty(str)) { return Color.clear; }
		uint colorUInt;
		if (GetColorUIntFromString(str, out colorUInt)) {
			uint r = (colorUInt >> 24) & 0xffu;
			uint g = (colorUInt >> 16) & 0xffu;
			uint b = (colorUInt >> 8) & 0xffu;
			uint a = colorUInt & 0xffu;
			return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
		}
		str = TrimBracket(str);
		string[] splits = str.Split(',');
		if (splits.Length == 4) {
			int r, g, b, a;
			if (int.TryParse(splits[0].Trim(), out r) && int.TryParse(splits[1].Trim(), out g) &&
				int.TryParse(splits[2].Trim(), out b) && int.TryParse(splits[3].Trim(), out a)) {
				return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
			}
		} else if (splits.Length == 3) {
			int r, g, b;
			if (int.TryParse(splits[0].Trim(), out r) && int.TryParse(splits[1].Trim(), out g) &&
				int.TryParse(splits[2].Trim(), out b)) {
				return new Color(r / 255f, g / 255f, b / 255f);
			}
		}
		return Color.clear;
	}

	private static bool GetColorUIntFromString(string str, out uint color) {
		if (reg_color32.IsMatch(str)) {
			color = System.Convert.ToUInt32(str, 16);
		} else if (reg_color24.IsMatch(str)) {
			color = (System.Convert.ToUInt32(str, 16) << 8) | 0xffu;
		} else {
			color = 0u;
			return false;
		}
		return true;
	}

	private static string[] GetStringsFromString(string str) {
		str = TrimBracket(str);
		if (string.IsNullOrEmpty(str)) { return new string[0]; }
		return str.Split(',');
	}

	enum eFieldTypes {
		Unknown, Int, Ints,Double,Doubles, Float, Floats, Vector2, Vector3, Vector4, Rect, Color, String, Strings
	}

	static eFieldTypes GetFieldType(string typename) {
		eFieldTypes type = eFieldTypes.Unknown;
		if (!string.IsNullOrEmpty(typename)) {
			switch (typename.Trim().ToLower()) {
				case "int": type = eFieldTypes.Int; break;
				case "ints": case "int[]": case "[int]": type = eFieldTypes.Ints; break;
				case "float": type = eFieldTypes.Float; break;
				case "double": type = eFieldTypes.Double; break;
				case "doubles": case "double[]": case "[double]": type = eFieldTypes.Doubles; break;
				case "floats": case "float[]": case "[float]": type = eFieldTypes.Floats; break;
				case "vector2": type = eFieldTypes.Vector2; break;
				case "vector3": type = eFieldTypes.Vector3; break;
				case "vector4": type = eFieldTypes.Vector4; break;
				case "rect": type = eFieldTypes.Rect; break;
				case "color": type = eFieldTypes.Color; break;
				case "string": type = eFieldTypes.String; break;
				case "strings": case "string[]": case "[string]": type = eFieldTypes.Strings; break;
			}
		}
		return type;
	}

	static string GetFieldTypeName(eFieldTypes type) {
		string name = null;
		switch (type) {
			case eFieldTypes.Int: name = "int"; break;
			case eFieldTypes.Ints: name = "int[]"; break;
			case eFieldTypes.Float: name = "float"; break;
			case eFieldTypes.Floats: name = "float[]"; break;
            case eFieldTypes.Double: name = "double"; break;
            case eFieldTypes.Doubles: name = "double[]"; break;
            case eFieldTypes.Vector2: name = "Vector2"; break;
			case eFieldTypes.Vector3: name = "Vector3"; break;
			case eFieldTypes.Vector4: name = "Vector4"; break;
			case eFieldTypes.Rect: name = "Rect"; break;
			case eFieldTypes.Color: name = "Color"; break;
			case eFieldTypes.String: name = "string"; break;
			case eFieldTypes.Strings: name = "string[]"; break;
		}
		return name;
	}

	static bool IsTypeSupportted(string typeName, string propertyName) {
		try {
			if (System.Enum.Parse(typeof(SerializedPropertyType), typeName) == null) {
				return false;
			}
			if (typeof(SerializedProperty).GetProperty(propertyName) == null) {
				return false;
			}
		} catch {
			return false;
		}
		return true;
	}

	static bool CheckClassName(string str) {
		return Regex.IsMatch(str, @"^[A-Z][A-Za-z0-9_]*$");
	}

	static bool CheckFieldName(string name) {
		return Regex.IsMatch(name, @"^[A-Za-z_][A-Za-z0-9_]*$");
	}

	static string CapitalFirstChar(string str) {
		return str[0].ToString().ToUpper() + str.Substring(1);
	}

	static string TrimBracket(string str) {
		if (str.StartsWith("[") && str.EndsWith("]")) {
			return str.Substring(1, str.Length - 2);
		}
		return str;
	}

	static bool CheckIsNameSpaceValid(string ns) {
		if (string.IsNullOrEmpty(ns)) { return true; }
		return Regex.IsMatch(ns, @"(\S+\s*\.\s*)*\S+");
	}

	static bool CheckIsDirectoryValid(string path) {
		if (path == "Assets") { return true; }
		path = path.Replace('\\', '/');
		if (!path.StartsWith("Assets/")) { return false; }
		if (path.Contains("//")) { return false; }
		char[] invalidChars = Path.GetInvalidPathChars();
		string[] splits = path.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0, imax = splits.Length; i < imax; i++) {
			string dir = splits[i].Trim();
			if (string.IsNullOrEmpty(dir)) { return false; }
			if (dir.IndexOfAny(invalidChars) >= 0) { return false; }
		}
		return true;
	}

	static ExcelToScriptableObjectGlobalConfigs global_configs = new ExcelToScriptableObjectGlobalConfigs();
	static List<ExcelToScriptableObjectSetting> excel_settings = null;

	static void ReadsSettings() {
		if (excel_settings == null || excel_settings.Count <= 0) {
			excel_settings = new List<ExcelToScriptableObjectSetting>();
			string json = File.Exists(SETTINGS_PATH) ? File.ReadAllText(SETTINGS_PATH, Encoding.UTF8) : null;
			if (!string.IsNullOrEmpty(json)) {
				ExcelToScriptableObjectSettings settings = JsonUtility.FromJson<ExcelToScriptableObjectSettings>(json);
				global_configs = settings.configs;
				if (settings.excels != null) {
					excel_settings.AddRange(settings.excels);
				}
			}
		}
		if (global_configs == null) { global_configs = new ExcelToScriptableObjectGlobalConfigs(); }
	}

	static ExcelToScriptableObjectSetting ReadExcelSettings(string excelName) {
		ReadsSettings();
		for (int i = 0, imax = excel_settings.Count; i < imax; i++) {
			ExcelToScriptableObjectSetting excel = excel_settings[i];
			if (excel.excel_name == excelName) { return excel; }
		}
		return null;
	}

	static void WriteSettings() {
		if (excel_settings == null) { return; }
		ExcelToScriptableObjectSettings data = new ExcelToScriptableObjectSettings();
		data.configs = global_configs;
		data.excels = excel_settings.ToArray();
		File.WriteAllText(SETTINGS_PATH, JsonUtility.ToJson(data, true), Encoding.UTF8);
	}

	private string[] mFolders;
	private int[] mFolderInts;

	private List<ExcelToScriptableObjectSetting> mCachedSettings = new List<ExcelToScriptableObjectSetting>();

	private bool mToWriteAssets = false;

	void OnFocus() {
		ReadsSettings();
		List<string> folders = new List<string>();
		Queue<string> toCheckFolders = new Queue<string>();
		toCheckFolders.Enqueue("Assets");
		while (toCheckFolders.Count > 0) {
			string folder = toCheckFolders.Dequeue();
			folders.Add(folder);
			string[] subFolders = AssetDatabase.GetSubFolders(folder);
			for (int i = 0, imax = subFolders.Length; i < imax; i++) {
				toCheckFolders.Enqueue(subFolders[i].Replace('\\', '/'));
			}
		}
		mFolders = folders.ToArray();
		mFolderInts = new int[mFolders.Length];
		for (int i = 0, imax = mFolderInts.Length; i < imax; i++) {
			mFolderInts[i] = i;
		}
		for (int i = 0, imax = excel_settings.Count; i < imax; i++) {
			excel_settings[i].UpdateDirIndices(mFolders);
		}
	}

	void Update() {
		if (mToWriteAssets && !EditorApplication.isCompiling) {
			mToWriteAssets = false;
			string[] excelNames = EditorPrefs.GetString("excel_to_scriptableobject", "").Split('#');
			EditorPrefs.DeleteKey("excel_to_scriptableobject");
			for (int i = 0, imax = excelNames.Length; i < imax; i++) {
				string excelName = excelNames[i];
				if (string.IsNullOrEmpty(excelName)) { continue; }
				ExcelToScriptableObjectSetting settings = ReadExcelSettings(excelName);
				if (settings != null) { Process(settings, false); }
			}
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
	}

	bool mGUIStyleInited = false;
	GUIStyle mStyleTextArea;
	Vector2 mScroll = Vector2.zero;

	void OnGUI() {
		if (!mGUIStyleInited) {
			mGUIStyleInited = true;
			mStyleTextArea = GUI.skin.FindStyle("TextArea") ?? GUI.skin.FindStyle("AS TextArea");
		}
		mCachedSettings.Clear();
		EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Global Settings :");
		EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
		GUILayout.Space(12f);
		EditorGUILayout.BeginVertical();
		global_configs.field_row = EditorGUILayout.IntField("Field Row (0 based)", global_configs.field_row);
		global_configs.type_row = EditorGUILayout.IntField("Type Row (0 based)", global_configs.type_row);
		global_configs.data_from_row = EditorGUILayout.IntField("Data From Row (0 based)", global_configs.data_from_row);
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.LabelField("Excel Settings :");
		EditorGUILayout.EndVertical();
		GUILayout.Space(108f);
		EditorGUILayout.BeginVertical(GUILayout.Width(100f));
		GUILayout.FlexibleSpace();
		Color cachedGUIBGColor = GUI.backgroundColor;
		GUI.backgroundColor = Color.green;
		bool processAll = GUILayout.Button("Process All", GUILayout.Width(100f), GUILayout.Height(30f));
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add New Excel", GUILayout.Width(100f), GUILayout.Height(20f))) {
			excel_settings.Add(new ExcelToScriptableObjectSetting());
		}
		GUI.backgroundColor = cachedGUIBGColor;
		EditorGUILayout.EndVertical();
		GUILayout.Space(8f);
		EditorGUILayout.EndHorizontal();
		int insertAt = -1;
		int removeAt = -1;
		mScroll = EditorGUILayout.BeginScrollView(mScroll, false, false);
		for (int i = 0, imax = excel_settings.Count; i < imax; i++) {
			cachedGUIBGColor = GUI.backgroundColor;
			EditorGUILayout.BeginHorizontal(mStyleTextArea);
			GUI.backgroundColor = (i & 1) == 0 ? new Color(0.6f, 0.6f, 0.7f) : new Color(0.8f, 0.8f, 0.8f);
			EditorGUILayout.BeginVertical(mStyleTextArea, GUILayout.MinHeight(20f));
			GUI.backgroundColor = cachedGUIBGColor;
			ExcelToScriptableObjectSetting setting = excel_settings[i];
			EditorGUILayout.BeginHorizontal();
			cachedGUIBGColor = GUI.backgroundColor;
			EditorGUILayout.LabelField("Excel File", string.IsNullOrEmpty(setting.excel_name) ? "(Not Selected)" : setting.excel_name);
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Select", GUILayout.Width(64f))) {
				if (Event.current.shift && !string.IsNullOrEmpty(setting.excel_name)) {
					if (Event.current.button == 0) {
						string folder = Path.GetDirectoryName(setting.excel_name);
						EditorUtility.OpenWithDefaultApp(folder);
					} else {
						EditorUtility.OpenWithDefaultApp(setting.excel_name);
					}
				} else {
					string p = EditorUtility.OpenFilePanel("Select Excel File", ".", "xlsx");
					if (!string.IsNullOrEmpty(p)) {
						string projPath = Application.dataPath;
						projPath = projPath.Substring(0, projPath.Length - 6);
						if (p.StartsWith(projPath)) { p = p.Substring(projPath.Length, p.Length - projPath.Length); }
						setting.excel_name = p;
						GUI.changed = true;
					}
				}
			}
			GUI.backgroundColor = cachedGUIBGColor;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			if (setting.dir_manual_edit) {
				cachedGUIBGColor = GUI.backgroundColor;
				GUI.backgroundColor = CheckIsDirectoryValid(setting.script_directory) ? Color.white : Color.red;
				string scriptDirectory = EditorGUILayout.TextField("Script Directory", setting.script_directory);
				if (setting.script_directory != scriptDirectory) {
					setting.script_directory = scriptDirectory.Replace('\\', '/');
				}
				GUI.backgroundColor = CheckIsDirectoryValid(setting.asset_directory) ? Color.white : Color.red;
				string assetDirectory = EditorGUILayout.TextField("Asset Directory", setting.asset_directory);
				if (setting.asset_directory != assetDirectory) {
					setting.asset_directory = assetDirectory.Replace('\\', '/');
				}
				GUI.backgroundColor = cachedGUIBGColor;
			} else {
				int scriptIndex = EditorGUILayout.IntPopup("Script Directory", setting.script_dir_index, mFolders, mFolderInts);
				int assetIndex = EditorGUILayout.IntPopup("Asset Directory", setting.asset_dir_index, mFolders, mFolderInts);
				if (scriptIndex != setting.script_dir_index) {
					setting.script_dir_index = scriptIndex;
					setting.script_directory = mFolders[setting.script_dir_index];
				}
				if (assetIndex != setting.asset_dir_index) {
					setting.asset_dir_index = assetIndex;
					setting.asset_directory = mFolders[setting.asset_dir_index];
				}
			}
			setting.name_space = EditorGUILayout.TextField("NameSpace", setting.name_space);
			EditorGUILayout.EndVertical();
			GUILayout.Space(8f);
			bool isDirEditManually = EditorGUILayout.ToggleLeft("Edit Manually", setting.dir_manual_edit, GUILayout.Width(92f));
			if (isDirEditManually != setting.dir_manual_edit) {
				setting.dir_manual_edit = isDirEditManually;
				if (!setting.dir_manual_edit) { setting.UpdateDirIndices(mFolders); }
			}
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(4f);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			setting.use_hash_string = EditorGUILayout.ToggleLeft("Use Hash String", setting.use_hash_string);
			setting.hide_asset_properties = EditorGUILayout.ToggleLeft("Hide Asset Properties", setting.hide_asset_properties);
			setting.generate_get_method_if_possible = EditorGUILayout.ToggleLeft("Generate Get Method If Possible", setting.generate_get_method_if_possible);
			EditorGUI.BeginDisabledGroup(!setting.generate_get_method_if_possible);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(16f);
			setting.key_to_multi_values = EditorGUILayout.ToggleLeft("ID or Key to Multi Values", setting.key_to_multi_values);
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			setting.use_public_items_getter = EditorGUILayout.ToggleLeft("Public Items Getter", setting.use_public_items_getter);
			setting.compress_color_into_int = EditorGUILayout.ToggleLeft("Compress Color into Integer", setting.compress_color_into_int);
			setting.treat_unknown_types_as_enum = EditorGUILayout.ToggleLeft("Treat Unknown Types as Enum", setting.treat_unknown_types_as_enum);
			setting.generate_tostring_method = EditorGUILayout.ToggleLeft("Generate ToString Method", setting.generate_tostring_method);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUILayout.Width(100f));
			if (GUILayout.Button("Insert", GUILayout.Height(20f))) {
				insertAt = i;
			}
			cachedGUIBGColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Delete", GUILayout.Height(20f))) {
				removeAt = i;
			}
			GUILayout.Space(8f);
			GUI.backgroundColor = cachedGUIBGColor;
			bool processable = !string.IsNullOrEmpty(setting.excel_name) &&
				CheckIsNameSpaceValid(setting.name_space) &&
				CheckIsDirectoryValid(setting.script_directory) &&
				CheckIsDirectoryValid(setting.asset_directory);
			cachedGUIBGColor = GUI.backgroundColor;
			GUI.backgroundColor = processable ? Color.green : Color.white;
			EditorGUI.BeginDisabledGroup(!processable);
			if (GUILayout.Button("Process Excel", GUILayout.Height(30f)) || (processable && processAll)) {
				mCachedSettings.Add(setting);
			}
			GUI.backgroundColor = cachedGUIBGColor;
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView();
		EditorGUI.EndDisabledGroup();
		if (insertAt >= 0) {
			excel_settings.Insert(insertAt, new ExcelToScriptableObjectSetting());
		}
		if (removeAt >= 0) {
			excel_settings.RemoveAt(removeAt);
		}
		if (GUI.changed) {
			WriteSettings();
		}
		if (mCachedSettings.Count > 0) {
			List<string> processingExcels = new List<string>();
			for (int i = 0, imax = mCachedSettings.Count; i < imax; i++) {
				if (Process(mCachedSettings[i], true)) {
					processingExcels.Add(mCachedSettings[i].excel_name);
				}
			}
			if (processingExcels.Count > 0) {
				EditorPrefs.SetString("excel_to_scriptableobject", string.Join("#", processingExcels.ToArray()));
				mToWriteAssets = true;
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
			}
		}
    }

}

[System.Serializable]
public class ExcelToScriptableObjectSettings {
	public ExcelToScriptableObjectGlobalConfigs configs;
	public ExcelToScriptableObjectSetting[] excels;
}

[System.Serializable]
public class ExcelToScriptableObjectGlobalConfigs {
	public int field_row = 0;
	public int type_row = 1;
	public int data_from_row = 2;
}

[System.Serializable]
public class ExcelToScriptableObjectSetting {
	public string excel_name;
	public string script_directory = "Assets";
	public string asset_directory = "Assets";
	public string name_space;
	public bool use_hash_string = false;
	public bool hide_asset_properties = true;
	public bool generate_get_method_if_possible = true;
	public bool key_to_multi_values = false;
	public bool use_public_items_getter = false;
	public bool compress_color_into_int = true;
	public bool treat_unknown_types_as_enum = false;
	public bool generate_tostring_method = true;
	[System.NonSerialized]
	public bool dir_manual_edit;
	[System.NonSerialized]
	public int script_dir_index;
	[System.NonSerialized]
	public int asset_dir_index;
	public void UpdateDirIndices(string[] folders) {
		script_dir_index = -1;
		asset_dir_index = -1;
		for (int i = 0, imax = folders.Length; i < imax; i++) {
			string folder = folders[i];
			if (script_directory == folder) { script_dir_index = i; }
			if (asset_directory == folder) { asset_dir_index = i; }
		}
	}
}
