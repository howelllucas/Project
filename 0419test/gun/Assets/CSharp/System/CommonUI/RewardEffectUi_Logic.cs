
using EZ.Data;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{
    public partial class RewardEffectUi
    {
        private Dictionary<GoodsType, int> m_IdEffectCountDic = new Dictionary<GoodsType, int>();
        private float m_RewardEffectRadius = 0.5f;
        private int m_RewardEffectNum = 10;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            base.ChangeLanguage();

            PowerItem.PowerItem.Init();
        }
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            m_IdEffectCountDic.Add(GoodsType.GOLD, 0);
            m_IdEffectCountDic.Add(GoodsType.DIAMOND, 0);
            m_IdEffectCountDic.Add(GoodsType.CAMPSITE_REWARD, 0);
            m_IdEffectCountDic.Add(GoodsType.KEY, 0);
            RegisterListener();
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<GoodsType, int, Vector3>(MsgIds.ShowRewardGetEffect, ShowRewardEffect);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.ShowRewardGetEffect);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.GunCardOpt, ShowPowerEffect);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.GunCardOpt);

        }
        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<GoodsType, int, Vector3>(MsgIds.ShowRewardGetEffect, ShowRewardEffect);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.GunCardOpt, ShowPowerEffect);

        }

        private void ShowRewardEffect(GoodsType goods, int count, Vector3 startPos)
        {
            if (goods == GoodsType.KEY)
            {
                ShowRewardKey(goods, count, startPos);
                return;
            }
            if (goods != GoodsType.GOLD && goods != GoodsType.DIAMOND && goods != GoodsType.CAMPSITE_REWARD)
                return;

            int effectNum = count > m_RewardEffectNum || count < 0 ? m_RewardEffectNum : count;
            TokenUI tokenUi = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            if (tokenUi != null)
            {
                m_IdEffectCountDic[goods] += effectNum;
                tokenUi.UnLockById(goods, false, false);
                for (int i = 0; i < effectNum; i++)
                {
                    RewardEffectUi_RewardItem rewardEffect = RewardItem.GetInstance();
                    float radius = 0f;
                    if (effectNum > 1)
                    {
                        radius = Random.Range(m_RewardEffectRadius * 0.2f, m_RewardEffectRadius);
                    }
                    float angle = (float)Random.Range(0, 360);
                    float x = radius * Mathf.Cos(angle);
                    float y = radius * Mathf.Sin(angle);
                    Vector3 effectPos = new Vector3(startPos.x + x, startPos.y + y, startPos.z);
                    rewardEffect.Init(goods, effectPos, startPos, this, tokenUi);
                }

            }
        }

        private void ShowRewardKey(GoodsType goods, int count, Vector3 startPos)
        {
            if (goods != GoodsType.KEY)
                return;

            int effectNum = count > m_RewardEffectNum || count < 0 ? m_RewardEffectNum : count;
            CommonUI commonUI = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            if (commonUI != null)
            {
                m_IdEffectCountDic[goods] += effectNum;
                //tokenUi.UnLockById(goods, false, false);
                for (int i = 0; i < effectNum; i++)
                {
                    var rewardEffect = RewardKey.GetInstance();
                    float radius = 0f;
                    if (effectNum > 1)
                    {
                        radius = Random.Range(m_RewardEffectRadius * 0.2f, m_RewardEffectRadius);
                    }
                    float angle = (float)Random.Range(0, 360);
                    float x = radius * Mathf.Cos(angle);
                    float y = radius * Mathf.Sin(angle);
                    Vector3 effectPos = new Vector3(startPos.x + x, startPos.y + y, startPos.z);
                    rewardEffect.Init(effectPos, startPos, this, commonUI);
                }

            }
        }

        private void ShowPowerEffect()
        {
            PowerItem.PowerItem.UpdatePower();
        }
        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        public bool EndOne(GoodsType goods)
        {
            int num;
            m_IdEffectCountDic.TryGetValue(goods, out num);
            if (num == 0)
            {
                return true;
            }
            m_IdEffectCountDic[goods] -= 1;
            return m_IdEffectCountDic[goods] == 0;
        }
    }
}
