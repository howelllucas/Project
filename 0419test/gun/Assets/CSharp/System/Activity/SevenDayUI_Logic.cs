using UnityEngine;
using System.Collections;
using EZ.DataMgr;
using System.Collections.Generic;
using EZ.Data;
using UnityEngine.UI;
using EZ.Util;

namespace EZ
{
    public partial class SevenDayUI
    {
        private RectTransform_Image_Container m_Cur;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            NormalBtn.button.onClick.AddListener(OnNormal);
            AdBtn.button.onClick.AddListener(WatchADs);
            Btn1.button.onClick.AddListener(TouchClose);
            RegisterListeners();
            FreshWeek();

            base.ChangeLanguage();
        }
        public override void TouchClose()
        {
            Global.gApp.gUiMgr.ClosePanel(Wndid.SevenDayPanel);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }


        private void FreshWeek()
        {
            int curIndex = GetShowIndex();
            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LOGIN];
            for (int i = (curIndex / 7) * 7; i < (curIndex / 7 + 1) * 7; i++)
            {
                QuestItemDTO questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(configs[i].quest_id);

                RectTransform_Image_Container day = ReflectionUtil.GetValueByProperty<SevenDayUI, RectTransform_Image_Container>("day" + (i % 7 + 1), this);

                //20191021 陈冬要求不读取配置图片
                //Image awardIcon = day.rectTransform.GetChild(1).GetComponent<Image>();
                //awardIcon.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(configs[i].awardIcon);

                //显示物品数目
                QuestItem config = Global.gApp.gGameData.QuestData.Get(configs[i].quest_id);

                ItemItem questAward = Global.gApp.gGameData.ItemData.Get((int)configs[i].award[0]);
                if (ItemTypeConstVal.isWeapon(questAward.showtype))
                {
                    day.rectTransform.Find("m_rewardGun").GetComponent<Image>().sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(questAward.image_grow);
                    day.rectTransform.Find("m_rewardGun").gameObject.SetActive(true);
                    day.rectTransform.Find("m_reward").gameObject.SetActive(false);
                    day.rectTransform.Find("m_Num").gameObject.SetActive(false);
                    if (questItemDTO.state == QuestStateConstVal.CAN_RECEIVE)
                    {
                        Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest(configs[i].quest_id, 1, BehaviorTypeConstVal.OPT_SEVEN_DAY_LOGIN);
                        questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(configs[i].quest_id);
                        AfLog(configs[i].quest_id);
                    }
                    Transform quality = day.rectTransform.Find("m_rewardGun").transform.Find("quality");
                    if (quality != null)
                    {
                        if (questAward.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
                        {
                            quality.gameObject.GetComponent<Image>().sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, questAward.qualevel));
                        } else
                        {
                            quality.gameObject.GetComponent<Image>().sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2));
                        }
                        
                    }
                    Transform effect = day.rectTransform.Find("m_rewardGun").transform.Find("effect");
                    if (questItemDTO.state != QuestStateConstVal.RECEIVED && effect != null)
                    {
                        EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.QUALITY);
                        string effectName = questAward.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? questAward.qualevel.ToString() : "common";
                        GameObject effectObj = UiTools.GetEffect(string.Format(effectItem.path, effectName), effect);
                        effectObj.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                        ParticleSystem[] pss = effectObj.GetComponentsInChildren<ParticleSystem>();
                        foreach (ParticleSystem ps in pss)
                        {
                            ps.GetComponent<Renderer>().sortingOrder = 45;
                        }
                    }
                } else
                {
                    day.rectTransform.Find("m_Num").gameObject.SetActive(true);
                    day.rectTransform.Find("m_rewardGun").gameObject.SetActive(false);
                    day.rectTransform.Find("m_reward").gameObject.SetActive(true);
                }

                day.rectTransform.Find("m_Num").GetComponent<Text>().text = UiTools.FormateMoneyUP(config.award[1]);

                //已经领取
                day.rectTransform.Find("m_getMask").gameObject.SetActive(questItemDTO.state == QuestStateConstVal.RECEIVED);

                //当前框
                day.rectTransform.Find("m_current").gameObject.SetActive(i == curIndex);
                if (i == curIndex)
                {
                    m_Cur = day;
                }
                if (i == curIndex && questItemDTO.state != QuestStateConstVal.CAN_RECEIVE)
                {
                    NormalBtn.button.interactable = false;
                    AdBtn.button.interactable = false;
                }
            }
        }

        private int GetShowIndex()
        {
            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LOGIN];
            int cur = configs.Count - 1;
            for (int i = 0; i < configs.Count; i++)
            {
                QuestItemDTO questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(configs[i].quest_id);
                if (questItemDTO.cur == configs[i].condition[1])
                {
                    cur = i;
                    break;
                } else if (questItemDTO.state == QuestStateConstVal.CAN_RECEIVE)
                {
                    Global.gApp.gSystemMgr.GetQuestMgr().MissQuestReceive(configs[i].quest_id);
                }
            }

            return cur;
        }

        

        private void OnReceiveWeek(int questId, int times)
        {
            Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest(questId, times, BehaviorTypeConstVal.OPT_SEVEN_DAY_LOGIN, m_Cur.rectTransform.position);
            AfLog(questId);
            FreshWeek();
        }

        private static void AfLog(int questId)
        {
            QuestItem questCfg = Global.gApp.gGameData.QuestData.Get(questId);
            if (questCfg != null && questCfg.type == QuestConst.TYPE_LOGIN && questCfg.condition[0] == FilterTypeConstVal.SUM_LOGIN_DAY)
            {
                if (questCfg.condition[1] == 1)
                {
                    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_checkin_1st);
                }
                else if (questCfg.condition[1] == 2)
                {
                    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_checkin_2nd);
                }
                else if (questCfg.condition[1] == 3)
                {
                    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_checkin_3rd);
                }
                else if (questCfg.condition[1] == 5)
                {
                    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_checkin_5th);
                }
                else if (questCfg.condition[1] == 7)
                {
                    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_checkin_7th);
                }
                else if (questCfg.condition[1] == 14)
                {
                    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_checkin_14th);
                }
                else if (questCfg.condition[1] == 30)
                {
                    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_checkin_30th);
                }

            }
        }

        private void OnNormal()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.SEVEN_DAY_NORMAL);
            int questId = Global.gApp.gSystemMgr.GetQuestMgr().GetShouldReceiveId4SevenDay();
            if (questId == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3037);
                return;
            }
            OnReceiveWeek(questId, 1);
        }

        private void WatchADs()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.SEVEN_DAY_AD);
            int questId = Global.gApp.gSystemMgr.GetQuestMgr().GetShouldReceiveId4SevenDay();
            if (questId == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3037);
                return;
            }

            //观看广告
            AdManager.instance.ShowRewardVedio(CompleteAd, AdShowSceneType.SEVEN_DAY, 0, 0, 0);
            //CompleteAd(true);
        }

        private void CompleteAd(bool res)
        {
            if (res)
            {
                int questId = Global.gApp.gSystemMgr.GetQuestMgr().GetShouldReceiveId4SevenDay();
                OnReceiveWeek(questId, 2);
            }
            
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
