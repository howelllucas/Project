using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class skillItemIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    Vector2 vecPoint = Vector2.one;
    private RectTransform canvasRect;
    //鼠标拖动后获得的新的坐标
    Transform newTrans;
    //鼠标点位置和图片中心的差值
    private Vector2 offsetDis;

    private Image iconImage;

    private int id;

    shortCut oldParent;
    shortCut nowParent;

    private bool isMouseStop = false;

    private void Start()
    {
        iconImage = this.transform.GetComponent<Image>();
        canvasRect = GameObject.Find("bg").transform as RectTransform;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldParent = this.transform.parent.GetComponent<shortCut>();
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position,
            eventData.enterEventCamera, out vecPoint);
        if (isRect)
        {
            offsetDis = this.GetComponent<RectTransform>().anchoredPosition - vecPoint;
        }
        iconImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.enterEventCamera, out vecPoint);
        //当鼠标拖拽的时候触发
        if (isRect)
        {
            this.GetComponent<RectTransform>().anchoredPosition = vecPoint + offsetDis;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;

        if (go != null)//不是空
        {
            if (go.tag == "Inventory_item_grid")//空格子
            {
                resetPositionAndParent(this.transform, go.transform);
                go.GetComponent<shortCut>().setID(oldParent.id);
                oldParent.ClearInfo();
            }
            else if (go.tag == "Inventory_item")//有物品
            {
                nowParent = go.transform.parent.GetComponent<shortCut>();
                int id = nowParent.id;
                //int num = nowParent.num;


                resetPositionAndParent(this.transform, go.transform.parent.transform);
                nowParent.setID(oldParent.id);


                resetPositionAndParent(go.transform, oldParent.transform);
                oldParent.setID(id);

            }
        }
        else
        {
            resetPositionAndParent(this.transform, this.transform);
        }
        iconImage.raycastTarget = true;
    }

    //重置坐标和设置父物体
    private void resetPositionAndParent(Transform child, Transform newTrans)
    {
        child.SetParent(newTrans);
        child.localPosition = Vector3.zero;

    }
}
