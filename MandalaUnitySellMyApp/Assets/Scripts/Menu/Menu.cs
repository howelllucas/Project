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
  * Scene: Sve
  * Object: Menu objekti
  * Description: Skripta zaduzena za Menu-je
  **/
public class Menu : MonoBehaviour {


	private Animator _animtor;

	public bool IsOpen
	{
		get
		{
			return _animtor.GetBool("IsOpen");
		}
		set
		{
			_animtor.SetBool("IsOpen", value);
		}
	}

	// Use this for initialization
	public void Awake () 
	{
		_animtor = GetComponent<Animator> ();

		var rect = GetComponent<RectTransform> ();
		rect.offsetMax = rect.offsetMin = new Vector2 (0, 0);
	}

	public void ResetObject()
	{
		gameObject.SetActive (false);
	}
	
	public void DisableObject(string gameObjectName)
	{
		GameObject gameObject= GameObject.Find (gameObjectName);
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}

	
}
