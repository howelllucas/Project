#if UNITY_IOS && UNITY_EDITOR

using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

public static class SDKDSXcodePostProcess /*: MonoBehaviour*/
{
    [PostProcessBuild]
    static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        EditProj(pathToBuiltProject, pathToBuiltProject);
        EditInfoPlist(pathToBuiltProject);
    }

    static void EditProj(string pathToBuiltProject, string path)
    {
        string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

        PBXProject pbxProj = new PBXProject();
        pbxProj.ReadFromFile(projPath);
#if UNITY_2019_3_OR_NEWER
        string targetGuid = pbxProj.GetUnityFrameworkTargetGuid();
#else
        string targetGuid = pbxProj.TargetGuidByName("Unity-iPhone");
#endif
        pbxProj.AddFrameworkToProject(targetGuid, "libz.1.dylib", false);
        CopyAndAddFile(pbxProj, targetGuid, path, "Sdkds/Plugins/iOS/kfmt.dat", "Libraries/Plugins/iOS/SDKDSlaySDK");
        CopyAndAddFile(pbxProj, targetGuid, path, "Sdkds/Plugins/iOS/sdkdsSupport.dat", "Libraries/Plugins/iOS/SDKDSlaySDK");
        string resourcesDirectoryPath = pathToBuiltProject + "/Frameworks/Sdkds/Plugins/iOS/Resources";
        if (Directory.Exists(resourcesDirectoryPath) == false)
        {
            Debug.Log("copy SDKDSPromotion.bundle");
            CopyAndAddBundleToBuild(pbxProj, targetGuid, path, "Sdkds/Plugins/iOS/Resources", "Frameworks/Sdkds/Plugins/iOS/Resources", "SDKDSPromotion");
        }
        pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("/usr/lib/libz.1.dylib", "Frameworks/libz.1.dylib", PBXSourceTree.Absolute));
        pbxProj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");

        pbxProj.WriteToFile(projPath);
    }
    
    static void EditInfoPlist(string filePath)
    {
        string path = filePath + "/Info.plist";

        PlistDocument plistDocument = new PlistDocument();
        plistDocument.ReadFromFile(path);
        PlistElementDict dict = plistDocument.root.AsDict();

		// 添加白名单(add Application Queries Schemes)
		PlistElementArray array = dict.CreateArray("LSApplicationQueriesSchemes");

        // tianmao/taobao Application Queries Schemes:
		array.AddString("tmall");
		array.AddString("taobao");
        
        plistDocument.WriteToFile(path);
    }

    static void CopyAndAddBundleToBuild(PBXProject pbxProj, string targetGuid, string pathToBuiltProject, string relativeSrcPath, string relativeDstPath, string bundleName)
    {
        string fullSrcPath = Application.dataPath + "/" + relativeSrcPath;
        string fullDesPath = pathToBuiltProject + "/" + relativeDstPath;
        CopyDirectory(fullSrcPath, fullDesPath, new string[]{
            ".framework",
            ".mm",
            ".c",
            ".m",
            ".h",
            ".xib",
            ".a",
            ".plist",
            ".org",
            ".json",
            ".png",
            ".dat",
            ".strings",
            ".lproj",
            ".pic",
            ".meta",
            ""
        }, false);
        if (bundleName.Contains(".bundle") == false)
        {
            bundleName = bundleName + ".bundle";
        }
        string bundleFullPath = fullDesPath + "/" + bundleName;
        string projectPath = relativeDstPath + "/" + bundleName;
        pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(bundleFullPath, projectPath, PBXSourceTree.Absolute));
    }

    static void AddResToProject(PBXProject pbxProj, string targetGuid, string pathToBuiltProject, string srcPath, string dstPath)
    {
        string frameworksPath = Application.dataPath + "/" + srcPath;
        string[] directories = Directory.GetDirectories(frameworksPath, "*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < directories.Length; i++)
        {
            string path = directories[i];

            string name = path.Replace(frameworksPath + "/", "");
            string destDirName = pathToBuiltProject + "/" + dstPath + "/" + name;

            if (Directory.Exists(destDirName))
                Directory.Delete(destDirName, true);

            Debug.Log("copy: " + path + " => " + destDirName);
            CopyDirectory(path, destDirName, new string[] { ".meta", ".framework", ".mm", ".c", ".m", ".h", ".xib", ".a", ".plist", ".org", ".json", ".png", "" }, false);

            foreach (string file in Directory.GetFiles(destDirName, "*.*", SearchOption.AllDirectories))
                pbxProj.AddFileToBuild(targetGuid
                    , pbxProj.AddFile(file, file.Replace(pathToBuiltProject + "/", ""), PBXSourceTree.Source)
                );
        }
    }

    static void CopyAndAddFile(PBXProject pbxProj, string targetGuid, string pathToBuiltProject, string srcFilePath, string dstPath)
    {
        string absSrcFilePath = Application.dataPath + "/" + srcFilePath;
        string srcFileName = System.IO.Path.GetFileName(absSrcFilePath);
        string fullDstPath = pathToBuiltProject + "/" + dstPath;
        string absDestFileName = fullDstPath + "/" + srcFileName;
        string relDestFileName = dstPath + "/" + srcFileName;

        if (!Directory.Exists(fullDstPath))
            Directory.CreateDirectory(fullDstPath);
        if(File.Exists(absSrcFilePath))
        {
            File.Copy(absSrcFilePath, absDestFileName, true);
            pbxProj.AddFileToBuild(targetGuid
                , pbxProj.AddFile(relDestFileName, relDestFileName, PBXSourceTree.Source)
            );
        }
        
    }

    static void CopyDirectory(string srcPath, string dstPath, string[] excludeExtensions, bool overwrite = true)
    {
        if (!Directory.Exists(srcPath))
            return;
        if (!Directory.Exists(dstPath))
            Directory.CreateDirectory(dstPath);
        foreach (string file in Directory.GetFiles(srcPath, "*.*", SearchOption.TopDirectoryOnly))
        {
            if (excludeExtensions == null || _isIncludeExt(excludeExtensions, Path.GetExtension(file)))
            {
                File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)), overwrite);
            }
        }

        foreach (string dir in Directory.GetDirectories(srcPath))
            CopyDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)), excludeExtensions, overwrite);
    }

    static bool _isIncludeExt(string[] excludeExtensions, string fileExt)
    {
        foreach (string ext in excludeExtensions)
        {
            if (ext == fileExt)
            {
                return true;
            }
        }
        return false;
    }

    private static bool InjectCode(string[] fileNames, string anchor, string injection, bool before = false)
    {
        bool injected = false;
        foreach (var fileName in fileNames)
        {
            if (File.Exists(fileName))
                injected |= InjectCode(fileName, anchor, injection, before);
        }
        return injected;
    }

    private static bool InjectCode(string fileName, string anchor, string injection, bool before)
    {
        if (!File.Exists(fileName))
            return false;

        string[] afterParts = anchor.Split(' ');

        var fileContent = File.ReadAllText(fileName);

        var beforePos = 0;
        var afterPos = 0;
        if (afterParts.Length > 0)
            while (true)
            {
                var start = true;
                var match = true;
                foreach (var part in afterParts)
                {
                    var nextPos = fileContent.IndexOf(part, afterPos);

                    if (nextPos < 0)
                        return false;

                    if (!start)
                    {
                        var whitespace = fileContent.Substring(afterPos, nextPos - afterPos);

                        if (whitespace.Trim().Length > 0)
                        {
                            match = false;
                            break;
                        }
                    }
                    else
                    {
                        beforePos = nextPos;
                    }
                    start = false;
                    afterPos = nextPos + part.Length;
                }
                if (match)
                    break;
            }

        bool injected = false;
        if (before)
        {
            if (beforePos >= 0 && !fileContent.Substring(0, beforePos).EndsWith(injection))
            {
                fileContent = fileContent.Substring(0, beforePos) + injection + fileContent.Substring(beforePos);
                injected = true;
            }
        }
        else
        {
            if (afterPos >= 0 && !fileContent.Substring(afterPos).StartsWith(injection))
            {
                fileContent = fileContent.Substring(0, afterPos) + injection + fileContent.Substring(afterPos);
                injected = true;
            }
        }
        File.WriteAllText(fileName, fileContent);
        return injected;
    }
}

#endif