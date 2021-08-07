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

/**
  * Scene:Splash
  * Object:Main Camera
  * Description: F-ja zaduzena za ucitavanje MainScene-e, kao i vizuelni prikaz inicijalizaije CrossPromotion-e i ucitavanja scene
  **/
public class SplashScene : MonoBehaviour
{
	
    int appStartedNumber;
    AsyncOperation progress = null;
    GameObject progressBar;
    Image[] loadingObjects;
    int myProgress = 0;
    string sceneToLoad;
    // Use this for initialization
    void Start()
    {
//		if(PlayerPrefs.HasKey("TutorialCompleted"))
//		{
        sceneToLoad = "MainScene";
//		}
//		else
//			sceneToLoad = "TutorialLevel";
		
        progressBar = GameObject.Find("ProgressBar");
        loadingObjects = new Image[progressBar.transform.childCount];

        for (int i = 0; i < progressBar.transform.childCount; i++)
        {
            loadingObjects[i] = progressBar.transform.GetChild(i).gameObject.GetComponent<Image>();
            loadingObjects[i].enabled = false;
        }

        if (PlayerPrefs.HasKey("appStartedNumber"))
        {
            appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
        }
        else
        {
            appStartedNumber = 0;
        }
        appStartedNumber++;
        PlayerPrefs.SetInt("appStartedNumber", appStartedNumber);
        StartCoroutine(LoadScene());
    }

    /// <summary>
    /// Coroutine koja ceka dok se ne inicijalizuje CrossPromotion, menja progres ucitavanja CrossPromotion-a, kao i progres ucitavanje scene, i taj progres se prikazuje u Update-u
    /// </summary>
    IEnumerator LoadScene()
    {
		
        yield return new WaitForSeconds(1f);

        while (myProgress < 3)
        {
            loadingObjects[myProgress].enabled = true;
            myProgress += 1;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);

        while (myProgress < 6)
        {
            loadingObjects[myProgress].enabled = true;
            myProgress += 1;
            yield return new WaitForSeconds(0.3f);
        }
            
        progress = Application.LoadLevelAsync(sceneToLoad);
		
        yield return progress;
		
    }

    void Update()
    {
        if (progress != null && progress.progress > 0.7f)
        {
            loadingObjects[6].enabled = true;
        }
		
    }
}
