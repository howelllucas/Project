using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using DG.Tweening;

namespace EZ
{
    public class ShopGroupUI : MonoBehaviour
    {
        public int group;
        public string groupName;
        public Transform itemParent;
        private List<Shop_TableItem> itemDataList = new List<Shop_TableItem>();
        private Dictionary<int, ShopItemUIBase> itemUIDic = new Dictionary<int, ShopItemUIBase>();
        private DOTweenAnimation showAnim;

        public float Pos
        {
            get
            {
                return transform.localPosition.y;
            }
        }

        protected virtual void Awake()
        {
            showAnim = GetComponent<DOTweenAnimation>();
        }

        public virtual void Init()
        {
            var dataList = TableMgr.singleton.ShopTable.GetShopItemsByGroup(group);
            if (dataList == null)
            {
                gameObject.SetActive(false);
                return;
            }

            itemDataList.Clear();
            var buffer = itemUIDic;
            itemUIDic = new Dictionary<int, ShopItemUIBase>();
            for (int i = 0; i < dataList.Count; i++)
            {
                var data = dataList[i];
                if (!IsValid(data))
                {
                    if (buffer.ContainsKey(data.id))
                    {
                        buffer[data.id].Recycle();
                        buffer[data.id].gameObject.SetActive(false);
                    }
                    continue;
                }

                itemDataList.Add(data);
                ShopItemUIBase item;
                if (buffer.ContainsKey(data.id))
                {
                    item = buffer[data.id];
                    item.gameObject.SetActive(true);
                }
                else
                {
                    var obj = Global.gApp.gResMgr.InstantiateObj(data.prefab);
                    obj.transform.SetParent(itemParent, false);
                    item = obj.GetComponent<ShopItemUIBase>();
                }

                item.Init(data);
                itemUIDic[data.id] = item;
            }

            if (itemDataList.Count <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void Refresh(int changeItemId = -1)
        {
            if (itemDataList == null)
                return;
            if (changeItemId > 0 && !itemUIDic.ContainsKey(changeItemId))
                return;

            for (int i = itemDataList.Count - 1; i >= 0; i--)
            {
                var data = itemDataList[i];
                if (!IsValid(data))
                {
                    if (itemUIDic.ContainsKey(data.id))
                    {
                        itemUIDic[data.id].gameObject.SetActive(false);
                    }
                    itemDataList.RemoveAt(i);
                }
            }

            if (itemDataList.Count <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        protected virtual bool IsValid(Shop_TableItem res)
        {
            return !ShopMgr.singleton.IsSellOut(res);
        }

        public void PlayShowAnim()
        {
            if (showAnim != null)
                showAnim.DORestart();
        }
    }
}