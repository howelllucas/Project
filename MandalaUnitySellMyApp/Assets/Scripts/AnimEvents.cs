/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

/// <summary>
/// GameObject: Objects with animator component.
/// Description: Contains methods called from animations.
/// </summary>
public class AnimEvents : MonoBehaviour
{

    // Loads new scene from transition
    public void LoadSceneFromAnim()
    {
        Application.LoadLevel(GlobalVariables.sceneToLoad);
    }

    // Loads chosen category in Selection scene
    public void LoadCategory()
    {
//        switch (SelectionManager1.currentCategory)
//        {
//            case 0:
//                SelectionManager1.categoryObjects = Resources.LoadAll("0", typeof(Texture2D));
//                break;
//            case 1:
//                SelectionManager1.categoryObjects = Resources.LoadAll("1", typeof(Texture2D));
//                break;
//            case 2:
//                SelectionManager1.categoryObjects = Resources.LoadAll("2", typeof(Texture2D));
//                break;
//            case 3:
//                SelectionManager1.categoryObjects = Resources.LoadAll("3", typeof(Texture2D));
//                break;
//            case 4:
//                SelectionManager1.categoryObjects = Resources.LoadAll("4", typeof(Texture2D));
//                break;
//            default:
//                break;
//        }

        SelectionManager1.categoryObjects = Resources.LoadAll(SelectionManager1.currentCategory.ToString(), typeof(Texture2D));
		
        SelectionManager1.category = new Texture2D[SelectionManager1.categoryObjects.Length];
        for (int i = 0; i < SelectionManager1.categoryObjects.Length; i++)
        {
            SelectionManager1.category[i] = (Texture2D)SelectionManager1.categoryObjects[i];
        }

        GameObject.Find("SelectionManager").GetComponent<SelectionManager1>().UpdateMandalaImages();
        Resources.UnloadUnusedAssets();
        GetComponent<Animator>().Play("Default");
    }
}
