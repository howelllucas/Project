using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapShopNPC : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (weaponUI.instance.isshow == false)
            {
                weaponUI.instance.showShop();
            }
            else
            {
                weaponUI.instance.hideShop();
            }
        }
    }
}
