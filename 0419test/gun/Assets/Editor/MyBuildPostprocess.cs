#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class MyBuildPostprocess
{
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);

            string target = pbxProject.TargetGuidByName("Unity-iPhone");
            pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

            pbxProject.AddFrameworkToProject(target, "Security.framework", false);
            pbxProject.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
            pbxProject.AddFrameworkToProject(target, "JavaScriptCore.framework", false);

            pbxProject.WriteToFile(projectPath);

            // Get plist
            string plistPath = path + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            var rootDict = plist.root;

            //// Change value of NSCameraUsageDescription in Xcode plist
            //var buildKey = "NSCameraUsageDescription";
            //rootDict.SetString(buildKey, "Taking screenshots");

            var buildKey2 = "ITSAppUsesNonExemptEncryption";
            rootDict.SetString(buildKey2, "false");

            string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
            if (rootDict.values.ContainsKey(exitsOnSuspendKey))
            {
                rootDict.values.Remove(exitsOnSuspendKey);
            }


            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif