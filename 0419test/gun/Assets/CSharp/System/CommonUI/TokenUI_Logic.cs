using DG.Tweening;
using Game;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class TokenUI
    {
        private Dictionary<GoodsType, bool> m_idEndLock = new Dictionary<GoodsType, bool>();
        private TaskInfo taskInfo;
        private bool goldAddBtnValid = true;
        private bool diamondAddBtnValid = true;
        //private float m_DelayShowTime = 0.0f;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            m_idEndLock.Add(GoodsType.DIAMOND, true);
            m_idEndLock.Add(GoodsType.GOLD, true);
            m_idEndLock.Add(GoodsType.CAMPSITE_REWARD, true);

            GoldAddBtn.button.onClick.AddListener(OnAddGoldClick);
            DiamondAddBtn.button.onClick.AddListener(OnAddDiamondClick);
            PlayerIconBtn.button.onClick.AddListener(OnPlayerIconBtnClick);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            DiamondChanged(PlayerDataMgr.singleton.Diamond);
            GoldChanged(PlayerDataMgr.singleton.Gold);
            CampRewardChanged(CampsiteMgr.singleton.TotalRewardVal);
            RefreshCampRate();
            RegisterListeners();
            RefreshGoldAddBtnActive();
            RefreshDiamondAddBtnActive();
        }

        public override void Recycle()
        {
            UnRegisterListeners();
            base.Recycle();
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void RegisterListeners()
        {
            UnRegisterListeners();
            Global.gApp.gMsgDispatcher.AddListener<CurrencyType>(MsgIds.CurrencyChange, OnCurrencyChange);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.CampsiteTotalRewardChange, OnCampRewardChange);
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.CampsitePointDataChange, OnCampsitePointDataChange);
            Global.gApp.gMsgDispatcher.AddListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<CurrencyType>(MsgIds.CurrencyChange, OnCurrencyChange);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.CampsiteTotalRewardChange, OnCampRewardChange);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CampsitePointDataChange, OnCampsitePointDataChange);
            Global.gApp.gMsgDispatcher.RemoveListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        public void SetBgImageActive(bool active)
        {
            Bg.image.enabled = active;
        }

        public RectTransform GetIconById(GoodsType goods)
        {
            if (goods == GoodsType.GOLD)
            {
                return GoldIcon.rectTransform;
            }
            else if (goods == GoodsType.DIAMOND)
            {
                return DiamondIcon.rectTransform;
            }
            else if (goods == GoodsType.CAMPSITE_REWARD)
            {
                return CampRewardIcon.rectTransform;
            }
            return null;
        }

        public void UnLockById(GoodsType goods, bool endLock, bool playDT)
        {
            DOTweenAnimation doTween = null;
            switch (goods)
            {
                case GoodsType.DIAMOND:
                    {
                        m_idEndLock[goods] = endLock;
                        doTween = DiamondNode.gameObject.GetComponent<DOTweenAnimation>();
                        DiamondChanged(PlayerDataMgr.singleton.Diamond);
                    }
                    break;
                case GoodsType.GOLD:
                    {
                        m_idEndLock[goods] = endLock;
                        doTween = GoldNode.gameObject.GetComponent<DOTweenAnimation>();
                        GoldChanged(PlayerDataMgr.singleton.Gold);
                    }
                    break;
                case GoodsType.CAMPSITE_REWARD:
                    {
                        m_idEndLock[goods] = endLock;
                        doTween = CampRewardNode.gameObject.GetComponent<DOTweenAnimation>();
                        if (endLock)
                            CampRewardChanged(CampsiteMgr.singleton.TotalRewardVal);
                    }
                    break;
            }

            if (doTween != null && playDT)
            {
                doTween.DORestart();
            }
        }

        public void SetAddGoldValid(bool valid)
        {
            goldAddBtnValid = valid;
            RefreshGoldAddBtnActive();
        }

        private void RefreshGoldAddBtnActive()
        {
            GoldAddBtn.gameObject.SetActive(goldAddBtnValid && PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.Expedition));
        }

        public void SetAddDiamondValid(bool valid)
        {
            diamondAddBtnValid = valid;
            RefreshDiamondAddBtnActive();
        }

        private void RefreshDiamondAddBtnActive()
        {
            DiamondAddBtn.gameObject.SetActive(diamondAddBtnValid && PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.ShopTab));
        }

        private void OnCurrencyChange(CurrencyType type)
        {
            switch (type)
            {
                case CurrencyType.GOLD:
                    {
                        GoldChanged(PlayerDataMgr.singleton.Gold);
                    }
                    break;
                case CurrencyType.DIAMOND:
                    {
                        DiamondChanged(PlayerDataMgr.singleton.Diamond);
                    }
                    break;
                case CurrencyType.KEY:
                    break;
                default:
                    break;
            }
        }

        private void OnCampRewardChange()
        {
            CampRewardChanged(CampsiteMgr.singleton.TotalRewardVal);
        }

        private void GoldChanged(BigInteger val)
        {
            if (!m_idEndLock[GoodsType.GOLD])
            {
                return;
            }
            AssetChangeImp(GoldTxt.text, val.ToSymbolString());

            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

        private void DiamondChanged(double val)
        {
            if (!m_idEndLock[GoodsType.DIAMOND])
            {
                return;
            }
            AssetChangeImp(DiamondTxt.text, UiTools.FormateMoney(val));
        }

        private void CampRewardChanged(double val)
        {
            if (!m_idEndLock[GoodsType.CAMPSITE_REWARD])
            {
                return;
            }
            AssetChangeImp(CampRewardTxt.text, UiTools.FormateMoney(val));
        }

        private void AssetChangeImp(Text text, string val)
        {
            //DelayCallBack delayCallBack = text.gameObject.GetComponent<DelayCallBack>();
            //if (delayCallBack)
            //{
            //    DestroyImmediate(delayCallBack);
            //    //停止播放dotween
            //    DOTweenAnimation doTween = text.GetComponentInParent<DOTweenAnimation>();
            //    if (doTween != null)
            //    {
            //        doTween.DOPlay();
            //        gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            //        {
            //            doTween.DOPause();
            //        }, 0.6f, true);

            //    }
            //}

            //if (m_DelayShowTime > 0)
            //{
            //    var delayCallBack = text.gameObject.AddComponent<DelayCallBack>();
            //    delayCallBack.SetAction(() =>
            //    {
            //        text.text = val;
            //    }, m_DelayShowTime, true);
            //}
            //else
            //{
            //    text.text = val;
            //}
            //m_DelayShowTime = -1;

            text.text = val;
        }

        private void OnCampsitePointDataChange(int index)
        {
            RefreshCampRate();
        }

        private void RefreshCampRate()
        {
            CampRateTxt.text.text = string.Format("{0}/s", UiTools.FormateMoney(CampsiteMgr.singleton.TotalRewardRate));
        }

        private void OnAddGoldClick()
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.CommonUIIndexChange4Param, 2, "expedition");
        }

        private void OnAddDiamondClick()
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.CommonUIIndexChange4Param, 3, "diamond");
        }

        private void OnPlayerIconBtnClick()
        {
            //打开玩家界面
            Global.gApp.gUiMgr.OpenPanel(Wndid.PlayerInfoUI);
        }

        private void OnModuleOpen(GameModuleType module)
        {
            switch (module)
            {
                case GameModuleType.Expedition:
                    RefreshGoldAddBtnActive();
                    break;
                case GameModuleType.ShopTab:
                    RefreshDiamondAddBtnActive();
                    break;
            }
        }
    }
}