/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;


public class FillStickerSlidebar : EditorWindow
{

    Texture sourceTex;
    GameObject prefab;
    Transform stickersHolder;

    Object[] obj;
    GameObject clone;
    Sprite sprite;

    [MenuItem("Mandala/Fill Stickers Slidebar")]
    public static void ShowWindow()
    {
        GetWindow<FillStickerSlidebar>("Fill Stickers");
    }

    void OnGUI()
    {
        sourceTex = (Texture)EditorGUILayout.ObjectField("Texture ", sourceTex, typeof(Texture), false);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab ", prefab, typeof(GameObject));
        stickersHolder = (Transform)EditorGUILayout.ObjectField("Stickers Holder ", stickersHolder, typeof(Transform));

        GUILayout.Space(20);

        if (GUILayout.Button("Fill from texture", EditorStyles.toolbarButton))
        {
            Fill();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
        {
            Clear();
        }
    }

    void Fill()
    {
        obj = null;
        obj = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(sourceTex));

        stickersHolder.GetComponent<RectTransform>().sizeDelta += new Vector2(150, 0);
        for (int i = 0; i < obj.Length; i++)
        {
            stickersHolder.GetComponent<RectTransform>().sizeDelta += new Vector2(150, 0);
            sprite = obj[i] as Sprite;
            clone = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
            clone.transform.SetParent(stickersHolder);
            clone.transform.SetAsLastSibling();
            clone.transform.localScale = Vector3.one;
            clone.transform.localPosition = Vector3.zero;
            clone.GetComponent<Image>().sprite = sprite;
            clone.name = "StickerButton";
        }
        stickersHolder.GetComponent<RectTransform>().sizeDelta += new Vector2(150, 0);
    }

    void Clear()
    {
        while (stickersHolder.childCount > 0)
        {
            DestroyImmediate(stickersHolder.GetChild(0).gameObject);
        }
        stickersHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 114.5f);
    }
}












