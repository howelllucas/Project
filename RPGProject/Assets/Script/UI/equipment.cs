using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipment : MonoBehaviour
{
    public static equipment instance;
    public GameObject equipMentItem;
    public GameObject headgear;
    public GameObject armor;
    public GameObject righthand;
    public GameObject lefthand;
    public GameObject shoe;
    public GameObject accessory;

    public bool isshow = false;

    private RectTransform rectTran;

    private playerStatus ps;

    private int attack;
    private int def;
    private int speed;

    void Start()
    {
        instance = this;
        rectTran = this.GetComponent<RectTransform>();

        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<playerStatus>();
    }

    public void showTable()
    {
        rectTran.localPosition = new Vector3(0, 0, 0);
        isshow = true;
    }
    public void hideTable()
    {
        rectTran.localPosition = new Vector3(-1200, 0, 0);
        isshow = false;
    }
    //脱装备
    public void TakeOff(int id,GameObject go)
    {
        Inventory.instance.getId(id);
        GameObject.Destroy(go);
        UpdateProperty();
    }
    //穿戴装备
    public bool Dress(int id)
    {
        ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(id);
        
        if (info.type!=ObjectType.Equip)
        {
            Debug.Log("333");
            return false;
        }
        if (ps.playerType==PlayerType.Magician)
        {
            if (info.applicationType==ApplicationType.Swordman)
            {
                return false;
            }
        }
        if (ps.playerType==PlayerType.Swordman)
        {
            if (info.applicationType==ApplicationType.Magician)
            {
                return false;
            }
        }
        
        //下面是可穿戴的情况处理
        GameObject parent = null;
        switch (info.dressType)
        {
            case DressType.Headgear:
                parent = headgear;
                break;
            case DressType.Armor:
                parent = armor;
                break;
            case DressType.LeftHand:
                parent = lefthand;
                break;
            case DressType.RightHand:
                parent = righthand;
                break;
            case DressType.Shoe:
                parent = shoe;
                break;
            case DressType.Accessory:
                parent = accessory;
                break;
            default:
                break;
        }
        equipmentItem item = parent.GetComponentInChildren<equipmentItem>();
        if (item != null)//说明穿了同样类型的装备
        {

            //先把这个id的物品返回到背包
            Inventory.instance.getId(item.id);
            item.EquipMIsetId(info.id);
        }
        else//空格子
        {
            
            GameObject.Instantiate(equipMentItem, parent.transform);
            equipmentItem item1 = parent.GetComponentInChildren<equipmentItem>();
            item1.EquipMIsetId(info.id);
        }
        UpdateProperty();
        return true;
    }

    void UpdateProperty()
    {
        this.attack = 0;
        this.def = 0;
        this.speed = 0;

        equipmentItem item_headgear= headgear.GetComponentInChildren<equipmentItem>();
        PlusProperty(item_headgear);
        equipmentItem item_armor= headgear.GetComponentInChildren<equipmentItem>();
        PlusProperty(item_armor);
        equipmentItem item_righthand = headgear.GetComponentInChildren<equipmentItem>();
        PlusProperty(item_righthand);
        equipmentItem item_lefthand = headgear.GetComponentInChildren<equipmentItem>();
        PlusProperty(item_lefthand);
        equipmentItem item_shoe = headgear.GetComponentInChildren<equipmentItem>();
        PlusProperty(item_shoe);
        equipmentItem item_accessory = headgear.GetComponentInChildren<equipmentItem>();
        PlusProperty(item_accessory);
    }

    void PlusProperty(equipmentItem item)
    {
        if (item!=null)
        {
            ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(item.id);
            this.attack += info.attack;
            this.def += info.def;
            this.speed += info.speed;
        }
        
    }
}
