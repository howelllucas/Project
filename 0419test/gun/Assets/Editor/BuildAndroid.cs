using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAndroid
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
    private static BuildTarget ms_buildTarget = BuildTarget.Android;

    private static string ANDROID_PROJECT = "../Android";

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
    static int BundleVersionCode = 20190117;
    static string ScriptingDefineSymbols = "GAME_DEBUG";
    static string PSBundleIdentifier = "com.ptstudio.tiles3";
    static string PSCompanyName = "cmplay";
    static string ApplicationDisplayName = "PT3";



    public static void Build()
    {
        Debug.Log("Build");
        UpdateBuildFlag();

        PlayerSettings.bundleVersion = BundleVersion;

        BuildOptions buildOption = BuildOptions.None;

        // 指定导出路径
        string locationPathName = ANDROID_PROJECT;

        // 指定AndroidBuildSystem
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        // 构建版本号
        PlayerSettings.Android.bundleVersionCode = BundleVersionCode;

        //指定bundle id
        PlayerSettings.applicationIdentifier = PSBundleIdentifier;

        //指定公司名
        PlayerSettings.companyName = PSCompanyName;


        // 指定BuildOptions,ScriptingDefineSymbols
        if (ms_isDebugBuild)
        {
            buildOption |= BuildOptions.Development;
            buildOption |= BuildOptions.AllowDebugging;
            buildOption |= BuildOptions.ConnectWithProfiler;
            buildOption |= BuildOptions.AcceptExternalModificationsToPlayer;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, ScriptingDefineSymbols);
        }
        else
        {
            buildOption |= BuildOptions.None;
            buildOption |= BuildOptions.AcceptExternalModificationsToPlayer;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, ScriptingDefineSymbols);
        }

        BuildPipeline.BuildPlayer(ms_scenes, locationPathName, ms_buildTarget, buildOption);
    }
}