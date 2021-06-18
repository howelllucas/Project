using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ;

namespace Game
{
    public class ShopMgr : Singleton<ShopMgr>
    {

        public void OnBuySuccess(int shopId)
        {
            PlayerDataMgr.singleton.DB.shopData.AddBuyNum(shopId);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShopBuySuccess, shopId);   
        }

        public bool IsSellOut(Shop_TableItem res)
        {
            return res.limit_count > 0 && PlayerDataMgr.singleton.DB.shopData.GetBuyNum(res.id) >= res.limit_count;
        }
    }
}