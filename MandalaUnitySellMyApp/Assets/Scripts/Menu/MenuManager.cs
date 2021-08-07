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
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
  * Scene:All
  * Object:Canvas
  * Description: Class for handling PopUps and UI
  **/
public class MenuManager : MonoBehaviour
{
	
    public Menu currentMenu;
    public Animator transitionScreen;
    public GameObject[] disabledObjects;

    [Header("MainSceneButtons")]
    public RectTransform musicUI;
    //    public RectTransform removeAdsUI;
    //    public RectTransform crossPromoUI;
    //    public RectTransform unlockAllUI;
    public RectTransform soundUI;

    [Header("Loading")]
    public GameObject loading;
    public AdvancedMobilePaint.PaintUndoManager undoManager;



    Menu currentPopUpMenu;
    //	[HideInInspector]
    //	public Animator openObject;
    GameObject ratePopUp, crossPromotionInterstitial;
    Animator playAnim;
    Coroutine pauseCoroutine;

    //    public GameObject nativeAdScroll;

    void Start()
    {
        Input.multiTouchEnabled = true;
        if (Application.loadedLevelName == "MainScene")
        {
            GlobalVariables.mainSceneLoadCount += 1;
            crossPromotionInterstitial = GameObject.Find("PopUps/PopUpInterstitial");
            ratePopUp = GameObject.Find("PopUps/PopUpRate");
        }

        if (Application.loadedLevelName == "Gameplay")
            Input.multiTouchEnabled = true;
        else
            Input.multiTouchEnabled = false;

        if (disabledObjects != null)
        {
            for (int i = 0; i < disabledObjects.Length; i++)
                disabledObjects[i].SetActive(false);
        }

//        if (disabledObjects != null)
//        {
//            for (int i = 0; i < disabledObjects.Length; i++)
//            {
//                if (!GlobalVariables.removeAds)
//                {
//                    if (disabledObjects[i].name == "PopUpDialogSavePicture" || disabledObjects[i].name == "PopUpDialog")
//                        disabledObjects[i].transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, 100); //Pozicionira popUp-ove
//                }
//            }
//        }
		
        //		if(Application.loadedLevelName!= "MapScene")
        //			ShowMenu(currentMenu.gameObject);	
		
        if (Application.loadedLevelName == "MainScene")
        {
            RepositionUIButtons();

            SoundManager.Instance.Stop_GameplayMusic();	//TODO Otkomentarisati kad se ubaci muzika
            SoundManager.Instance.Play_MenuMusic();

            if (PlayerPrefs.HasKey("alreadyRated"))
            {
                Rate.alreadyRated = PlayerPrefs.GetInt("alreadyRated");
            }
            else
            {
                Rate.alreadyRated = 0;
            }
			
            if (Rate.alreadyRated == 0)
            {
                Rate.appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
//				Debug.Log("appStartedNumber "+Rate.appStartedNumber);
				
                if (Rate.appStartedNumber >= 3)
                {
                    Rate.appStartedNumber = 0;
                    PlayerPrefs.SetInt("appStartedNumber", Rate.appStartedNumber);
                    PlayerPrefs.Save();
                    ShowPopUpMenu(ratePopUp);
					
                }
                else
                {
                    if (!GlobalVariables.removeAds)
                    {
                        if (GlobalVariables.mainSceneLoadCount > 1)
                            ShowStartInterstitial();
                    }
                }
            }
            else
            {
                if (!GlobalVariables.removeAds)
                {
                    if (GlobalVariables.mainSceneLoadCount > 1)
                        ShowStartInterstitial();
                }
            }			
			
        }

        if (Application.loadedLevelName == "Gameplay")
        {
            SoundManager.Instance.Stop_MenuMusic();
            SoundManager.Instance.Play_GameplayMusic();
        }
		
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            for (int i = disabledObjects.Length - 1; i >= 0; i--)
            {
                if (disabledObjects[i].activeInHierarchy)
                {
                    if (disabledObjects[i].name == "PopUpMessage")
                    {
                        disabledObjects[i].transform.Find("AnimationHolder/Body/ButtonsHolder/ButtonOk").GetComponent<Button>().onClick.Invoke();
                    }
                    else if (disabledObjects[i].name == "PopUpDialogSavePicture" || disabledObjects[i].name == "PopUpDialog")
                    {
                        disabledObjects[i].transform.Find("AnimationHolder/Body/ButtonsHolder/ButtonNo").GetComponent<Button>().onClick.Invoke();
                    }
                    else if (disabledObjects[i].name == "SelectMandala")
                    {
                        GameObject.Find("SelectionManager").GetComponent<SelectionManager1>().backButton.GetComponent<Button>().onClick.Invoke();
                    }
                    else
                    {
                        disabledObjects[i].transform.Find("AnimationHolder/Body/ButtonExit").GetComponent<Button>().onClick.Invoke();
                    }
                    goto Exit;
                }
            }

            if (Application.loadedLevelName == "Selection")
            {
                GameObject.Find("SelectionManager").GetComponent<SelectionManager1>().homeButton.GetComponent<Button>().onClick.Invoke();
                goto Exit;
            }

            if (Application.loadedLevelName == "Gameplay")
            {
                GameObject.Find("Canvas/UI/AnimationHolder/Home").GetComponent<Button>().onClick.Invoke();
            }

            if (Application.loadedLevelName == "MainScene")
            {				
                if (GlobalVariables.removeAds)
                    Application.Quit();
                else
                    AdsManager.Instance.ShowInterstitial(GlobalVariables.exitInterstitialID);					
            }

            Exit:
            Debug.Log("Back pressed");
        }
    }

    /// <summary>
    /// Funkcija koja pali(aktivira) objekat
    /// </summary>
    /// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se upali</param>
    public void EnableObject(GameObject gameObject)
    {
		
        if (gameObject != null)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Funkcija koja gasi objekat
    /// </summary>
    /// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se ugasi</param>
    public void DisableObject(GameObject gameObject)
    {
		
        if (gameObject != null)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// F-ja koji poziva ucitavanje Scene
    /// </summary>
    /// <param name="levelName">Level name.</param>
    public void LoadScene(string levelName)
    {
        if (levelName != "")
        {
            try
            {
                Application.LoadLevel(levelName);
            }
            catch (System.Exception e)
            {
                Debug.Log("Can't load scene: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Can't load scene: Level name to set");
        }
    }

    /// <summary>
    /// F-ja koji poziva asihrono ucitavanje Scene
    /// </summary>
    /// <param name="levelName">Level name.</param>
    public void LoadSceneAsync(string levelName)
    {
        if (levelName != "")
        {
            try
            {
                Application.LoadLevelAsync(levelName);
            }
            catch (System.Exception e)
            {
                Debug.Log("Can't load scene: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Can't load scene: Level name to set");
        }
    }

    /// <summary>
    /// Pkrece animaciju tranzicije koja na kraju ucitava novi nivo.
    /// </summary>
    /// <param name="levelName">Level name.</param>
    public void LoadSceneWithTransition(string levelName)
    {
        GlobalVariables.sceneToLoad = levelName;
        transitionScreen.Play("LoadScene");
    }

    public void SetGameMode(string s)
    {
        GlobalVariables.gameMode = s;
    }

    /// <summary>
    /// Funkcija za prikaz Menu-ja koji je pozvan kao Menu
    /// </summary>
    /// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
    public void ShowMenu(GameObject menu)
    {
        if (currentMenu != null)
            currentMenu.IsOpen = false;
		
        menu.gameObject.SetActive(true);
        currentMenu = menu.GetComponent<Menu>();
        currentMenu.IsOpen = true;
		
    }

    /// <summary>
    /// Funkcija za zatvaranje Menu-ja koji je pozvan kao Meni
    /// </summary>
    /// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
    public void CloseMenu(GameObject menu)
    {
        if (menu != null)
        {
            menu.GetComponent<Menu>().IsOpen = false;
            menu.SetActive(false);
        }
    }

    /// <summary>
    /// Funkcija za prikaz Menu-ja koji je pozvan kao PopUp-a
    /// </summary>
    /// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
    public void ShowPopUpMenu(GameObject menu)
    {
        menu.gameObject.SetActive(true);
        currentPopUpMenu = menu.GetComponent<Menu>();
        currentPopUpMenu.IsOpen = true;
    }

    /// <summary>
    /// Funkcija za zatvaranje Menu-ja koji je pozvan kao PopUp-a, poziva inace coroutine-u, ima delay zbog animacije odlaska Menu-ja
    /// </summary>
    /// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
    public void ClosePopUpMenu(GameObject menu)
    {
        StartCoroutine("HidePopUp", menu);
    }

    /// <summary>
    /// Couorutine-a za zatvaranje Menu-ja koji je pozvan kao PopUp-a
    /// </summary>
    /// /// <param name="menu">Game object koji se prosledjuje, mora imati na sebi skriptu Menu.</param>
    IEnumerator HidePopUp(GameObject menu)
    {
        menu.GetComponent<Menu>().IsOpen = false;
        yield return new WaitForSeconds(1.2f);

        menu.SetActive(false);
    }

    /// <summary>
    /// Funkcija za prikaz poruke preko Log-a, prilikom klika na dugme
    /// </summary>
    /// /// <param name="message">poruka koju treba prikazati.</param>
    public void ShowMessage(string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// Funkcija za prikaz CrossPromotion StartInterstitial-a
    /// </summary>
    public void ShowStartInterstitial()
    {
//        CrossPromotion.Instance.InitializeStartInterstitial();
//        ShowPopUpMenu(crossPromotionInterstitial);
    }

    /// <summary>
    /// Funkcija za prikaz CrossPromotion ExitInterstitial-a
    /// </summary>
    public void ShowExitInterstitial()
    {
//        CrossPromotion.Instance.InitializeExitInterstitial();
//        ShowPopUpMenu(crossPromotionInterstitial);
    }

    /// <summary>
    /// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
    /// </summary>
    /// <param name="messageTitleText">naslov koji treba prikazati.</param>
    /// <param name="messageText">custom poruka koju treba prikazati.</param>
    public void ShowPopUpMessage(string messageTitleText, string messageText)
    {
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = messageTitleText;
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = messageText;
        ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);

    }

    /// <summary>
    /// Funkcija koja podesava naslov CustomMessage-a, i ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageCustomMessageText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
    /// </summary>
    /// <param name="messageTitleText">naslov koji treba prikazati.</param>
    public void ShowPopUpMessageTitleText(string messageTitleText)
    {
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = messageTitleText;
    }

    /// <summary>
    /// Funkcija koja podesava poruku CustomMessage, i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageTitleText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
    /// </summary>
    /// <param name="messageText">custom poruka koju treba prikazati.</param>
    public void ShowPopUpMessageCustomMessageText(string messageText)
    {
        transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = messageText;		
        ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
    }

    /// <summary>
    /// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
    /// </summary>
    /// <param name="dialogTitleText">naslov koji treba prikazati.</param>
    /// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
    public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
    {
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = dialogTitleText;
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = dialogMessageText;
        ShowPopUpMenu(transform.Find("PopUps/PopUpDialog").gameObject);
    }

    /// <summary>
    /// Funkcija koja podesava naslov dialoga, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogCustomMessageText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
    /// </summary>
    /// <param name="dialogTitleText">naslov koji treba prikazati.</param>
    public void ShowPopUpDialogTitleText(string dialogTitleText)
    {
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = dialogTitleText;
    }

    /// <summary>
    /// Funkcija koja podesava poruku dialoga i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogTitleText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
    /// </summary>
    /// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
    public void ShowPopUpDialogCustomMessageText(string dialogMessageText)
    {
        transform.Find("PopUps/PopUpDialog/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = dialogMessageText;		
        ShowPopUpMenu(transform.Find("PopUps/PopUpDialog").gameObject);
    }

    //TODO Otkomentarisati kad stigne zvuk

    #region Zvuk

    public void ButtonClickSound()
    {
        SoundManager.Instance.Play_ButtonClick();	
    }

    #endregion

    /// <summary>
    /// Menja polozaj dugmeta na sceni. Pozvati nakon sto se setuje RemoveAds.
    /// </summary>
    public void RepositionUIButtons()
    {
//        if (GlobalVariables.removeAds)
//        {
////			testRemoveAds.SetActive(false);
//
//            if (GlobalVariables.unlockAll)
//            {
////				testUnlockAll.SetActive(false);
//
//                musicUI.anchoredPosition = new Vector2(-225, 15);
//                removeAdsUI.gameObject.SetActive(false);
//                crossPromoUI.anchoredPosition = new Vector2(0, -95);
//                unlockAllUI.gameObject.SetActive(false);
//                soundUI.anchoredPosition = new Vector2(225, 15);
//            }
//            else
//            {
//                musicUI.anchoredPosition = new Vector2(-300, 54);
//                removeAdsUI.gameObject.SetActive(false);
//                crossPromoUI.anchoredPosition = new Vector2(-150, -95);
//                unlockAllUI.anchoredPosition = new Vector2(150, -95);
//                soundUI.anchoredPosition = new Vector2(300, 54);
//            }
//        }
//        else
//        {
//            if (GlobalVariables.unlockAll)
//            {
////				testUnlockAll.SetActive(false);
//
//                musicUI.anchoredPosition = new Vector2(-300, 54);
//                removeAdsUI.anchoredPosition = new Vector2(-150, -95);
//                crossPromoUI.anchoredPosition = new Vector2(150, -95);
//                unlockAllUI.gameObject.SetActive(false);
//                soundUI.anchoredPosition = new Vector2(300, 54);
//            }
//            else
//            {
//                musicUI.anchoredPosition = new Vector2(-300, 54);
//                removeAdsUI.anchoredPosition = new Vector2(-160, -80);
//                crossPromoUI.anchoredPosition = new Vector2(0, -150);
//                unlockAllUI.anchoredPosition = new Vector2(160, -80);
//                soundUI.anchoredPosition = new Vector2(300, 54);
//            }
//        }
    }

    public void PopUpDialogYesButton()
    {
        if (!GlobalVariables.removeAds)
            AdsManager.Instance.ShowInterstitial(GlobalVariables.homeResetActionID);
        if (GameObject.Find("Canvas/PopUps/PopUpDialog/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text == "Go to Home Screen?")
        {
            LoadSceneWithTransition("MainScene");
        }
        else
        {
            GameObject.Find("Canvas/SubMenus").GetComponent<SubMenusController>().DeleteButton();
            ;
            ClosePopUpMenu(GameObject.Find("Canvas/PopUps/PopUpDialog"));
        }
    }

    public void ActionHomeReset()
    {
        if (GlobalVariables.removeAds)
            return;
        AdsManager.Instance.ShowInterstitial(GlobalVariables.homeResetActionID);		
    }

    public void ActionSavePicture()
    {
        if (GlobalVariables.removeAds)
            return;
        AdsManager.Instance.ShowInterstitial(GlobalVariables.homeResetActionID);
    }

    public void StartUndoCoroutine()
    {
        if (!loading.activeInHierarchy)
            StartCoroutine("UndoCoroutine");
    }

    IEnumerator UndoCoroutine()
    {
        loading.SetActive(true);
        yield return new WaitForSeconds(0.02f);
        undoManager.UndoLastStep();
        while (undoManager.doingWork)
            yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);
        loading.SetActive(false);
    }


    //    public void HideAdScrollRect()
    //    {
    //        if (nativeAdScroll != null)
    //            nativeAdScroll.GetComponent<FacebookNativeAd>().CancelAndHideAd();
    //    }
}

















