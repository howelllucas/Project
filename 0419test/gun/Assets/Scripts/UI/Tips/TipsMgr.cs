using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TipsMgr : Singleton<TipsMgr>
    {
        private Dictionary<RedTipsType, List<RedTipsUI>> redTipsDic = new Dictionary<RedTipsType, List<RedTipsUI>>();
        private HashSet<RedTipsType> closeTipsList = new HashSet<RedTipsType>();
        private Dictionary<RedTipsType, int> redTipsCountBuffer = new Dictionary<RedTipsType, int>();

        public void Init()
        {
            RefreshRedTipsData();
        }

        public void Clear()
        {
            //redTipsDic.Clear();
            foreach (var list in redTipsDic.Values)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i] == null)
                        list.RemoveAt(i);
                }
            }
        }
        //显示浮动提示
        //public void ShowTips(string tips, params object[] args)
        //{
        //    var str = LanguageMgr.GetText(tips, args);
        //    var tips_ui = UIMgr.singleton.FindUIObject<MessageTipsUI>();
        //    if (tips_ui != null)
        //    {
        //        tips_ui.ShowTips(str);
        //    }
        //    else
        //    {
        //        UIMgr.singleton.Open("MessageTipsUI", str);
        //    }
        //}

        //显示确认提示框
        //public void ShowMesaageBox(Action onFinish, string tips, params object[] args)
        //{
        //    var str = LanguageMgr.GetText(tips, args);
        //    var tips_ui = UIMgr.singleton.FindUIObject<MessageBoxUI>();
        //    if (tips_ui == null)
        //    {
        //        tips_ui = UIMgr.singleton.Open("MessageBoxUI") as MessageBoxUI;

        //    }
        //    tips_ui.ShowMessage(str, onFinish);
        //}

        public bool AddRedTips(RedTipsType type, RedTipsUI ui)
        {
            if (closeTipsList.Contains(type))
                return false;

            if (!redTipsDic.ContainsKey(type))
                redTipsDic[type] = new List<RedTipsUI>();

            redTipsDic[type].Add(ui);

            return CheckRedTips(type);
        }

        public void CloseTips(RedTipsType type)
        {
            if (!redTipsDic.ContainsKey(type))
                return;

            closeTipsList.Add(type);

            RefreshRedTipsUI(true);

            //redTipsDic.Remove(type);
        }

        public bool CheckRedTips(RedTipsType type)
        {
            if (closeTipsList.Contains(type))
                return false;

            return GetRedTipsCount(type) > 0;
        }

        public int GetRedTipsCount(RedTipsType type)
        {
            int count;
            if (!redTipsCountBuffer.TryGetValue(type, out count))
                count = 0;

            return count;
        }

        private int CheckRedTipsCount(RedTipsType type)
        {
            switch (type)
            {
                case RedTipsType.IdleReward:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.IdleReward))
                            return 0;

                        if (IdleRewardMgr.singleton.GetProcess() >= 0.5f)
                        {
                            return 1;
                        }
                    }
                    break;
                case RedTipsType.QuickIdle:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.IdleReward))
                            return 0;

                        int cost;
                        if (IdleRewardMgr.singleton.GetQuickIdleCost(out cost) && cost <= 0)
                            return 1;
                    }
                    break;
                case RedTipsType.CampsiteSetGun:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.BuildSetGun))
                            return 0;

                        if (CampsiteMgr.singleton.HasPointCanSetGun())
                            return 1;
                    }
                    break;
                case RedTipsType.ShopOneBox:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.ShopTab))
                            return 0;

                        int count = 0;
                        foreach (Box_TableItem item in TableMgr.singleton.BoxTable.getEnumerator())
                        {
                            count += PlayerDataMgr.singleton.GetCurrency((CurrencyType)item.key);
                        }
                        return count;
                    }
                case RedTipsType.ShopTenBox:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.ShopTab))
                            return 0;
                        int count = 0;
                        foreach (Box_TableItem item in TableMgr.singleton.BoxTable.getEnumerator())
                        {
                            count += PlayerDataMgr.singleton.GetCurrency((CurrencyType)item.key) / 10;
                        }
                        return count;
                    }
                case RedTipsType.NewCard:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunTab))
                            return 0;
                        return PlayerDataMgr.singleton.GetNewCardCount();
                    }
                case RedTipsType.LvUpCard:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunTab))
                            return 0;
                        return PlayerDataMgr.singleton.GetLvUpCardCount();
                    }
                case RedTipsType.StarUpCard:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunTab))
                            return 0;
                        foreach (var card in PlayerDataMgr.singleton.DB.cardDatas)
                        {
                            if (PlayerDataMgr.singleton.CanCardStarUp(card.Key))
                                return 1;
                        }
                    }
                    break;
                case RedTipsType.FuseCard:
                    {
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunTab))
                            return 0;
                        if (!PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunFuse))
                            return 0;
                        if (PlayerDataMgr.singleton.CanFuseCard())
                            return 1;
                    }
                    break;
                case RedTipsType.StarReward:
                    {
                        if (PlayerDataMgr.singleton.GetChapterStar() / 10 > PlayerDataMgr.singleton.DB.chapterData.starList.Count)
                            return 1;
                    }
                    break;
            }

            return 0;
        }

        private void RefreshRedTipsData()
        {
            foreach (RedTipsType tipsType in Enum.GetValues(typeof(RedTipsType)))
            {
                if (tipsType == RedTipsType.None)
                    continue;
                int count = CheckRedTipsCount(tipsType);
                int countBuffer;
                if (!redTipsCountBuffer.TryGetValue(tipsType, out countBuffer) || count > countBuffer)
                {
                    closeTipsList.Remove(tipsType);
                }
                redTipsCountBuffer[tipsType] = count;
            }
        }

        private void RefreshRedTipsUI(bool forceAll = false)
        {
            foreach (RedTipsType tipsType in Enum.GetValues(typeof(RedTipsType)))
            {
                if (tipsType == RedTipsType.None)
                    continue;
                if (closeTipsList.Contains(tipsType) && !forceAll)
                    continue;
                List<RedTipsUI> uiList;
                if (redTipsDic.TryGetValue(tipsType, out uiList) && uiList != null)
                {
                    foreach (var tip in uiList)
                    {
                        if (tip == null)
                            continue;

                        tip.UpdateRed();
                    }
                }
            }
        }

        public void UpdateRedTips()
        {
            RefreshRedTipsData();
            RefreshRedTipsUI();
        }
    }
}