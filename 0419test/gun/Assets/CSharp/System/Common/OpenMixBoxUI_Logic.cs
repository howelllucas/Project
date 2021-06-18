using UnityEngine;
using System.Collections;
using EZ.DataMgr;
using EZ.Data;
using System.Collections.Generic;
using System;
using DG.Tweening;
using UnityEngine.Events;

namespace EZ
{
    public partial class OpenMixBoxUI
    {
        private List<ItemDTO> m_ItemDTOs;
        private List<OpenMixBoxUI_card> m_Cards;

        private float m_Delay = 0.3f;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            m_ItemDTOs = arg as List<ItemDTO>;
            m_Cards = new List<OpenMixBoxUI_card>();

            box_open.gameObject.SetActive(false);
            box_close.gameObject.SetActive(true);
            card.gameObject.SetActive(false);
            for (int i = m_ItemDTOs.Count - 1; i >= 0; i --)
            {
                ItemDTO itemDTO = m_ItemDTOs[i];
                OpenMixBoxUI_card cardUI = card.GetInstance();
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(itemDTO.itemId);
                cardUI.num.text.text = UiTools.FormateMoneyUP(itemDTO.num);
                cardUI.icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemDTO.itemId));
                m_Cards.Add(cardUI);
                cardUI.gameObject.SetActive(true);
                cardUI.Show.gameObject.SetActive(false);
                cardUI.title.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(itemCfg.sourceLanguage);
            }

            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            {
                OnOpen();
            }, m_Delay, true);
            RegisterListeners();

            base.ChangeLanguage();
        }
        public void ChangeBoxImage(int index)
        {
            box_open.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.BOX_OPEN_PATH, index));
            box_close.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.BOX_CLOSE_PATH, index));
        }

        private void OnOpen()
        {
            Tweener tweener = box_close.rectTransform.DOPunchRotation(new Vector3(0, 0, 7), 1f, 40, 1);
            tweener.SetEase(Ease.OutCubic);
            tweener.SetDelay(0.5f);
            tweener.SetLoops(1);
            tweener.OnComplete(CompleteOpen);
            tweener.Play();

            //CompleteOpen();
        }

        private void CompleteOpen()
        {
            for (int i = m_Cards.Count - 1; i >= 0; i --)
            {
                OpenMixBoxUI_card cardUI = m_Cards[i];
                if (i < m_Cards.Count - 1)
                {
                    gameObject.AddComponent<DelayCallBack>().SetAction(() =>
                    {
                        cardUI.Show.gameObject.SetActive(true);
                    }, m_Delay * (m_Cards.Count - i - 1), true);
                } else
                {
                    cardUI.Show.gameObject.SetActive(true);
                }

            }
            box_open.gameObject.SetActive(true);
            box_close.gameObject.SetActive(false);
            Global.gApp.gAudioSource.PlayOneShot("box",true);
            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
                Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            }, m_Delay * (m_Cards.Count - 0.7f), true);
        }

        public override void TouchClose()
        {
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

