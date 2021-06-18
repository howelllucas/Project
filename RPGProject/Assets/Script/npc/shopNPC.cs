using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopNPC : MonoBehaviour
{

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (shop.instance.isshow==false)
            {
                shop.instance.showShop();
            }
            else
            {
                shop.instance.hideShop();
            }
        }
    }
}
