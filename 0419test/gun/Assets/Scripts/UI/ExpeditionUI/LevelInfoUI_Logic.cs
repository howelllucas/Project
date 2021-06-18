using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using UnityEngine.UI;
using System.Numerics;

namespace EZ
{

    public partial class LevelInfoUI
    {
        private List<Text> starTextList = new List<Text>();
        private List<GameObject> curStarList = new List<GameObject>();
        private Level_TableItem levelRes;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);


            levelRes = arg as Level_TableItem;
            if (levelRes != null)
            {
                ShowStars();
            }

        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            RegisterListeners();

            starTextList.Clear();
            starTextList.Add(StarText1.text);
            starTextList.Add(StarText2.text);
            starTextList.Add(StarText3.text);

            curStarList.Clear();
            curStarList.Add(Star1.gameObject);
            curStarList.Add(Star2.gameObject);
            curStarList.Add(Star3.gameObject);

            CloseBtn.button.onClick.AddListener(TouchClose);
            StartBtn.button.onClick.AddListener(OnStartClick);
        }

        private void RegisterListeners()
        {
            //Global.gApp.gMsgDispatcher.AddListener(MsgIds.StarReward, ShowRewardInfo);
        }

        private void UnRegisterListeners()
        {
            //Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.StarReward, ShowRewardInfo);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void ShowStars()
        {
            if (levelRes == null)
                return;

            var stageData = PlayerDataMgr.singleton.GetStageData(levelRes.id);
            if (stageData == null)
                return;

            LevelTitle.text.text = LanguageMgr.GetText("Explore_RewardPage_Title", levelRes.id);
            BigInteger gold = levelRes.gold;
            GoldCount.text.text = string.Format("x{0}", gold.ToSymbolString());

            if (levelRes.reward > 0)
            {
                LevelReward.gameObject.SetActive(true);
                LevelRewardIcon.image.sprite = GameGoodsMgr.singleton.GetGameGoodsIcon((GoodsType)levelRes.reward);
                LevelRewardCount.text.text = string.Format("x{0}", levelRes.reward_count);
            }
            else
            {
                LevelReward.gameObject.SetActive(false);
            }

            for (int i = 0; i < levelRes.starList.Length; ++i)
            {
                var res = TableMgr.singleton.LevelStarRateTable.GetItemByID(levelRes.starList[i]);
                if (res == null)
                    return;

                starTextList[i].text = LanguageMgr.GetText(res.tid_name, levelRes.cardLevel);

                if (stageData.starList.Contains(res.id))
                    curStarList[i].SetActive(true);
                else
                    curStarList[i].SetActive(false);
            }

        }

        private void OnStartClick()
        {
            if (PlayerDataMgr.singleton.EnterStage(levelRes.id))
            {
                Global.gApp.gUiMgr.ClossAllPanel();
                Global.gApp.gGameCtrl.ChangeToFightScene(levelRes.id);
            }
            else
            {
                Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, "Stay tuned ");
            }
        }
    }
}