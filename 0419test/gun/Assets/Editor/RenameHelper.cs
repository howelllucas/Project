using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class RenameHelper
{
    [MenuItem("Assets/RenameChildrens")]
    public static void Rename()
    {
        var roots = Selection.assetGUIDs;
        foreach (var root in roots)
        {
            string path = AssetDatabase.GUIDToAssetPath(root);
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                string title = dir.Name.ToLower();
                var files = dir.GetFiles();
                foreach (var file in files)
                {
                    if (file.Name.Contains(".meta"))
                        continue;
                    if (file.Name.Contains(".cs"))
                        continue;
                    if (file.Name.Contains(".prefab"))
                        continue;
                    
                    if (file.Name.StartsWith(title, System.StringComparison.CurrentCulture))
                        continue;
                   

                    string oriPath = path + "/" + file.Name;

                    string rename = file.Name.ToLower();
                    if (!file.Name.StartsWith(title, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        rename = title + "_" + rename;
                    }

                    var result = AssetDatabase.RenameAsset(oriPath, rename);
                    Debug.Log("RenameResult:" + result);
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
}
