using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class FightResultManager
    {
        public enum RetryType
        {
            NONE = 0,
            MDT = 1,
            AD = 2
        }

        public enum RewardType
        {
            NONE = 0,
            MDT = 1,
            AD = 2
        }
        public enum FightState
        {
            FAIL = 0,
            SUCCESS = 1,
            QUIT = 2
        }

        private static FightResultManager mInstance;
        public static FightResultManager instance
        {
            get
            {
                if (mInstance == null) mInstance = new FightResultManager();
                return mInstance;
            }
        }

        public double AwardGold { get; private set; }
        public float KillProgress { get; set; }
        private bool m_Win = false;
        public const int MULT_PARAM = 3;
        public const int MULT_ITEMID = 7005;   //  多倍消耗道具ID
        public const int MULT_ITEMCOUNT = 1;   //  多倍消耗道具数量


        private Action<bool> mReviveResult = null;
        public const int REVIVE_ITEMID = 7005;  //  复活消耗道具ID
        public static int REVIVE_ITEMCOUNT = 1;  //  复活消耗道具数量


        private int m_RetryType;
        private int m_RewardType;
        private int m_FightState;
        private int m_GetCoin;


        public FightResultManager()
        {
            GeneralConfigItem reviveCostCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.REVIVE_MDT);
            REVIVE_ITEMCOUNT = int.Parse(reviveCostCfg.content);
        }

        public void ShowPausePanel()
        {

            //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_PAUSE);
            Global.gApp.gAudioSource.Pause(); Global.gApp.gUiMgr.OpenPanel(Wndid.FightPausePanel);
        }

        public void ShowReivePopup(Action<bool> result)
        {
            mReviveResult = result;
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightRespawnPanel);
        }

        public void ShowWinPopup(double gold)
        {
            m_Win = true;
            AwardGold = gold;
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightWinPanel);
            FightWin panel = Global.gApp.gUiMgr.GetPanelCompent<FightWin>(Wndid.FightWinPanel);
            panel.OnClose += () =>
            {
                ShowResultPopup();

                //弹出app评分
                //if (Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() ==
                //    int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.PULL_UP_APP_REVIEW_STAGE).content) + 1)
                //{
                //    //#if UNITY_IPHONE
                //    //            //sdk
                //    //                SdkdsNativeUtil.Instance.pullupAppReview();
                //    //#else
                //    Global.gApp.gUiMgr.OpenPanel(Wndid.EvaluateUI);
                //    //#endif
                //}
            };
        }
        public void ShowLosePopup(double gold)
        {
            m_Win = false;
            AwardGold = gold;
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightLosePanel);
            FightLose panel = Global.gApp.gUiMgr.GetPanelCompent<FightLose>(Wndid.FightLosePanel);
            //panel.OnClose += ShowResultPopup;
            panel.OnClose += () =>
            {
                Global.gApp.gGameCtrl.ChangeToMainScene(2);
            };
        }

        public void ShowResultPopup()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightResultPanel);
        }

        public void SelectAward(int mult)
        {
            ItemDTO itemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, AwardGold * mult, BehaviorTypeConstVal.OPT_BALANCE);
            itemDTO.paramStr1 = mult.ToString();
            GameItemFactory.GetInstance().AddItem(itemDTO);
            m_GetCoin = (int)itemDTO.num;
            AwardGold = 0;
            if (m_Win)
            {
                PassItem passItem = Global.gApp.CurScene.GetPassData();
                ItemDTO item1DTO = new ItemDTO(SpecialItemIdConstVal.EXP, passItem.levelEXP, BehaviorTypeConstVal.OPT_BALANCE);
                GameItemFactory.GetInstance().AddItem(item1DTO);
                AddFightItem(ref SpecialItemIdConstVal.FIGIT_WIN_RES);
                m_Win = false;
            }
            else
            {
                PassItem passItem = Global.gApp.CurScene.GetPassData();
                float worldTime = (Global.gApp.CurScene as FightScene).GetCurTime();
                string totalTimeStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.FAIL_STAGE_EXPBASE_TIME).content;
                string expPercentageStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.FAIL_STAGE_EXP_PRAM).content;
                float totalTime = 1;
                float.TryParse(totalTimeStr, out totalTime);
                float expPercentage = 0;
                float.TryParse(expPercentageStr, out expPercentage);
                float newExp = expPercentage * (Mathf.Min(worldTime / totalTime, 1)) * passItem.levelEXP;
                ItemDTO item1DTO = new ItemDTO(SpecialItemIdConstVal.EXP, newExp, BehaviorTypeConstVal.OPT_BALANCE);
                GameItemFactory.GetInstance().AddItem(item1DTO);
            }
            AddFightItem(ref SpecialItemIdConstVal.FIGIT_END_RES);
            foreach (int key in Global.gApp.CurScene.GetWaveMgr().GetCurKillMonsterInfo().Keys)
            {
                int value = Global.gApp.CurScene.GetWaveMgr().GetCurKillMonsterInfo()[key];
                //总的
                Global.gApp.gSystemMgr.GetNpcMgr().NpcQuestChange(FilterTypeConstVal.KILL_ZOMBIE, 0d, value);
                //指定怪的
                Global.gApp.gSystemMgr.GetNpcMgr().NpcQuestChange(FilterTypeConstVal.KILL_ZOMBIE, key, value);
            }
            Global.gApp.gGameCtrl.ChangeToMainScene(2);
        }

        private void AddFightItem(ref int[] itemArray)
        {
            foreach (int itemId in itemArray)
            {
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(itemId);
                string itemName = itemCfg.name;
                double itemCount = Global.gApp.CurScene.GetMainPlayerComp().GetPlayerData().GetDropResCount(itemName);
                if (itemCount > 0)
                {

                    ItemDTO weaponChip = new ItemDTO(itemId, itemCount, BehaviorTypeConstVal.OPT_BALANCE);
                    GameItemFactory.GetInstance().AddItem(weaponChip);
                }

            }


        }
        public void SelectRevive(bool result)
        {
            if (mReviveResult == null) return;
            mReviveResult(result);
            mReviveResult = null;
        }

        public void SetRetryType(RetryType retryType)
        {
            m_RetryType = (int)retryType;
        }
        public int GetRetryType()
        {
            return m_RetryType;
        }

        public void SetRewardType(RewardType rewardType)
        {
            m_RewardType = (int)rewardType;
        }
        public int GetRewardType()
        {
            return m_RewardType;
        }
        public void SetFightState(FightState fightState)
        {
            m_FightState = (int)fightState;
        }
        public int GetFightState()
        {
            return m_FightState;
        }
        public int GetCoin()
        {
            return m_GetCoin;
        }
        public void SetCoin(int coin)
        {
            m_GetCoin = coin;
        }

    }
}