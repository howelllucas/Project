using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class skillItemIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject iconPrefab;


    Vector2 vecPoint = Vector2.one;
    private RectTransform canvasRect;
    //鼠标拖动后获得的新的坐标
    Transform newTrans;
    //鼠标点位置和图片中心的差值
    private Vector2 offsetDis;

    private Image iconImage;

    private int id;

    Transform oldParent;
    Transform nowParent;

    //private bool isMouseStop = false;

    private void Start()
    {
        iconImage = this.transform.GetComponent<Image>();
        canvasRect = GameObject.Find("bg").transform as RectTransform;
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldParent = this.transform;
        if (this.GetComponentInParent<shortCutGrid>())
        {
            return;
        }
        this.id = GetComponentInParent<skillItem>().id;
        this.transform.SetParent (this.transform.root);
        bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position,
            eventData.enterEventCamera, out vecPoint);
        if (isRect)
        {
            offsetDis = this.GetComponent<RectTransform>().anchoredPosition - vecPoint;
        }
        GameObject[] itemList = GameObject.FindGameObjectsWithTag("skillItem");
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i].transform.childCount!= itemList.Length)
            {
                GameObject go = Instantiate(iconPrefab, itemList[i].transform);
                go.GetComponent<Image>().sprite = itemList[i].GetComponent<skillItem>().icon_image.sprite;
                go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-130, 0, 0);
                
            }
        }
        //if (eventData.pointerCurrentRaycast.gameObject.tag=="skillItem")
        //{
        //    GameObject go= Instantiate(iconPrefab, eventData.pointerCurrentRaycast.gameObject.transform);
        //    go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-130, 0, 0);
        //}
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
            if (go.tag == "shortCut")//空格子
            {
                
                go.GetComponent<shortCutGrid>().setSkill(id);
                
            }
            else if (go.tag == "shortCutItem")//有物品
            {
                
                go.GetComponentInParent<shortCutGrid>().ClearInfo();
                go.GetComponentInParent<shortCutGrid>().setSkill(id);
                

            }
            else
            {
                resetPositionAndParent(this.transform, nowParent);
            }
        }
        else
        {
            resetPositionAndParent(this.transform, oldParent);
        }
        iconImage.raycastTarget = true;
    }

    //重置坐标和设置父物体
    private void resetPositionAndParent(Transform child, Transform newTrans)
    {
        child.SetParent(newTrans);
        child.localPosition = Vector3.zero;

    }

    //public void setIcon(int id)
    //{

    //    SkillInfo info = SkillsInfo.instance.GetSkillInfoById(id);
    //    iconImage.sprite = Resources.Load<Sprite>("Icon/" + info.icon_name);
    //}
}
