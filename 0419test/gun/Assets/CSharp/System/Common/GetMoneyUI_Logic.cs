using UnityEngine;
using System.Collections;
using EZ.DataMgr;
using EZ.Data;
using System.Collections.Generic;
using System;

namespace EZ
{
    public partial class GetMoneyUI
    {

        private QuestItemDTO m_questItemDTO;
        private QuestItem m_QuestCfg;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            m_questItemDTO = arg as QuestItemDTO;
            m_QuestCfg = Global.gApp.gGameData.QuestData.Get(m_questItemDTO.id);
            airdropicon.image.sprite = Resources.Load(m_QuestCfg.awardIcon, typeof(Sprite)) as Sprite;
            ItemItem awardCfg = Global.gApp.gGameData.ItemData.Get((int)m_QuestCfg.award[0]);
            if (ItemTypeConstVal.isWeapon(awardCfg.showtype))
            {
                CmIcon.gameObject.SetActive(false);
                CmNum.gameObject.SetActive(false);
            } else
            {
                CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, m_QuestCfg.award[0]));
                CmNum.text.text = UiTools.FormateMoneyUP(m_QuestCfg.award[1]);
            }
            Tips.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(3047), m_QuestCfg.condition[1]);
            Btn1.button.onClick.AddListener(TouchClose);
            RegisterListeners();

            //直接领取
            OnReceive();

            GeneralConfigItem generalConfigItem = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.OPEN_BOX_DELAY_SECOND);
            int delay = int.Parse(generalConfigItem.content);
            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            {
                TouchClose();
            }, delay, true);

            base.ChangeLanguage();
        }

        private void OnReceive()
        {
            airdropicon.image.sprite = Resources.Load(m_QuestCfg.awardOpen, typeof(Sprite)) as Sprite;

            Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest(m_questItemDTO.id, 1, BehaviorTypeConstVal.OPT_LEVEL_DETAIL, Airdropiconbg.rectTransform.position);
        }

        public override void TouchClose()
        {
            Global.gApp.gUiMgr.ClosePanel(Wndid.GetMoneyUI);
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

