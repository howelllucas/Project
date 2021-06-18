using UnityEngine;
using UnityEditor;
using System;

public class BuildIOS
{
	private static string[] ms_scenes =
	{
        "Assets/Scenes/StartScene.unity",
        "Assets/Scenes/LoadingScene.unity",
        "Assets/Scenes/BattleScene.unity",
        "Assets/Scenes/CityScene.unity",
        "Assets/Scenes/EmptyScene.unity"
    };

	private static bool ms_isDebugBuild = false;
	private static BuildTarget ms_buildTarget = BuildTarget.iOS;

	private static string XCODE_PROJECT = "../IOS/PT3";

	private static void UpdateBuildFlag()
	{
		string[] args = System.Environment.GetCommandLineArgs();
        foreach (string oneArg in args)
        {
            if (oneArg != null && oneArg.Length > 0)
            {
                if (oneArg.ToLower().Contains("-debug"))
                {
                    Debug.Log("\"-debug\" is detected, switch to debug build.");
                    ms_isDebugBuild = true;
                    break;
                }
                else if (oneArg.ToLower().Contains("-release"))
                {
                    Debug.Log("\"-release\" is detected, switch to release build.");
                    ms_isDebugBuild = false;
                    break;
                }
            }
        }
			
		if (ms_isDebugBuild)
		{
			Debug.Log("neither \"-debug\" nor \"-release\" is detected, current is to debug build.");
		}
		else
		{
			Debug.Log("neither \"-debug\" nor \"-release\" is detected, current is to release build.");
		}
	}


	static string BundleVersion = "0.1.0";
	static string BuildNumber = "20190117";
	static string ScriptingDefineSymbols = "GAME_DEBUG";
	static string PSBundleIdentifier = "com.ptstudio.tiles3";
	static string PSCompanyName = "cmplay";
	static string ApplicationDisplayName = "PT3";

	// ios
	static string AppleDeveloperTeam = "X9K2269A6H";
	
	public static void Build()
	{
        BuildNumber = DateTime.Now.ToString("yyyyMMdd");

        Debug.Log("Build");
		UpdateBuildFlag ();

		PlayerSettings.bundleVersion = BundleVersion;

		BuildOptions buildOption = BuildOptions.None;

        // 指定导出路径
        string locationPathName = XCODE_PROJECT;
		
        // 构建版本号
		PlayerSettings.iOS.buildNumber = BuildNumber;

		//指定bundle id
		PlayerSettings.applicationIdentifier = PSBundleIdentifier;

		//指定公司名
		PlayerSettings.companyName = PSCompanyName;

		//指定team id
		PlayerSettings.iOS.appleDeveloperTeamID = AppleDeveloperTeam;
		//指定显示名称
		PlayerSettings.iOS.applicationDisplayName = ApplicationDisplayName;
       

		// 指定BuildOptions,ScriptingDefineSymbols
		if (ms_isDebugBuild)
		{
			buildOption |= BuildOptions.Development;
			buildOption |= BuildOptions.AllowDebugging;
			buildOption |= BuildOptions.ConnectWithProfiler;
			buildOption |= BuildOptions.AcceptExternalModificationsToPlayer;

			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS,ScriptingDefineSymbols);
		}
		else
		{
			buildOption |= BuildOptions.None;
			buildOption |= BuildOptions.AcceptExternalModificationsToPlayer;

			PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS,ScriptingDefineSymbols);
		}

		BuildPipeline.BuildPlayer(ms_scenes, locationPathName, ms_buildTarget, buildOption);
	}
}