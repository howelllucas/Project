using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class FightLose
    {
        public event System.Action OnClose;
        private float m_DelayCloseTime = 1f;
        private float m_CalcDelayTime = 1f;
        private bool m_CanClose = false;
        private float m_DtDelayTime = 0.25f;
        private float m_StartDelayTime = 0.5f;
        private float m_CurTime = 0;
        private List<FightLose_ResIcon> m_FightWinIcons = new List<FightLose_ResIcon>();
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            int iconCount = InitNode();
            m_CalcDelayTime = (iconCount - 1) * m_DtDelayTime + m_StartDelayTime;
            m_DelayCloseTime += m_CalcDelayTime;
            base.ChangeLanguage();
        }
        private int InitNode()
        {
            int index = 0;
            index = AddResIcon(ref SpecialItemIdConstVal.FIGIT_END_RES, index);
            if (index == 0)
            {
                m_ScrollNode.gameObject.GetComponent<ScrollRect>().enabled = false;
            }
            else
            {
                foreach (FightLose_ResIcon fightWin_ResIcon in m_FightWinIcons)
                {
                    fightWin_ResIcon.Icon.image.raycastTarget = true;
                }
            }
            return index;
        }
        private int AddResIcon(ref int[] itemArray, int index)
        {
            foreach (int itemId in itemArray)
            {
                ItemItem itemItem = Global.gApp.gGameData.ItemData.Get(itemId);
                string itemName = itemItem.name;
                int itemCount = Global.gApp.CurScene.GetMainPlayerComp().GetPlayerData().GetDropResCount(itemName);
                if (itemCount > 0)
                {
                    float delayTime = index * m_DtDelayTime + m_StartDelayTime;
                    index++;
                    FightLose_ResIcon itemUI = ResIcon.GetInstance();
                    itemUI.transform.SetAsLastSibling();
                    itemUI.gameObject.SetActive(true);
                    itemUI.Init(delayTime, itemCount, itemItem);
                    m_FightWinIcons.Add(itemUI);
                }
            }
            return index;
        }
        private void Update()
        {
            m_CurTime += Time.deltaTime;
            if (m_CurTime > m_DelayCloseTime)
            {
                m_CanClose = true;
            }
        }
        public override void TouchClose()
        {
            if (m_CanClose)
            {
                base.TouchClose();
                OnClose?.Invoke();
            }
            else if (m_CurTime < m_CalcDelayTime)
            {
                m_CurTime = m_CalcDelayTime;
                foreach (FightLose_ResIcon fightWin_ResIcon in m_FightWinIcons)
                {
                    fightWin_ResIcon.SetDelayTimeZero();
                }
            }

            //Global.gApp.gGameCtrl.ChangeToMainScene();
            //Global.gApp.gUiMgr.OpenPanel(Wndid.MainPanel);
            //Global.gApp.gUiMgr.OpenPanel(Wndid.CommonPanel);
        }
    }
}
