using UnityEngine;
using System.Collections;
using EZ.DataMgr;
using EZ.Data;
using System.Collections.Generic;
using System;
using DG.Tweening;
using UnityEngine.Events;
using EZ.Util;

namespace EZ
{
    public partial class OpenBoxUI
    {
        private ItemDTO m_ItemDTO;

        private const int m_ADTimes = 3;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            m_ItemDTO = arg as ItemDTO;
            ConsumeValue.text.text = "×" + UiTools.FormateMoneyUP(m_ItemDTO.num);
            ConsumeIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, m_ItemDTO.itemId));
            box_open.gameObject.SetActive(false);
            box_close.gameObject.SetActive(true);
            ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(m_ItemDTO.itemId);
            if (itemCfg != null)
            {
                if (itemCfg.openBoxImg != null && itemCfg.openBoxImg != string.Empty)
                {
                    box_open.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(itemCfg.openBoxImg);
                }
                if (itemCfg.closeBoxImg != null && itemCfg.closeBoxImg != string.Empty)
                {
                    box_close.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(itemCfg.closeBoxImg);
                }
            }

            m_UI_openbox_white.gameObject.SetActive(m_ItemDTO.itemId == SpecialItemIdConstVal.ENERGY);
            m_UI_openbox_purple.gameObject.SetActive(m_ItemDTO.itemId == SpecialItemIdConstVal.MDT);
            m_UI_openbox.gameObject.SetActive(m_ItemDTO.itemId != SpecialItemIdConstVal.ENERGY && m_ItemDTO.itemId != SpecialItemIdConstVal.MDT);

            bool haveThree = itemCfg.id == SpecialItemIdConstVal.GOLD;
            Btn1.gameObject.SetActive(haveThree);
            Btn2.gameObject.SetActive(haveThree);
            if (haveThree)
            {
                Btn1.button.onClick.AddListener(OnClick1);
                Btn2.button.onClick.AddListener(OnClick3);
                CmNum.text.text = UiTools.FormateMoney(m_ItemDTO.num);
                CmNumAd.text.text = UiTools.FormateMoney(m_ItemDTO.num * m_ADTimes);
            } else
            {
                gameObject.AddComponent<DelayCallBack>().SetAction(() =>
                {
                    if(m_ItemDTO.param3 > 0)
                    {
                        Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest((int)m_ItemDTO.param3, 1, BehaviorTypeConstVal.OPT_LEVEL_DETAIL);
                    }
                    
                    OnOpen();
                }, 0.8f, true);
            } 


            RegisterListeners();
            base.ChangeLanguage();


        }

        private void OnClick1()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.LEVEL_DETAIL_AWARD_NORMAL);
            Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest((int)m_ItemDTO.param3, 1, BehaviorTypeConstVal.OPT_LEVEL_DETAIL);
            OnOpen();
        }

        private void OnClick3()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.LEVEL_DETAIL_AWARD_AD);
            //调用广告sdk
            AdManager.instance.ShowRewardVedio(CompleteAd, AdShowSceneType.LEVEL_DETAIL_THREE, 0, 0, 0);
        }

        private void CompleteAd(bool result)
        {
            if (result)
            {
                Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest((int)m_ItemDTO.param3, m_ADTimes, BehaviorTypeConstVal.OPT_LEVEL_DETAIL);
                m_ItemDTO.num *= m_ADTimes;
                OnOpen();
            }
            else
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3040);
            }
            
        }

        private void OnOpen()
        {
            Btn1.gameObject.SetActive(false);
            Btn2.gameObject.SetActive(false);
            ConsumeValue.text.text = "×" + UiTools.FormateMoneyUP(m_ItemDTO.num);

            //Tweener tweener = box_close.rectTransform.DOShakeScale(1f, new Vector3(0.08f, 0.08f, 0.08f), 6, 90);
            Tweener tweener = box_close.rectTransform.DOPunchRotation(new Vector3(0, 0, 7), 0.6f, 40, 1);
            tweener.SetEase(Ease.OutCubic);
            tweener.SetLoops(1);
            tweener.OnComplete(CompleteOpen);
            tweener.Play();
        }

        private void CompleteOpen()
        {
            box_open.gameObject.SetActive(true);
            box_close.gameObject.SetActive(false);
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, m_ItemDTO.itemId, (int)m_ItemDTO.num * 2, box_open.rectTransform.position + new Vector3(0, 1.6f, 0));

            GeneralConfigItem generalConfigItem = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.OPEN_BOX_DELAY_SECOND);

            int delay = int.Parse(generalConfigItem.content);
            if (m_ItemDTO.itemId == SpecialItemIdConstVal.GOLD)
            {
                Global.gApp.gAudioSource.PlayOneShot("coin_box",true);
            } else
            {
                Global.gApp.gAudioSource.PlayOneShot("box",true);
            }
            
            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
                TouchClose();
            }, delay, true);
        }

        public override void TouchClose()
        {
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
            base.TouchClose();
        }


        private void RegisterListeners()
        {
        }

        private void UnRegisterListeners()
        {
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}

