using DG.Tweening;
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class GetRewardUI
    {
        private float mCoin;
        private int mAdTimes = 3;

        private ItemDTO m_ItemDTO;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            InitCoinInfo();
            Btn1.button.onClick.AddListener(OnClickOpen);
            Btn2.button.onClick.AddListener(OnClickAdOpen);

            base.ChangeLanguage();
        }

        private void InitCoinInfo()
        {
            FallBox_dataItem config = Global.gApp.gGameData.FallBoxDataConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            mCoin = config.coin;
            CmNum.text.text = UiTools.FormateMoneyUP(mCoin);
            CmNumAd.text.text = UiTools.FormateMoneyUP(mCoin * mAdTimes);

            airdropicon_close.gameObject.SetActive(true);
            airdropicon_open.gameObject.SetActive(false);
        }

        private void OnClickClokse()
        {
            base.TouchClose();
        }


        private void OnClickOpen()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.ONLINE_REWARD_NORMAL);
            Btn1.button.enabled = false;
            ReceiveAward(1);
        }

        private void OnClickAdOpen()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.ONLINE_REWARD_AD);
            Btn2.button.enabled = false;
            //观看广告
            AdManager.instance.ShowRewardVedio(CompleteAd, AdShowSceneType.GET_REWARD, 0, 0, 0);
            //CompleteAd(true);
        }

        private void CompleteAd(bool res)
        {
            if (res)
            {
                ReceiveAward(mAdTimes);
            }
            
        }

        private void ReceiveAward(int times)
        {
            m_ItemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, mCoin * times, BehaviorTypeConstVal.OPT_RECEIVE_SUPPLY);
            m_ItemDTO.paramStr1 = times.ToString();
            CommonUI commonUi = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            if (commonUi != null)
            {
                //commonUi.UnLockById(m_ItemDTO.itemId, false, false);
            }
            GameItemFactory.GetInstance().AddItem(m_ItemDTO);

            //Tweener tweener = airdropicon_close.rectTransform.DOShakeScale(1f, new Vector3(0.2f, 0.2f, 0.2f), 6, 90);
            Tweener tweener = airdropicon_close.rectTransform.DOPunchRotation(new Vector3(0, 0, 7), 0.6f, 40, 1);
            tweener.SetEase(Ease.OutElastic);
            tweener.SetLoops(1);
            tweener.OnComplete(CompleteOpen);
            tweener.Play();

        }

        private void CompleteOpen()
        {
            Btn1.gameObject.SetActive(false);
            Btn2.gameObject.SetActive(false);
            airdropicon_close.gameObject.SetActive(false);
            airdropicon_open.gameObject.SetActive(true);
            //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.8f);
            Vector3 pos = new Vector3(airdropicon_close.rectTransform.position.x, airdropicon_close.rectTransform.position.y + 1.5f, airdropicon_close.rectTransform.position.z);
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, m_ItemDTO.itemId, (int)m_ItemDTO.num, pos);

            Global.gApp.gAudioSource.PlayOneShot("coin_box",true);
            GeneralConfigItem generalConfigItem = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.OPEN_BOX_DELAY_SECOND);
            int delay = int.Parse(generalConfigItem.content);
            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            {
                TouchClose();
            }, delay, true);
        }
    }
}
