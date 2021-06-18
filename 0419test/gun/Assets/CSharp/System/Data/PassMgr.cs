using EZ.Data;
using EZ.Util;using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ.DataMgr
{
    public enum PassType
    {
        MainPass = 1,
        BranckPass = 2,
    }
    public class PassMgr : BaseDataMgr<PassDTO>
    {
        private int m_PassEnterTimes = 0;
        private int m_EnterBranchPassId = 0;
        public PassMgr()
        {
            OnInit();
        }
        public override void OnInit()
        {
            base.OnInit();
            Init("pass");
            if (m_Data == null)
            {
                m_Data = new PassDTO();
                SaveData();
            }
            m_PassEnterTimes = m_Data.enterTimes;
        }

        public void AfterInit()
        {
            Global.gApp.gSystemMgr.GetQuestMgr().QuestChange(FilterTypeConstVal.PASS_ID, 0f);
            // 第一次进入游戏 初始化
            if(m_Data.fBPIndex < 0)
            {
                // 兼容老玩家 计算初始化偏移值 
                m_Data.curFBPPassOffset = m_Data.curPassId % 100000 - 1;
                m_Data.curOBPPassOffset = m_Data.curPassId % 100000 - 1;
                m_Data.fBPPassed = false;
                m_Data.fBPIndex = 0;
                m_Data.fBPOffset = 0;
                m_Data.oBPPassed = false;
                m_Data.oBPIndex = 0;
                m_Data.oBPOffset = 0;
                SaveData();
            }
            else
            {
                if (m_Data.fBPPassed)
                {
                    PassBranchItem[] fBPItems = Global.gApp.gGameData.PassBranchConfig.items;
                    // 处理强制关卡 offset // 表示 通关之后 增加了新的关卡 
                    if (fBPItems.Length > m_Data.fBPIndex + 1)
                    {
                        // 从新计算最大推移 保证 新增加的关卡能被玩家玩
                        // 600 - 250 = 350 关的 偏移 ，350 + 250  当前 需要支线的 关卡 就是 第600 关可以打下一个了
                        // 此时 重置 fBPOffset = 0 不需要再累加
                        m_Data.curFBPPassOffset = m_Data.curPassId % 100000 - fBPItems[m_Data.fBPIndex + 1].mainPassId;
                        m_Data.curFBPPassOffset = Mathf.Max(0, m_Data.curFBPPassOffset);
                        // 重置强制偏移值。通过重新计算总偏移  小偏移没有用了
                        m_Data.fBPOffset = 0;
                        m_Data.fBPPassed = false;
                        m_Data.fBPIndex++;
                        SaveData();
                    }
                }
            }
        }

        public PassBranchItem GetBranchPassItem()
        {
            PassBranchItem[] fBFItems = Global.gApp.gGameData.PassBranchConfig.items;
            if (m_Data.fBPIndex >= 0 && m_Data.fBPIndex < fBFItems.Length)
            {
                return fBFItems[m_Data.fBPIndex];
            }
            else
            {
                return null;
            }
        }
        public int GetBranchPassId()
        {
            if (!m_Data.fBPPassed)
            {
                PassBranchItem[] fBFItems = Global.gApp.gGameData.PassBranchConfig.items;
                int curPassIndex = m_Data.curPassId % 100000;
                int curFBPPassedId = fBFItems[m_Data.fBPIndex].mainPassId + m_Data.curFBPPassOffset + m_Data.fBPOffset;
                if (curPassIndex == curFBPPassedId)
                {
                    return fBFItems[m_Data.fBPIndex].passId;
                }
                else
                {
                    return -1;
                }
            }
            return -1;
        }
        public void DelayBranchPass()
        {
            string delayPassNumStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.NUMBER_OFDELAY_PASS).content;
            int delayPassNumInt = int.Parse(delayPassNumStr);
            int newOffset = m_Data.fBPOffset + delayPassNumInt;
            m_Data.fBPOffset = newOffset;
            SaveData();
        }
        private void GameWinForBranchInfo(int enterPassId)
        {
            if (enterPassId == 400003)
            {
                Global.gApp.gSystemMgr.GetMiscMgr().AddDialogue(1007);
            }

            if (!m_Data.fBPPassed)
            {
                if (enterPassId == 400004)
                {
                    Player player = Global.gApp.CurScene.GetMainPlayerComp();
                    if (player != null)
                    {
                        player.GetPlayerData().AddDropResIfNotExit(FightNpcPlayer.FightDropNPC.Npc_oldwoman.ToString(), 1);
                        player.GetPlayerData().AddDropResIfNotExit(FightNpcPlayer.FightDropNPC.Npc_boy.ToString(), 1);
                        player.GetPlayerData().AddDropResIfNotExit(FightNpcPlayer.FightDropNPC.Npc_doctor03.ToString(), 1);
                        player.GetPlayerData().AddDropResIfNotExit(FightNpcPlayer.FightDropNPC.Npc_human00.ToString(), 1);
                        player.GetPlayerData().AddDropResIfNotExit(FightNpcPlayer.FightDropNPC.Npc_human02.ToString(), 1);
                        player.GetPlayerData().AddDropResIfNotExit(FightNpcPlayer.FightDropNPC.Npc_human03.ToString(), 1);
                        player.GetPlayerData().AddDropResIfNotExit(FightNpcPlayer.FightDropNPC.Npc_human04.ToString(), 1);
                    }
                }
                // 再次解救小男孩的关卡；
                else if (enterPassId == 400009)
                {
                    Global.gApp.gSystemMgr.GetNpcMgr().SetShowBoyNpc(true);
                    Global.gApp.gSystemMgr.GetNpcMgr().NpcQuestChange(FilterTypeConstVal.GET_ITEM, SpecialItemIdConstVal.NPC_BOY, 1);

                }
                PassBranchItem[] fBFItems = Global.gApp.gGameData.PassBranchConfig.items;
                PassBranchItem curfBFItem = fBFItems[m_Data.fBPIndex];
                if (curfBFItem.passId == enterPassId)
                {
                    if (fBFItems.Length > m_Data.fBPIndex + 1)
                    {
                        m_Data.fBPIndex++;
                        SaveData();
                    }
                    else
                    {
                        m_Data.fBPPassed = true;
                        SaveData();
                    }
                }
            }
        }
        public bool CheckBranchPassEnterd(int checkPassId)
        {
            if(!m_Data.fBPPassed)
            {
                PassBranchItem[] fBFItems = Global.gApp.gGameData.PassBranchConfig.items;
                PassBranchItem fBFItem = fBFItems[m_Data.fBPIndex];
                return checkPassId < fBFItem.passId;
            }
            else
            {
                return true;
            }
        }
        public int EnterBranchPass()
        {
            int branchPassCount = m_Data.branchPass.Count;
            if (branchPassCount > 0)
            {
                int passId = m_Data.branchPass[UnityEngine.Random.Range(0, branchPassCount)];
                if(EnterBranchPassImp(passId))
                {
                    return passId;
                }
            }
            return -1 ;
        }
 
        private bool EnterBranchPassImp(int passId)
        {
            int branchPassCount = m_Data.branchPass.Count;
            if (branchPassCount > 0 && m_Data.branchPass.Remove(passId))
            {
                m_EnterBranchPassId = passId;
                PassItem passItem = GetPassItem();
                if (passItem != null)
                {
                    SceneItem sceneItem = Global.gApp.gGameData.SceneDate.Get(passItem.sceneID);
                    if (branchPassCount == sceneItem.missionLimit)
                    {
                        m_Data.recordTime = DateTimeUtil.GetMills(DateTime.Now);
                    }
                    SaveData();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public int FreshAndGetBranchPassCount()
        {
            PassItem passItem = GetPassItem();
            if(passItem != null){
                SceneItem sceneItem = Global.gApp.gGameData.SceneDate.Get(passItem.sceneID); 
                double curTime = DateTimeUtil.GetMills(DateTime.Now);
                if (m_Data.recordTime <= 0)
                {
                    m_Data.recordTime = curTime;
                }
                double dtTime = sceneItem.missionInterval * 1000;
                double totalDtTime = curTime - m_Data.recordTime;
                List<int> branchPass = m_Data.branchPass;
                while (totalDtTime >= dtTime && branchPass.Count < sceneItem.missionLimit)
                {
                    totalDtTime -= dtTime;
                    int curRate = 0;
                    int randomRate = UnityEngine.Random.Range(0, 10001);
                    int index = 0;
                    foreach(int rate in sceneItem.missionWaveRate)
                    {
                        curRate += rate;
                        if(randomRate < curRate)
                        {
                            branchPass.Add(sceneItem.missionWaveID[index]);
                            break;
                        }
                        index++;
                    }
                }
                if(branchPass.Count == sceneItem.missionLimit)
                {
                    m_Data.recordTime = curTime;
                }
                else
                {
                    m_Data.recordTime = curTime - totalDtTime;
                }
                SaveData();
                return branchPass.Count;
            }
            return 0;
        } 
        public void EnterPass()
        {
            m_PassEnterTimes = ++m_Data.enterTimes;
            SaveData();
        }
        public PassItem GetPrePassItem()
        {
            PassItem curPass = Global.gApp.gGameData.PassData.Get(m_Data.curPassId-1);
            return curPass;
        }
        public PassItem GetCurPassItem()
        {
            PassItem curPass = Global.gApp.gGameData.PassData.Get(m_Data.curPassId);
            return curPass;
        }
        public PassItem GetPassItem()
        {
            PassItem passItem = GetCurPassItem();
            if (passItem == null)
            {
                passItem = GetPrePassItem();
            }
            return passItem;
        }
        public PassItem GetNextPassItem()
        {
            PassItem curPass = Global.gApp.gGameData.PassData.Get(m_Data.curPassId + 1);
            return curPass;
        }

        public PassItem GetPassItemById(int id)
        {
            PassItem curPass = Global.gApp.gGameData.PassData.Get(id);
            return curPass;
        }
        public int GetPassSerial()
        {
            return m_Data.curPassId % 100000;
        }
        public int GetCurPassId()
        {
            return m_Data.curPassId;
        }
        public void SetCurPassId(int passId)
        {
            m_Data.enterTimes = 0;
            m_Data.curPassId = passId;
            SaveData();
            Global.gApp.gSystemMgr.GetQuestMgr().QuestChange(FilterTypeConstVal.PASS_ID, 0f);
        }
        public int GetPassEnterTimes()
        {
            return m_Data.enterTimes > 0 ? m_Data.enterTimes : m_PassEnterTimes;
        }

        public int GetCurEnterTimes()
        {
            return m_Data.enterTimes;
        }

        public void ShowTankUiState()
        {
           if (m_Data.passMaxPass && !m_Data.hasOpenTankUi)
           {
                m_Data.hasOpenTankUi = true;
                SaveData();
                Global.gApp.gUiMgr.OpenPanel(Wndid.ThanksUI);
           }
        }
        public int GetBranckId(int passId, int reviveTimes, int duration)
        {
            if (!m_Data.fBPPassed)
            {
                int enterPassId = passId % 100000;
                PassBranchItem[] fBFItems = Global.gApp.gGameData.PassBranchConfig.items;
                PassBranchItem curfBFItem = fBFItems[m_Data.fBPIndex];
                if (curfBFItem.mainPassId == enterPassId)
                {
                    return curfBFItem.passId;
                }
            }
            return -1;
        }
        public void GameSucess(int passId, int reviveTimes, int duration)
        {
            GameWinForBranchInfo(passId);
            if (m_Data.curPassId == passId)
            {
                //ELK 日志打点 游戏成功
                ELKLogMgr.GetInstance().SendELKLog4Pass(ELKLogMgr.PASS_SUCCESS, reviveTimes, duration);

                PassItem passItem = GetCurPassItem();
                int nextPassId = passItem.nextID;
                if (nextPassId > Global.gApp.gGameData.m_MaxNormalId)
                {
                    nextPassId = Global.gApp.gGameData.m_MaxNormalId;
                    m_Data.passMaxPass = true;
                }
                else
                {
                    m_Data.passMaxPass = false;
                    m_Data.hasOpenTankUi = false;
                }
                SetCurPassId(nextPassId);
            }
            else
            {
                if (m_Data.passMaxPass)
                {
                    //表示有新关卡 
                    PassItem passItem = GetNextPassItem();
                    if(passItem != null)
                    {
                        m_Data.passMaxPass = false;
                        SaveData();
                    }
                }
            }
        }

        public void GameLose(int passId, int reviveTimes, int duration)
        {
            if (m_Data.passMaxPass)
            {
                // 表示有新关卡
                PassItem passItem = GetNextPassItem();
                if (passItem != null)
                {
                    m_Data.passMaxPass = false;
                    SaveData();
                }
            }
            if (m_Data.curPassId == passId)
            {
                //ELK 日志打点 游戏失败
                ELKLogMgr.GetInstance().SendELKLog4Pass(ELKLogMgr.PASS_FAIL, reviveTimes, duration);
            }
        }

        public int GetUserStep()
        {
            GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
            int initId = Convert.ToInt32(initPassIdConfig.content);
            for (int i = 0; i < Global.gApp.gGameData.PassStep.Count; i++)
            {
                int start = i == 0 ? initId + 1 : Global.gApp.gGameData.PassStep[i - 1] + 1;
                int end = Global.gApp.gGameData.PassStep[i];
                int curPassId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
                if (curPassId >= start && curPassId <= end)
                {
                    return i;
                }
            }
            return 0;
        }

        public ItemItem GetNextGun(float openType)
        {
            SortedList<int, ItemItem> list;
            Global.gApp.gGameData.OpenOrderGun.TryGetValue(openType, out list);
            if (list == null)
            {
                return null;
            }
            foreach (ItemItem cfg in list.Values)
            {
                if (!Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(cfg))
                {
                    return cfg;
                }
            }
            return null;
        }

        public string GetParamStr(SkillItem m_ShowSkillItem, float[] param)
        {
            if (m_ShowSkillItem.percentage == 1)
            {
                return Math.Round(100 * (param[0] - m_ShowSkillItem.init)) + "%";
            }
            else
            {
                return (param[0] - m_ShowSkillItem.init).ToString();
            }
        }

        public bool GetHasPassedMaxPass()
        {
            if(m_Data.curPassId == Global.gApp.gGameData.m_MaxNormalId)
            {
                return m_Data.passMaxPass;
            }
            else
            {
                if (m_Data.passMaxPass)
                {
                    m_Data.passMaxPass = false;
                    SaveData();
                }
                return m_Data.passMaxPass;
            }
        }
        public void PauseExit()
        {

        }
        public void ResetBranckPass()
        {
            // 兼容老玩家 计算初始化偏移值 
            m_Data.curFBPPassOffset = m_Data.curPassId % 100000 - 1;
            m_Data.curOBPPassOffset = m_Data.curPassId % 100000 - 1;
            m_Data.fBPPassed = false;
            m_Data.fBPIndex = 0;
            m_Data.fBPOffset = 0;
            m_Data.oBPPassed = false;
            m_Data.oBPIndex = 0;
            m_Data.oBPOffset = 0;
            SaveData();
        }
        public void PassTimesChange(int passId)
        {
            int times = 0;
            if (!m_Data.passTimesDic.TryGetValue(passId.ToString(), out times))
            {
                m_Data.passTimesDic[passId.ToString()] = times;
            }
            m_Data.passTimesDic[passId.ToString()]++;
            SaveData();
        }

        public int GetPassTimes(int passId)
        {
            int times = 0;
            m_Data.passTimesDic.TryGetValue(passId.ToString(), out times);
            return times;
        }

        protected override void Init(string filePath)
        {
            base.Init(filePath);
        }
    }
}
