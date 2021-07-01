using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponUI : MonoBehaviour
{
    public static weaponUI instance;
    RectTransform tableTrans;

    public bool isshow=false;

    private int[] weaponList;
    private ObjectInfo info;
    public GameObject weaponItem;
    public Transform content;

    void Start()
    {
        instance = this;
        tableTrans = this.GetComponent<RectTransform>();
        tableTrans.localPosition = new Vector3(-1200, 0, 0);

        //读取装备列表保存到list中

        weaponList=getWeaponList();
        for (int i = 0; i < weaponList.Length; i++)
        {

            GameObject go = GameObject.Instantiate(weaponItem, content);


            go.GetComponent<weaponItem>().setID(weaponList[i]);
        }
    }
    private int[] getWeaponList()
    {
        weaponList = new int[22];
        
        for (int i = 2001; i < 2023; i++)
        {
            
            weaponList[i-2001] = i;
            
        }
        return weaponList;
    }
    public void showShop()
    {
        tableTrans.localPosition = new Vector3(0, 0, 0);
        isshow = true;
    }
    public void hideShop()
    {
        tableTrans.localPosition = new Vector3(-1200, 0, 0);
        isshow = false;
    }

    public void closeBtn()
    {
        hideShop();
    }
}
