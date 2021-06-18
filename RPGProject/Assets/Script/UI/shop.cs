using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shop : MonoBehaviour
{
    public static shop instance;
    public bool isshow = false;
    public GameObject dialogTable;
    public InputField inputField;
    public int castCoin = 0;

    RectTransform tableRect;
    private int buyNum = 0;
    private int buyId = 0;
   
    void Start()
    {
        instance = this;
        tableRect = this.GetComponent<RectTransform>();
        tableRect.localPosition = new Vector3(-1200, 0, 0);
    }

    public void showShop()
    {
        tableRect.localPosition = new Vector3(0, 0, 0);
        isshow = true;
    }
    public void hideShop()
    {
        tableRect.localPosition = new Vector3(-1200, 0, 0);
        isshow = false;
    }

    public void closeBtn()
    {
        hideShop();
    }

    public void button1001()
    {
        buy(1001);
    }
    public void button1002()
    {
        buy(1002);
    }
    public void button1003()
    {
        buy(1003);
    }
    public void buy(int id)
    {
        showDialogTable();
        buyId = id;
        
    }
    //显示ok面板
    public void showDialogTable()
    {
        dialogTable.SetActive(true);
        inputField.text = buyNum.ToString();
    }

    public void buttonAccept()
    {
        ObjectInfo info = ObjectsInfo.instance.GetObjectInfoById(buyId);
        int coinById = info.price_buy;
        int num = int.Parse(inputField.text);

        castCoin = coinById * num;
        bool success = Inventory.instance.getCoin(castCoin);
        if (success)
        {
            if (num > 0)
            {
                Inventory.instance.getId(buyId, num);
            }
        }
        hideShop();
    }
}
