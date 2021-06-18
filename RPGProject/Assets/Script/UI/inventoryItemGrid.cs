using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryItemGrid : MonoBehaviour
{
    public int id = 0;
    private ObjectInfo info = null;
    public int num = 0;

    private Text text;
    private void Start()
    {
        text = this.transform.GetComponentInChildren<Text>();
    }

    public void setID(int id,int num = 1)
    {
        this.id = id;
        this.num = num;
        info = ObjectsInfo.instance.GetObjectInfoById(id);
        Inventory_item item = this.GetComponentInChildren<Inventory_item>();
        item.setId(id);
        text.enabled = true;
        text.text = num.ToString();
    }
    public void ClearInfo()
    {
        id = 0;
        info = null;
        num = 0;
        text.enabled = false;
    }
    public void plusNum(int num = 1)
    {
        this.num += num;
        text.text = this.num.ToString();
    }
    public bool MinusNum(int num = 1)
    {
        if (this.num>=num)
        {
            this.num -= num;
            
            if (this.num==0)
            {
                
                ClearInfo();
                GameObject.Destroy(this.GetComponentInChildren <Inventory_item>().gameObject);
            }
            text.text = this.num.ToString();
            return true;
        }
        return false;
    }
}
