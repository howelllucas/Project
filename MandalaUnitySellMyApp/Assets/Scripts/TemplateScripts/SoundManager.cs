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

/**
  * Scene:All
  * Object:SoundManager
  * Description: Skripta zaduzena za zvuke u apliakciji, njihovo pustanje, gasenje itd...
  **/
public class SoundManager : MonoBehaviour {

	public static int musicOn = 1;
	public static int soundOn = 1;
	public static bool forceTurnOff = false;

	public AudioSource buttonClick;
	public AudioSource menuMusic;
	public AudioSource gameplayMusic;
	public AudioSource takePhoto;
	



	static SoundManager instance;

	public static SoundManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
			}

			return instance;
		}
	}

	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);

		if(PlayerPrefs.HasKey("SoundOn"))
		{
			musicOn = PlayerPrefs.GetInt("MusicOn");
			soundOn = PlayerPrefs.GetInt("SoundOn");
		}

		Screen.sleepTimeout = SleepTimeout.NeverSleep; 
	}

	public void Play_ButtonClick()
	{
		if(buttonClick.clip != null && soundOn == 1)
			buttonClick.Play();
	}

	public void Play_MenuMusic()
	{
		if(menuMusic.clip != null && musicOn == 1)
			menuMusic.Play();
	}

	public void Stop_MenuMusic()
	{
		if(menuMusic.clip != null && musicOn == 1)
			menuMusic.Stop();
	}

	public void Play_GameplayMusic()
	{
		if(gameplayMusic.clip != null && musicOn == 1)
		{
			gameplayMusic.Play();
		}
	}

	public void Stop_GameplayMusic()
	{
		if(gameplayMusic.clip != null && musicOn == 1)
		{
			gameplayMusic.Stop();
		}
	}

	public void Play_TakePhotoSound()
	{
		if (takePhoto.clip != null && soundOn == 1)
			takePhoto.Play ();

	}

	/// <summary>
	/// Corutine-a koja za odredjeni AudioSource, kroz prosledjeno vreme, utisava AudioSource do 0, gasi taj AudioSource, a zatim vraca pocetni Volume na pocetan kako bi AudioSource mogao opet da se koristi
	/// </summary>
	/// <param name="sound">AudioSource koji treba smanjiti/param>
	/// <param name="time">Vreme za koje treba smanjiti Volume/param>
	IEnumerator FadeOut(AudioSource sound, float time)
	{
		float originalVolume = sound.volume;
		while(sound.volume != 0)
		{
			sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
			yield return null;
		}
		sound.Stop();
		sound.volume = originalVolume;
	}

	/// <summary>
	/// F-ja koja Mute-uje sve zvuke koja su deca SoundManager-a
	/// </summary>
	public void MuteAllSounds()
	{
		foreach (Transform t in transform)
		{
			t.GetComponent<AudioSource>().mute = true;
		}
	}

	/// <summary>
	/// F-ja koja Unmute-uje sve zvuke koja su deca SoundManager-a
	/// </summary>
	public void UnmuteAllSounds()
	{
		foreach (Transform t in transform)
		{
			t.GetComponent<AudioSource>().mute = false;
		}
	}
	
}
