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
  * Scene: MainScene
  * Object:PopUpRate
  * Description: Skripta koja je zaduzena za PopUpRate Menu i rejtovanje aplikacije
  **/
public class Rate : MonoBehaviour {

	string rateURL;
	[Header("Linkovi za RATE")]
	public string rateUrlAndroid;
	public string rateUrlIOS;
	public string rateUrlWinPhone;
	public string rateUrlWinStore;
	public string rateUrlMAC;
	public static int appStartedNumber,alreadyRated;
	bool rateClicked = false;

	// Use this for initialization
	void Start () {

		#if UNITY_ANDROID
		rateURL = rateUrlAndroid;
		#elif UNITY_IOS
		rateURL = rateUrlIOS;
		#elif (UNITY_WP8 || UNITY_WP8_1)
		rateURL = rateUrlWinPhone;
		#elif (UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
		rateURL = rateUrlWinStore;
		#elif UNITY_STANDALONE_OSX
		rateURL = rateUrlMAC;
		#endif
	}

	/// <summary>
	/// Funkcija koja zavisno od broja(1-5), vodi(4-5) ili ne vodi(1-3) korisnika da rejtuje aplikaciju
	/// </summary>
	/// /// <param name="number">Game object koji se prosledjuje i koji treba da se upali</param>
	public void RateClicked(int number)
	{
		if(!rateClicked)
		{
			alreadyRated = 1;
			PlayerPrefs.SetInt("alreadyRated",alreadyRated);
			PlayerPrefs.Save();
			rateClicked=true;
			StartCoroutine("ActivateStars",number);
		}
	}

	/// <summary>
	/// Coroutine koja vodi korisnika da rejtuje aplikaciju,  i pamti da je korisnik rate-ovao aplikaciju, i samim tim vise ne izlazi Rate PopUpMenu
	/// </summary>
	/// <param name="number">Game object koji se prosledjuje i koji treba da se upali</param>
	IEnumerator ActivateStars(int number)
	{
		for(int i=1;i<=number;i++)
		{
			GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG"+i+"/Star"+i).GetComponent<Image>().enabled = true;
		}
		Application.OpenURL(rateURL);
		yield return new WaitForSeconds(0.5f);
		HideRateMenu(GameObject.Find("PopUpRate"));
		yield return null;
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();

	}

	/// <summary>
	/// F-ja koja prikazuje Rate Menu
	/// </summary>
	public void ShowRateMenu()
	{
		transform.GetComponent<Animator>().Play("Open");
	}

	/// <summary>
	/// F-ja koja sklanja Rate Menu
	/// </summary>
	/// <param name="menu">Game object koji se prosledjuje i koji treba da se skloni</param>
	public void HideRateMenu(GameObject menu)
	{
		GameObject.Find("Canvas").GetComponent<MenuManager>().ClosePopUpMenu(menu);
	}

	/// <summary>
	/// F-ja koja sklanja Rate Menu, i pamti da korisnik nece da rate-uje aplikaciju, i samim tim vise ne izlazi Rate PopUpMenu
	/// </summary>
	/// <param name="menu">Game object koji se prosledjuje i koji treba da se skloni</param>
	public void NoThanks()
	{

		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();
		HideRateMenu(GameObject.Find("PopUpRate"));
	}
}
