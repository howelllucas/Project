using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class StarRewardUI_StarReward
    {
        private LevelStarReward_TableItem starRewardRes;

        private void Awake()
        {
            InitNode();
        }

        public void Init(LevelStarReward_TableItem res)
        {
            starRewardRes = res;

            StarCount.text.text = starRewardRes.id.ToString();
            RewardIcon.image.sprite = GameGoodsMgr.singleton.GetGameGoodsIcon((GoodsType)starRewardRes.reward);
            RewardNum.text.text = string.Format("x{0}", starRewardRes.reward_count); 

            var starCount = PlayerDataMgr.singleton.GetChapterStar();
            if (PlayerDataMgr.singleton.DB.chapterData.starList.Contains(starRewardRes.id))
            {
                RewardBtn.gameObject.SetActive(false);
                GetFrame.gameObject.SetActive(true);
            }
            else
            {
                GetFrame.gameObject.SetActive(false);
                RewardBtn.gameObject.SetActive(true);
                if (starCount >= starRewardRes.id)
                    m_UnfinishImg.gameObject.SetActive(false);
                else
                    m_UnfinishImg.gameObject.SetActive(true);
            }
        }
        private void InitNode()
        {
            RewardBtn.button.onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            PlayerDataMgr.singleton.ReceiveChapterStarReward(starRewardRes.id, ()=> {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowRewardGetEffect, (GoodsType)starRewardRes.reward,
                                    starRewardRes.reward_count, gameObject.transform.position);
            });
        }
    }
}