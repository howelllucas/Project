using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{
    public partial class CardCampDataChangeBubble
    {
        public float showDuration = 5f;

        private float timer;
        private int cardId;
        private CampsitePointMgr campPointMgr;

        private double recordRewardFactor;
        private bool recordAuto;

        public void Init(int cardId, int campPointIndex)
        {
            this.cardId = cardId;
            this.campPointMgr = CampsiteMgr.singleton.GetPointByIndex(campPointIndex);
            RefreshData(false);
        }

        private void RefreshData(bool refreshUI)
        {
            if (campPointMgr == null)
                return;

            double rewardFactor;
            float intervalFactor;
            campPointMgr.GetCardTotalFactorOnPoint(cardId, out rewardFactor, out intervalFactor);

            bool auto = campPointMgr.CheckCardIsAuto(cardId);

            if (refreshUI)
            {
                RewardFactorBefore.text.text = string.Format("x{0}", UiTools.FormateMoney(recordRewardFactor));
                RewardFactorCur.text.text = string.Format("x{0}", UiTools.FormateMoney(rewardFactor));

                if (auto != recordAuto)
                {
                    AutoDataChange.gameObject.SetActive(true);
                    AutoData.gameObject.SetActive(false);
                    AutoValBefore.text.text = LanguageMgr.GetText(recordAuto ? "CardPage_Tips_Open" : "CardPage_Tips_Close");
                    AutoValCur.text.text = LanguageMgr.GetText(auto ? "CardPage_Tips_Open" : "CardPage_Tips_Close");
                }
                else
                {
                    AutoDataChange.gameObject.SetActive(false);
                    AutoData.gameObject.SetActive(true);
                    if (auto)
                    {
                        AutoVal.text.text = LanguageMgr.GetText("CardPage_Tips_Open");
                        AutoLvTip.gameObject.SetActive(false);
                    }
                    else
                    {
                        AutoVal.text.text = LanguageMgr.GetText("CardPage_Tips_Close");
                        AutoLvTip.text.text = LanguageMgr.GetText("CardPage_Tips_Tips", campPointMgr.AutoLv);
                        AutoLvTip.gameObject.SetActive(true);
                    }
                }
            }

            recordRewardFactor = rewardFactor;
            recordAuto = auto;
        }

        public void Show()
        {
            timer = 0;
            RefreshData(true);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= showDuration)
            {
                Close();
            }
        }

    }
}