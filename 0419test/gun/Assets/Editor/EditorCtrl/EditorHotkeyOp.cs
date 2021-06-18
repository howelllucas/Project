using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorHotkeyOp : EditorWindow
{
    private static EditorHotkeyOp Instance;

    private const string LastScenePrefKey = "Pt3.LastSceneOpen";

    [MenuItem("PT3/Open Main Scene #s")]
    public static void OpenFirstScene()
    {
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        var currentScene = EditorSceneManager.GetActiveScene().path;
#else
            var currentScene = EditorApplication.currentScene;
#endif
        var mainScene = "Assets/Scenes/StartScene.unity";
        if (mainScene != currentScene)
            EditorPrefs.SetString(LastScenePrefKey, currentScene);

        Debug.Log("Open Start Game Scene!");
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        EditorSceneManager.OpenScene(mainScene);
#else
            EditorApplication.OpenScene(mainScene);
#endif
    }

    [MenuItem("PT3/Open City Scene #c")]
    public static void OpenCityScene()
    {
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        var currentScene = EditorSceneManager.GetActiveScene().path;
#else
            var currentScene = EditorApplication.currentScene;
#endif
        var mainScene = "Assets/Scenes/CityScene.unity";
        if (mainScene != currentScene)
            EditorPrefs.SetString(LastScenePrefKey, currentScene);

        Debug.Log("Open City Scene!");
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        EditorSceneManager.OpenScene(mainScene);
#else
            EditorApplication.OpenScene(mainScene);
#endif
    }

    [MenuItem("PT3/Open Battle Scene #b")]
    public static void OpenBattleScene()
    {
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        var currentScene = EditorSceneManager.GetActiveScene().path;
#else
            var currentScene = EditorApplication.currentScene;
#endif
        var mainScene = "Assets/Scenes/BattleScene.unity";
        if (mainScene != currentScene)
            EditorPrefs.SetString(LastScenePrefKey, currentScene);

        Debug.Log("Open City Scene!");
#if UNITY_5 || UNITY_2017_1_OR_NEWER
        EditorSceneManager.OpenScene(mainScene);
#else
            EditorApplication.OpenScene(mainScene);
#endif
    }

    [MenuItem("PT3/Open Last Scene #l")]
    public static void OpenLastScene()
    {
        var lastScene = EditorPrefs.GetString(LastScenePrefKey);
        Debug.Log("Open Last Game Scene!");
        if (!string.IsNullOrEmpty(lastScene))
        {

#if UNITY_5 || UNITY_2017_1_OR_NEWER
            EditorSceneManager.OpenScene(lastScene);
#else
                EditorApplication.OpenScene(lastScene);
#endif

        }
        else
        {
            Debug.LogWarning("Not found last scene!");
        }
    }

    [MenuItem("PT3/清空Persistent")]
    public static void ClearPersistent()
    {
        //if (GUILayout.Button("清空Persistent", GUILayout.Width(100), GUILayout.Height(20)))
        {
            Directory.Delete(Application.persistentDataPath, true);

            Debug.LogWarning("ClearPersistent!");
        }
    }
}
