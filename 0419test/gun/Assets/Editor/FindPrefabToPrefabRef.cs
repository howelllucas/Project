using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;


public class FindPrefabToPrefabRef : EditorWindow
{

    enum OPTIONS
    {
        prefab = 0,
        unity
    }

    Object target = null;                        //要查找的目标文件
    List<Object> results = new List<Object>();   //查找的结果
    OPTIONS findType = OPTIONS.prefab;

    [MenuItem("EZ/Tools/FindPrefabRef")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(FindPrefabToPrefabRef));
    }

    void OnGUI()
    {
        findType = (OPTIONS)EditorGUILayout.EnumPopup("查找目标类型", findType);

        GUILayout.Label("查找文件:");
        target = (Object)EditorGUILayout.ObjectField(target, typeof(Object), true);
        if (GUILayout.Button("查找"))
        {
            results.Clear();
            Debug.Log("开始查找.");
            FindOBJ(Application.dataPath + "/Resources/Prefabs/Bullet");

            FindOBJ(Application.dataPath + "/Resources/Prefabs/Weapon");
        }
        if (results.Count > 0)
        {
            foreach (Object t in results)
            {
                EditorGUILayout.ObjectField(t, typeof(Object), false);
            }
        }
        else
        {
            GUILayout.Label("无数据");
        }
    }
    void FindOBJ(string path)
    {
        //获取到所有要查找类型的文件目录
        string[] files = Directory.GetFiles(path, "*." + findType.ToString(), SearchOption.AllDirectories);
        Debug.Log(findType.ToString() + "遍历数量:" + files.Length);
        List<Object> filelst = new List<Object>();
        for (int i = 0; i < files.Length; i++)
        {
            //当前prefab下的所有引用到的资源 .png .FBX .mat .shader .cs .anim .controller .fbx .TTF .tga等
            string[] source = AssetDatabase.GetDependencies(new string[] { files[i].Replace(Application.dataPath, "Assets") });
            for (int j = 0; j < source.Length; j++)
            {
                string str = AssetDatabase.GetAssetPath(target);
                if (source[j] == str)
                {
                    //Debug.Log("包含选取文件prefab地址:" + files[i].ToString());
                    results.Add(AssetDatabase.LoadMainAssetAtPath(files[i].Replace(Application.dataPath, "Assets")));
                }
            }
        }
    }
}
