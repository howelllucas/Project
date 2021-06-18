using EZ.Data;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace EZ.DataMgr
{
    //新手引导数据
    public class NewbieGuideMgr : BaseDataMgr<NewbieGuideDTO>
    {

        public NewbieGuideMgr()
        {
            OnInit();
        }

        public override void OnInit()
        {
            base.OnInit();
            Init("newbieGuide");

            if (m_Data == null)
            {
                m_Data = new NewbieGuideDTO();

            }

            bool update = false;
            foreach (NewbieGuideItem config in Global.gApp.gGameData.NewbieGuideData.items)
            {
                int state;

                if (!m_Data.map.TryGetValue(config.id.ToString(), out state))
                {
                    update = true;
                    m_Data.map[config.id.ToString()] = WeaponStateConstVal.NONE;
                }
                else if (config.quit == 1 && m_Data.map[config.id.ToString()] == WeaponStateConstVal.NONE)
                {
                    if (m_Data.map[config.root_id.ToString()] == WeaponStateConstVal.EXIST)
                    {
                        update = true;
                        m_Data.map[config.id.ToString()] = WeaponStateConstVal.EXIST;
                        m_Data.curId = 0;
                    }
                }
            }

            if (update)
            {
                SaveData();
            }
        }


        protected override void Init(string filePath)
        {
            base.Init(filePath);
        }

        //检查是否触发新手引导变化
        public void OnStart(NewbieGuideButton nButton)
        {
            Global.gApp.gGameCtrl.AddGlobalTouchMask();

            foreach (int nId in nButton.NewbieGuideIds)
            {
                
                NewbieGuideItem nConfig = Global.gApp.gGameData.NewbieGuideData.Get(nId);
                //引导配置是否存在
                if (nConfig == null)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3025, nId.ToString());
                    continue;
                }
                NewbieGuideItem rootConfig = Global.gApp.gGameData.NewbieGuideData.Get(nConfig.root_id);
                //同时只有一个引导在进行
                if (m_Data.curId > 0 && Global.gApp.gGameData.NewbieGuideData.Get(m_Data.curId).root_id != nConfig.root_id)
                {
                    continue;
                }

                int nState = m_Data.map[nId.ToString()];
                if (nState == WeaponStateConstVal.EXIST)
                {
                    continue;
                }

                //引导条件是否满足
                if (nConfig.condition != null && nConfig.condition[0] != 0 && nConfig.condition.Length >= 2)
                {
                    if (!FilterFactory.GetInstance().Filter(nConfig.condition))
                    {
                        continue;
                    }

                }

                //引导条件1是否满足
                if (nConfig.condition1 != null && nConfig.condition1[0] != 0 && nConfig.condition1.Length >= 2)
                {
                    if (!FilterFactory.GetInstance().Filter(nConfig.condition1))
                    {
                        continue;
                    }
                }

                //上个引导完成状态
                if ((nConfig.id == nConfig.root_id || (nConfig.pre_id != 0 && m_Data.map[nConfig.pre_id.ToString()] == WeaponStateConstVal.EXIST))
                   && FilterFactory.GetInstance().JudgeNewbieButton(rootConfig.condition, nConfig, nButton))
                {
                    bool skip = false;
                    //对于特殊情况（武器解锁引导，非主武器，如果已经有使用过了，将不再引导）
                    if (rootConfig.condition[0] == FilterTypeConstVal.WEAPON_UNLOCK && rootConfig.condition[1] != ItemTypeConstVal.BASE_MAIN_WEAPON)
                    {
                        int unlockNum = 0;
                        
                        foreach (ItemItem cfg in Global.gApp.gGameData.ShowOrderGun[Convert.ToInt32(rootConfig.condition[1])].Values)
                        {
                            Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(cfg);
                            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(cfg.name) == WeaponStateConstVal.EXIST)
                            {
                                unlockNum++;
                                if (unlockNum == rootConfig.condition[2])
                                {
                                    skip = true;
                                    break ;
                                }
                            }
                        }
                    }

                    if (skip)
                    {
                        
                        foreach (NewbieGuideItem ni in Global.gApp.gGameData.NewbieGuideData.items)
                        {
                            if (ni.root_id == nConfig.root_id)
                            {
                                m_Data.map[ni.id.ToString()] = WeaponStateConstVal.EXIST;
                            }
                        }
                        SaveData();
                    } else
                    {
                        m_Data.curId = nConfig.id;
                        if (!string.IsNullOrEmpty(nButton.Param))
                        {
                            nButton.gameObject.AddComponent<DelayCallBack>().SetAction(() =>
                            {
                                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ChangeScrollView4Guide);
                            }, 0.2f);
                        }
                        //Global.gApp.gGameCtrl.GetFrameCtrl().GetScene().GetTimerMgr().AddTimer(nConfig.delay, 1, (dt, end) => { DelayAddGameGuide(nButton, nConfig); });
                        nButton.gameObject.AddComponent<DelayCallBack>().SetAction(() => { DelayAddGameGuide(nButton, nConfig); }, nConfig.delay);
                        //处理链尾
                        
                        SaveData();

                        break;
                    }

                }
            }

            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();

            
        }

        private void DelayAddGameGuide(NewbieGuideButton nButton, NewbieGuideItem nConfig)
        {
            Global.gApp.gMsgDispatcher.Broadcast<Transform>(MsgIds.AddGameGuideUi, nButton.NewbieButton.transform);
            Global.gApp.gMsgDispatcher.Broadcast<int, float, bool, bool>(MsgIds.AddGuidePlotUi, nConfig.tips, nConfig.tips_y, false, false);
        }

        //点击事件
        public void OnClick(NewbieGuideButton nButton)
        {
            
            Global.gApp.gGameCtrl.AddGlobalTouchMask();

            foreach (int nId in nButton.NewbieGuideIds)
            {
                NewbieGuideItem nConfig = Global.gApp.gGameData.NewbieGuideData.Get(nId);
                //引导配置是否存在
                if (nConfig == null)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3025, nId.ToString());
                    continue;
                }

                NewbieGuideItem rootConfig = Global.gApp.gGameData.NewbieGuideData.Get(nConfig.root_id);
                //同时只有一个引导在进行
                if (m_Data.curId > 0 && Global.gApp.gGameData.NewbieGuideData.Get(m_Data.curId).root_id != nConfig.root_id)
                {
                    continue;
                }

                int nState = m_Data.map[nId.ToString()];
                if (nState == WeaponStateConstVal.EXIST)
                {
                    continue;
                }

                //引导条件是否满足
                if (nConfig.condition != null && nConfig.condition[0] != 0 && nConfig.condition.Length >= 2)
                {
                    if (!FilterFactory.GetInstance().Filter(nConfig.condition))
                    {
                        continue;
                    }

                }

                //引导条件1是否满足
                if (nConfig.condition1 != null && nConfig.condition1[0] != 0 && nConfig.condition1.Length >= 2)
                {
                    if (!FilterFactory.GetInstance().Filter(nConfig.condition1))
                    {
                        continue;
                    }

                }

                //上个引导完成状态
                if ((nConfig.id == nConfig.root_id || (nConfig.pre_id != 0 && m_Data.map[nConfig.pre_id.ToString()] == WeaponStateConstVal.EXIST))
                   && FilterFactory.GetInstance().JudgeNewbieButton(rootConfig.condition, nConfig, nButton))
                {
                    
                    Global.gApp.gMsgDispatcher.Broadcast<Transform>(MsgIds.rmGameGuideUi, nButton.NewbieButton.transform);
                    Global.gApp.gMsgDispatcher.Broadcast(MsgIds.RmGuidePlotUi);

                    m_Data.map[nConfig.id.ToString()] = WeaponStateConstVal.EXIST;
                    SaveData();

                    //处理链尾
                    if (nConfig.post_id != 0)
                    {
                        NewbieGuideItem postConfig = Global.gApp.gGameData.NewbieGuideData.Get(nConfig.post_id);
                        int postState = m_Data.map[nConfig.post_id.ToString()];

                        //链尾只有文本提示文本
                        if (postConfig.post_id == 0 && postConfig.onlyText == 1)
                        {
                            m_Data.curId = 0;
                            m_Data.map[nConfig.post_id.ToString()] = WeaponStateConstVal.EXIST;
                            //Global.gApp.gGameCtrl.GetFrameCtrl().GetScene().GetTimerMgr().AddTimer(nConfig.delay, 1, (dt, end) => 
                            //{ Global.gApp.gMsgDispatcher.Broadcast<int, float, bool, bool>(MsgIds.AddGuidePlotUi, postConfig.tips, postConfig.tips_y, true, false); });
                            nButton.gameObject.AddComponent<DelayCallBack>().SetAction(()=> {
                                Global.gApp.gMsgDispatcher.Broadcast<int, float, bool, bool>(MsgIds.AddGuidePlotUi, postConfig.tips, postConfig.tips_y, true, false);
                            }, postConfig.delay);
                            SaveData();
                        }

                        NewbieGuideButton[] newBieButtons = nButton.transform.parent.GetComponentsInChildren<NewbieGuideButton>();
                        
                        foreach (NewbieGuideButton newBieButton in newBieButtons)
                        {
                            OnStart(newBieButton);
                        }
                    } else
                    {
                        m_Data.curId = 0;
                        SaveData();
                    }

                    break;

                }
            }

            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
        }
        

        public int GetCurGuideId()
        {
            return m_Data.curId;
        }
    }
}

