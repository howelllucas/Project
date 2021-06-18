////自动生成状态机。

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.Rendering;

public class AnimatorPrefabUtils : EditorWindow  
{
    static string ReadAnimationPath;
    static string RootNodeName = "Position";
    public static string GenerateAnimatorPath = "Assets/Arts/Character/Gen/";

    public static string GeneratePrefabPath = "Assets/Arts/Character/Gen/";
    ///---------------------------------------------------------
    public static string AnimPath = "Assets/Arts/Character";
    public static List<string> AllAnimList = new List<string>();
    [MenuItem("EZ/动画信息工具")]
    static void AddWindow()
    {
        AllAnimList.Clear();
        DirectoryInfo dir = new DirectoryInfo(AnimPath);
        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
        foreach (FileSystemInfo i in fileinfo)
        {
            if (i is DirectoryInfo)            //判断是否文件夹
            {
                AllAnimList.Add(i.FullName);
            }
            //创建窗口
        }
        AnimatorPrefabUtils window = (AnimatorPrefabUtils)EditorWindow.GetWindow(typeof(AnimatorPrefabUtils), false, "Fbx动画信息导出工具 create by daiqixiang 2019-3-1 ");
        window.Show();
    }
    private void OnGUI()
    {
        GUIStyle text_style = new GUIStyle();
        text_style.fontSize = 20;
        text_style.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("资源路径");
        GUILayout.Label("AnimPath");

        int splitCount = 5;
        for (int i = 0; i < AllAnimList.Count; i++)
        {
            if (splitCount == 5)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            splitCount--;
            string path = AllAnimList[i];
            int newPathIndex = path.IndexOf("Character");
            if (GUILayout.Button(path.Substring(newPathIndex)))
            {
                ReadAnimationPath = path.Substring(path.IndexOf("Assets"));
                GenerateAnimatorPrefab();
                EditorUtility.DisplayDialog("提示",ReadAnimationPath, "确认");
            }
            if (splitCount == 0)
            {
                splitCount = 5;
                EditorGUILayout.EndHorizontal();
            }
        }
    }
    private static void ConfigurationFbx(List<string> files)
    {
        for (int k = 0; k < files.Count; k++)
        {
            string fbxPath = files[k].Substring(files[k].IndexOf("Assets\\"));
            ModelImporter model = AssetImporter.GetAtPath(fbxPath) as ModelImporter;
            model.meshCompression = ModelImporterMeshCompression.High;
            model.isReadable = false;
            model.optimizeMesh = true;
            model.importBlendShapes = false;
            model.weldVertices = true;
            model.addCollider = false;
            model.importVisibility = false;
            model.importCameras = false;
            model.importLights = false;
            model.preserveHierarchy = false;
            model.swapUVChannels = false;
            model.generateSecondaryUV = false;
            model.importNormals = ModelImporterNormals.None;
            model.importTangents = ModelImporterTangents.None;
            model.animationCompression = ModelImporterAnimationCompression.Optimal;
            model.indexFormat = ModelImporterIndexFormat.UInt16;
            model.optimizeGameObjects = true;
            //SerializedObject modelImporterObj = new SerializedObject(model);
            //SerializedProperty rootNodeProperty = modelImporterObj.FindProperty("m_HumanDescription.m_RootMotionBoneName");

            ////ModelImporterClipAnimation.ClipAnimationMaskType
            //if (!rootNodeProperty.stringValue.Equals(RootNodeName))
            //{
            //    rootNodeProperty.stringValue = RootNodeName;
            //    modelImporterObj.ApplyModifiedProperties();
            //}
            model.SaveAndReimport();
        }
        AssetDatabase.Refresh();
    }
    public static void GenerateAnimatorPrefab()
    {
        List<string> files = new List<string>();
        string rootFbxPath = "";
        FindFile(ReadAnimationPath, "*.FBX", files);
        ConfigurationFbx(files); 
        for (int k = 0;k < files.Count;k++)
        {
            int startIdx = files[k].LastIndexOf('@'); 
            if (startIdx < 0)
            {
                rootFbxPath = files[k];
                files.RemoveAt(k);
                break;
            }
        }
        GenerateAnimatorPrefabs(files, rootFbxPath);

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        //生成动画时长信息
        //AnimationClipDuration.CreateAnimationClipDuration();
    }

    static void GenerateAnimatorPrefabs(List<string> files, string rootFbxPath)
    {
        int startIndex = rootFbxPath.LastIndexOf("\\");
        int endIndex = rootFbxPath.LastIndexOf(".");
        string name = rootFbxPath.Substring(startIndex + 1, (endIndex - startIndex - 1));


        string controllerPath = name;
        controllerPath += ".controller";
        controllerPath = GenerateAnimatorPath + controllerPath;
        AnimatorController controller = GenerateAnimators(controllerPath, files);
        AssetDatabase.SaveAssets();

        GeneratePrefabs(name, rootFbxPath,controller);
    }

