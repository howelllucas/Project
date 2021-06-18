using EZ.Data;
using EZ.DataMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampMatBag_MatItemUI
    {
        CampRecycleItem m_RecycleItem;
        ItemItem m_ItemItem;
        CampMatBag m_Parrent;
        public void Init(CampRecycleItem recycleItem, ItemItem itemItem, CampMatBag parent)
        {
            m_Parrent = parent;
            m_RecycleItem = recycleItem;
            m_ItemItem = itemItem;
            MatItemBtn.button.onClick.AddListener(OpenRecycleUi);
            transform.SetSiblingIndex(m_RecycleItem.id);
            FreshUi();
        }
        private void OpenRecycleUi()
        {
            double count = GameItemFactory.GetInstance().GetItem(m_RecycleItem.id);
            if(count > 0)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampMatRecycleDetailUi, this);
            }
        }
        public CampRecycleItem GetRecycleItem()
        {
            return m_RecycleItem;
        }
        public void FreshUi()
        {
            double count = GameItemFactory.GetInstance().GetItem(m_RecycleItem.id);
            if (count > 0)
            {
                gameObject.SetActive(true);
                MatIcon.image.sprite = Resources.Load(m_ItemItem.image_grow, typeof(Sprite)) as Sprite;
                MatName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(m_ItemItem.sourceLanguage);
                CurCount.text.text = ((int)(count + 0.5d)).ToString();
            }
            else
            {
                m_Parrent.RemoveItem(this);
            }
        }
    }
}
