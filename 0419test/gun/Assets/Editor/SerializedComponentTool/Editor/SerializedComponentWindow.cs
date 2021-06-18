using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Cheetah.Common.SerializeTools {

	public class SerializedComponentWindow : EditorWindow {

		#region MenuItem

		[MenuItem("Cheetah/Serialize Tools/C# Serialized Component")]
		[MenuItem("Assets/Cheetah/Serialize Tools/C# Serialized Component", false)]
		static void OpenSerializedComponentWindow() {
			SerializedComponentWindow win = GetWindow<SerializedComponentWindow>("Component Viewer");
			win.minSize = new Vector2(480f, 300f);
			win.mObj = Selection.activeGameObject;
			win.Show();
		}


		[MenuItem("Assets/Cheetah/Serialize Tools/C# Serialized Component", true)]
		static bool CanAssetOpenSerializedComponentWindow() {
			return Selection.activeGameObject != null;
		}

		#endregion

		#region supported component types

		[SupportedComponentType]
		static SupportedTypeData DefineTypeTransform() {
			return new SupportedTypeData(typeof(Transform), int.MinValue, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeRectTransform() {
			return new SupportedTypeData(typeof(RectTransform), int.MinValue, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeText() {
			return new SupportedTypeData(typeof(UnityEngine.UI.Text), 100, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeButton() {
			return new SupportedTypeData(typeof(UnityEngine.UI.Button), 101, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeToggle() {
			return new SupportedTypeData(typeof(UnityEngine.UI.Toggle), 101, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeSlider() {
			return new SupportedTypeData(typeof(UnityEngine.UI.Slider), 101, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeScrollbar() {
			return new SupportedTypeData(typeof(UnityEngine.UI.Scrollbar), 101, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeInputField() {
			return new SupportedTypeData(typeof(UnityEngine.UI.InputField), 101, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeImage() {
			return new SupportedTypeData(typeof(UnityEngine.UI.Image), 102, null, null, null, null);
		}
		[SupportedComponentType]
		static SupportedTypeData DefineTypeRawImage() {
			return new SupportedTypeData(typeof(UnityEngine.UI.RawImage), 102, null, null, null, null);
		}
		[SupportedComponentType]
        static SupportedTypeData DefineTypeButtonEx()
        {
			return new SupportedTypeData(typeof(UnityEngine.UI.ButtonEx), 101, null, null, null, null);
        }

        private static Dictionary<int, SupportedTypeData> supported_type_datas;

		private static SupportedTypeData GetSupportedTypeData(Type type) {
			if (type == null) { return null; }
			if (supported_type_datas == null) { supported_type_datas = new Dictionary<int, SupportedTypeData>(); }
			if (supported_type_datas.Count <= 0) {
				Type tComponent = typeof(Component);
				Type attr = typeof(SupportedComponentTypeAttribute);
				Type dele = typeof(DefineSupportedTypeDelegate);
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0, imax = assemblies.Length; i < imax; i++) {
					Type[] types = assemblies[i].GetTypes();
					for (int j = 0, jmax = types.Length; j < jmax; j++) {
						Type tt = types[j];
						MethodInfo[] methods = tt.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						for (int k = 0, kmax = methods.Length; k < kmax; k++) {
							MethodInfo method = methods[k];
							if (!Attribute.IsDefined(method, attr)) { continue; }
							DefineSupportedTypeDelegate func = Delegate.CreateDelegate(dele, method, false) as DefineSupportedTypeDelegate;
							if (func == null) {
								Debug.LogErrorFormat("Method '{0}.{1}' with 'SupportedComponentType' should match 'DefineSupportedTypeDelegate' !", tt.FullName, method.Name);
								continue;
							}
							SupportedTypeData td = null;
							try { td = func(); } catch (Exception e) { Debug.LogException(e); }
							if (td == null || td.type == null) { continue; }
							if (!td.type.IsSubclassOf(tComponent)) {
								Debug.LogErrorFormat("Type should be sub class of 'Component' ! Error type : '{0}' .", td.type.FullName);
								continue;
							}
							string showName = td.showName;
							if (string.IsNullOrEmpty(showName)) { showName = td.type.Name; }
							string codeTypeName = td.codeTypeName;
							string nameSpace = td.nameSpace;
							if (string.IsNullOrEmpty(codeTypeName)) {
								codeTypeName = td.type.Name;
								nameSpace = td.type.Namespace;
							}
							string variableName = td.variableName;
							if (string.IsNullOrEmpty(variableName)) {
								variableName = td.type.Name;
								variableName = variableName.Substring(0, 1).ToLower() + variableName.Substring(1);
							}
							td = new SupportedTypeData(td.type, td.priority, showName, nameSpace, codeTypeName, variableName);
							supported_type_datas.Add(td.type.GetHashCode(), td);
						}
					}
				}
			}
			SupportedTypeData data;
			return supported_type_datas.TryGetValue(type.GetHashCode(), out data) ? data : null;
		}

		private static int SortSupportedTypeDatas(SupportedTypeData l, SupportedTypeData r) {
			if (l.priority == r.priority) { return string.Compare(l.type.Name, r.type.Name); }
			return l.priority < r.priority ? -1 : 1;
		}

		#endregion

		#region core api

		private static Regex variable_regex = new Regex(@"^[_a-zA-Z][_0-9a-zA-Z]*$");
		private static bool MatchVariableName(string name) {
			if (string.IsNullOrEmpty(name)) { return false; }
			return variable_regex.IsMatch(name);
		}

		private static ObjectComponents CollectComponents(Transform root, string rootName, string cls, string clsVar) {
			if (root == null) { return null; }
			List<ObjectComponents> components = new List<ObjectComponents>();
			Stack<Transform> trans = new Stack<Transform>(64);
			trans.Push(root);
			while (trans.Count > 0) {
				Transform t = trans.Pop();
				string name = t.name;
				bool isItem = t != root && name.StartsWith("i_");
				if (!isItem) {
					for (int i = t.childCount - 1; i >= 0; i--) {
						trans.Push(t.GetChild(i));
					}
				}
				if (t == root) { continue; }
				if (!isItem && !name.StartsWith("m_")) { continue; }
				name = name.Substring(2, name.Length - 2);
				string varName = name;
				string typeName = null;
				string typeVarName = null;
				if (isItem) {
					typeName = string.Concat(cls, "_", name);
					typeVarName = name;
					int ivv = name.IndexOf("||");
					if (ivv >= 0) {
						varName = name.Substring(0, ivv);
						typeName = name.Substring(ivv + 2, name.Length - ivv - 2);
						typeVarName = typeName;
					} else {
						int iv = name.IndexOf('|');
						if (iv >= 0) {
							varName = name.Substring(0, iv);
							typeVarName = name.Substring(iv + 1, name.Length - iv - 1);
							typeName = string.Concat(cls, "_", typeVarName);
						}
					}
					if (!MatchVariableName(typeName)) { continue; }
				}
				if (!MatchVariableName(varName)) { continue; }
				ObjectComponents ocs = null;
				if (isItem) {
					ocs = CollectComponents(t, varName, typeName, typeVarName);
				}
				if (ocs == null) {
					ocs = new ObjectComponents(t.gameObject, varName, null, null, null);
				}
				components.Add(ocs);
			}
			return new ObjectComponents(root.gameObject, rootName, cls, clsVar, components);
		}

		#endregion

		#region assist

		private class CodeProperties {
			public string nameSpace;
			public bool partialClass;
			public string className;
			public string baseClass;
			public bool publicProperty;
		}

		static Regex reg_namespace = new Regex(@"namespace\s+((\S+\s*\.\s*)*\S+)\s*\{");
		static Regex reg_class_def = new Regex(@"public\s+(partial\s+){0,1}class\s+(\S+)\s*:\s*(\S+)");
		static Regex reg_subclass_start = new Regex(@"\[System\.Serializable\]");
		static Regex reg_public_property = new Regex(@"public\s+\S+\s+\S+\s*\{\s*get\s*\{\s*return\s+\S+\s*;\s*\}\s*\}");
		private static CodeProperties CheckCodeAtPath(string path) {
			if (!File.Exists(path)) { return null; }
			string code = null;
			try {
				code = File.ReadAllText(path);
			} catch (Exception e) {
				Debug.LogException(e);
			}
			if (code == null) { return null; }
			//Log.w(code);
			Match matchClassDefine = reg_class_def.Match(code);
			if (!matchClassDefine.Success) { return null; }
			int i0 = matchClassDefine.Index + matchClassDefine.Length;
			GroupCollection groups = matchClassDefine.Groups;
			CodeProperties cp = new CodeProperties();
			cp.partialClass = groups[1].Success;
			cp.className = groups[2].Value;
			cp.baseClass = groups[3].Value;
			Match matchNS = reg_namespace.Match(code);
			string ns = matchNS.Success ? matchNS.Groups[1].Value : null;
			if (!string.IsNullOrEmpty(ns)) {
				ns = ns.Replace(" ", "").Replace("\t", "");
			}
			cp.nameSpace = ns;
			Match matchSubclassStart = reg_subclass_start.Match(code, i0);
			int i1 = code.Length;
			if (matchSubclassStart.Success) { i1 = matchSubclassStart.Index; }
			cp.publicProperty = reg_public_property.Match(code, i0, i1 - i0).Success;
			//Log.df("partial : {0}  public property : {1}  base class : {2}",
			//	cp.isPartialClass, cp.publicProperty, cp.baseClass);
			return cp;
		}

		#endregion

		private static MD5CryptoServiceProvider md5_calc;
		private static string GetKey(string key) {
			if (md5_calc == null) {
				md5_calc = new MD5CryptoServiceProvider();
			}
			string str = string.Concat(Application.dataPath, "SerializedComponent4CSharp", key);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
			return BitConverter.ToString(md5_calc.ComputeHash(bytes));
		}

		private GameObject mObj;
		private GameObject mPrevObj;
		private ObjectComponents mRootComponents = null;
		private List<ObjectComponentsWithIndent> mDrawingComponents = new List<ObjectComponentsWithIndent>();

		private List<string> mUsedCodeFolders = new List<string>();
		private bool mFolderManualEdit = false;
		private string[] mFolderList;
		private int mFolderIndex;

		private List<string> mUsedNameSpaces = new List<string>();
		private bool mNameSpaceManualEdit = false;
		private string[] mNameSpaceList;
		private int mNameSpaceIndex;

		private List<string> mUsedBaseClasss = new List<string>();
		private string[] mBaseClassList;

		private bool mDefaultPartialClass;
		private bool mDefaultPublicProperty;
		private bool mDefaultPartialItemClass;
		private bool mDefaultPublicItemProperty;

		private bool mGUIStyleInited = false;
		private GUIStyle mStyleBoldLabel;
		private GUIStyle mStyleBox;

		private bool mToSetSerializedObjects = false;

		private Vector2 mScroll;

		void OnEnable() {
			ResetFolderList();
			ResetNameSpaceList();
			ResetBaseClassList();
			mDefaultPartialClass = EditorPrefs.GetBool(GetKey("partial_class"), false);
			mDefaultPublicProperty = EditorPrefs.GetBool(GetKey("public_property"), true);
			mDefaultPartialItemClass = EditorPrefs.GetBool(GetKey("partial_item_class"), false);
			mDefaultPublicItemProperty = EditorPrefs.GetBool(GetKey("public_item_property"), true);
			mPrevObj = null;
			if (mObj != null) {
				string key = GetKey("set_serialized_objects");
				if (EditorPrefs.GetString(key, null) == mObj.name) {
					mToSetSerializedObjects = true;
				}
				EditorPrefs.DeleteKey(key);
			}
		}

		void OnGUI() {
			if (!mGUIStyleInited) {
				mGUIStyleInited = true;
				mStyleBoldLabel = "BoldLabel";
				mStyleBox = "CN Box";
			}
			EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);
			mObj = EditorGUILayout.ObjectField("Game Object", mObj, typeof(GameObject), true) as GameObject;
			if (mObj != mPrevObj) {
				mPrevObj = mObj;
				mDrawingComponents.Clear();
				if (mObj != null) {
					mRootComponents = CollectComponents(mObj.transform, mObj.name, mObj.name, null);
					if (mRootComponents != null) {
						string folder = mFolderList[mFolderIndex].Replace('\\', '/');
						if (!folder.EndsWith("/")) { folder = folder + "/"; }
						Stack<ObjectComponentsWithIndent> componentsStack = new Stack<ObjectComponentsWithIndent>();
						List<string> folders = new List<string>();
						string[] scripts = AssetDatabase.FindAssets("t:MonoScript");
						for (int i = 0, imax = scripts.Length; i < imax; i++) {
							string scriptPath = AssetDatabase.GUIDToAssetPath(scripts[i]);
							if (!scriptPath.EndsWith("/" + mRootComponents.cls + ".cs")) { continue; }
							folders.Add(scriptPath.Substring(0, scriptPath.Length - mRootComponents.cls.Length - 3));
						}
						SortedList<int, KeyValuePair<string, string>> sortedFolders = new SortedList<int, KeyValuePair<string, string>>();
						if (!folders.Contains(folder)) { folders.Add(folder); }
						for (int fi = folders.Count - 1; fi >= 0; fi--) {
							string f = folders[fi];
							string prevNS = null;
							int score = f == folder ? 10 : 0;
							mDrawingComponents.Clear();
							ObjectComponentsWithIndent ocwi = new ObjectComponentsWithIndent();
							ocwi.indent = 0;
							ocwi.components = mRootComponents;
							componentsStack.Push(ocwi);
							while (componentsStack.Count > 0) {
								ocwi = componentsStack.Pop();
								mDrawingComponents.Add(ocwi);
								List<ObjectComponents> ocsList = ocwi.components.itemComponents;
								if (ocsList != null) {
									//CheckCodePartialAndPublicProperty(p, ref ocwi.components.particalClass)
									for (int i = ocsList.Count - 1; i >= 0; i--) {
										ObjectComponentsWithIndent nocwi = new ObjectComponentsWithIndent();
										nocwi.indent = ocwi.indent + 1;
										nocwi.components = ocsList[i];
										componentsStack.Push(nocwi);
									}
								}
								if (ocwi.indent <= 0) {
									ocwi.components.partialClass = mDefaultPartialClass;
									ocwi.components.publicProperty = mDefaultPublicProperty;
								} else {
									ocwi.components.partialClass = mDefaultPartialItemClass;
									ocwi.components.publicProperty = mDefaultPublicItemProperty;
								}
								CodeProperties cp = null;
								if (!string.IsNullOrEmpty(ocwi.components.cls)) {
									cp = CheckCodeAtPath(string.Concat(f, ocwi.components.cls, ".cs"));
								}
								if (cp != null && cp.className == ocwi.components.cls) {
									score += 15;
									ocwi.components.partialClass = cp.partialClass;
									ocwi.components.publicProperty = cp.publicProperty;
									ocwi.components.baseClass = cp.baseClass;
									ocwi.components.baseClassIndex = -1;
									if (ocwi.indent <= 0) {
										prevNS = cp.nameSpace;
										if (!string.IsNullOrEmpty(prevNS) && prevNS == mNameSpaceList[mNameSpaceIndex]) {
											score += 100;
										}
									}
								}
							}
							if (!sortedFolders.ContainsKey(-score)) {
								sortedFolders.Add(-score, new KeyValuePair<string, string>(f, prevNS));
							}
						}
						foreach (KeyValuePair<int, KeyValuePair<string, string>> kv in sortedFolders) {
							string pFolder = kv.Value.Key;
							string prevNS = kv.Value.Value;
							mFolderIndex = -1;
							for (int i = mFolderList.Length - 1; i >= 0; i--) {
								string fi = mFolderList[i];
								if (fi == pFolder || (fi.Length == pFolder.Length - 1 && pFolder.StartsWith(fi))) {
									mFolderIndex = i;
									break;
								}
							}
							if (mFolderIndex < 0) {
								mFolderIndex = mFolderList.Length - 1;
								mFolderList[mFolderIndex] = pFolder.Substring(0, pFolder.Length - 1);
							}
							if (prevNS == null) { prevNS = ""; }
							mNameSpaceIndex = mUsedNameSpaces.IndexOf(prevNS);
							if (mNameSpaceIndex < 0) {
								mNameSpaceIndex = mNameSpaceList.Length - 1;
								mNameSpaceList[mNameSpaceIndex] = prevNS;
							} else if (mNameSpaceManualEdit) {
								mNameSpaceList[mNameSpaceList.Length - 1] = prevNS;
							} else {
								mNameSpaceList[mNameSpaceList.Length - 1] = "";
							}
							break;
						}
					}
				}
			}
			if (mObj == null && mDrawingComponents.Count > 0) {
				mDrawingComponents.Clear();
			}
			GUILayout.Space(8f);
			#region code folder
			EditorGUILayout.LabelField("生成代码目录");
			EditorGUILayout.BeginHorizontal();
			if (mFolderManualEdit) {
				int index = mFolderList.Length - 1;
				EditorGUI.BeginChangeCheck();
				mFolderList[index] = EditorGUILayout.DelayedTextField(mFolderList[index]);
				if (EditorGUI.EndChangeCheck()) {
					mFolderIndex = index;
				}
			} else {
				mFolderIndex = EditorGUILayout.Popup(mFolderIndex, mFolderList);
			}
			if (GUILayout.Button(mFolderManualEdit ? "选择" : "手动", GUILayout.Width(36f))) {
				mFolderManualEdit = !mFolderManualEdit;
				if (mFolderManualEdit) {
					mFolderList[mFolderList.Length - 1] = mFolderList[mFolderIndex];
				}
			}
			if (GUILayout.Button("浏览", GUILayout.Width(36f))) {
				string folder = EditorUtility.SaveFolderPanel("选择生成代码目录", mFolderList[mFolderIndex], "");
				if (!string.IsNullOrEmpty(folder)) {
					if (folder.StartsWith(Application.dataPath)) {
						folder = folder.Substring(Application.dataPath.Length - 6);
						mFolderIndex = mFolderList.Length - 1;
						mFolderList[mFolderIndex] = folder;
					}
				}
			}
			if (GUILayout.Button("删除", GUILayout.Width(36f))) {
				if (mFolderIndex == mFolderList.Length - 1) {
					mFolderList[mFolderIndex] = "";
				} else {
					mUsedCodeFolders.RemoveAt(mFolderIndex);
					SaveUsedFolders();
					ResetFolderList();
				}
			}
			EditorGUILayout.EndHorizontal();
			#endregion
			GUILayout.Space(8f);
			#region name space
			EditorGUILayout.BeginHorizontal();
			if (mNameSpaceManualEdit) {
				int index = mNameSpaceList.Length - 1;
				EditorGUI.BeginChangeCheck();
				mNameSpaceList[index] = EditorGUILayout.DelayedTextField("代码命名空间", mNameSpaceList[index]);
				if (EditorGUI.EndChangeCheck()) {
					mNameSpaceIndex = index;
				}
			} else {
				mNameSpaceIndex = EditorGUILayout.Popup("代码命名空间", mNameSpaceIndex, mNameSpaceList);
			}
			if (GUILayout.Button(mNameSpaceManualEdit ? "选择" : "手动", GUILayout.Width(36f))) {
				mNameSpaceManualEdit = !mNameSpaceManualEdit;
				if (mNameSpaceManualEdit) {
					mNameSpaceList[mNameSpaceList.Length - 1] = mNameSpaceList[mNameSpaceIndex];
				}
			}
			if (GUILayout.Button("删除", GUILayout.Width(36f))) {
				if (mNameSpaceIndex == mNameSpaceList.Length - 1) {
					mNameSpaceList[mNameSpaceIndex] = "";
				} else {
					mUsedNameSpaces.RemoveAt(mNameSpaceIndex);
					SaveUsedNameSpaces();
					ResetNameSpaceList();
				}
			}
			EditorGUILayout.EndHorizontal();
			#endregion
			EditorGUI.BeginChangeCheck();
			mDefaultPartialClass = EditorGUILayout.Toggle("默认使用partial类", mDefaultPartialClass);
			if (EditorGUI.EndChangeCheck()) {
				EditorPrefs.SetBool(GetKey("partial_class"), mDefaultPartialClass);
			}
			EditorGUI.BeginChangeCheck();
			mDefaultPublicProperty = EditorGUILayout.Toggle("默认使用public属性", mDefaultPublicProperty);
			if (EditorGUI.EndChangeCheck()) {
				EditorPrefs.SetBool(GetKey("public_property"), mDefaultPublicProperty);
			}
			EditorGUI.BeginChangeCheck();
			mDefaultPartialItemClass = EditorGUILayout.Toggle("默认Item使用partial类", mDefaultPartialItemClass);
			if (EditorGUI.EndChangeCheck()) {
				EditorPrefs.SetBool(GetKey("partial_item_class"), mDefaultPartialItemClass);
			}
			EditorGUI.BeginChangeCheck();
			mDefaultPublicItemProperty = EditorGUILayout.Toggle("默认Item使用public属性", mDefaultPublicItemProperty);
			if (EditorGUI.EndChangeCheck()) {
				EditorPrefs.SetBool(GetKey("public_item_property"), mDefaultPublicItemProperty);
			}
			GUILayout.Space(8f);
			EditorGUILayout.LabelField("序列化节点组件列表");
			mScroll = EditorGUILayout.BeginScrollView(mScroll, false, false);
			int count = mDrawingComponents.Count;
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(4f);
			EditorGUILayout.BeginVertical();
			for (int i = 0; i < count; i++) {
				Color cachedBgColor = GUI.backgroundColor;
				if ((i & 1) == 0) {
					GUI.backgroundColor = cachedBgColor * 0.8f;
				}
				int indent = mDrawingComponents[i].indent;
				if (indent > 0) {
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(12f * indent);
				}
				EditorGUILayout.BeginVertical(mStyleBox, GUILayout.MinHeight(10f));
				GUI.backgroundColor = cachedBgColor;
				ObjectComponents ocs = mDrawingComponents[i].components;
				int cCount = ocs.Count;
				EditorGUILayout.LabelField(ocs.name, mStyleBoldLabel);
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(12f);
				EditorGUILayout.BeginVertical();
				for (int j = 0; j < cCount; j++) {
					ComponentData cd = ocs[j];
					EditorGUILayout.ObjectField(cd.type.showName, cd.component, cd.type.type, true);
				}
				if (ocs.itemComponents != null) {
					ocs.partialClass = EditorGUILayout.Toggle("使用partial类", ocs.partialClass);
					ocs.publicProperty = EditorGUILayout.Toggle("使用public属性", ocs.publicProperty);
					EditorGUILayout.BeginHorizontal();
					if (ocs.baseClassIndex < 0) {
						ocs.baseClass = EditorGUILayout.DelayedTextField("继承自基类", ocs.baseClass);
					} else {
						EditorGUI.BeginChangeCheck();
						mBaseClassList[mBaseClassList.Length - 1] = ocs.baseClassIndex >= mBaseClassList.Length - 1 ?
							ocs.baseClass : "";
						ocs.baseClassIndex = EditorGUILayout.Popup("继承自基类", ocs.baseClassIndex, mBaseClassList);
						if (EditorGUI.EndChangeCheck()) {
							ocs.baseClass = mBaseClassList[ocs.baseClassIndex];
						}
					}
					if (GUILayout.Button(ocs.baseClassIndex < 0 ? "选择" : "手动", GUILayout.Width(36f))) {
						if (ocs.baseClassIndex < 0) {
							ocs.baseClassIndex = mUsedBaseClasss.IndexOf(ocs.baseClass);
							if (ocs.baseClassIndex < 0) { ocs.baseClassIndex = mUsedBaseClasss.Count; }
						} else {
							ocs.baseClassIndex = -1;
						}
					}
					if (i == 0 && GUILayout.Button("删除", GUILayout.Width(36f))) {
						if (ocs.baseClassIndex < mBaseClassList.Length - 1) {
							mUsedBaseClasss.RemoveAt(ocs.baseClassIndex);
							SaveUsedBaseClasses();
							ResetBaseClassList();
						}
						ocs.baseClass = mBaseClassList[ocs.baseClassIndex];
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				if (indent > 0) {
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
			GUILayout.Space(4f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndScrollView();
			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup(mObj == null || mRootComponents == null);
			if (GUILayout.Button("生成并挂载", GUILayout.Width(80f))) {
				string folder = mFolderList[mFolderIndex];
				if (!mUsedCodeFolders.Contains(folder)) {
					mUsedCodeFolders.Add(folder);
					SaveUsedFolders();
					ResetFolderList();
				}
				string ns = mNameSpaceList[mNameSpaceIndex];
				if (!mUsedNameSpaces.Contains(ns)) {
					mUsedNameSpaces.Add(ns);
					SaveUsedNameSpaces();
					ResetNameSpaceList();
				}
				bool usedBaseClassesChanged = false;
				for (int i = 0; i < count; i++) {
					ObjectComponents ocs = mDrawingComponents[i].components;
					if (!string.IsNullOrEmpty(ocs.baseClass) && !mUsedBaseClasss.Contains(ocs.baseClass)) {
						mUsedBaseClasss.Add(ocs.baseClass);
						usedBaseClassesChanged = true;
					}
				}
				if (usedBaseClassesChanged) {
					SaveUsedBaseClasses();
					ResetBaseClassList();
				}
				if (!folder.EndsWith("/")) { folder = folder + "/"; }
				List<CodeObject> codes = GetCodes(ns);
				bool flag = false;
				for (int i = 0, imax = codes.Count; i < imax; i++) {
					CodeObject code = codes[i];
					string path = string.Concat(folder, code.filename);
					string fileMd5 = null;
					if (File.Exists(path)) {
						try {
							using (FileStream fs = File.OpenRead(path)) {
								fileMd5 = BitConverter.ToString(md5_calc.ComputeHash(fs));
							}
						} catch (Exception e) { Debug.LogException(e); }
					}
					byte[] bytes = Encoding.UTF8.GetBytes(code.code);
					bool toWrite = true;
					if (!string.IsNullOrEmpty(fileMd5)) {
						if (BitConverter.ToString(md5_calc.ComputeHash(bytes)) == fileMd5) {
							toWrite = false;
						}
					}
					if (toWrite) {
						File.WriteAllBytes(path, bytes);
						flag = true;
					}
				}
				//if (flag) {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
           
                //}
				if (EditorApplication.isCompiling) {
					EditorPrefs.SetString(GetKey("set_serialized_objects"), mObj.name);
				} else {
					mToSetSerializedObjects = true;
				}
			}
			if (GUILayout.Button("预览代码", GUILayout.Width(80f))) {
				Rect rect = position;
				rect.x += 32f;
				rect.y += 32f;
				rect.width = 800f;
				CodePreviewWindow pw = GetWindow<CodePreviewWindow>("代码预览");
				pw.position = rect;
				pw.codes = GetCodes(mNameSpaceList[mNameSpaceIndex]);
				pw.Show();
			}
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			if (mToSetSerializedObjects) {
				mToSetSerializedObjects = false;
				SerializeObject(mNameSpaceList[mNameSpaceIndex], mRootComponents);
			}
		}

		private void SerializeObject(string ns, ObjectComponents ocs) {
			Queue<ObjectComponents> components = new Queue<ObjectComponents>();
			components.Enqueue(ocs);
			Stack<ObjectComponents> sortedComponents = new Stack<ObjectComponents>();
			while (components.Count > 0) {
				ObjectComponents oc = components.Dequeue();
				sortedComponents.Push(oc);
				for (int i = 0, imax = oc.itemComponents.Count; i < imax; i++) {
					ObjectComponents ioc = oc.itemComponents[i];
					if (ioc.itemComponents != null) { components.Enqueue(ioc); }
				}
			}
			while (sortedComponents.Count > 0) {
				ObjectComponents oc = sortedComponents.Pop();
				//TODO find type
				string typeFullName = string.IsNullOrEmpty(ns) ? oc.cls : string.Concat(ns, ".", oc.cls);
				//Log.d(typeFullName);
				Type type = Type.GetType(typeFullName + ",Assembly-CSharp");
				//End TODO
				//Log.d(type);
				if (type == null) {
					Debug.LogErrorFormat("Cannot Find Type for {0} !", oc.cls);
				} else if (!type.IsSubclassOf(typeof(MonoBehaviour))) {
					Debug.LogErrorFormat("Type : '{0}' is not subclass of MonoBehaviour !", type.FullName);
				} else {
					oc.type = type;
					Component component = oc.go.GetComponent(type);
					if (component == null) {
						component = oc.go.AddComponent(type);
					}
					SerializedObject so = new SerializedObject(component);
					List<ObjectComponents> itemComponents = oc.itemComponents;
					int count = itemComponents.Count;
					for (int i = 0; i < count; i++) {
						ObjectComponents ioc = itemComponents[i];
						SerializedProperty pObj = so.FindProperty(oc.publicProperty ? "m_" + ioc.name : ioc.name);
						if (pObj == null) {
							Debug.LogErrorFormat(ioc.go, "Cannot Find property for node '{0}' !", ioc.go.name);
							continue;
						}
						SerializedProperty pGO = pObj.FindPropertyRelative("m_GameObject");
						pGO.objectReferenceValue = ioc.go;
						int cCount = ioc.Count;
						for (int j = 0; j < cCount; j++) {
							ComponentData cd = ioc[j];
							//Log.dt(ioc.name, cd.type.codeTypeName + " " + cd.type.variableName);
							SerializedProperty pComponent = pObj.FindPropertyRelative("m_" + cd.type.variableName);
							if (pComponent == null) {
								Debug.LogErrorFormat(cd.component, "Cannot Find property for Component '{0}' at '{1}' !",
									cd.type.variableName, ioc.go.name);
								continue;
							}
							pComponent.objectReferenceValue = cd.component;
						}
						if (ioc.itemComponents != null && ioc.type != null) {
							SerializedProperty pItem = pObj.FindPropertyRelative("m_" + ioc.clsVar);
							if (pItem == null) {
								Debug.LogErrorFormat(ioc.go, "Cannot Find item property for Component '{0}' at '{1}' !",
									ioc.type.Name, ioc.go.name);
							} else {
								pItem.objectReferenceValue = ioc.go.GetComponent(ioc.type);
							}
						}
					}
					so.ApplyModifiedProperties();
				}
			}
		}

		private List<CodeObject> GetCodes(string ns) {
			List<CodeObject> codes = new List<CodeObject>();
			if (mRootComponents == null) { return codes; }
			List<ClassData> clses = new List<ClassData>();
			Queue<ObjectComponents> components = new Queue<ObjectComponents>();
			components.Enqueue(mRootComponents);
			while (components.Count > 0) {
				ObjectComponents ocs = components.Dequeue();
				List<ObjectComponents> itemComponents = ocs.itemComponents;
				if (itemComponents == null) { continue; }
				ClassData cd = null;
				for (int i = clses.Count - 1; i >= 0; i--) {
					if (clses[i].cls == ocs.cls) {
						cd = clses[i];
						break;
					}
				}
				if (cd == null) {
					cd = new ClassData();
					cd.cls = ocs.cls;
					clses.Add(cd);
				}
				if (!GetClass(ocs, cd)) {
					// TODO class not match...
				}
				/*string code = GetCode(ns, ocs);
				if (!string.IsNullOrEmpty(code)) {
					codes.Add(new CodeObject(string.Concat(ocs.cls, ".cs"), code));
				}*/
				for (int i = 0, imax = itemComponents.Count; i < imax; i++) {
					ObjectComponents ioc = itemComponents[i];
					if (ioc.itemComponents == null) { continue; }
					components.Enqueue(ioc);
				}
			}
			Comparison<SupportedTypeData> typeSorter = SortSupportedTypeDatas;
			for (int i = 0, imax = clses.Count; i < imax; i++) {
				ClassData cls = clses[i];
				for (int j = cls.fields.Count - 1; j >= 0; j--) {
					cls.fields[j].components.Sort(typeSorter);
				}
				string code = GetCode(ns, clses[i]);
				if (!string.IsNullOrEmpty(code)) {
					codes.Add(new CodeObject(string.Concat(cls.cls, ".cs"), code));
				}
			}
			return codes;
		}

		private class ClassData {
			public List<string> usings = new List<string>();
			public string cls;
			public string baseClass;
			public bool partialClass;
			public bool publicProperty;
			public List<FieldData> fields = new List<FieldData>();
		}
		private class FieldData {
			public string name;
			public string itemType;
			public string itemVar;
			public List<SupportedTypeData> components = new List<SupportedTypeData>();
		}

		private static bool GetClass(ObjectComponents ocs, ClassData cls) {
			if (cls.cls != ocs.cls) { return false; }
			if (cls.baseClass != null && cls.baseClass != ocs.baseClass) { return false; }
			cls.baseClass = ocs.baseClass;
			cls.partialClass |= ocs.partialClass;
			cls.publicProperty |= ocs.publicProperty;
			List<ObjectComponents> objComponents = ocs.itemComponents;
			for (int i = 0, imax = objComponents.Count; i < imax; i++) {
				ObjectComponents oc = objComponents[i];
				FieldData field = null;
				for (int j = cls.fields.Count - 1; j >= 0; j--) {
					FieldData f = cls.fields[j];
					if (f.name == oc.name) {
						field = f;
						break;
					}
				}
				if (field == null) {
					field = new FieldData();
					field.name = oc.name;
					field.itemType = null;
					cls.fields.Add(field);
				}
				for (int j = 0, jmax = oc.Count; j < jmax; j++) {
					SupportedTypeData type = oc[j].type;
					for (int k = field.components.Count - 1; k >= 0; k--) {
						if (field.components[k].showName == type.showName) {
							type = null;
							break;
						}
					}
					if (type != null) { field.components.Add(type); }
				}
				if (oc.itemComponents != null) {
					string itemClass = oc.cls;
					for (int k = field.components.Count - 1; k >= 0; k--) {
						if (field.components[k].showName == itemClass) {
							itemClass = null;
							break;
						}
					}
					if (itemClass != null) {
						field.components.Add(new SupportedTypeData(null, 10000, oc.cls, null, oc.cls, oc.clsVar));
						field.itemType = oc.cls;
						field.itemVar = oc.clsVar;
					}
				}
			}
			return true;
		}

		private static string GetCode(string ns, ClassData cls) {
			List<string> usings = new List<string>();
			SortedList<string, SupportedTypeData[]> dataClasses = new SortedList<string, SupportedTypeData[]>();
			StringBuilder code = new StringBuilder();
			Dictionary<string, KeyValuePair<string, string>> itemClasses = new Dictionary<string, KeyValuePair<string, string>>();
			string codeIndent = "";
			if (!string.IsNullOrEmpty(ns)) {
				codeIndent = "\t";
				code.AppendLine(string.Format("namespace {0} {{", ns));
				code.AppendLine();
			}
			code.AppendLine(string.Format("{0}public {1}class {2} : {3} {{",
				codeIndent, cls.partialClass ? "partial " : "", cls.cls, cls.baseClass));
			code.AppendLine();
			List<string> tempStrings = new List<string>();
			for (int i = 0, imax = cls.fields.Count; i < imax; i++) {
				FieldData field = cls.fields[i];
				tempStrings.Clear();
				for (int j = 0, jmax = field.components.Count; j < jmax; j++) {
					SupportedTypeData typeData = field.components[j];
					tempStrings.Add(typeData.type == null ? typeData.codeTypeName : typeData.type.Name);
					if (!string.IsNullOrEmpty(typeData.nameSpace) && !usings.Contains(typeData.nameSpace)) {
						usings.Add(typeData.nameSpace);
					}
				}
				string objTypeName = string.Concat(string.Join("_", tempStrings.ToArray()), "_Container");
				if (!dataClasses.ContainsKey(objTypeName)) {
					dataClasses.Add(objTypeName, field.components.ToArray());
				}
				tempStrings.Clear();
				code.AppendLine(string.Format("{0}\t[SerializeField]", codeIndent));
				if (cls.publicProperty) {
					code.AppendLine(string.Format("{0}\tprivate {1} m_{2};", codeIndent, objTypeName, field.name));
					code.AppendLine(string.Format("{0}\tpublic {1} {2} {{ get {{ return m_{2}; }} }}",
						codeIndent, objTypeName, field.name));
				} else {
					code.AppendLine(string.Format("{0}\tprivate {1} {2};", codeIndent, objTypeName, field.name));
				}
				code.AppendLine();
				if (!string.IsNullOrEmpty(field.itemType) && !string.IsNullOrEmpty(field.itemVar) && !itemClasses.ContainsKey(objTypeName)) {
					itemClasses.Add(objTypeName, new KeyValuePair<string, string>(field.itemType, field.itemVar));
				}
			}
			foreach (KeyValuePair<string, SupportedTypeData[]> kv in dataClasses) {
				code.AppendLine(string.Format("{0}\t[System.Serializable]", codeIndent));
				code.AppendLine(string.Format("{0}\t{1} class {2} {{",
					codeIndent, cls.publicProperty ? "public" : "private", kv.Key));
				code.AppendLine();
				code.AppendLine(string.Format("{0}\t\t[SerializeField]", codeIndent));
				code.AppendLine(string.Format("{0}\t\tprivate GameObject m_GameObject;", codeIndent));
				code.AppendLine(string.Format("{0}\t\tpublic GameObject gameObject {{ get {{ return m_GameObject; }} }}", codeIndent));
				code.AppendLine();
				for (int i = 0, imax = kv.Value.Length; i < imax; i++) {
					SupportedTypeData typeData = kv.Value[i];
					code.AppendLine(string.Format("{0}\t\t[SerializeField]", codeIndent));
					code.AppendLine(string.Format("{0}\t\tprivate {1} m_{2};",
						codeIndent, typeData.codeTypeName, typeData.variableName));
					code.AppendLine(string.Format("{0}\t\tpublic {1} {2} {{ get {{ return m_{2}; }} }}",
						codeIndent, typeData.codeTypeName, typeData.variableName));
					code.AppendLine();
				}
				KeyValuePair<string, string> typeAndVar;
				if (itemClasses.TryGetValue(kv.Key, out typeAndVar)) {
					code.AppendLine(string.Format("{0}\t\tprivate Queue<{1}> mCachedInstances;", codeIndent, typeAndVar.Key));
					code.AppendLine(string.Format("{0}\t\tpublic {1} GetInstance() {{", codeIndent, typeAndVar.Key));
					code.AppendLine(string.Format("{0}\t\t\t{1} instance = null;", codeIndent, typeAndVar.Key));
					code.AppendLine(string.Format("{0}\t\t\tif (mCachedInstances != null) {{", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\t\twhile ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {{", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\t\t\tinstance = mCachedInstances.Dequeue();", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\t\t}}", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\t}}", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tif (instance == null || instance.Equals(null)) {{", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\t\tinstance = Instantiate<{1}>(m_{2});", codeIndent, typeAndVar.Key, typeAndVar.Value));
					code.AppendLine(string.Format("{0}\t\t\t}}", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tTransform t0 = m_{1}.transform;", codeIndent, typeAndVar.Value));
					code.AppendLine(string.Format("{0}\t\t\tTransform t1 = instance.transform;", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tt1.SetParent(t0.parent);", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tt1.localPosition = t0.localPosition;", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tt1.localRotation = t0.localRotation;", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tt1.localScale = t0.localScale;", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tt1.SetSiblingIndex(t0.GetSiblingIndex() + 1);", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\treturn instance;", codeIndent));
					code.AppendLine(string.Format("{0}\t\t}}", codeIndent));
					code.AppendLine(string.Format("{0}\t\tpublic bool CacheInstance({1} instance) {{", codeIndent, typeAndVar.Key));
					code.AppendLine(string.Format("{0}\t\t\tif (instance == null || instance.Equals(null)) {{ return false; }}", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tif (mCachedInstances == null) {{ mCachedInstances = new Queue<{1}>(); }}", codeIndent, typeAndVar.Key));
					code.AppendLine(string.Format("{0}\t\t\tif (mCachedInstances.Contains(instance)) {{ return false; }}", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tinstance.gameObject.SetActive(false);", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\tmCachedInstances.Enqueue(instance);", codeIndent));
					code.AppendLine(string.Format("{0}\t\t\treturn true;", codeIndent));
					code.AppendLine(string.Format("{0}\t\t}}", codeIndent));
					code.AppendLine();
					if (!usings.Contains("System.Collections.Generic")) { usings.Add("System.Collections.Generic"); }
				}
				code.AppendLine(string.Format("{0}\t}}", codeIndent));
				code.AppendLine();
			}
			code.AppendLine(string.Format("{0}}}", codeIndent));
			if (!string.IsNullOrEmpty(ns)) {
				code.AppendLine();
				code.AppendLine("}");
			}
			if (!usings.Contains("UnityEngine")) { usings.Add("UnityEngine"); }
			if (!string.IsNullOrEmpty(ns)) { usings.Remove(ns); }
			usings.Sort();
			StringBuilder codeUsings = new StringBuilder();
			for (int i = 0, imax = usings.Count; i < imax; i++) {
				codeUsings.AppendLine(string.Format("using {0};", usings[i]));
			}
			codeUsings.AppendLine();
			return codeUsings.ToString() + code.ToString();
		}

		private void SaveUsedFolders() {
			string folders = string.Join("|", mUsedCodeFolders.ToArray());
			EditorPrefs.SetString(GetKey("saved_folders"), folders);
			EditorPrefs.SetInt(GetKey("saved_folder_index"), mFolderIndex);
		}

		private void ResetFolderList() {
			mUsedCodeFolders.Clear();
			string savedFolders = EditorPrefs.GetString(GetKey("saved_folders"), null);
			mFolderIndex = EditorPrefs.GetInt(GetKey("saved_folder_index"), 0);
			if (!string.IsNullOrEmpty(savedFolders)) {
				mUsedCodeFolders.AddRange(savedFolders.Split('|'));
			}
			if (mUsedCodeFolders.Count <= 0) {
				mUsedCodeFolders.Add("Assets");
			}
			string editingFolder = "";
			if (mFolderList != null && mFolderList.Length > 0) {
				editingFolder = mFolderList[mFolderList.Length - 1];
			}
			mUsedCodeFolders.Add(editingFolder);
			mFolderList = mUsedCodeFolders.ToArray();
			int folderCount = mUsedCodeFolders.Count;
			mUsedCodeFolders.RemoveAt(folderCount - 1);
			if (mFolderIndex < 0) {
				mFolderIndex = 0;
			} else if (mFolderIndex >= folderCount) {
				mFolderIndex = folderCount - 1;
			}
		}

		private void SaveUsedNameSpaces() {
			string ns = string.Join("|", mUsedNameSpaces.ToArray());
			EditorPrefs.SetString(GetKey("saved_namespaces"), ns);
			EditorPrefs.SetInt(GetKey("saved_namespace_index"), mNameSpaceIndex);
		}

		private void ResetNameSpaceList() {
			mUsedNameSpaces.Clear();
			string savedNameSpaces = EditorPrefs.GetString(GetKey("saved_namespaces"), null);
			mNameSpaceIndex = EditorPrefs.GetInt(GetKey("saved_namespace_index"), 0);
			if (!string.IsNullOrEmpty(savedNameSpaces)) {
				mUsedNameSpaces.AddRange(savedNameSpaces.Split('|'));
			}
			if (mUsedNameSpaces.Count <= 0) {
				mNameSpaceManualEdit = true;
			}
			string editingNameSpace = "";
			if (mNameSpaceList != null && mNameSpaceList.Length > 0) {
				editingNameSpace = mNameSpaceList[mNameSpaceList.Length - 1];
			}
			mUsedNameSpaces.Add(editingNameSpace);
			mNameSpaceList = mUsedNameSpaces.ToArray();
			int namespaceCount = mUsedNameSpaces.Count;
			mUsedNameSpaces.RemoveAt(namespaceCount - 1);
			if (mNameSpaceIndex < 0) {
				mNameSpaceIndex = 0;
			} else if (mNameSpaceIndex >= namespaceCount) {
				mNameSpaceIndex = namespaceCount - 1;
			}
		}

		private void SaveUsedBaseClasses() {
			string classes = string.Join("|", mUsedBaseClasss.ToArray());
			EditorPrefs.SetString(GetKey("saved_baseclasses"), classes);
		}

		private void ResetBaseClassList() {
			mUsedBaseClasss.Clear();
			string savedBaseClasss = EditorPrefs.GetString(GetKey("saved_baseclasses"), null);
			if (!string.IsNullOrEmpty(savedBaseClasss)) {
				mUsedBaseClasss.AddRange(savedBaseClasss.Split('|'));
			}
			if (mUsedBaseClasss.Count <= 0) {
				mUsedBaseClasss.Add("MonoBehaviour");
			}
			mUsedBaseClasss.Add("");
			mBaseClassList = mUsedBaseClasss.ToArray();
			int baseclassCount = mUsedBaseClasss.Count;
			mUsedBaseClasss.RemoveAt(baseclassCount - 1);
		}

		private struct ComponentData {
			public SupportedTypeData type;
			public Component component;
		}

		private struct ObjectComponentsWithIndent {
			public int indent;
			public ObjectComponents components;
		}

		private class ObjectComponents {
			public readonly string name;
			public readonly GameObject go;
			public readonly string cls;
			public readonly string clsVar;
			public readonly List<ObjectComponents> itemComponents;
			public int Count { get { return mComponents.Count; } }
			public int baseClassIndex;
			public string baseClass = "MonoBehaviour";
			public bool partialClass;
			public bool publicProperty;
			public Type type;
			public ComponentData this[int i] {
				get { return mComponents[i]; }
			}
			public ObjectComponents(GameObject go, string name, string cls, string clsVar, List<ObjectComponents> itemComponents) {
				this.go = go;
				this.name = name;
				this.cls = cls;
				this.clsVar = clsVar;
				this.itemComponents = itemComponents;
				temp_components.Clear();
				go.GetComponents<Component>(temp_components);
				for (int i = 0, imax = temp_components.Count; i < imax; i++) {
					Component component = temp_components[i];
					if (itemComponents != null && !(component is Transform)) { continue; }
					SupportedTypeData std = GetSupportedTypeData(component.GetType());
					if (std == null) { continue; }
					ComponentData data = new ComponentData();
					data.type = std;
					data.component = component;
					mComponents.Add(data);
				}
				temp_components.Clear();
			}
			private List<ComponentData> mComponents = new List<ComponentData>();
			private static List<Component> temp_components = new List<Component>();
		}

		private class CodeObject {
			public readonly string filename;
			public readonly string code;
			public readonly GUIContent codeContent;
			public CodeObject(string filename, string code) {
				this.filename = filename;
				this.code = code;
				codeContent = new GUIContent(code);
			}
		}

		private class CodePreviewWindow : EditorWindow {
			public List<CodeObject> codes;
			private Vector2[] mScrolls;
			private bool mStyleInited = false;
			private GUIStyle mStyleBox;
			private GUIStyle mStyleMessage;
			void OnGUI() {
				if (!mStyleInited) {
					mStyleInited = true;
					mStyleBox = "CN Box";
					mStyleMessage = "CN Message";
				}
				if (codes != null) {
					if (mScrolls == null || mScrolls.Length != codes.Count) {
						mScrolls = new Vector2[codes.Count];
					}
					for (int i = 0, imax = codes.Count; i < imax; i++) {
						CodeObject code = codes[i];
						EditorGUILayout.LabelField(code.filename);
						GUILayout.Space(2f);
						EditorGUILayout.BeginHorizontal();
						GUILayout.Space(4f);
						EditorGUILayout.BeginVertical(mStyleBox);
						mScrolls[i] = EditorGUILayout.BeginScrollView(mScrolls[i], false, false);
						Rect rect = GUILayoutUtility.GetRect(code.codeContent, mStyleBox);
						EditorGUI.SelectableLabel(rect, code.code, mStyleMessage);
						EditorGUILayout.EndScrollView();
						EditorGUILayout.EndVertical();
						EditorGUILayout.EndHorizontal();
					}
				}
			}
		}

	}

}