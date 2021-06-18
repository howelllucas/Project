using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace EZ
{

    public partial class StarRewardUI
    {
        private List<StarRewardUI_StarReward> rewardItemList = new List<StarRewardUI_StarReward>();

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            ShowRewardInfo();
                   
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            RegisterListeners();

            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.StarReward, ShowRewardInfo);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.StarReward, ShowRewardInfo);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void ShowRewardInfo()
        {
            ClearShowList();

            StarCount.text.text = LanguageMgr.GetText("Explore_StarsPage_Own", PlayerDataMgr.singleton.GetChapterStar());

            foreach (LevelStarReward_TableItem starRes in TableMgr.singleton.LevelStarRewardTable.getEnumerator())
            {
                var rewardItem = StarReward.GetInstance();

                rewardItem.Init(starRes);
                rewardItem.transform.SetParent(StarRewardRoot.gameObject.transform);
                rewardItem.gameObject.SetActive(true);
                rewardItem.transform.SetAsLastSibling();
                rewardItemList.Add(rewardItem);

 
            }

        }

        private void ClearShowList()
        {
            foreach (var obj in rewardItemList)
            {
                StarReward.CacheInstance(obj);
            }
            rewardItemList.Clear();

        }
    }
}