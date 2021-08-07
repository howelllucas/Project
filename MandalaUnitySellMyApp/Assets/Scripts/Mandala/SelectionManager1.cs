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
using System.Collections;

/// <summary>
/// <para>Scene: Selection</para>
/// <para>Object: SelectionManager</para>
/// <para>Description: Mandala selection</para>
/// </summary>
public class SelectionManager1 : MonoBehaviour
{


    [Header("Menus")]
    public MenuManager canvas;
    public GameObject categoryMenu;
    public GameObject mandalaMenu;
    public GameObject backButton;
    public GameObject homeButton;

    //	[Header("NativeAds")]
    //	public ScrollRectSnapController categoryScrollNativeAd;
    //	public ScrollRectSnapController mandalaScrollNativeAd;

    int skipped;
    int randomCategory, randomMandala;

    public static int currentCategory;
    public static Object[] categoryObjects;
    public static Texture2D[] category;

    void Awake()
    {
        backButton.SetActive(false);
        homeButton.SetActive(true);
    }

    void Start()
    {
        if (GlobalVariables.gameMode == "Magic")
        {
            randomCategory = (int)Random.Range(0, 5);
            randomMandala = (int)Random.Range(0, 10);

//			Debug.Log("Randomization: " + randomCategory + " and " + randomMandala);

            LoadCategoryForMagicMode(randomCategory);
            GlobalVariables.selectedTexture = category[randomMandala];

            Application.LoadLevel("Gameplay");
        }
        canvas.ShowMenu(categoryMenu);
        //categoryScrollNativeAd.LoadCurrentAdInScroll(1f);
    }

    public void SelectCategory(int index)
    {
//		selectedCategory = index;
        LoadCategory(index);
//		UpdateMandalaImages();
//		categoryScrollNativeAd.CancelAllAds();
        canvas.StopCoroutine("HidePopUp");
        canvas.ShowPopUpMenu(mandalaMenu);
//		Debug.Log("Mandala menu shown");
        canvas.ClosePopUpMenu(categoryMenu);
//		mandalaScrollNativeAd.LoadCurrentAdInScroll(0.2f);
        backButton.SetActive(true);
        homeButton.SetActive(false);
    }

    public void UpdateMandalaImages()
    {
        skipped = 0;
        for (int i = 0; i < mandalaMenu.transform.GetChild(0).GetChild(0).childCount; i++)
        {
//			Debug.Log("Index i: " +i + " Skipped " + skipped);
            if (mandalaMenu.transform.GetChild(0).GetChild(0).GetChild(i).name == "Mandala")
                mandalaMenu.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetComponent<RawImage>().texture = category[i - skipped];
            else
            {
                skipped += 1;
            }
        }
    }

    public void SelectTexture(int index)
    {
        GlobalVariables.selectedTexture = category[index];
    }

    public void BackButtonPressed()
    {
//		mandalaScrollNativeAd.CancelAllAds();
        canvas.StopCoroutine("HidePopUp");
        canvas.ShowPopUpMenu(categoryMenu);
        canvas.ClosePopUpMenu(mandalaMenu);
//		categoryScrollNativeAd.LoadCurrentAdInScroll(0.2f);
        backButton.SetActive(false);
        homeButton.SetActive(true);
    }

    void LoadCategoryForMagicMode(int index)
    {
//        switch (index)
//        {
//            case 0:
//                categoryObjects = Resources.LoadAll("1", typeof(Texture2D));
//                break;
//            case 1:
//                categoryObjects = Resources.LoadAll("2", typeof(Texture2D));
//                break;
//            case 2:
//                categoryObjects = Resources.LoadAll("3", typeof(Texture2D));
//                break;
//            case 3:
//                categoryObjects = Resources.LoadAll("4", typeof(Texture2D));
//                break;
//            case 4:
//                categoryObjects = Resources.LoadAll("5", typeof(Texture2D));
//                break;
//            default:
//                break;
//        }
		
        categoryObjects = Resources.LoadAll(index.ToString(), typeof(Texture2D));

        category = new Texture2D[categoryObjects.Length];
        for (int i = 0; i < categoryObjects.Length; i++)
        {
            category[i] = (Texture2D)categoryObjects[i];
        }
        Resources.UnloadUnusedAssets();
    }

    void LoadCategory(int index)
    {
//		Debug.Log("Load category");
        currentCategory = index;
        GameObject.Find("Canvas/Transition").GetComponent<Animator>().Play("LoadCategory");

    }
}




















