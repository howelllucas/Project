using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponItem : MonoBehaviour
{
    private int id;
    private ObjectInfo info;

    private Image icon;
    private Text name_text;
    private Text effect_text;
    private Text price_text;

    private Button buy_button;

    void Awake()
    {
        icon = this.transform.Find("icon").GetComponent<Image>();
        name_text = this.transform.Find("name_text").GetComponent<Text>();
        effect_text = this.transform.Find("xiaoguo_text").GetComponent<Text>();
        price_text = this.transform.Find("shoujia_text").GetComponent<Text>();
        buy_button = this.transform.Find("Button").GetComponent<Button>();
    }
    
    public void setID(int id)
    {
        this.id = id;
        info = ObjectsInfo.instance.GetObjectInfoById(id);

        icon.sprite = Resources.Load<Sprite>("Icon/" + info.icon_name);
        name_text.text = info.name;
        if (info.attack!=0)
        {
            effect_text.text = "+"+info.attack.ToString()+"伤害";
        }
        if (info.def!=0)
        {
            effect_text.text = "+" + info.def.ToString() + "防御";
        }
        if (info.speed!=0)
        {
            effect_text.text = "+" + info.speed.ToString() + "速度";
        }
        price_text.text = info.price_buy.ToString();

        

    }

    public void onClickBuyButton()
    {
        int priceNum= ObjectsInfo.instance.GetObjectInfoById(id).price_buy;
        bool success = Inventory.instance.getCoin(priceNum);
        if (success)
        {
            Inventory.instance.getId(id);
        }
    }
}
