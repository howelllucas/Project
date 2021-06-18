using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 物品信息类，单例模式的text资源读取，放入字典,GetObjectInfoById方法
/// </summary>
public class ObjectsInfo : MonoBehaviour
{
    //外部传入的text资源
    public TextAsset objectsInfoListText;
    //text文本
    private string strText;
    //每一行信息集合
    private string[] strArray;
    //每一行内用逗号分割的单独信息集合
    private string[] proArray;
    //字典
    private Dictionary<int, ObjectInfo> objectInfoDict = new Dictionary<int, ObjectInfo>();
    //单例模式
    public static ObjectsInfo instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //测试
        ReadInfo();
        //Debug.Log(objectInfoDict.Keys.Count);
    }
    //供外部调取的通过ID得到物品信息
    public ObjectInfo GetObjectInfoById(int id)
    {
        ObjectInfo info = null;
        objectInfoDict.TryGetValue(id, out info);
        return info;
    }

    //解析text文件
    private void ReadInfo()
    {
        strText = objectsInfoListText.text;
        strArray = strText.Split('\n');
        //str是单行的信息
        foreach (string str in strArray)
        {
            proArray = str.Split(',');
            ObjectInfo info = new ObjectInfo();

            info.id = int.Parse(proArray[0]);
            info.name = proArray[1];
            info.icon_name = proArray[2];
            string str_type = proArray[3];

            ObjectType type = ObjectType.Drug;
            
            switch (str_type)
            {
                case "Drug":
                    type = ObjectType.Drug;
                    break;
                case "Equip":
                    type = ObjectType.Equip;
                    break;
                case "Mat":
                    type = ObjectType.Mat;
                    break;
                default:
                    break;
            }
            info.type = type;
            if (type==ObjectType.Drug)
            {
                info.hp = int.Parse(proArray[4]);
                info.mp = int.Parse(proArray[5]);
                info.price_sell = int.Parse(proArray[6]);
                info.price_buy = int.Parse(proArray[7]);
            }
            else if (type == ObjectType.Equip)
            {
                info.attack = int.Parse(proArray[4]);
                info.def = int.Parse(proArray[5]);
                info.speed = int.Parse(proArray[6]);
                info.price_sell = int.Parse(proArray[9]);
                info.price_buy = int.Parse(proArray[10]);
                string str_dress = proArray[7];
                switch (str_dress)
                {
                    case "Headgear" :
                    info.dressType = DressType.Headgear;
                        break;
                    case "Armor":
                        info.dressType = DressType.Armor;
                        break;
                    case "LeftHand":
                        info.dressType = DressType.LeftHand;
                        break;
                    case "RightHand":
                        info.dressType = DressType.RightHand;
                        break;
                    case "Shoe":
                        info.dressType = DressType.Shoe;
                        break;
                    case "Accessory":
                        info.dressType = DressType.Accessory;
                        break;
                    default:
                        break;
                }
                string str_AppType = proArray[8];
                switch (str_AppType)
                {
                    case "Swordman":
                        info.applicationType = ApplicationType.Swordman;
                        break;
                    case "Magician":
                        info.applicationType = ApplicationType.Magician;
                        break;
                    case "Common":
                        info.applicationType = ApplicationType.Common;
                        break;
                    default:
                        break;
                }
            }
            
            //用读取到的id当做字典的key，用每次创建的info保存单行信息
            objectInfoDict.Add(info.id, info);
        }
    }

    
  
}
//物品类型
public enum ObjectType
{
    Drug,
    Equip,
    Mat
}
//装备类型
public enum DressType
{
    Headgear,
    Armor,
    RightHand,
    LeftHand,
    Shoe,
    Accessory
}
public enum ApplicationType
{
    Swordman,
    Magician,
    Common
}
public class ObjectInfo
{
    public int id;
    public string name;
    public string icon_name;
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;

    public int attack;
    public int def;
    public int speed;
    public DressType dressType;
    public ApplicationType applicationType;
}
