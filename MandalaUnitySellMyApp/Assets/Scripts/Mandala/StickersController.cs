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
using AdvancedMobilePaint;

/// <summary>
/// <para>Scene: Gameplay</para>
/// <para>Object: StickersHolder</para>
/// <para>Description: Gameplay</para>
/// </summary>
public class StickersController : MonoBehaviour {

	public GameObject stickerPrefab;
	public float scaleFactor;
	public int rotateAngle;
	public float scaleMax;
	public float scaleMin;
	public bool canTransfrom = false;

	UStep stickerUndoStep;
	GameObject clone;

	public static GameObject currentSticker;

	public void InstantiateSticker(Sprite s)
	{
		clone = (GameObject) Instantiate (stickerPrefab,Vector3.zero,Quaternion.identity);
		clone.transform.SetParent(transform);
		clone.transform.localScale = Vector3.one;
		clone.transform.localPosition = Vector3.zero;
		clone.GetComponent<Image>().sprite = s;
		SelectSticker(clone);
		stickerUndoStep = new UStep();
		stickerUndoStep.type = -1;
		stickerUndoStep.sticker = clone;
		transform.parent.GetComponent<PaintUndoManager>().AddStep(stickerUndoStep);
	}

	public static void SelectSticker(GameObject sticker)
	{
//		if(currentSticker != null)
//			currentSticker.transform.GetChild(0).gameObject.SetActive(false);
//		currentSticker = sticker;
//		if(sticker != null)
//			currentSticker.transform.GetChild(0).gameObject.SetActive(true);
		if(sticker == null)
		{
			if(currentSticker != null)
				currentSticker.transform.GetChild(0).gameObject.SetActive(false);
			currentSticker = null;
		}
		else
		{
			if(currentSticker != null)
				currentSticker.transform.GetChild(0).gameObject.SetActive(false);
			currentSticker = sticker;
			currentSticker.transform.GetChild(0).gameObject.SetActive(true);
		}

	}

	public void ScaleUp ()
	{
		if (currentSticker != null /*&& currentSticker.transform.localScale.x < scaleMax*/)
		{
			AddTransformUndoStep();
			StartCoroutine("ScaleUpCrtn");
		}
	}
	
	public void ScaleDown()
	{
		if (currentSticker != null)
		{
			AddTransformUndoStep();
			StartCoroutine("ScaleDownCrtn");
		}
	}
	
	public void RotateLeft()
	{
		if (currentSticker != null)
		{
			AddTransformUndoStep();
			StartCoroutine("RotateLeftCrtn");
		}
	}
	
	public void RotateRight()
	{
		if (currentSticker != null)
		{
			AddTransformUndoStep();
			StartCoroutine("RotateRightCrtn");
		}
	}
	
	public void Done ()
	{
//		Debug.Log("Doned");
		if (currentSticker == null)
			return;
		else
		{
//			currentSticker.transform.GetChild(0).gameObject.SetActive(false);
//			currentSticker = null;
			SelectSticker(null);
		}

	}
	
	public void DeleteSelectedSticker()
	{
//		Debug.Log("Sticker deleted? " + currentSticker);
//		stickerUndoStep.stickerDeleted = currentSticker.GetComponent<Sticker>().stickerIndex;
//		stickerUndoStep.stickerLocalPos = currentSticker.transform.localPosition;
//		stickerUndoStep.stickerScale = currentSticker.transform.localScale;
//		stickerUndoStep.stickerRotation = currentSticker.transform.rotation;

//		Destroy (currentSticker);
		if(currentSticker == null)return;

		stickerUndoStep = new UStep();
		stickerUndoStep.type = -3;
		stickerUndoStep.sticker = currentSticker;
		currentSticker.SetActive(false);
		SelectSticker(null);
		transform.parent.GetComponent<PaintUndoManager>().AddStep(stickerUndoStep);
	}

	IEnumerator ScaleUpCrtn()
	{
		canTransfrom=true;
		while(canTransfrom && currentSticker.transform.localScale.x < scaleMax)
		{
			currentSticker.transform.localScale += new Vector3(scaleFactor*0.8f,scaleFactor*0.8f,0);
			yield return new WaitForSeconds(0.02f);
		}
	}

	IEnumerator ScaleDownCrtn()
	{
		canTransfrom=true;
		while(canTransfrom && currentSticker.transform.localScale.x > scaleMin)
		{
			currentSticker.transform.localScale -= new Vector3(scaleFactor*0.8f,scaleFactor*0.8f,0);
			yield return new WaitForSeconds(0.02f);
		}
	}

	IEnumerator RotateLeftCrtn()
	{
		canTransfrom = true;
		while(canTransfrom)
		{
			currentSticker.transform.Rotate(new Vector3(0,0,rotateAngle));
			yield return new WaitForSeconds(0.02f);
		}
	}

	IEnumerator RotateRightCrtn()
	{
		canTransfrom = true;
		while(canTransfrom)
		{
			currentSticker.transform.Rotate(new Vector3(0,0,-rotateAngle));
			yield return new WaitForSeconds(0.02f);
		}
	}

	void OnApplicationPause(bool p)
	{
		if(p)
		{
			canTransfrom=false;
		}
	}

	public void StopCoroutineStuff()
	{
		canTransfrom=false;
	}

	void AddTransformUndoStep()
	{
		stickerUndoStep = new UStep();
		stickerUndoStep.type = -2;
		stickerUndoStep.sticker = currentSticker;
		stickerUndoStep.stickerLocalPos = currentSticker.transform.localPosition;
		stickerUndoStep.stickerScale = currentSticker.transform.localScale;
		stickerUndoStep.stickerRotation = currentSticker.transform.rotation;
		transform.parent.GetComponent<PaintUndoManager>().AddStep(stickerUndoStep);
	}
}




















