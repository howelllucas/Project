using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResImprotPost : AssetPostprocessor
{
    //模型导入之前调用
    private static List<string> NormalInprotList = new List<string>
    {
          "9002",
          "weapon_",
          "Npc_",
          "Enemy1003",
          "Enemy2001",
          "Enemy300",
          "Enemy2011"
    };
    private void CfgSceneModel()
    {
        if (this.assetPath.Contains("custom") || this.assetPath.Contains("Custom"))
        {
            return;
        }
        if (this.assetPath.Contains("Assets/Arts/Scene"))
        {
            ModelImporter model = (ModelImporter)assetImporter;
            //model.importNormals = ModelImporterNormals.Import;
            FormateFbx(ref model);
        }
        else if (this.assetPath.Contains("Assets/Arts/Character"))
        {

            ModelImporter model = (ModelImporter)assetImporter;
            FormateFbx(ref model);
            model.importMaterials = true;
            model.importAnimation = true;
            bool importNormals = false;
            foreach (string keyWord in NormalInprotList)
            {
                if (this.assetPath.Contains(keyWord))
                {
                    importNormals = true;
                    break;
                }
            }
            if (importNormals)
            {
                model.importNormals = ModelImporterNormals.Import;
            }
            else
            {
                model.importNormals = ModelImporterNormals.None;
            }
            model.animationType = ModelImporterAnimationType.Generic;
        }
        else if (this.assetPath.Contains("Assets/Arts/Effect"))
        {
            ModelImporter model = (ModelImporter)assetImporter;
            FormateFbx(ref model);
            model.importNormals = ModelImporterNormals.None;
        }
    }

    public static void FormateFbx(ref ModelImporter model)
    {
        model.animationType = ModelImporterAnimationType.None;
        model.importMaterials = false;
        model.importAnimation = false;

        model.meshCompression = ModelImporterMeshCompression.High;
        model.importTangents = ModelImporterTangents.None;
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
        model.animationCompression = ModelImporterAnimationCompression.Optimal;
        model.indexFormat = ModelImporterIndexFormat.UInt16;
    }
    public void OnPreprocessModel()
    {
        CfgSceneModel();
    }

    //    private static readonly string[] s_BindPoints = new string[]
    //{
    //    "node_head",
    //    "node_arm",
    //    "node_leg",
    //};

    //    private Animator m_Animator = null;
    //    private void Awake()
    //    {
    //        m_Animator = GetComponent<Animator>();
    //        Assert.IsNotNull(m_Animator);
    //        foreach (var bindPoint in s_BindPoints)
    //        {
    //            GameObject bindPointObj = new GameObject(bindPoint);
    //            bindPointObj.layer = gameObject.layer;
    //            bindPointObj.transform.SetParent(transform);
    //        }
    //        m_Animator.Rebind();    // important!
    //    }

    //    public static string[] NodeNames = new string[]
    //{
    //    "node_head",
    //    "node_arm",
    //    "node_leg",
    //};

    //    void OnPreprocessModel()
    //    {
    //        ModelImporter importer = assetImporter as ModelImporter;
    //        importer.optimizeGameObjects = true;
    //        若实际骨骼中没有以下节点，可能会报错，但不影响使用
    //        importer.extraExposedTransformPaths = EWEquipmentsBase.nodeNames;
    //    }
    ////模型导入之前调用
    //public void OnPostprocessModel(GameObject go)
    //{
    //    // for skeleton animations.

    //    List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(go));
    //    if (animationClipList.Count == 0)
    //    {
    //        AnimationClip[] objectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
    //        animationClipList.AddRange(objectList);
    //    }

    //    foreach (AnimationClip theAnimation in animationClipList)
    //    {

    //        try
    //        {
    //            //去除scale曲线
    //            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
    //            {
    //                string name = theCurveBinding.propertyName.ToLower();
    //                if (name.Contains("scale"))
    //                {
    //                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
    //                }
    //            }

    //            //浮点数精度压缩到f3
    //            AnimationClipCurveData[] curves = null;
    //            curves = AnimationUtility.GetAllCurves(theAnimation);
    //            Keyframe key;
    //            Keyframe[] keyFrames;
    //            for (int ii = 0; ii < curves.Length; ++ii)
    //            {
    //                AnimationClipCurveData curveDate = curves[ii];
    //                if (curveDate.curve == null || curveDate.curve.keys == null)
    //                {
    //                    //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
    //                    continue;
    //                }
    //                keyFrames = curveDate.curve.keys;
    //                for (int i = 0; i < keyFrames.Length; i++)
    //                {
    //                    key = keyFrames[i];
    //                    key.value = float.Parse(key.value.ToString("f3"));
    //                    key.inTangent = float.Parse(key.inTangent.ToString("f3"));
    //                    key.outTangent = float.Parse(key.outTangent.ToString("f3"));
    //                    keyFrames[i] = key;
    //                }
    //                curveDate.curve.keys = keyFrames;
    //                theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
    //            }
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", assetPath, e));
    //        }
    //    }
    //}
    ////    //纹理导入之前调用，针对入到的纹理进行设置
    //    public void OnPreprocessTexture()
    //    {
    //        Debug.Log("OnPreProcessTexture=" + this.assetPath);
    //        TextureImporter impor = this.assetImporter as TextureImporter;
    //        impor.textureFormat = TextureImporterFormat.ARGB32;
    //        impor.maxTextureSize = 512;
    //        impor.textureType = TextureImporterType.Default;
    //        impor.mipmapEnabled = false;

    //    }
    //    public void OnPostprocessTexture(Texture2D tex)
    //    {
    //        Debug.Log("OnPostProcessTexture=" + this.assetPath);
    //    }

    public void OnPreprocessTexture()
    {
        if (this.assetPath.Contains("Assets/Arts/Effect"))
        {
            if (!this.assetPath.Contains("custom"))
            {
                TextureImporter impor = this.assetImporter as TextureImporter;
                impor.textureType = TextureImporterType.Default;
                ResFormatDeal.FormatTexture(ref impor, TextureImporterFormat.ASTC_RGBA_12x12);
                Debug.Log(" improt Texture path " + this.assetPath);
                AssetDatabase.SaveAssets();
            }
        }
        else if (this.assetPath.Contains("Assets/Arts/UI"))
        {

            if (!this.assetPath.Contains("custom"))
            {
                TextureImporter impor = this.assetImporter as TextureImporter;
                impor.textureType = TextureImporterType.Sprite;
                if (!this.assetPath.Contains("pkm"))
                {
                    ResFormatDeal.FormatTexture(ref impor, TextureImporterFormat.ASTC_RGBA_8x8);
                }
                else
                {
                    ResFormatDeal.FormatTexture(ref impor, TextureImporterFormat.ASTC_RGBA_12x12);
                }
                Debug.Log(" improt Texture path " + this.assetPath);
                AssetDatabase.SaveAssets();
            }

        }
        else if (this.assetPath.Contains("Assets/Arts/Character"))
        {
            if (!this.assetPath.Contains("custom"))
            {
                TextureImporter impor = this.assetImporter as TextureImporter;
                ResFormatDeal.FormatTexture(ref impor);
                Debug.Log(" improt Texture path " + this.assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        else if (this.assetPath.Contains("Assets/Arts/Scene"))
        {
            if (!this.assetPath.Contains("custom"))
            {
                TextureImporter impor = this.assetImporter as TextureImporter;
                ResFormatDeal.FormatTexture(ref impor);
                Debug.Log(" improt Texture path " + this.assetPath);
                AssetDatabase.SaveAssets();
            }
        }
        else if (this.assetPath.Contains("Resources/UI"))
        {
            if (!this.assetPath.Contains("custom"))
            {
                TextureImporter impor = this.assetImporter as TextureImporter;
                impor.textureType = TextureImporterType.Sprite;
                ResFormatDeal.FormatTexture(ref impor, TextureImporterFormat.ASTC_RGBA_8x8);
                AssetDatabase.SaveAssets();
            }
        }
    }
    //    public void OnPostprocessAudio(AudioClip clip)
    //    {

    //    }
    //    public void OnPreprocessAudio()
    //    {
    //        AudioImporter audio = this.assetImporter as AudioImporter;
    //        audio.format = AudioImporterFormat.Compressed;
    //    }
    //    //所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的
    public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // for skeleton animations.
        //foreach (string str in importedAsset)
        //{
        //    if (str.EndsWith(".anim"))
        //    {


        //        AnimationClip theAnimation = (AnimationClip)AssetDatabase.LoadAssetAtPath(str, typeof(AnimationClip));
        //        try
        //        {
        //            //去除scale曲线
        //            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
        //            {
        //                string name = theCurveBinding.propertyName.ToLower();
        //                if (name.Contains("scale"))
        //                {
        //                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
        //                }
        //            }

        //            //浮点数精度压缩到f3
        //            AnimationClipCurveData[] curves = null;
        //            curves = AnimationUtility.GetAllCurves(theAnimation);
        //            Keyframe key;
        //            Keyframe[] keyFrames;
        //            for (int ii = 0; ii < curves.Length; ++ii)
        //            {
        //                AnimationClipCurveData curveDate = curves[ii];
        //                if (curveDate.curve == null || curveDate.curve.keys == null)
        //                {
        //                    Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
        //                    continue;
        //                }
        //                keyFrames = curveDate.curve.keys;
        //                for (int i = 0; i < keyFrames.Length; i++)
        //                {
        //                    key = keyFrames[i];
        //                    key.value = float.Parse(key.value.ToString("f3"));
        //                    key.inTangent = float.Parse(key.inTangent.ToString("f3"));
        //                    key.outTangent = float.Parse(key.outTangent.ToString("f3"));
        //                    keyFrames[i] = key;
        //                }
        //                curveDate.curve.keys = keyFrames;
        //                theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
        //            }
        //        }

        //        catch (System.Exception e)
        //        {
        //            Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", str, e));
        //        }
        //    }
        //    AssetDatabase.SaveAssets();
        //}
        AssetDatabase.Refresh();
    }
    //        Debug.Log("OnPostprocessAllAssets");
    //        foreach (string str in importedAsset)
    //        {
    //            Debug.Log("importedAsset = " + str);
    //        }
    //        foreach (string str in deletedAssets)
    //        {
    //            Debug.Log("deletedAssets = " + str);
    //        }
    //        foreach (string str in movedAssets)
    //        {
    //            Debug.Log("movedAssets = " + str);
    //        }
    //        foreach (string str in movedFromAssetPaths)
    //        {
    //            Debug.Log("movedFromAssetPaths = " + str);
    //        }
    //    }

}
