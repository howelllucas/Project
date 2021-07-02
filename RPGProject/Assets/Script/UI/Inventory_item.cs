using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// 物品节点类，用于更换设置物品
/// </summary>
public class Inventory_item : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    Vector2 vecPoint = Vector2.one;
    private RectTransform canvasRect;
    //鼠标拖动后获得的新的坐标
    Transform newTrans;
    //鼠标点位置和图片中心的差值
    private Vector2 offsetDis;
    private ObjectInfo info;

    private Image iconImage;

    private int id;

    inventoryItemGrid oldParent;
    inventoryItemGrid nowParent;

    private bool isMouseStop = false;

    GameObject dressgo;
     void Awake()
    {
        iconImage = this.transform.GetComponent<Image>();
        canvasRect = GameObject.Find("bg").transform as RectTransform;
        
    }
    private void Update()
    {
        if (isMouseStop)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //穿戴操作
                
                bool success = equipment.instance.Dress(id);
                if (success)//如果成功，需要当前物品减1
                {
                    
                    this.transform.GetComponentInParent<inventoryItemGrid>().MinusNum();
                }
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        oldParent = this.transform.parent.GetComponent<inventoryItemGrid>();
        bool isRect= RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position,
            eventData.enterEventCamera, out vecPoint);
        if (isRect)
        {
            offsetDis = this.GetComponent<RectTransform>().anchoredPosition - vecPoint;
        }
        iconImage.raycastTarget = false;



    }

    public void OnDrag(PointerEventData eventData)
    {
        bool isRect=RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.enterEventCamera, out vecPoint);
        //当鼠标拖拽的时候触发
        if (isRect)
        {
            this.GetComponent<RectTransform>().anchoredPosition = vecPoint + offsetDis;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        
        if (go!=null)//不是空
        {
            if (go.tag== "Inventory_item_grid")//空格子
            {
                resetPositionAndParent(this.transform,go.transform);
                go.GetComponent<inventoryItemGrid>().setID(oldParent.id, oldParent.num);
                oldParent.ClearInfo();
            }
            else if (go.tag == "Inventory_item")//有物品
            {
                nowParent = go.transform.parent.GetComponent<inventoryItemGrid>();
                int id = nowParent.id;
                int num = nowParent.num;
                

                resetPositionAndParent(this.transform, go.transform.parent.transform);
                nowParent.setID(oldParent.id,oldParent.num);
                
                
                resetPositionAndParent(go.transform, oldParent.transform);
                oldParent.setID(id,num);

            }
            else if (go.tag == "shortCut")
            {
                if (info.type==ObjectType.Drug)
                {
                    resetPositionAndParent(this.transform, this.transform);
                    go.GetComponent<shortCutGrid>().setDrug(id,oldParent.num);
                }
            }
        }
        else
        {
            resetPositionAndParent(this.transform,this.transform);
        }
        iconImage.raycastTarget = true;
    }
    //重置坐标和设置父物体
    private void resetPositionAndParent(Transform child, Transform newTrans)
    {
        child.SetParent(newTrans);
        child.localPosition= Vector3.zero;
        
    }
    //根据id设置物品
    public void setId(int id)
    {
        this.id = id;
        info = ObjectsInfo.instance.GetObjectInfoById(id);
        //更新图片显示
        iconImage.sprite = Resources.Load<Sprite>("Icon/" + info.icon_name);
    }
    public void setImage(string iconName)
    {
        iconImage.sprite = Resources.Load<Sprite>("Icon/" + iconName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dressgo = eventData.pointerCurrentRaycast.gameObject;
        isMouseStop = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseStop = false;
    }
}
