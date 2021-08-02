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

/*Scene: N/A
 *Object:PopUpDialogSavePicture
 *Opis:Pomocna skripta za komunikaciju izmedju  PopUpDialogSavePicture objekta i DialogSavePictureManager objekta
 ****/
public class DialogBackPressResult : MonoBehaviour {

	/// <summary>
	/// Postavlja rezultat prilikom zatvaranja dijaloga
	/// </summary>
	/// <param name="result">If set to <c>true</c> result.</param>
	public void SetResult(bool result)
	{
		GameObject.Find ("DialogSavePictureManager").GetComponent<DialogSavePictureManager> ().SetDialogResult (result);


	}
	/// <summary>
	/// Postavlja stanje dijaloga na  zatvoren(state=false)
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void SetState(bool state)
	{
		GameObject.Find ("DialogSavePictureManager").GetComponent<DialogSavePictureManager> ().SetDialogState (state);

	}
}

