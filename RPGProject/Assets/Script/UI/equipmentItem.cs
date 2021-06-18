using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class equipmentItem : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    bool isMouseStop = false;
    Image image;
    public int id;
    void Awake()
    {
        image = this.transform.GetComponent<Image>();
    }
    private void Update()
    {
        if (isMouseStop)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //脱下操作
                equipment.instance.TakeOff(id,this.gameObject);
                
                
            }
        }
    }



    public void EquipMIsetId(int id)
    {
        this.id = id;
        ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(id);
        Sprite sp = Resources.Load<Sprite>("Icon/" + info.icon_name);
        image.sprite = sp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        isMouseStop = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseStop = false;
    }
}
