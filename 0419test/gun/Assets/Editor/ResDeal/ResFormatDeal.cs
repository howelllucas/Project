using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ResFormatDeal
{

    static string Android = "Android";
    static string IOS = "iPhone";
    [MenuItem("EZ/Image/FormatSelectTextureNoAlpha")]
    public static void FormatSelectTextureNoAlpha()
    {
        Debug.LogWarning("开始");
        Object[] selectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        for (int i = 0; i < selectedAsset.Length; i++)
        {
            Texture2D tex = selectedAsset[i] as Texture2D;
            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(tex));
            FormatTexture(ref ti);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex));
        }
        AssetDatabase.SaveAssets();
        Debug.LogWarning("结束");
    }
    [MenuItem("EZ/Image/FormateFbx")]
    public static void FormateFbx()
    {
        Debug.LogWarning("开始");
        Object[] selectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        for (int i = 0; i < selectedAsset.Length; i++)
        {
            ModelImporter importer = (ModelImporter)ModelImporter.GetAtPath(AssetDatabase.GetAssetPath(selectedAsset[i]));
            if (importer != null)
            {
                ResImprotPost.FormateFbx(ref importer);
                importer.importNormals = ModelImporterNormals.None;
                importer.importMaterials = true;
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(importer));
            }
        }
        AssetDatabase.SaveAssets();
        Debug.LogWarning("结束");
    }
    [MenuItem("EZ/Image/FormatSelectTextureAlpha")]
    public static void FormatSelectTextureAlpha()
    {
        Debug.LogWarning("开始");
        Object[] selectedAsset = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        for (int i = 0; i < selectedAsset.Length; i++)
        {
            Texture2D tex = selectedAsset[i] as Texture2D;
            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(tex));
            FormatTexture(ref ti,TextureImporterFormat.ASTC_RGBA_12x12);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tex));
        }
        AssetDatabase.SaveAssets();
        Debug.LogWarning("结束");
    }
    public static void FormatTexture(ref TextureImporter ti, TextureImporterFormat format = TextureImporterFormat.ASTC_RGB_12x12)
    {
        ti.isReadable = false;
        ti.mipmapEnabled = false;
        TextureImporterPlatformSettings settingAndroid = ti.GetPlatformTextureSettings(Android);
        //settingAndroid.maxTextureSize = 2048;
        settingAndroid.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
        settingAndroid.format = format;
        settingAndroid.compressionQuality = 100;
        settingAndroid.overridden = true;

        ti.SetPlatformTextureSettings(settingAndroid);

        TextureImporterPlatformSettings settingIos = ti.GetPlatformTextureSettings(IOS);
        //settingIos.maxTextureSize = 2048;
        settingIos.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
        settingIos.format = format;
        settingIos.compressionQuality = 100;
        settingIos.overridden = true;

        ti.SetPlatformTextureSettings(settingIos);
    }
    public static void ReImportAllModel()
    {
        string FbxPath = "Assets";
        DirectoryInfo direction = new DirectoryInfo(FbxPath);
        FileInfo[] files = direction.GetFiles("*.FBX", SearchOption.AllDirectories);
        foreach (FileSystemInfo file in files)
        {
            if (file.Name.Contains("Appear"))           //判断是否文件夹
            {
                string name = file.FullName.Substring(file.FullName.IndexOf("Assets"));
                AssetDatabase.CopyAsset(name, name);
                AssetDatabase.Refresh();
                break;
            }

        }
    }
    [MenuItem("EZ/AnimationOption")]
    public static void OptionAnimatinoClip()
    {
        string FbxPath = "Assets";
        DirectoryInfo direction = new DirectoryInfo(FbxPath);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        foreach (FileSystemInfo file in files)
        {

            if (file.Name.EndsWith(".anim"))           //判断是否文件夹
            {
                string[] contents = File.ReadAllLines(file.FullName, Encoding.Default);
                StringBuilder buffer = new StringBuilder();
                for (int i = 0; i < contents.Length; i++)
                {
                    string line = contents[i];
                    int index = line.IndexOf("{x");
                    if (index > 0)
                    {
                        string firstStr = line.Substring(0, index);
                        buffer.Append(firstStr);
                        string secondStr = line.Substring(index);
                        buffer.Append("{");
                        int endIndex = secondStr.Length - 1;
                        if (line.IndexOf('}') > 0)
                        {
                            endIndex = secondStr.Length - 2;
                        }
                        string[] split1 = secondStr.Substring(1, endIndex).Split(',');
                        for (int m = 0; m < split1.Length; m++)
                        {
                            if (split1[m].IndexOf(':') > 0)
                            {
                                string[] split2 = split1[m].Trim().Split(':');
                                buffer.Append(split2[0]);
                                buffer.Append(": ");
                                float newVal = (float)System.Convert.ToDouble(split2[1]) * 1000;
                                buffer.Append(Mathf.Floor(newVal) / 1000);
                                if (m < split1.Length - 1)
                                {
                                    buffer.Append(",");
                                }
                            }

                        }
                        if (endIndex == secondStr.Length - 2)
                        {
                            buffer.Append("}");
                        }
                    }
                    else
                    {
                        buffer.Append(line);
                    }
                    buffer.Append("\n");
                }
                File.WriteAllText(file.FullName, buffer.ToString());
                AssetDatabase.Refresh();
            }
        }

    }
    [MenuItem("EZ/清理ParticleSystem无效Mesh")]
    public static void CheckParticleSystemRenderer()
    {
        Object[] gos = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var item in gos)
        {
            // Filter non-prefab type,
            if (PrefabUtility.GetPrefabType(item) != PrefabType.Prefab)
            {
                continue;
            }

            GameObject gameObj = item as GameObject;

            ParticleSystemRenderer[] renders = gameObj.GetComponentsInChildren<ParticleSystemRenderer>(true);
            foreach (var renderItem in renders)
            {
                if (renderItem.renderMode != ParticleSystemRenderMode.Mesh)
                {
                    renderItem.mesh = null;
                    EditorUtility.SetDirty(gameObj);
                }
            }
        }

        AssetDatabase.SaveAssets();
    }
    [MenuItem("EZ/清理材质球纹理引用")]
    public static void ClearMatProperties()
    {
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets);
        Debug.Log(objs.Length);
        for (int i = 0; i < objs.Length; ++i)
        {
            Material mat = objs[i] as Material;

            if (mat)
            {
                SerializedObject psSource = new SerializedObject(mat);
                SerializedProperty emissionProperty = psSource.FindProperty("m_SavedProperties");
                SerializedProperty texEnvs = emissionProperty.FindPropertyRelative("m_TexEnvs");

                if (CleanMaterialSerializedProperty(texEnvs, mat))
                {
                    Debug.LogError("Find and clean useless texture propreties in " + mat.name);
                }
                Debug.LogError("Find and clean useless texture propreties in " + mat.name);
                psSource.ApplyModifiedProperties();
                EditorUtility.SetDirty(mat);
            }
        }

        AssetDatabase.SaveAssets();
    }

    //true: has useless propeties
    private static bool CleanMaterialSerializedProperty(SerializedProperty property, Material mat)
    {
        bool res = false;

        for (int j = property.arraySize - 1; j >= 0; j--)
        {
            string propertyName = property.GetArrayElementAtIndex(j).FindPropertyRelative("first").FindPropertyRelative("name").stringValue;

            if (!mat.HasProperty(propertyName))
            {
                if (propertyName.Equals("_MainTex"))
                {
                    //_MainTex是内建属性，是置空不删除，否则UITexture等控件在获取mat.maintexture的时候会报错
                    if (property.GetArrayElementAtIndex(j).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue != null)
                    {
                        property.GetArrayElementAtIndex(j).FindPropertyRelative("second").FindPropertyRelative("m_Texture").objectReferenceValue = null;
                        Debug.Log("Set _MainTex is null");
                        res = true;
                    }
                }
                else
                {
                    property.DeleteArrayElementAtIndex(j);
                    Debug.Log("Delete property in serialized object : " + propertyName);
                    res = true;
                    break;
                }
            }
        }
        if (res)
        {
            CleanMaterialSerializedProperty(property, mat);
        }
        return res;
    }
}
