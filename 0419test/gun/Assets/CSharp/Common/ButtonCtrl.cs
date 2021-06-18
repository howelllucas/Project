using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    private bool bDown = false; 
    private bool bEnter = false;

    private Vector3 orginScale = Vector3.one;

    private void Start()
    {
        orginScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
	{
		this.bDown = true;
		this.PlayDown();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.bEnter && this.bDown)
		{
			this.PlayUp();
		}
		this.bDown = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.bEnter = true;
		if (this.bDown)
		{
			this.PlayDown();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.bEnter = false;
		if (this.bDown)
		{
			this.PlayUp();
		}
	}


	// 播放点击下的动画
	private void PlayDown()
	{
        Tweener doT = transform.DOScale(orginScale * 0.87f, 0.1f);
	}

	// 播放放开的动画
	private void PlayUp()
	{
        Tweener doT = transform.DOScale(orginScale, 0.3f);
        doT.SetEase(Ease.OutElastic);
    }

}
