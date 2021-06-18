using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Game
{
    public class ReplaceComponentBatch : EditorWindow
    {
        protected string srcComponentName;
        protected string dstComponentName;

        [MenuItem("批量脚本替换/通用替换")]
        public static void OpenGeneralReplaceComponentWindow()
        {
            var window = GetWindow<ReplaceComponentBatch>("通用脚本替换");
        }

        protected virtual void OnGUI()
        {
            srcComponentName = EditorGUILayout.TextField("被替换组件名", srcComponentName);
            dstComponentName = EditorGUILayout.TextField("替换组件名", dstComponentName);
            EditorGUILayout.LabelField("注:替换顺序为先添加,后删除");
            EditorGUILayout.LabelField("无法共存的脚本(如UGUI)将替换失败!");
            if (GUILayout.Button("替换"))
            {
                ReplaceComponent(srcComponentName, dstComponentName);
            }
        }

        protected void ReplaceComponent(string srcName, string dstName)
        {
            Type srcType = StringToType(srcName);
            if (srcType == null)
            {
                return;
            }
            Type dstType = StringToType(dstName);
            if (dstType == null)
            {
                return;
            }

            ReplaceComponent(srcType, dstType);
        }

        protected Type StringToType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(typeName))
                    {
                        return typeArray[j];
                    }
                }
            }

            Debug.LogError("不存在类型：" + typeName);
            return null;

        }

        protected void ReplaceComponent(Type srcType, Type dstType)
        {
            var allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");

            for (int i = 0; i < allPrefabGuids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(allPrefabGuids[i]);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var srcComps = prefab.GetComponentsInChildren(srcType, true);
                if (srcComps.Length <= 0)
                    continue;
                for (int j = 0; j < srcComps.Length; j++)
                {
                    var srcComp = srcComps[j];
                    if (!NeedReplace(srcComp))
                        continue;
                    EditorUtility.DisplayProgressBar("正在处理预制体:" + path, string.Format("第{0}个组件", j + 1), (float)j / srcComps.Length);
                    DoReplace(srcComp, dstType);
                }
                AssetDatabase.SaveAssets();
            }
            EditorUtility.ClearProgressBar();

            var curScenePath = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
            foreach (EditorBuildSettingsScene es in EditorBuildSettings.scenes)
            {
                if (es.enabled)
                {
                    string name = es.path;

                    var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(name);

                    var allGameObject = Resources.FindObjectsOfTypeAll<GameObject>();
                    for (int i = 0; i < allGameObject.Length; i++)
                    {
                        var srcComp = allGameObject[i].GetComponent(srcType);
                        if (srcComp == null)
                            continue;
                        if (!NeedReplace(srcComp))
                            continue;
                        EditorUtility.DisplayProgressBar("正在处理场景:" + es.path, string.Format("物体:{0}", allGameObject[i].name), (float)i / allGameObject.Length);
                        DoReplace(srcComp, dstType);
                    }
                    UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                    UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
                }
            }

            EditorUtility.ClearProgressBar();
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(curScenePath);
        }

        protected virtual bool NeedReplace(Component component)
        {
            var isPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(component.gameObject);
            var isCompOverride = PrefabUtility.IsAddedComponentOverride(component);

            //Debug.Log(string.Format("{0} is instance: {1} | is override: {2}", component.name, isPrefabInstance, isCompOverride));

            return isCompOverride || !isPrefabInstance;
        }

        protected virtual void DoReplace(Component srcComp, Type dstType)
        {
            var dstComp = srcComp.gameObject.GetComponent(dstType);
            if (dstComp == null)
            {
                dstComp = srcComp.gameObject.AddComponent(dstType);
                OnReplace(srcComp, dstComp);
            }
            DestroyImmediate(srcComp, true);
        }

        protected virtual void OnReplace(Component srcComponent, Component dstComponent)
        {
            var srcBhv = srcComponent as Behaviour;
            var dstBhv = dstComponent as Behaviour;
            if (srcBhv != null && dstBhv != null)
            {
                dstBhv.enabled = srcBhv.enabled;
            }
        }
    }
}