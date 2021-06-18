using EZ.Data;
using EZ.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ.DataMgr
{
    //任务mgr
    public class QuestMgr : BaseDataMgr<Dictionary<string, QuestDTO>>
    {
        public QuestMgr()
        {
            OnInit();
        }
        public override void OnInit()
        {
            base.OnInit();
            Init("quest");

            if (m_Data == null)
            {
                m_Data = new Dictionary<string, QuestDTO>();
            }
        }
        protected override void Init(string filePath)
        {
            base.Init(filePath);
        }

        public QuestItemDTO GetQuestItemDTO(int questId)
        {
            QuestItem itemConfig = Global.gApp.gGameData.QuestData.Get(questId);
            if (itemConfig != null)
            {
                return m_Data[itemConfig.type.ToString()].questItemDTOMap[itemConfig.quest_id.ToString()];
            }
            return null;
        }

        public void AfterInit()
        {
            GeneralConfigItem generalConfigItem = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUEST_TYPE_FRESH_DAY);
            for (int type = 1; type <= generalConfigItem.contents.Length; type ++)
            {
                if (m_Data.ContainsKey(type.ToString()))
                {
                    FreshQuestDTO(m_Data[type.ToString()], generalConfigItem);
                }
                else
                {
                    QuestDTO questDTO = MakeQuestDTO(type);
                    m_Data.Add(type.ToString(), questDTO);
                }
            }

            
            SaveData();
        }

        private DateTime GetLastFreshDay(int type)
        {
            QuestDTO questDTO = m_Data[type.ToString()];
            if (questDTO == null)
            {
                questDTO = MakeQuestDTO(type);
            }
            double lastMills = m_Data[type.ToString()].lastMills;
            return DateTimeUtil.GetDate(lastMills);
        }

        //检查配置，刷新
        private void FreshQuestDTO(QuestDTO questDTO, GeneralConfigItem generalConfigItem)
        {
            List<QuestItem> questConfigs = Global.gApp.gGameData.QuestTypeMapData[questDTO.type];
            int freshDay = int.Parse(generalConfigItem.contents[questDTO.type - 1]);

            DateTime lastFreshDay = GetLastFreshDay(questDTO.type);
            DateTime nowDate = DateTime.Today;
            int addDayNum = (nowDate - lastFreshDay).Days;

            if (freshDay > 0 && addDayNum >= 1)
            {
                foreach (QuestItem itemConfig in questConfigs)
                {
                    //if (questDTO.questItemDTOMap.ContainsKey(itemConfig.quest_id.ToString()))
                    //{
                    //    QuestItemDTO itemDTO = questDTO.questItemDTOMap[itemConfig.quest_id.ToString()];
                    //    itemDTO.state = QuestStateConstVal.UNFINISH;
                    //    itemDTO.cur = 0;
                    //} else
                    //{
                    //    QuestItemDTO itemDTO = MakeQuestItemDTO(itemConfig);
                    //    questDTO.questItemDTOMap.Add(itemConfig.quest_id.ToString(), itemDTO);
                    //}

                    QuestItemDTO itemDTO;
                    if (questDTO.questItemDTOMap.TryGetValue(itemConfig.quest_id.ToString(), out itemDTO))
                    {
                        itemDTO.state = QuestStateConstVal.UNFINISH;
                        itemDTO.cur = 0;
                    }
                    else
                    {
                        itemDTO = MakeQuestItemDTO(itemConfig);
                        questDTO.questItemDTOMap.Add(itemConfig.quest_id.ToString(), itemDTO);
                    }
                }
                questDTO.lastMills = DateTimeUtil.GetMills(nowDate);
            } else
            {
                foreach (QuestItem itemConfig in questConfigs)
                {
                    if (!questDTO.questItemDTOMap.ContainsKey(itemConfig.quest_id.ToString()))
                    {
                        QuestItemDTO itemDTO = MakeQuestItemDTO(itemConfig);
                        questDTO.questItemDTOMap.Add(itemConfig.quest_id.ToString(), itemDTO);
                    }
                }
            }

            SaveData();
        }

        private QuestDTO MakeQuestDTO(int questType)
        {
            QuestDTO questDTO = new QuestDTO(questType);
            List<QuestItem> questConfigs = Global.gApp.gGameData.QuestTypeMapData[questType];
            foreach (QuestItem itemConfig in questConfigs)
            {
                QuestItemDTO itemDTO = MakeQuestItemDTO(itemConfig);
                questDTO.questItemDTOMap.Add(itemConfig.quest_id.ToString(), itemDTO);
            }
            questDTO.lastMills = DateTimeUtil.GetMills(DateTimeUtil.initDate);
            return questDTO;
        }

        private QuestItemDTO MakeQuestItemDTO(QuestItem itemConfig)
        {
            double defaultValue = FilterFactory.GetInstance().GetDefault(itemConfig.condition);
            QuestItemDTO itemDTO = new QuestItemDTO(itemConfig.quest_id, defaultValue);
            if (defaultValue >= itemConfig.condition[1])
            {
                itemDTO.state = QuestStateConstVal.CAN_RECEIVE;
            }
            return itemDTO;
        }

        public void QuestChange(float conditionType, float param)
        {
            List<QuestItem> itemConfigs;
            if (!Global.gApp.gGameData.ConditionTypeQuestMapData.TryGetValue((int)conditionType, out itemConfigs))
            {
                return;
            }
            bool isUpdate = false;
            foreach (QuestItem itemConfig in itemConfigs)
            {
                float[] paramArray = new float[] { itemConfig.quest_id, param };
                if (FilterFactory.GetInstance().FilterQuest(itemConfig.condition, paramArray))
                {
                    isUpdate = true;
                }
            }
            if (isUpdate)
            {
                SaveData();
            }
        }

        public QuestDTO GetQuestTypeDTO(int type)
        {
            return m_Data[type.ToString()];
        }

        public bool ReceiveQuest(int questId, int times, int behaviorType, Vector3 position)
        {
            QuestItemDTO data = GetQuestItemDTO(questId);
            if (data.state != QuestStateConstVal.CAN_RECEIVE)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3036);
                return false;
            }
            QuestItem config = Global.gApp.gGameData.QuestData.Get(questId);
            ItemDTO itemDTO = new ItemDTO(Convert.ToInt32(config.award[0]), config.award[1] * times, behaviorType);
            itemDTO.paramStr1 = questId.ToString();
            itemDTO.paramStr2 = times.ToString();
            ItemItem awardCfg = Global.gApp.gGameData.ItemData.Get(itemDTO.itemId);
            //无法播放特效的不播
            if (ItemTypeConstVal.isWeapon(awardCfg.showtype))
            {
                return ReceiveQuest(questId, times, behaviorType);
            }

            data.state = QuestStateConstVal.RECEIVED;
            //发送奖励

            //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.8f);
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, itemDTO.itemId, (int)itemDTO.num, position);
            GameItemFactory.GetInstance().AddItem(itemDTO);

            SaveData();
            return true;
        }

        public bool ReceiveQuest(int questId, int times, int behaviorType)
        {
            QuestItemDTO data = GetQuestItemDTO(questId);
            if (data.state != QuestStateConstVal.CAN_RECEIVE)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3036);
                return false;
            }
            data.state = QuestStateConstVal.RECEIVED;
            //发送奖励
            QuestItem config = Global.gApp.gGameData.QuestData.Get(questId);
            ItemDTO itemDTO = new ItemDTO(Convert.ToInt32(config.award[0]), config.award[1] * times, behaviorType);
            itemDTO.paramStr1 = questId.ToString();
            itemDTO.paramStr2 = times.ToString();
            GameItemFactory.GetInstance().AddItem(itemDTO);

            SaveData();
            return true;
        }

        public bool MissQuestReceive(int questId)
        {
            QuestItemDTO data = GetQuestItemDTO(questId);
            if (data.state != QuestStateConstVal.CAN_RECEIVE)
            {
                return false;
            }
            data.state = QuestStateConstVal.RECEIVED;

            SaveData();
            return true;
        }

        public void CheckLoginWeaponAward()
        {
            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LOGIN];
            foreach (QuestItem config in configs)
            {
                QuestItemDTO questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(config.quest_id);

                ItemItem questAward = Global.gApp.gGameData.ItemData.Get((int)config.award[0]);
                if (ItemTypeConstVal.isWeapon(questAward.showtype))
                {
                    if (questItemDTO.state == QuestStateConstVal.CAN_RECEIVE)
                    {
                        Global.gApp.gGameCtrl.AddGlobalTouchMask();
                        //Global.gApp.gGameCtrl.GetFrameCtrl().GetScene().GetTimerMgr().AddTimer(1.2f, 1, (dt, end) =>
                        //{

                        //});
                        //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_SEVEN_DAY);
                        Global.gApp.gUiMgr.OpenPanel(Wndid.SevenDayPanel);

                        Global.gApp.gGameCtrl.GetFrameCtrl().GetScene().GetTimerMgr().AddTimer(0.8f, 1, (dt, end) =>
                        {
                            Global.gApp.gUiMgr.OpenPanel(Wndid.NextDayWeaponUi, questAward);

                            NextDayWeapon nextDayWeaponUi = Global.gApp.gUiMgr.GetPanelCompent<NextDayWeapon>(Wndid.NextDayWeaponUi);
                            if (nextDayWeaponUi != null)
                            {
                                nextDayWeaponUi.ChangeInfo4SevenDay();
                            }
                            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
                        });
                        
                        
                        return;
                    }
                }
            }
        }


        public int GetShouldReceiveId4SevenDay()
        {
            int questId = 0;
            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LOGIN];
            foreach (QuestItem config in configs)
            {
                QuestItemDTO questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(config.quest_id);

                ItemItem questAward = Global.gApp.gGameData.ItemData.Get((int)config.award[0]);
                if (ItemTypeConstVal.isWeapon(questAward.showtype))
                {
                    if (questItemDTO.state == QuestStateConstVal.CAN_RECEIVE)
                    {
                        ReceiveQuest(config.quest_id, 1, BehaviorTypeConstVal.OPT_SEVEN_DAY_LOGIN);
                        continue;
                    }
                } else if (questItemDTO.state == QuestStateConstVal.CAN_RECEIVE)
                {
                    questId = config.quest_id;
                    break;
                }
            }

            return questId;
        }

        public QuestItemDTO GetLevelDetailQuest()
        {
            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LEVEL_DETAIL];
            for (int i = 0; i < configs.Count; i++)
            {
                QuestItemDTO questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(configs[i].quest_id);
                if (questItemDTO.state == QuestStateConstVal.CAN_RECEIVE)
                {
                    //if (i == configs.Count - 1)
                    //{
                    //    return questItemDTO;
                    //} else if (questItemDTO.cur == configs[i].condition[1])
                    //{
                    //    return questItemDTO;
                    //} else if (Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(configs[i + 1].quest_id).state == QuestStateConstVal.UNFINISH)
                    //{
                    //    return questItemDTO;
                    //} else
                    //{
                    //    //不是当前的直接领取，只提示最近的一次
                    //    Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest(configs[i].quest_id, 1, BehaviorTypeConstVal.OPT_LEVEL_DETAIL);
                    //}
                    ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get((int)configs[i].award[0]);
                    if (itemCfg == null || ItemTypeConstVal.isWeapon(itemCfg.showtype))
                    {
                        Global.gApp.gSystemMgr.GetQuestMgr().ReceiveQuest(configs[i].quest_id, 1, BehaviorTypeConstVal.OPT_LEVEL_DETAIL);
                    } else
                    {
                        return questItemDTO;
                    }
                    
                } 
            }
            return null;
        }
        public QuestItemDTO GetNextLevelDetailQuest()
        {
            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LEVEL_DETAIL];
            for (int i = 0; i < configs.Count; i++)
            {
                QuestItemDTO questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO(configs[i].quest_id);
                if (questItemDTO.state == QuestStateConstVal.UNFINISH)
                {
                    return questItemDTO;
                }
            }
            return null;
        }

    }

    public class QuestConst
    {
        //任务大类型枚举
        //成就任务
        public const int TYPE_ACHIEVE = 1;
        //日任务
        public const int TYPE_DAY = 2;
        //登录奖励任务
        public const int TYPE_LOGIN = 3;
        //关卡进度成就
        public const int TYPE_LEVEL_DETAIL = 4;

    }
}

