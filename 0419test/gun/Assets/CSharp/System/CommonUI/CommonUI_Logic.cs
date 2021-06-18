using DG.Tweening;
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using Game;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class CommonUI
    {

        private string m_CurPannel = Wndid.HomeUI;
        private int m_CurPannelIndex = 2;

        private int m_OrderLayer = 35;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            button.tabGroup.OnIndexChanged.AddListener(OnTabGroupIndexChange);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            m_OrderLayer = info.Order;

            OnCommonIndexChange(m_CurPannelIndex);

            shop.shop.Lockimg.gameObject.SetActive(!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.ShopTab));
            weapon.weapon.Lockimg.gameObject.SetActive(!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunTab));
            RegisterListeners();

        }

        private void OnButton(int index)
        {
            OnClick();
            switch (index)
            {
                case 1:
                    if (PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunTab))
                    {
                        CloseSkillPanel(Wndid.GunUI);
                        m_CurPannelIndex = index;
                    }
                    else
                    {
                        ResetTabGroup();
                        //tips
                    }
                    break;
                case 2:
                    CloseSkillPanel(Wndid.HomeUI);
                    m_CurPannelIndex = index;
                    break;
                case 3:
                    if (PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.ShopTab))
                    {
                        CloseSkillPanel(Wndid.ShopPanel);
                        m_CurPannelIndex = index;
                    }
                    else
                    {
                        ResetTabGroup();
                        //tips
                    }
                    break;
                default:
                    Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3024, index.ToString());
                    m_CurPannelIndex = 2;
                    m_CurPannel = Wndid.HomeUI;
                    CloseSkillPanel(m_CurPannel);
                    break;
            }
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

        private void OnButton<T>(int index, T param)
        {
            OnClick();
            switch (index)
            {
                case 1:
                    if(PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunTab))
                    {
                        CloseSkillPanel<T>(Wndid.GunUI, param);
                        m_CurPannelIndex = index;
                    }
                    else
                    {
                        ResetTabGroup();
                        //tips
                    }
                    break;
                case 2:
                    CloseSkillPanel<T>(Wndid.HomeUI, param);
                    m_CurPannelIndex = index;
                    break;
                case 3:
                    if (PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.ShopTab))
                    {
                        CloseSkillPanel<T>(Wndid.ShopPanel, param);
                        m_CurPannelIndex = index;
                    }
                    else
                    {
                        ResetTabGroup();
                        //tips
                    }
                    break;
                default:
                    Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3024, index.ToString());
                    m_CurPannelIndex = 2;
                    m_CurPannel = Wndid.HomeUI;
                    CloseSkillPanel<T>(m_CurPannel, param);
                    break;
            }
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

        private void ResetTabGroup()
        {
            button.tabGroup.SetIndex(m_CurPannelIndex - 1);
        }

        private void OnTabGroupIndexChange(int index)
        {
            OnButton(index + 1);
        }

        private void OnCommonIndexChange(int index)
        {
            button.tabGroup.SetIndex(index - 1);
            OnButton(index);
        }

        private void OnCommonIndexChange4Param<T>(int index, T param)
        {
            button.tabGroup.SetIndex(index - 1);
            OnButton<T>(index, param);
        }

        private void RegisterListeners()
        {
            UnRegisterListeners();

            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.CommonUIIndexChange, OnCommonIndexChange);
            Global.gApp.gMsgDispatcher.AddListener<int, string>(MsgIds.CommonUIIndexChange4Param, OnCommonIndexChange4Param);
            Global.gApp.gMsgDispatcher.AddListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CommonUIIndexChange, OnCommonIndexChange);
            Global.gApp.gMsgDispatcher.RemoveListener<int, string>(MsgIds.CommonUIIndexChange4Param, OnCommonIndexChange4Param);
            Global.gApp.gMsgDispatcher.RemoveListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void CloseSkillPanel(string openPanel)
        {
            if (m_CurPannel.Equals(openPanel))
            {
                return;
            }
            Global.gApp.gUiMgr.OpenPanel(openPanel);
            if (!string.IsNullOrEmpty(m_CurPannel))
                Global.gApp.gUiMgr.ClosePanel(m_CurPannel);
            m_CurPannel = openPanel;
        }

        public void ResetOrderLayer()
        {
            transform.Find("Canvas1").GetComponent<Canvas>().sortingOrder = m_OrderLayer;
        }
        public void SetOrderLayer(int order)
        {
            transform.Find("Canvas1").GetComponent<Canvas>().sortingOrder = order;
        }
        private void CloseSkillPanel<T>(string openPanel, T param)
        {

            if (!string.IsNullOrEmpty(m_CurPannel))
                Global.gApp.gUiMgr.ClosePanel(m_CurPannel);
            Global.gApp.gUiMgr.OpenPanel<T>(openPanel, param);
            m_CurPannel = openPanel;
        }

        private void OnModuleOpen(GameModuleType module)
        {
            switch (module)
            {
                case GameModuleType.ShopTab:
                    {
                        shop.shop.Lockimg.gameObject.SetActive(false);
                    }
                    break;
                case GameModuleType.GunTab:
                    {
                        weapon.weapon.Lockimg.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
}
