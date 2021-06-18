using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;

namespace EZ
{
    public class ShopItemUIBase : MonoBehaviour
    {
        protected Shop_TableItem res;
        public Image bgIconImg;
        protected GameObject bgEfObj;

        protected virtual void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnBuyBtnClick);
        }

        public virtual void Init(Shop_TableItem res)
        {
            if (res == this.res)
                return;
            this.res = res;
            if (!string.IsNullOrEmpty(res.bgIcon))
                bgIconImg.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(res.bgIcon);
            if (!string.IsNullOrEmpty(res.bgEf))
            {
                bgEfObj = Global.gApp.gResMgr.InstantiateObj(res.bgEf);
                bgEfObj.transform.SetParent(transform, false);
            }
        }

        public virtual void Recycle()
        {

        }

        protected virtual void OnBuyBtnClick()
        {

        }

        //子类在购买成功后调用
        protected void OnBuySuccess()
        {
            ShopMgr.singleton.OnBuySuccess(res.id);
        }
    }
}