    static AnimatorController GenerateAnimators(string file, List<string> files)
    {
        string animationPath = "";
        AnimatorController animator = AnimatorController.CreateAnimatorControllerAtPath(file);
        AnimatorControllerLayer layer = animator.layers[0];
        AnimatorStateMachine stateMachine = layer.stateMachine;
        for(int i = 0;i< files.Count;i++)
        { 
            var j = 0;
            string fbx = files[i];
            fbx = fbx.Substring(fbx.IndexOf("Assets\\"));

            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(fbx);
            string filepath = Path.GetDirectoryName(fbx);
            foreach (var obj in objs)
            {
                if (obj is AnimationClip && obj.name.IndexOf("__preview__") == -1)
                {
                    animationPath = filepath + "\\" + obj.name + ".anim";
                    animationPath = animationPath.Replace('\\', '/');
                    animationPath = animationPath.Replace("//", "/");
                    animationPath = animationPath.Replace("//", "/");
                    //animationPath = animationPath.Replace("Arts", "Resources");
                    //Debug.Log(animationPath);
                    AnimationClip clip = Instantiate(obj) as AnimationClip;
                    AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
                    clipSetting.keepOriginalPositionXZ = false;//false为Root Node Position, true为Original.
                    clipSetting.keepOriginalPositionY = false;
                    clipSetting.keepOriginalOrientation = false;
                    AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
                    AssetDatabase.CreateAsset(clip, animationPath);
                    AnimatorState state = stateMachine.AddState(obj.name, new Vector3(250 + j * 200, 52 * i, 0));
                    j = j + 1;
                    state.motion = clip;
                }
            }
        }

        return animator;
        
    }

    static void GeneratePrefabs(string name, string fbxPath, AnimatorController controller)
    {
        name = name.Replace('\\', '/');
        string prefabPath = GeneratePrefabPath + name + ".prefab";
        fbxPath = fbxPath.Substring(fbxPath.IndexOf("Assets\\"));
        GameObject go;
        bool ShowDestroy;
        if (!AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)))
        {
            ShowDestroy = true;
            GameObject origalGo = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
            go = Instantiate(origalGo);
        }
        else
        {
            ShowDestroy = false;
            GameObject origalGo = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
            go = Instantiate(origalGo);
        }
        EditorUtility.SetDirty(go);
        Selection.activeObject = go;

        if (!go.GetComponent<Animator>())
        {
            //Debug.Log("添加一个animator");
            go.AddComponent<Animator>();
        }
        Animator animator = go.GetComponent<Animator>();
        animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = false;
        if (prefabPath.Contains("300"))
        {
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        }
        else
        {
            animator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
        SkinnedMeshRenderer[] SkinnedMeshRenders = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer _render in SkinnedMeshRenders)
        {
            _render.quality = SkinQuality.Auto;
            _render.lightProbeUsage = LightProbeUsage.Off;
            _render.reflectionProbeUsage = ReflectionProbeUsage.Off;
            _render.shadowCastingMode = ShadowCastingMode.On;
            _render.receiveShadows = false;
            _render.skinnedMotionVectors = false;
            _render.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        }
        if (!AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)))
        {
            //Debug.Log(prefabPath);
            PrefabUtility.CreatePrefab(prefabPath, go);
        }
        else
        {
            PrefabUtility.CreatePrefab(prefabPath, go);
            //Debug.Log(">>>>>>>>>>>>>>>>>>>>已经存在prefab，不在生成新的prefab，如果想重新生成，请将旧的手动删除");
            //Debug.Log(prefabPath);
        }
        if (ShowDestroy)
        {
            DestroyImmediate(go);
        }
        else
        {
            DestroyImmediate(go);
            //Resources.UnloadAsset(go);
        }
    }

    static void FindFile(string path, string searchPattern, List<string> files)
    {
        path += "\\";
        DirectoryInfo dir = new DirectoryInfo(path);
        foreach (DirectoryInfo di in dir.GetDirectories())
            FindFile(di.ToString(), searchPattern, files);

        foreach (FileInfo fi in dir.GetFiles(searchPattern))
        {
            files.Add(fi.ToString());
        }
    }

    static void ClassifyFiles(List<string> files, Dictionary<string, List<string>> classifyFiles)
    {
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            int pos = file.IndexOf('@');
            string classifyName;
            if (pos == -1)
                classifyName = file;
            else
            {
                classifyName = file.Substring(0, file.IndexOf('@'));
                classifyName += ".FBX";
            }
            List<string> classifyFilesVal;
            if (classifyFiles.ContainsKey(classifyName))
            {
                classifyFilesVal = classifyFiles[classifyName];
                //Debug.Log("file===" + file);
                classifyFilesVal.Add(file);
            }
            else
            {
                classifyFilesVal = new List<string>();
                classifyFiles.Add(classifyName, classifyFilesVal);
            }
        }
    }
}
