
using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampMatBag
    {
        List<CampMatBag_MatItemUI> m_AllCampRecycleItem = new List<CampMatBag_MatItemUI>();
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            InitNode();
            CreateMatItem();
            base.ChangeLanguage();
        }
        public void InitNode()
        {
            CloseBtn.button.onClick.AddListener(TouchClose);
        }
        private void CreateMatItem()
        {
            CampRecycleItem[] campRecycleItems = Global.gApp.gGameData.CampRecycleConfig.items;
            foreach (CampRecycleItem recycleItem in campRecycleItems)
            {
                double count = GameItemFactory.GetInstance().GetItem(recycleItem.id);
                if (count > 0)
                {
                    ItemItem itemItem = Global.gApp.gGameData.ItemData.Get(recycleItem.id);
                    CampMatBag_MatItemUI matItem = MatItemUI.GetInstance();
                    matItem.Init(recycleItem, itemItem,this);
                    m_AllCampRecycleItem.Add(matItem);
                }
            }
            FreshTipsState();
        }
        public void RemoveItem(CampMatBag_MatItemUI item)
        {
            MatItemUI.CacheInstance(item);
            m_AllCampRecycleItem.Remove(item);
            FreshTipsState();
        }

        public void FreshTipsState()
        {
            m_EmptyTip.gameObject.SetActive(m_AllCampRecycleItem.Count == 0);
        }
    }
}
