using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampsiteCardData
    {
        private CampsitePointMgr targetPoint;
        private int cardId;

        private void Awake()
        {
            AutoBtn.button.onClick.AddListener(OnAutoBtnClick);
        }

        private void OnEnable()
        {
            //Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.CampsitePointDataChange, OnCampsitePointDataChange);
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.GunCardDataChange, OnCardDataChange);
        }

        private void OnDisable()
        {
            //Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CampsitePointDataChange, OnCampsitePointDataChange);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.GunCardDataChange, OnCardDataChange);
        }

        public void Init(CampsitePointMgr targetPoint, int cardId)
        {
            this.targetPoint = targetPoint;
            this.cardId = cardId;

            RefreshData(targetPoint, cardId);
        }

        private void RefreshData(CampsitePointMgr targetPoint, int cardId)
        {
            var gunRes = TableMgr.singleton.GunCardTable.GetItemByID(cardId);
            CardData.gameObject.SetActive(true);
            CardData.GunUI_GunCard.Init(gunRes);

            int skillId = PlayerDataMgr.singleton.GetCampSkillID(cardId);
            CampSkill.CampSkill.Init(skillId);
            CampSkill.CampSkill.SetValid(targetPoint.CheckIsValidSkill(skillId));
            AutoTipTxt.text.text = LanguageMgr.GetText("CampDetail_Tips_Auto", targetPoint.AutoLv);
            if (targetPoint.CheckCardIsAuto(cardId))
            {
                UIGray.Recovery(AutoIcon.image);
                AutoTxt.text.text = LanguageMgr.GetText("CampDetail_Rule_AutoOpen");
                AutoTxt.text.color = Color.green;
                AutoTipTxt.text.color = Color.green;
            }
            else
            {
                UIGray.SetUIGray(AutoIcon.image);
                AutoTxt.text.text = LanguageMgr.GetText("CampDetail_Rule_AutoClose");
                AutoTxt.text.color = Color.red;
                AutoTipTxt.text.color = Color.red;
            }

            double rewardFactor;
            float intervalFactor;
            targetPoint.GetCardTotalFactorOnPoint(cardId, out rewardFactor, out intervalFactor);
            RewardFactorTxt.text.text = string.Format("x{0}", UiTools.FormateMoney(rewardFactor));
            if (Mathf.Approximately(intervalFactor, 1f))
            {
                IntervalFactorNode.gameObject.SetActive(false);
            }
            else
            {
                IntervalFactorTxt.text.text = string.Format("-{0:#0.##%}", intervalFactor);
            }

        }

        private void OnCampsitePointDataChange(int pointIndex)
        {
            if (targetPoint != null && targetPoint.index == pointIndex)
            {
                RefreshData(targetPoint, cardId);
            }
        }

        private void OnCardDataChange(int cardId)
        {
            if (targetPoint != null && cardId == this.cardId)
            {
                RefreshData(targetPoint, cardId);
            }
        }

        private void OnAutoBtnClick()
        {
            AutoTip.gameObject.SetActive(!AutoTip.gameObject.activeSelf);
        }

        public void RegisterSkillClickListener(System.Action<CampSkill> callback)
        {
            CampSkill.CampSkill.onClick -= callback;
            CampSkill.CampSkill.onClick += callback;
        }
    }
}