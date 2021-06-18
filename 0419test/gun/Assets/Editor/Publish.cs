using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class Publish {

	private static string BASE_PATH = Directory.GetCurrentDirectory();
	private static string XCODE_PROJECT_NAME = "/CM_OutPut/XcodeProject";

	private static string export_asset_path = BASE_PATH + "/CM_OutPut/AndroidProject";
	private static string export_asset_data_path = BASE_PATH + "/CM_OutPut/AndroidProject/" + PlayerSettings.productName + "/src/main";
	private static string studio_project_path = BASE_PATH + "/AndroidProject";
	private static string studio_project_data_path = BASE_PATH + "/AndroidProject/src/main";

	[MenuItem("Cheetah/ExportAndroid")]
	public static void BuildAndroid() {
		Debug.Log("==========Start Build==========");
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
		SetBuildParameters(BuildTargetGroup.Android);
		if (Directory.Exists(export_asset_path)) {
			Directory.Delete(export_asset_path, true);
		}
		Directory.CreateDirectory(export_asset_path);

		BuildOptions buildOption = BuildOptions.AcceptExternalModificationsToPlayer;
		/*if (bool.Parse(isDevelopment)) {
			buildOption |= BuildOptions.Development;
		}*/

		//PlayerSettings.Android.forceInternetPermission = true;  // 强制使用网络（安卓Bugly使用）
		PlayerSettings.Android.keystorePass = "00000000";       // 密钥密码
		PlayerSettings.Android.keyaliasName = "ab.keystore";    // 密钥别名
		PlayerSettings.Android.keyaliasPass = "00000000";
		BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, export_asset_path, BuildTarget.Android, buildOption);

		if (Directory.Exists(studio_project_data_path)) {
			Directory.Delete(studio_project_data_path, true);
		}
		Directory.CreateDirectory(studio_project_data_path);
		CopyDirectory(export_asset_data_path, studio_project_data_path);
		//CopyDirectory(exportAssetResSourceDataPath, exportAssetResTargetDataPath);
		
		Debug.Log("==========Build End=============");
	}

	[MenuItem("Cheetah/ExportXcode")]
	public static void BuildIOS() {
		Debug.Log("==========Start Build==========");
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
#if UNITY_IOS || UNITY_IPHONE
		PlayerSettings.iOS.appleEnableAutomaticSigning = false;
#endif
		SetBuildParameters(BuildTargetGroup.iOS);
		string locationPathName = BASE_PATH + XCODE_PROJECT_NAME;
		Debug.Log("locationPathName: " + locationPathName);
		if (Directory.Exists(locationPathName)) {
			Directory.Delete(locationPathName, true);
		}

		Directory.CreateDirectory(locationPathName);

		//PlayerSettings.applicationIdentifier = "com.cmcm.arrowbrawl";
        BuildOptions buildOption = BuildOptions.CompressWithLz4;
		BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, locationPathName, BuildTarget.iOS, buildOption);

		Debug.Log("==========Build End=============");
	}

	private static void SetBuildParameters(BuildTargetGroup target) {
		string folder = Application.dataPath;
		folder = folder.Substring(0, folder.Length - 6);
		string buildParamFilePath = folder + "build_params.txt";
		if (!File.Exists(buildParamFilePath)) {
			Debug.LogError("[BuildVersion] no build_params.txt file found !");
			return;
		}
		StreamReader text = File.OpenText(buildParamFilePath);
		KVReader kvs = new KVReader(text);
		string version = kvs.GetStringValue("version", "");
		string appid;
		int buildNum;
		SetApplicationId(target, kvs.GetStringValue("application_id", null), out appid);
		bool b1 = SetBundleVersion(version);
		bool b2 = SetBuildNumber(target, kvs.GetIntValue("build_number_reset_to", -1), out buildNum);
		SetBuildType(target, kvs.GetStringValue("build_type", ""));
		if (target == BuildTargetGroup.Android && b1 && b2) {
			SetGradleFile(kvs.GetStringValue("gradle", null), appid, version, buildNum);
		}
		if (target == BuildTargetGroup.iOS) {
			SetExportOptionPlist(appid, kvs.GetStringValue("certificate_type", null));
		}
	}

	private static bool SetApplicationId(BuildTargetGroup target, string appid, out string id) {
		if (string.IsNullOrEmpty(appid)) {
			id = target == BuildTargetGroup.Unknown ? PlayerSettings.applicationIdentifier :
				PlayerSettings.GetApplicationIdentifier(target);
			return false;
		}
		if (target == BuildTargetGroup.Unknown) {
			PlayerSettings.applicationIdentifier = appid;
		} else {
			PlayerSettings.SetApplicationIdentifier(target, appid);
		}
		id = appid;
		return true;
	}

	private static bool SetBundleVersion(string text) {
		if (!Regex.IsMatch(text, @"^(\d+.){2,3}(\d+)$")) {
			Debug.LogError("[BuildVersion] format error");
			return false;
		}
		string[] vs = text.Split('.');
		PlayerSettings.bundleVersion = string.Join(".", vs, 0, 3);
		return true;
	}

	private static bool SetBuildNumber(BuildTargetGroup target, int resetTo, out int num) {
		string folder = Application.dataPath;
		folder = folder.Substring(0, folder.Length - 6);
		string buildNumberFilePath = folder + "build_number.txt";
		num = 1;
		if (resetTo > 0) {
			num = resetTo;
		} else {
			if (File.Exists(buildNumberFilePath)) {
				string t = File.ReadAllText(buildNumberFilePath);
				int n;
				if (!string.IsNullOrEmpty(t) && int.TryParse(t, out n)) {
					num = n + 1;
				}
			}
		}
		switch (target) {
			case BuildTargetGroup.iOS:
				PlayerSettings.iOS.buildNumber = num.ToString();
				break;
			case BuildTargetGroup.Android:
				PlayerSettings.Android.bundleVersionCode = num;
				break;
		}
		File.WriteAllText(buildNumberFilePath, num.ToString());
		return true;
	}

	private static bool SetBuildType(BuildTargetGroup target, string buildType) {
		if (string.IsNullOrEmpty(buildType)) { return false; }
		List<string> includeMacros = new List<string>();
		List<string> excludeMacros = new List<string>();
		switch (buildType) {
			case "debug":
				excludeMacros.Add("RELEASE_BUILD");
				break;
			case "release":
				includeMacros.Add("RELEASE_BUILD");
				break;
			default:
				Debug.LogErrorFormat("[Publish] Unknown build type '{0}' !", buildType);
				break;
		}
		string ms = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
		List<string> macros = new List<string>(ms.Split(';'));
		foreach (string macro in includeMacros) {
			if (!macros.Contains(macro)) { macros.Add(macro); }
		}
		foreach (string macro in excludeMacros) {
			macros.Remove(macro);
		}
		PlayerSettings.SetScriptingDefineSymbolsForGroup(target, string.Join(";", macros.ToArray()));
		return true;
	}

	private static bool SetGradleFile(string gradle, string appid, string version, int buildNum) {
		if (string.IsNullOrEmpty(gradle)) { return false; }
		string dir = "BuildTools/GradleTemplates/" + gradle;
		if (!Directory.Exists(dir)) { return false; }
		string[] gradles = Directory.GetFiles(dir, "*gradle*", SearchOption.TopDirectoryOnly);
		bool ret = false;
		foreach (string path in gradles) {
			if (!File.Exists(path)) { continue; }
			string content = File.ReadAllText(path);
			content = content.Replace("#APPLICATION_ID#", appid);
			content = content.Replace("#VERSION_CODE#", buildNum.ToString());
			content = content.Replace("#VERSION_NAME#", version);
			File.WriteAllText(studio_project_path + "/" + Path.GetFileName(path), content);
			ret = true;
		}
		return ret;
	}

	private static bool SetExportOptionPlist(string appid, string certificateType) {
		if (string.IsNullOrEmpty(certificateType)) { return false; }
		string path = string.Format("BuildTools/iOSCertifications/{0}/OptionPlist.plist", certificateType);
		if (!File.Exists(path)) { return false; }
		string content = File.ReadAllText(path);
		content = content.Replace("#APPLICATION_ID#", appid);
		File.WriteAllText(path, content);
		return true;
	}

	private static void CopyDirectory(string from, string to) {
		try {
			//检查是否存在目的目录  
			if (!Directory.Exists(to)) {
				Directory.CreateDirectory(to);
			}
			if (Directory.Exists(from)) {
				//先来复制文件  
				DirectoryInfo directoryInfo = new DirectoryInfo(from);
				FileInfo[] files = directoryInfo.GetFiles();
				//复制所有文件  
				foreach (FileInfo file in files) {
					string _toPath = Path.Combine(to, file.Name);
					//Debug.Log("拷贝文件 --->" + file.DirectoryName + "-->" + _toPath);
					if (File.Exists(_toPath)) {
						File.Delete(_toPath);
					}
					file.CopyTo(_toPath);
				}
				//最后复制目录  
				DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
				foreach (DirectoryInfo dir in directoryInfoArray) {
					CopyDirectory(Path.Combine(from, dir.Name), Path.Combine(to, dir.Name));
				}
				//Debug.Log("拷贝文件 " + from + " 成功");
			} else {
				Debug.Log("没有该目录： " + from);
			}
		} catch (System.Exception e) {
			Debug.Log("CreateDirectory is miss  " + e);
			if (e.Message.Contains("Disk full")) {
				throw new System.Exception("FileManager.CopyDirectory ------>" + e.Message);
			} else {
				throw new System.Exception("FileManager.CopyDirectory ------>\n" + e.Message);
			}
		}
	}
	private class KVReader {

		private Dictionary<string, string> mKVs = new Dictionary<string, string>();

		public KVReader(TextReader tr) {
			while (true) {
				string line = tr.ReadLine();
				if (string.IsNullOrEmpty(line)) { break; }
				int index = line.IndexOf('=');
				if (index <= 0) { continue; }
				string key = line.Substring(0, index).Trim();
				if (string.IsNullOrEmpty(key)) { continue; }
				string val = line.Substring(index + 1, line.Length - index - 1).Trim();
				mKVs.Add(key, val);
			}
		}

		public string GetStringValue(string key, string defaultValue) {
			string val;
			return mKVs.TryGetValue(key, out val) ? val : defaultValue;
		}

		public int GetIntValue(string key, int defaultValue) {
			string val;
			if (!mKVs.TryGetValue(key, out val)) { return defaultValue; }
			int intVal;
			return int.TryParse(val, out intVal) ? intVal : defaultValue;
		}

		public float GetFloatValue(string key, float defaultValue) {
			string val;
			if (!mKVs.TryGetValue(key, out val)) { return defaultValue; }
			float floatVal;
			return float.TryParse(val, out floatVal) ? floatVal : defaultValue;
		}

	}

}
