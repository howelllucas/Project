using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class FightResult
    {
        private int m_AdDelay = -1;
        private List<GameObject> curStarList = new List<GameObject>();
        private List<Text> starTextList = new List<Text>();

        private void Update()
        {
            if (m_AdDelay > 0)
            {
                m_AdDelay--;
                if (m_AdDelay == 0)
                {
                    m_AdDelay = -1;
                    DelayCompleteAd();
                }
            }
        }
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            curStarList.Clear();
            curStarList.Add(Star1.gameObject);
            curStarList.Add(Star2.gameObject);
            curStarList.Add(Star3.gameObject);

            starTextList.Clear();
            starTextList.Add(StarText1.text);
            starTextList.Add(StarText2.text);
            starTextList.Add(StarText3.text);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            FlushPanelInfo();

            Btn1.button.onClick.AddListener(OnclickMultAward);
            Btn2.button.onClick.AddListener(OnClickConfirm);
            Btn2.gameObject.SetActive(false);

            float delaySec = float.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.FIGHT_RESULT_BTN_DELAY_SEC).content);
            gameObject.AddComponent<DelayCallBack>().SetAction(()=> {Btn2.gameObject.SetActive(true); }, delaySec, true);
            waitAdTxt.gameObject.SetActive(false);
            base.ChangeLanguage();
        }

        private void FlushPanelInfo()
        {

            //CmNum3.text.text = UiTools.FormateMoneyUP(tmp * FightResultManager.MULT_PARAM);

            //var playerGo = Global.gApp.CurScene.GetMainPlayer();
            //if (playerGo == null)
            //    return;

            //var player = playerGo.GetComponent<Player>();
            //var curLife = player.GetPlayerData().GetHp();
            //var maxLife = player.GetPlayerData().GetMaxHp();
            //var reviveCount = player.GetPlayerData().GetReviveTimes();

            var stageData = PlayerDataMgr.singleton.GetStageData(PlayerDataMgr.singleton.EnterStageID);
            if (stageData == null)
                return;
            
                var starList = stageData.starList;
                ShowStars(starList);
                //CmNum.text.text = "";
            
            //else
            //{
                double gold = FightResultManager.instance.AwardGold;
                CmNum.text.text = UiTools.FormateMoneyUP(gold);

                //Game.PlayerDataMgr.singleton.RequestFinishStage((int)(gold), (b) =>
                //{
                //    if (b)
                //    {
                //        var starList = Game.PlayerDataMgr.singleton.GetStageStarList(true, curLife / maxLife, reviveCount);
                //        ShowStars(starList);
                //    }
                //});
            //}
        }

        private void ShowStars(List<int> starList)
        {
            if (starList == null)
                return;

            var lvRes = TableMgr.singleton.LevelTable.GetItemByID(PlayerDataMgr.singleton.EnterStageID);
            if (lvRes == null)
                return;


            for (int i = 0; i < lvRes.starList.Length;++i)
            {
                var res = TableMgr.singleton.LevelStarRateTable.GetItemByID(lvRes.starList[i]);
                if (res == null)
                    return;

                starTextList[i].text = LanguageMgr.GetText(res.tid_name, lvRes.cardLevel);

                if (starList.Contains(res.id))
                    curStarList[i].SetActive(true);
                else
                    curStarList[i].SetActive(false);  
            }

        }

        private void OnclickMultAward()
        {

            PassItem passItem = Global.gApp.CurScene.GetPassData();
            if ((SceneType)passItem.sceneType == SceneType.NormalScene)
            {
                //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_BALANCE_AD);
            } else
            {
                //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_SPECIAL_BALANCE_AD);
            }
            
            Btn1.button.enabled = false;
            Btn2.button.enabled = false;
            waitAdDelay.gameObject.AddComponent<DelayCallBack>().SetAction(() => { waitAdTxt.gameObject.SetActive(true); }, 2f, true);
            waitAdDelay.gameObject.AddComponent<DelayCallBack>().SetAction(()=> { Btn2.button.enabled = true; }, 5f, true);
            //调用广告
            AdManager.instance.ShowRewardVedio(CompleteAd, AdShowSceneType.BALANCE, 0, 0, 0);
        }

        private void DelayCompleteAd()
        {
            FightResultManager.instance.SetRewardType(FightResultManager.RewardType.AD);
            //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.8f);
            CompleteSelect(FightResultManager.MULT_PARAM);
            Vector3 v3 = new Vector3(AdIcon.rectTransform.position.x, AdIcon.rectTransform.position.y + 0f, AdIcon.rectTransform.position.z);
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.GOLD, 35, v3);
        }

        private void CompleteAd(bool res)
        {
            if (res)
            {
                m_AdDelay = 3;
            } else
            {

                DelayCallBack[] dcbs = waitAdDelay.gameObject.GetComponents<DelayCallBack>();
                if (dcbs != null && dcbs.Length > 0)
                {
                    foreach (DelayCallBack dcb in dcbs)
                    {
                        Destroy(dcb);
                    }
                }
                waitAdTxt.gameObject.SetActive(false);
                Btn1.button.enabled = true;
                Btn2.button.enabled = true;

                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3040);
            }
            
        }

        private void OnClickConfirm()
        {
            PassItem passItem = Global.gApp.CurScene.GetPassData();
            if ((SceneType)passItem.sceneType == SceneType.NormalScene)
            {
                //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_BALANCE_NORMAL);
            }
            else
            {
                //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_SPECIAL_BALANCE_NORMAL);
            }
            AdManager.instance.DeleteVedioCall();
            FightResultManager.instance.SetRewardType(FightResultManager.RewardType.NONE);
            Btn1.button.enabled = false;
            Btn2.button.enabled = false;
            //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.8f);
            //Vector3 v3 = new Vector3(NormalIcon.rectTransform.position.x, NormalIcon.rectTransform.position.y + 0f, NormalIcon.rectTransform.position.z);
            //Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.GOLD, 18, v3);
            CompleteSelect();
        }

        private void CompleteSelect(int award = 1)
        {
            //FightResultManager.instance.SelectAward(award);
            Global.gApp.gGameCtrl.ChangeToMainScene(2);

            Global.gApp.gAudioSource.PlayOneShot("fight_result",true);

            base.TouchClose();
        }
    }

}