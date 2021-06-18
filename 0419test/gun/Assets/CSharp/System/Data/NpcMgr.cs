using UnityEngine;
using System.Collections;
using System;
using EZ.Data;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace EZ.DataMgr
{
    public class NpcState
    {
        public const int None = 0;
        public const int OnGoing = 1;
        public const int UnReceived = 2;
        public const int Received = 3;
    }

    public class NpcMgr : BaseDataMgr<NpcDTO>
    {

        public NpcMgr()
        {
            OnInit();
        }

        public override void OnInit()
        {
            base.OnInit();
            Init("npc");
            if (m_Data == null)
            {
                m_Data = new NpcDTO();
            }
        }

        public void SetShowBoyNpc(bool state)
        {
            m_Data.showBoyNpc = state;
            SaveData();
        }
        public bool GetShowBoyNpcState()
        {
            return m_Data.showBoyNpc;
        }
        //npc任务map
        public List<NpcQuestItemDTO> NpcQuestList
        {
            get { return m_Data.npcQuestList; }
            set
            {
                m_Data.npcQuestList = value;
                SaveData();
            }
        }
        //上次刷新时间
        public double LastFreshTime
        {
            get { return m_Data.lastFreshTime; }
            set
            {
                m_Data.lastFreshTime = value;
                SaveData();
            }
        }
        //进入营地界面的时间
        public double OpenCampTime
        {
            get { return m_Data.OpenCampTime; }
            set
            {
                m_Data.OpenCampTime = value;
                SaveData();
            }
        }
        //npc的map
        public Dictionary<string, ItemDTO> NpcMap
        {
            get { return m_Data.npcMap; }
            set
            {
                m_Data.npcMap = value;
                SaveData();
            }
        }
        //npc的物品map
        public Dictionary<string, ItemDTO> NpcAwardMap
        {
            get { return m_Data.npcAwardMap; }
            set
            {
                m_Data.npcAwardMap = value;
                SaveData();
            }
        }
        //营地商店商品购买次数
        public Dictionary<string, int> CampShopTimesMap
        {
            get { return m_Data.CampShopTimesMap; }
            set
            {
                m_Data.CampShopTimesMap = value;
                SaveData();
            }
        }
        //营地技能
        public Dictionary<string, SkillItemDTO> CampBuffMap
        {
            get { return m_Data.campBuffMap; }
            set
            {
                m_Data.campBuffMap = value;
                SaveData();
            }
        }
        //营地等级
        public int CampShowLevel
        {
            get { return m_Data.campShowLevel; }
            set
            {
                m_Data.campShowLevel = value;
                SaveData();
            }
        }
        public void AfterInit()
        {
            //List<ItemDTO> initNpcs = GameItemFactory.GetInstance().DealItems(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_INIT_NPC).contents, BehaviorTypeConstVal.OPT_CAMP_INIT);
            //bool foreceFresh = false;
            //foreach (ItemDTO itemDTO in initNpcs)
            //{
            //    if (GameItemFactory.GetInstance().GetItem(itemDTO.itemId) == 0d)
            //    {
            //        foreceFresh = true;
            //        GameItemFactory.GetInstance().AddItem(itemDTO);
            //    }
            //}

            DateTime now = DateTime.Today;
            DateTime lastFresh = DateTimeUtil.GetDate(m_Data.lastFreshTime);
            int addDayNum = (now - lastFresh).Days;
            if (addDayNum > 0)
            {
                m_Data.heartAdWatchTimes = 0;
                Fresh(true);
            }
            else
            {
                // 第一次初始化 红心信息
                if (m_Data.npcQuestList.Count > 0 && m_Data.npcRedHeartList.Count == 0)
                {
                    ResetRedHeartInfo(true);
                }
            }

            //移除删除配置
            foreach (KeyValuePair<string, int> crrPair in m_Data.CampShopTimesMap)
            {
                CampShopItem csi = Global.gApp.gGameData.CampShopConfig.Get(int.Parse(crrPair.Key));
                if (csi == null)
                {
                    m_Data.CampShopTimesMap.Remove(crrPair.Key);
                }
            }
            //初始化商店购买次数
            foreach (CampShopItem csi in Global.gApp.gGameData.CampShopConfig.items) {
                if (!m_Data.CampShopTimesMap.ContainsKey(csi.id.ToString()))
                {
                    m_Data.CampShopTimesMap[csi.id.ToString()] = 0;
                }
                //武器需要根据当前情况处理
                ItemItem ii = Global.gApp.gGameData.ItemData.Get(csi.propId);
                if (ii != null && GameItemFactory.GetInstance().GetItem(csi.propId) > 0)
                {
                    m_Data.CampShopTimesMap[csi.id.ToString()] = 1;
                }
            }
            //初始化buff
            InitCampBuff(false);
            Global.gApp.gSystemMgr.GetNpcMgr().ResetNpcAtkLevel();
            SaveData();
        }

        public void InitCampBuff(bool isSave)
        {
            int campLevel = CalCampLevel();
            //营地技能初始化
            foreach (CampBuffItem cbi in Global.gApp.gGameData.CampBuffConfig.items)
            {
                SkillItemDTO dto;
                if (!CampBuffMap.TryGetValue(cbi.id, out dto))
                {
                    //需要解锁从0级开始，不需要从1级开始
                    //状态表示开启状态
                    int initState = campLevel >= cbi.campLevel ? WeaponStateConstVal.EXIST : WeaponStateConstVal.NONE;
                    int initLevel = cbi.unlockCost == 0 ? 1 : 0;
                    dto = new SkillItemDTO(cbi.id, initLevel, initState);
                    CampBuffMap[cbi.id] = dto;
                } else
                {
                    dto.state = campLevel >= cbi.campLevel ? WeaponStateConstVal.EXIST : WeaponStateConstVal.NONE;
                }
            }
            if (isSave)
            {
                SaveData();
            }

        }

        public void ResetNpcAtkLevel()
        {
            int totalNum = GetTotalNum(true);
            SkillItemDTO dto;
            CampBuffMap.TryGetValue("buff_atkNpc", out dto);
            int level = 0;
            for (int i = 0; i < Global.gApp.gGameData.CampBuffDataConfig.items.Length - 1; i++)
            {
                float min = i == 0 ? 0 : Global.gApp.gGameData.CampBuffDataConfig.items[i - 1].buff_atkNpc_cost[0];
                float max = Global.gApp.gGameData.CampBuffDataConfig.items[i].buff_atkNpc_cost[0];
                if (totalNum >= min && totalNum < max)
                {
                    level = i + 1;
                    break;
                }
            }
            if (level == 0)
            {
                level = Global.gApp.gGameData.CampBuffDataConfig.items.Length;
            }
            if (dto.level != level)
            {
                dto.level = level;
                SaveData();
            }
        }


        public void Fresh(bool task)
        {

            int maxStep = 4;// Global.gApp.gGameData.CampStepConfig.items.Length - 1;
            if (Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(maxStep) && task)
            {
                List<NpcQuestItemDTO> newList = new List<NpcQuestItemDTO>();
                //特殊npc不刷新
                foreach (NpcQuestItemDTO dto in m_Data.npcQuestList)
                {
                    CampNpcItem npcItem = Global.gApp.gGameData.CampNpcConfig.Get(dto.npcId);
                    if (npcItem.notFresh == 1)
                    {
                        ItemItem npcItemCfg = Global.gApp.gGameData.GetItemDataByName(npcItem.id);
                        ItemDTO npc = GetNpc(npcItemCfg.id);
                        if (npc == null || npc.num == 0)
                        {
                            continue;
                        }
                        if (dto.state == NpcState.OnGoing)
                        {
                            newList.Add(dto);
                        }
                        else
                        {
                            NpcQuestItemDTO nqid = RandomQuest(dto.npcId);
                            newList.Add(nqid);
                        }
                    }
                }
                //生成新的特殊npc
                foreach (CampNpcItem cni in Global.gApp.gGameData.NotFreshNpcList)
                {
                    ItemItem npcItemCfg = Global.gApp.gGameData.GetItemDataByName(cni.id);
                    ItemDTO npc = GetNpc(npcItemCfg.id);
                    if (npc != null && npc.num > 0 && !NpcQuestListContainNpcId(cni.id))
                    {
                        NpcQuestItemDTO nqid = RandomQuest(cni.id);
                        newList.Add(nqid);
                    }
                }

                m_Data.npcQuestList = null;
                m_Data.npcQuestList = newList;

                m_Data.lastFreshTime = DateTimeUtil.GetMills(DateTime.Today);
            } else
            {
                foreach (CampNpcItem cni in Global.gApp.gGameData.NotFreshNpcList)
                {
                    ItemItem npcItemCfg = Global.gApp.gGameData.GetItemDataByName(cni.id);
                    ItemDTO npc = GetNpc(npcItemCfg.id);
                    if (npc != null && npc.num > 0 && !NpcQuestListContainNpcId(cni.id))
                    {
                        NpcQuestItemDTO nqid;
                        if (Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(maxStep))
                        {
                            nqid = RandomQuest(cni.id);
                        } else
                        {
                            nqid = new NpcQuestItemDTO(cni.id);
                        }

                        m_Data.npcQuestList.Add(nqid);
                    }
                }
            }


            //重置数目
            ResetShowNumAndRedHeart(task);
        }

        public void CampStepEnd(int curStep)
        {
            //Debug.Log("CampStepEnd curStep = " + curStep);
            if (curStep >= 0)
            {
                CampStepItem campStepItem = Global.gApp.gGameData.CampStepConfig.Get(curStep);
                if (campStepItem != null)
                {
                    CampTasksItem campTasksItem = Global.gApp.gGameData.CampTasksConfig.Get(campStepItem.nextTaskId);
                    ItemItem itemItem = Global.gApp.gGameData.ItemData.Get(SpecialItemIdConstVal.NPC_OLDWOMAN);
                    for (int i = 0; i < m_Data.npcQuestList.Count; i++)
                    {
                        NpcQuestItemDTO dto = m_Data.npcQuestList[i];
                        if (dto.npcId == itemItem.name)
                        {
                            if (dto.npcQuestId > 0 && dto.state == NpcState.UnReceived)
                            {
                                CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
                                Vector3 effectUIPos = campsiteUI.GetOldWomanTaskNodeUiPos();
                                ReceiveQuestReward(i, effectUIPos);
                            }

                            dto.npcQuestId = campTasksItem == null ? -1 : campTasksItem.id;
                            dto.cur = 0;
                            dto.state = campTasksItem == null ? NpcState.None : NpcState.OnGoing;


                            Global.gApp.gMsgDispatcher.Broadcast<int, int>(MsgIds.TaskStateChanged, i, dto.state);
                            if (campTasksItem != null)
                            {
                                SaveData();
                                return;
                            }

                        }
                    }
                }
            }

            AfterInit();
        }

        public void NpcQuestChange(float conditionType, double param1, double param2)
        {
            List<CampTasksItem> itemConfigs;
            if (!Global.gApp.gGameData.CampTaskConditonMap.TryGetValue((int)conditionType, out itemConfigs))
            {
                return;
            }
            bool isUpdate = false;
            foreach (CampTasksItem itemConfig in itemConfigs)
            {
                double[] paramArray = new double[] { itemConfig.id, param1, param2 };
                if (FilterFactory.GetInstance().FilterCampTask(itemConfig.taskCondition, paramArray))
                {
                    isUpdate = true;
                }
            }
            if (isUpdate)
            {
                SaveData();
            }
        }

        public List<NpcQuestItemDTO> GetNpcQuestItemDTOListByQuestId(int questId)
        {
            List<NpcQuestItemDTO> list = new List<NpcQuestItemDTO>();
            foreach (NpcQuestItemDTO i in m_Data.npcQuestList)
            {
                if (i.npcQuestId == questId)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        private bool NpcQuestListContainNpcId(string npcId)
        {
            foreach (NpcQuestItemDTO dto in m_Data.npcQuestList)
            {
                if (dto.npcId.Equals(npcId))
                {
                    return true;
                }
            }
            return false;
        }

        public NpcQuestItemDTO RandomQuest(string npcId)
        {
            CampNpcItem npcCfg = Global.gApp.gGameData.CampNpcConfig.Get(npcId);
            if (npcCfg.dispatchTasks == null || npcCfg.dispatchTasks.Length == 0 || npcCfg.dispatchTasks[0] == 0)
            {
                return new NpcQuestItemDTO(npcId);
            }
            List<CampTasksItem> list = new List<CampTasksItem>();
            foreach (int kind in npcCfg.dispatchTasks)
            {
                List<CampTasksItem> cfgList;
                if (!Global.gApp.gGameData.CampTasksKindMap.TryGetValue(kind, out cfgList))
                {
                    continue;
                }
                foreach (CampTasksItem i in cfgList)
                {
                    list.Add(i);
                    break;
                }
            }
            if (list.Count == 0)
            {
                return new NpcQuestItemDTO(npcId);
            }
            int max = 0;
            foreach (CampTasksItem i in list)
            {
                max += i.taskWeight;
            }
            int ran = UnityEngine.Random.Range(0, max);
            int cur = 0;
            int ranKind = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (ran >= cur && ran < list[i].taskWeight + cur)
                {

                    ranKind = list[i].kind;
                    break;

                }
                cur += list[i].taskWeight;
            }

            Global.gApp.gGameData.CampTasksKindMap.TryGetValue(ranKind, out list);
            max = 0;
            foreach (CampTasksItem i in list)
            {
                max += i.detailWeight;
            }
            ran = UnityEngine.Random.Range(0, max);
            cur = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (ran >= cur && ran < list[i].detailWeight + cur)
                {

                    return new NpcQuestItemDTO(npcId, list[i].id);

                }
                cur += list[i].detailWeight;
            }

            return new NpcQuestItemDTO(npcId);
        }

        public NpcQuestItemDTO RandomQuest(string npcId, HashSet<int> kindSet)
        {
            CampNpcItem npcCfg = Global.gApp.gGameData.CampNpcConfig.Get(npcId);
            if (npcCfg.dispatchTasks == null || npcCfg.dispatchTasks.Length == 0 || npcCfg.dispatchTasks[0] == 0)
            {
                return new NpcQuestItemDTO(npcId);
            }
            List<CampTasksItem> list = new List<CampTasksItem>();
            foreach (int kind in npcCfg.dispatchTasks)
            {
                List<CampTasksItem> cfgList;
                if (!Global.gApp.gGameData.CampTasksKindMap.TryGetValue(kind, out cfgList) || kindSet.Contains(kind))
                {
                    continue;
                }
                foreach (CampTasksItem i in cfgList)
                {
                    list.Add(i);
                    break;
                }
            }
            if (list.Count == 0)
            {
                return new NpcQuestItemDTO(npcId);
            }
            int max = 0;
            foreach (CampTasksItem i in list)
            {
                max += i.taskWeight;
            }
            int ran = UnityEngine.Random.Range(0, max);
            int cur = 0;
            int ranKind = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (ran >= cur && ran < list[i].taskWeight + cur)
                {

                    ranKind = list[i].kind;
                    break;

                }
                cur += list[i].taskWeight;
            }


            Global.gApp.gGameData.CampTasksKindMap.TryGetValue(ranKind, out list);
            max = 0;
            foreach (CampTasksItem i in list)
            {
                max += i.detailWeight;
            }
            ran = UnityEngine.Random.Range(0, max);
            cur = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (ran >= cur && ran < list[i].detailWeight + cur)
                {
                    kindSet.Add(ranKind);
                    return new NpcQuestItemDTO(npcId, list[i].id);

                }
                cur += list[i].detailWeight;
            }

            return new NpcQuestItemDTO(npcId);
        }

        //获得npc物品
        public ItemDTO GetNpcAward(int itemId)
        {
            ItemDTO itemDTO = null;
            m_Data.npcAwardMap.TryGetValue(itemId.ToString(), out itemDTO);
            return itemDTO;
        }

        //获得npc
        public ItemDTO GetNpc(int itemId)
        {
            ItemDTO itemDTO = null;
            m_Data.npcMap.TryGetValue(itemId.ToString(), out itemDTO);
            return itemDTO;
        }

        public NpcQuestItemDTO MakeNpcQuestItemDTO(string npcId, int npcQuestId)
        {
            NpcQuestItemDTO dto = new NpcQuestItemDTO(npcId, npcQuestId);
            CampTasksItem cfg = Global.gApp.gGameData.CampTasksConfig.Get(npcQuestId);
            dto.cur = FilterFactory.GetInstance().GetDefault(cfg.taskCondition);
            return dto;
        }

        //重新设置显示npc数目
        //task表示是否生成任务
        public void ResetShowNumAndRedHeart(bool task)
        {
            RefreshRedHeardInfo(task);
            ResetShowNum(task);
            RefreshQuestInfo(task);
            SaveData();
        }
        public void ResetRedHeartInfo(bool task)
        {
            RefreshRedHeardInfo(task);
            RefreshQuestInfo(task);
            SaveData();
        }
        public int CalCampLevel()
        {
            int totalNum = GetTotalNum();
            int campLevel = Global.gApp.gGameData.GetCampLevel(totalNum);
            return campLevel;
        }
        private void ResetShowNum(bool task)
        {
            int maxStep = 4;// Global.gApp.gGameData.CampStepConfig.items.Length - 1;
            int totalNum = 0;
            Dictionary<string, int> npcNumMap = new Dictionary<string, int>();

            foreach (NpcQuestItemDTO dto in m_Data.npcQuestList)
            {
                CampNpcItem npcItem = Global.gApp.gGameData.CampNpcConfig.Get(dto.npcId);
                if (npcItem.notFresh != 1)
                {
                    if (!npcNumMap.ContainsKey(dto.npcId))
                    {
                        npcNumMap[dto.npcId] = 0;
                    }
                    npcNumMap[dto.npcId]++;
                }
            }
            totalNum = GetTotalNum();
            if (totalNum < Global.gApp.gGameData.NpcMax[0])
            {
                return;
            }
            int campLevel = Global.gApp.gGameData.GetCampLevel(totalNum);
            if (totalNum > Global.gApp.gGameData.NpcMax[Global.gApp.gGameData.NpcMax.Length - 1])
            {
                totalNum = Global.gApp.gGameData.NpcMax[Global.gApp.gGameData.NpcMax.Length - 1];
            }
            float showRatio = (float)(Global.gApp.gGameData.NpcShowMax[2 * campLevel - 1] - Global.gApp.gGameData.NpcShowMax[2 * campLevel - 2]) / (Global.gApp.gGameData.NpcMax[2 * campLevel - 1] - Global.gApp.gGameData.NpcMax[2 * campLevel - 2]);
            float taskRatio = (float)(Global.gApp.gGameData.NpcTaskMax[2 * campLevel - 1] - Global.gApp.gGameData.NpcTaskMax[2 * campLevel - 2]) / (Global.gApp.gGameData.NpcShowMax[2 * campLevel - 1] - Global.gApp.gGameData.NpcShowMax[2 * campLevel - 2]);
            double showNumD = Global.gApp.gGameData.NpcShowMax[2 * campLevel - 2] + (totalNum - Global.gApp.gGameData.NpcMax[2 * campLevel - 2]) * showRatio;
            int showNum;
            if (showNumD < 1d && showNumD > 0d)
            {
                showNum = 1;
            }
            else
            {
                showNum = Convert.ToInt32(Math.Floor(showNumD));
            }

            double taskNumD = Global.gApp.gGameData.NpcTaskMax[2 * campLevel - 2] + (showNum - Global.gApp.gGameData.NpcShowMax[2 * campLevel - 2]) * taskRatio;
            int taskNum;
            if (taskNumD < 1d && taskNumD > 0d)
            {
                taskNum = 1;
            }
            else
            {
                taskNum = Convert.ToInt32(Math.Floor(taskNumD));
            }
            //int showMax = Global.gApp.gGameData.NpcShowMax[Global.gApp.gGameData.NpcShowMax.Length - 1];
            //if (showNum > showMax)
            //{
            //    showNum = showMax;
            //    showRatio = ((float)Global.gApp.gGameData.NpcShowMax[campLevel - 1]) / totalNum;
            //}

            Debug.Log("totalNum = " + totalNum);
            Debug.Log("campLevel = " + campLevel);
            Debug.Log("showRatio = " + showRatio);
            Debug.Log("showNum = " + showNum);
            Debug.Log("taskRatio = " + taskRatio);
            Debug.Log("taskNum = " + taskNum);
            int curShow = 0;
            int curTask = 0;
            HashSet<int> kindSet = new HashSet<int>();
            //npc
            foreach (CampNpcItem npcItem in Global.gApp.gGameData.CampNpcConfig.items)
            {
                if (npcItem.notFresh != 1)
                {
                    ItemItem itemCfg = Global.gApp.gGameData.GetItemDataByName(npcItem.id);
                    ItemDTO itemDTO = null;
                    m_Data.npcMap.TryGetValue(itemCfg.id.ToString(), out itemDTO);
                    if (itemDTO == null || itemDTO.num == 0)
                    {
                        continue;
                    }

                    int npcShowNum = 0;
                    double npcShowNumD = (double)(itemDTO.num * showNum) / totalNum;
                    if (npcShowNumD < 1)
                    {
                        npcShowNum = 1;
                    }
                    else
                    {
                        npcShowNum = Convert.ToInt32(Math.Floor(npcShowNumD));
                    }
                    if (curShow + npcShowNum > showNum)
                    {
                        npcShowNum = showNum - curShow;
                        curShow = showNum;
                    }
                    else
                    {
                        curShow += npcShowNum;
                    }
                    Debug.Log("npcShowNum = " + npcShowNum);
                    int npcTaskNum = 0;
                    double npcTaskumD = (double)(npcShowNum * taskNum) / showNum;
                    if (npcTaskumD < 1)
                    {
                        npcTaskNum = 1;
                    }
                    else
                    {
                        npcTaskNum = Convert.ToInt32(Math.Floor(npcTaskumD));
                    }
                    if (curTask + npcTaskNum > taskNum)
                    {
                        npcTaskNum = taskNum - curTask;
                        curTask = taskNum;
                    }
                    else
                    {
                        curTask += npcTaskNum;
                    }
                    Debug.Log("npcTaskNum = " + npcTaskNum);
                    int initShowNum = 0;
                    npcNumMap.TryGetValue(npcItem.id, out initShowNum);
                    for (int i = initShowNum; i < npcShowNum; i++)
                    {
                        NpcQuestItemDTO nqid;
                        if (task && i < npcTaskNum && Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(maxStep))
                        {
                            nqid = RandomQuest(npcItem.id, kindSet);
                        }
                        else
                        {
                            nqid = new NpcQuestItemDTO(npcItem.id);
                        }
                        m_Data.npcQuestList.Add(nqid);
                    }
                }
            }

        }
        //计算非功能性npc总数
        public int GetTotalNum(bool ignore = false)
        {
            int totalNum = 0;
            //计算可生成npc总数
            foreach (ItemDTO itemDTO in m_Data.npcMap.Values)
            {
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(itemDTO.itemId);
                CampNpcItem npcItem = Global.gApp.gGameData.CampNpcConfig.Get(itemCfg.name);
                if (ignore || npcItem.notFresh != 1)
                {
                    totalNum += Convert.ToInt32(itemDTO.num);

                }
            }

            return totalNum;
        }

        //判断处理Npc离开
        public void NpcLeave()
        {
            if (!Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(4))
            {
                return;
            }

            DateTime nowDate = DateTime.Now;
            double nowMills = DateTimeUtil.GetMills(nowDate);
            if (m_Data.OpenCampTime == 0d)
            {
                OpenCampTime = nowMills;
                return;
            }


            double leaveSecs = (nowMills - m_Data.OpenCampTime) / DateTimeUtil.m_Sec_Mills;
            int protectSecs = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_LEAVE_PROTECT_TIME).content);
            int durSecs = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_LEAVE_DUR).content);
            if (leaveSecs <= protectSecs)
            {
                OpenCampTime = nowMills;
                return;
            }
            double leaveNum = (leaveSecs - protectSecs) / durSecs;
            int leaveMax = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_LEAVE_MAX).content);
            if (leaveNum > leaveMax)
            {
                leaveNum = leaveMax;
            }
            leaveNum = Math.Floor(leaveNum);
            int reduceNum = Convert.ToInt32(leaveNum);
            Debug.Log("应删人数 = " + leaveNum);
            if (reduceNum == 0)
            {
                OpenCampTime = nowMills;
                return;
            }
            OpenCampTime = nowMills;

            int totalNum = 0;
            // 计算可生成npc总数
            Dictionary<string, int> npcTotalNumMap = new Dictionary<string, int>();
            foreach (ItemDTO itemDTO in m_Data.npcMap.Values)
            {
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(itemDTO.itemId);
                CampNpcItem npcItem = Global.gApp.gGameData.CampNpcConfig.Get(itemCfg.name);
                if (npcItem.notFresh != 1)
                {
                    int num = Convert.ToInt32(itemDTO.num);
                    npcTotalNumMap[npcItem.id] = num;
                    totalNum += num;
                }
            }
            Debug.Log("总NPC数 = " + totalNum);
            int maxReduceNum = totalNum - int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_MAX_NUM).contents[0]);
            if (reduceNum > maxReduceNum)
            {
                Debug.Log("触发限制，剩余人数不得少于最小非功能人数 = " + (totalNum - maxReduceNum));
                reduceNum = maxReduceNum;
            }

            int showNum = 0;
            int taskNum = 0;
            List<int> canRemoveShowList = new List<int>();
            SortedDictionary<string, int> showNumMap = new SortedDictionary<string, int>();
            for (int i = 0; i < NpcQuestList.Count; i++)
            {
                NpcQuestItemDTO dto = NpcQuestList[i];
                CampNpcItem cfg = Global.gApp.gGameData.CampNpcConfig.Get(dto.npcId);
                if (cfg.notFresh != 1)
                {
                    showNum++;
                    if (dto.npcQuestId > -1)
                    {
                        taskNum++;
                    } else
                    {
                        canRemoveShowList.Add(i);
                    }
                    int itemShowNum = 0;
                    if (!showNumMap.TryGetValue(dto.npcId, out itemShowNum))
                    {
                        showNumMap[dto.npcId] = itemShowNum;
                    }
                    showNumMap[dto.npcId]++;
                } else
                {
                    Debug.Log(cfg.id);
                }
            }
            Debug.Log("显示NPC数 = " + showNum);
            Debug.Log("有任务NPC数 = " + taskNum);

            int randomMin = totalNum - showNum;
            bool removeShow = false;
            Dictionary<int, ItemDTO> reduceMap = new Dictionary<int, ItemDTO>();
            //如果不显示的npc足够扣除，只扣这部分
            if (randomMin >= reduceNum)
            {
                reduceMap = RandomReduceNotShow(reduceNum, showNumMap, npcTotalNumMap);
            } else
            {
                int randomMax = totalNum - taskNum;
                //不能扣已有任务的部分
                reduceNum = (reduceNum > randomMax) ? randomMax : reduceNum;
                reduceMap = RandomReduceNotShow(randomMin, showNumMap, npcTotalNumMap);
                Dictionary<int, ItemDTO> map = RandomReduceShow(reduceNum - randomMin, canRemoveShowList);
                removeShow = map.Count > 0;
                //合并物品
                foreach (ItemDTO itemDTO in map.Values)
                {
                    ItemDTO dto;
                    if (!reduceMap.TryGetValue(itemDTO.itemId, out dto))
                    {
                        reduceMap[itemDTO.itemId] = itemDTO;
                    } else
                    {
                        dto.num += itemDTO.num;
                    }
                }
            }

            //删除物品
            foreach (ItemDTO itemDTO in reduceMap.Values)
            {
                GameItemFactory.GetInstance().ReduceItem(itemDTO);
            }
            //删除显示npc
            if (removeShow)
            {
                List<NpcQuestItemDTO> newList = new List<NpcQuestItemDTO>();
                foreach (NpcQuestItemDTO dto in NpcQuestList)
                {
                    if (dto != null)
                    {
                        newList.Add(dto);
                    }
                }
                NpcQuestList = null;
                NpcQuestList = newList;
            }
            bool reLockInfo = false;
            foreach (ItemDTO itemDTO in reduceMap.Values)
            {
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(itemDTO.itemId);
                for (int i = 0; i < itemDTO.num; i++)
                {
                    NpcRedHeartItemDTO recordItem = null;
                    foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
                    {
                        // 此时标记 用作是否对应创建过
                        if (npcRedHeartItemDTO.npcId == itemCfg.name)
                        {
                            if (recordItem == null)
                            {
                                recordItem = npcRedHeartItemDTO;
                            }
                            else
                            {
                                if (npcRedHeartItemDTO.dropHeartNum < recordItem.dropHeartNum)
                                {
                                    recordItem = npcRedHeartItemDTO;
                                }
                            }
                        }
                    }
                    if (recordItem != null)
                    {
                        reLockInfo = true;
                        m_Data.npcRedHeartList.Remove(recordItem);
                    }
                }
            }
            if (reLockInfo)
            {
                ResetRedHeartInfo(true);
            }
            else
            {
                SaveData();
            }

            if (reduceMap.Count > 0)
            {
                InitCampBuff(true);
                Global.gApp.gSystemMgr.GetNpcMgr().ResetNpcAtkLevel();
                Global.gApp.gUiMgr.OpenPanel(Wndid.DialogueUI, "1002");
            }
        }

        private Dictionary<int, ItemDTO> RandomReduceNotShow(int reduceNum, SortedDictionary<string, int> showNumMap, Dictionary<string, int> npcTotalNumMap)
        {
            Debug.Log("不显示的npc离开 = " + reduceNum);
            Dictionary<int, ItemDTO> removeMap = new Dictionary<int, ItemDTO>();
            if (reduceNum == 0)
            {
                return removeMap;
            }
            int total = 0;
            List<int> canRemoveList = new List<int>();
            foreach (string key in npcTotalNumMap.Keys)
            {
                ItemItem cfg = Global.gApp.gGameData.GetItemDataByName(key);
                int showNum = 0;
                showNumMap.TryGetValue(key, out showNum);
                int notShowNum = npcTotalNumMap[key] - showNum;
                int old = total;
                total += notShowNum;
                for (int i = old; i < total; i++)
                {
                    canRemoveList.Add(cfg.id);
                }
            }

            canRemoveList = canRemoveList.Select(a => new { a, newID = Guid.NewGuid() }).OrderBy(b => b.newID).Select(c => c.a).ToList();
            for (int i = 0; i < reduceNum; i++)
            {
                ItemDTO rn;
                if (!removeMap.TryGetValue(canRemoveList[i], out rn))
                {
                    rn = new ItemDTO(canRemoveList[i], 1, BehaviorTypeConstVal.OPT_CAMP_NPC_LEAVE);
                    removeMap[canRemoveList[i]] = rn;
                } else
                {
                    rn.num++;
                }
            }

            return removeMap;
        }
        //从显示但没任务的npc中随机
        private Dictionary<int, ItemDTO> RandomReduceShow(int reduceNum, List<int> canRemoveShowList)
        {
            Debug.Log("显示d但没任务的npc离开 = " + reduceNum);
            Dictionary<int, ItemDTO> removeMap = new Dictionary<int, ItemDTO>();
            if (reduceNum == 0)
            {
                return removeMap;
            }

            canRemoveShowList = canRemoveShowList.Select(a => new { a, newID = Guid.NewGuid() }).OrderBy(b => b.newID).Select(c => c.a).ToList();
            for (int i = 0; i < reduceNum; i++)
            {
                NpcQuestItemDTO dto = NpcQuestList[canRemoveShowList[i]];
                ItemItem cfg = Global.gApp.gGameData.GetItemDataByName(dto.npcId);
                ItemDTO rn;
                if (!removeMap.TryGetValue(cfg.id, out rn))
                {
                    rn = new ItemDTO(cfg.id, 1, BehaviorTypeConstVal.OPT_CAMP_NPC_LEAVE);
                    removeMap[cfg.id] = rn;
                }
                else
                {
                    rn.num++;
                }
                NpcQuestList[canRemoveShowList[i]] = null;
            }

            return removeMap;
        }

        private void RefreshQuestInfo(bool task)
        {
            // 刷新锁定
            foreach (NpcQuestItemDTO npcQuestItemDTO in m_Data.npcQuestList)
            {
                // 初始化的时候 lockindex 设置成 -1 从新去锁定，否则就是中间刷新，
                if (task)
                {
                    npcQuestItemDTO.lockRedHeartIndex = -1;
                    npcQuestItemDTO.lockWorkerIndex = -1;
                }
                // lockIndex 小于 0表示是 新创建的任务 1 第二天刷新 2 中间刷新创建的新任务 那么就去绑定一个新的npc
                if (npcQuestItemDTO.lockRedHeartIndex < 0)
                {
                    int index = -1;
                    int recordIndex = -1;
                    NpcRedHeartItemDTO recordReadHeartItemDTO = null;
                    foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
                    {
                        // 此时标记 用作是否对应创建过
                        index++;
                        if (!npcRedHeartItemDTO.HasFocusOn && npcRedHeartItemDTO.npcId == npcQuestItemDTO.npcId)
                        {
                            //寻找当前红心数量最多的npc
                            if (recordReadHeartItemDTO == null || recordReadHeartItemDTO.dropHeartNum < npcRedHeartItemDTO.dropHeartNum)
                            {
                                recordReadHeartItemDTO = npcRedHeartItemDTO;
                                recordIndex = index;
                            }
                        }
                    }
                    if (recordReadHeartItemDTO != null)
                    {
                        recordReadHeartItemDTO.HasFocusOn = true;
                        npcQuestItemDTO.lockRedHeartIndex = recordIndex;
                    }
                }
                //worker 只有没有任务的worker 才会去做工作 
                if ((npcQuestItemDTO.npcId == SpecialItemIdConstVal.NPC_WORKER_STR || npcQuestItemDTO.npcId == SpecialItemIdConstVal.NPC_WORKER01_STR
                    ) && npcQuestItemDTO.lockWorkerIndex < 0 && (npcQuestItemDTO.npcQuestId <= 0 || npcQuestItemDTO.state == NpcState.None))
                {
                    int index = -1;
                    int recordIndex = -1;
                    NpcRedHeartItemDTO recordReadHeartItemDTO = null;
                    foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
                    {
                        // 此时标记 用作是否对应创建过
                        index++;
                        if (!npcRedHeartItemDTO.WorkerHasFocusOn && npcRedHeartItemDTO.npcId == npcQuestItemDTO.npcId)
                        {
                            //寻找当前钻石掉落数量最多的worker
                            if (recordReadHeartItemDTO == null || recordReadHeartItemDTO.dropDiamondNum < npcRedHeartItemDTO.dropDiamondNum)
                            {
                                recordReadHeartItemDTO = npcRedHeartItemDTO;
                                recordIndex = index;
                            }
                        }
                    }
                    if (recordReadHeartItemDTO != null)
                    {
                        recordReadHeartItemDTO.WorkerHasFocusOn = true;
                        npcQuestItemDTO.lockWorkerIndex = recordIndex;
                    }
                }
            }
            foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
            {
                // 没有npc 锁定 那么就Frozen forzen
                npcRedHeartItemDTO.CheckDropHeartRrozen();
                npcRedHeartItemDTO.CheckWorkerRrozenStae();
            }
        }
        private void RefreshRedHeardInfo(bool task)
        {
            // 清空锁定状态
            if (task)
            {
                foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
                {
                    npcRedHeartItemDTO.HasFocusOn = false;
                    npcRedHeartItemDTO.WorkerHasFocusOn = false;
                    npcRedHeartItemDTO.LockHasFocusOnTemp = false;
                }
            }
            else
            {
                foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
                {
                    npcRedHeartItemDTO.LockHasFocusOnTemp = false;
                }
            }
            // true 初始化 刷新 一般就是 把 所有的npcmap -》对应创建 红心信息清空状态并且
            foreach (KeyValuePair<string, ItemDTO> npcVal in m_Data.npcMap)
            {
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(npcVal.Value.itemId);
                for (int i = 0; i < npcVal.Value.num; i++)
                {
                    bool findTargetItem = false;
                    foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
                    {
                        // 此时标记 用作是否对应创建过
                        if (!npcRedHeartItemDTO.LockHasFocusOnTemp && npcRedHeartItemDTO.npcId == itemCfg.name)
                        {
                            npcRedHeartItemDTO.LockHasFocusOnTemp = true;
                            findTargetItem = true;
                            break;
                        }
                    }
                    // 没创建过那么就创建一个 并且锁定
                    if (!findTargetItem)
                    {
                        NpcRedHeartItemDTO redHeartItemDTO = new NpcRedHeartItemDTO(itemCfg.name);
                        redHeartItemDTO.LockHasFocusOnTemp = true;
                        m_Data.npcRedHeartList.Add(redHeartItemDTO);
                    }
                }
                // 设置成false 表示未跟某个 任务 锁定
                foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
                {
                    npcRedHeartItemDTO.LockHasFocusOnTemp = false;
                }
            }
        }
        //领取任务奖励
        public void ReceiveQuestReward(int index, Vector3 showPosition)
        {
            if (index < 0 || index >= m_Data.npcQuestList.Count)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3036);
            }
            NpcQuestItemDTO npcQuestItemDTO = m_Data.npcQuestList[index];
            if (npcQuestItemDTO.state != NpcState.UnReceived)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3036);
            }
            CampTasksItem cfg = Global.gApp.gGameData.CampTasksConfig.Get(npcQuestItemDTO.npcQuestId);
            List<ItemDTO> itemDTOs = GameItemFactory.GetInstance().DealItems(cfg.reward, BehaviorTypeConstVal.OPT_CAMP_RECEIVE_AWARD);
            for (int i = 0; i < itemDTOs.Count; i++)
            {
                if (itemDTOs[i].itemId == SpecialItemIdConstVal.GOLD)
                {
                    Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
                    itemDTOs[i].num *= gpiCfg.coinParams;
                }
                itemDTOs[i].paramStr1 = npcQuestItemDTO.npcQuestId.ToString();
                itemDTOs[i].paramStr2 = npcQuestItemDTO.npcId;
                Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, itemDTOs[i].itemId, (int)itemDTOs[i].num, showPosition);
                GameItemFactory.GetInstance().AddItem(itemDTOs[i]);
            }
            npcQuestItemDTO.state = NpcState.Received;

            if (cfg.fallHeart.Length == 2)
            {
                int lockRedHeartIndex = npcQuestItemDTO.lockRedHeartIndex;
                NpcRedHeartItemDTO redHeartItemDTO = GetRedHeartByIndex(lockRedHeartIndex);
                if (redHeartItemDTO != null)
                {
                    redHeartItemDTO.ForceAddRedHeart(int.Parse(cfg.fallHeart[1]));
                }
            }
            SaveData();
            Global.gApp.gMsgDispatcher.Broadcast<int, int>(MsgIds.TaskStateChanged, index, npcQuestItemDTO.state);
        }
        // 进入营地系统的时候开始创建出来
        public void FreshCampInfo()
        {
            FreshDropRedHeartInfo();
            FreshWorkerDropInfo();
            FreshConsume();
            SaveData();
        }
        public void FreshConsume()
        {
            if (!Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(3))
            {
                return;
            }
            double milliSecond = 0;
            foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
            {
                milliSecond += npcRedHeartItemDTO.BalanceConsume();
            }
            double consumeParam = milliSecond / 86400000;

            string[] curConsumes = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_DAY_COST).contents;
            int curId = 0;
            for (int i = 0; i < curConsumes.Length; i++)
            {
                if (i % 2 == 1)
                {
                    double curConsume = consumeParam * double.Parse(curConsumes[i]);
                    double curCount = GameItemFactory.GetInstance().GetItem(curId);
                    if (curConsume > curCount)
                    {
                        curConsume = curCount;
                    }
                    if (curConsume > 0)
                    {
                        ItemDTO addItemDTO = new ItemDTO(curId, curConsume, BehaviorTypeConstVal.OPT_NPC_CONSUME);
                        GameItemFactory.GetInstance().ReduceItem(addItemDTO);
                    }
                }
                else
                {
                    curId = int.Parse(curConsumes[i]);
                }
            }
        }
        public void DtFreshDropRedHeartInfo(NpcRedHeartItemDTO redHeartItemDTO)
        {
            if (!Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(1) || redHeartItemDTO.HasFrozen)
            {
                return;
            }
            string maxTimeStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_FALL_TOTAL_TIME).content;
            int maxTimeInt = int.Parse(maxTimeStr);
            redHeartItemDTO.FreshDropHeart(maxTimeInt);
        }
        private void FreshDropRedHeartInfo()
        {
            if (!Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(1))
            {
                return;
            }
            string maxTimeStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_FALL_TOTAL_TIME).content;
            int maxTimeInt = int.Parse(maxTimeStr);
            foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
            {
                npcRedHeartItemDTO.FreshDropHeart(maxTimeInt);
            }
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampRedTips);
        }
        private void FreshWorkerDropInfo()
        {
            foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
            {
                npcRedHeartItemDTO.FreshWorkerState();
            }
        }
        public int AddRedHeart(int index, int num, Vector3 effectPos)
        {
            if (index >= 0 && index <= m_Data.npcRedHeartList.Count)
            {
                NpcRedHeartItemDTO npcRedItemDTO = m_Data.npcRedHeartList[index];
                CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
                if (campsiteUI != null)
                {
                    campsiteUI.GetHeartRecordTool().AddHeartInfo(npcRedItemDTO, effectPos);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
        public int AddDiamond(int index, Vector3 showPosition)
        {
            if (index >= 0 && index <= m_Data.npcRedHeartList.Count)
            {
                NpcRedHeartItemDTO npcRedItemDTO = m_Data.npcRedHeartList[index];
                int addNum = npcRedItemDTO.PickUpDiamond();
                SaveData();
                if (addNum > 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.MDT, addNum, showPosition);
                    ItemDTO addItemDTO = new ItemDTO(SpecialItemIdConstVal.MDT, addNum, BehaviorTypeConstVal.OPT_CAMP_PICKUP_DIAMOND);
                    GameItemFactory.GetInstance().AddItem(addItemDTO);
                }
                return addNum;
            }
            return 0;
        }
        public NpcRedHeartItemDTO GetRedHeartByIndex(int index)
        {
            if (index >= 0 && index <= m_Data.npcRedHeartList.Count)
            {
                return m_Data.npcRedHeartList[index];
            }
            else
            {
                return null;
            }
        }
        public double GetAdDtTime()
        {
            return DateTimeUtil.GetMills(DateTime.Now) - m_Data.lastAdShowTime;
        }
        public void HeartAdComplet()
        {
            string addNumStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_AD_FALL_HEART).content;
            double addNum = double.Parse(addNumStr);
            m_Data.heartAdWatchTimes++;
            m_Data.lastAdShowTime = DateTimeUtil.GetMills(DateTime.Now);

            CampHeart_paramsItem gpiCfg = Global.gApp.gGameData.CampHeartParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());

            m_Data.AddAdRedHeart((int)(addNum * gpiCfg.coinParams + 0.5f));
            SaveData();
        }
        public bool HasAdTimes()
        {
            int m_AdHeartTotalTimes = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_ADLOOK_MAXNUM).content);
            return m_AdHeartTotalTimes > m_Data.heartAdWatchTimes;
        }
        public double GetAdLeftRedHeartCount()
        {
            return m_Data.leftAdHeart;
        }

        public int GetCampHeartAdWatchTimes()
        {
            return m_Data.heartAdWatchTimes; ;
        }
        public double PickAdRedHeart(double num, Vector3 effectPos)
        {
            CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI);
            if (campsiteUI != null)
            {
                campsiteUI.GetHeartRecordTool().AddHeartInfo(effectPos);
                return 1;
            }
            return 0;
        }

        public void PickHeartImp(ref List<NpcRedHeartItemDTO> redHeartInfos,int adCount,bool saveData = true)
        {
            double addNum = m_Data.PickRedHeart(adCount);
            foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in redHeartInfos)
            {
                int num = npcRedHeartItemDTO.PickUpHeart(1);
                addNum += num;
            }
            if (addNum > 0)
            {
                ItemDTO addItemDTO = new ItemDTO(SpecialItemIdConstVal.RED_HEART, addNum, BehaviorTypeConstVal.OPT_CAMP_PICKUP_HEART);
                GameItemFactory.GetInstance().AddItem(addItemDTO);
                if (saveData)
                {
                    SaveData();
                }
            }
        }
        public bool GetHasHeartAd()
        {
            double curTime = DateTimeUtil.GetMills(DateTime.Now);
            if (curTime - m_Data.lastAdShowTime > 360000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // 守卫想改
        public void FirstEnterFresh()
        {
            m_Data.FirstEnterFresh();
        }
        public double AddGuardReward(Vector3 dstPos)
        {
            double addCount = m_Data.AddGuardReward();
            if (addCount > 0)
            {
                SaveData();
                Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.GOLD, (int)addCount, dstPos);
                ItemDTO addItemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, addCount, BehaviorTypeConstVal.OPT_GUARD_GENERATE_COIN);
                GameItemFactory.GetInstance().AddItem(addItemDTO);
            }
            return addCount;
        }
        public double GetGuardRewardCount()
        {
            return m_Data.GetGuardCurDataRewardCount();
        }
        public void ResetConsumeTime()
        {
            foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
            {
                npcRedHeartItemDTO.ResetConsume();
            }
            SaveData();
        }
        public void ResetHeartTime()
        {
            foreach (NpcRedHeartItemDTO npcRedHeartItemDTO in m_Data.npcRedHeartList)
            {
                npcRedHeartItemDTO.ResetRedHeartTime();
            }
            SaveData();
        }
        public bool ForceAddHeart(NpcQuestItemDTO npcQuestItemDTO)
        {
            NpcRedHeartItemDTO npcRedHeartItemDTO = m_Data.npcRedHeartList[npcQuestItemDTO.lockRedHeartIndex];
            return npcRedHeartItemDTO.ForceAddRedHeart(1);
        }

        public bool LockBuff(SkillItemDTO dto)
        {
            return dto.level == 0 || dto.state == WeaponStateConstVal.NONE;
        }

        public float[] GetBuffParam(string key)
        {
            SkillItemDTO dto;
            if (!CampBuffMap.TryGetValue(key, out dto))
            {
                return null;
            }
            CampBuffItem campBuffItem = Global.gApp.gGameData.GetCampBuffById(key);
            if (campBuffItem == null)
            {
                return null;
            }
            if (LockBuff(dto))
            {
                Debug.Log(key + " = " + campBuffItem.initValue + ", Lock");
                return new float[] { campBuffItem.initValue };
            } else
            {
                CampBuff_dataItem dataI = Global.gApp.gGameData.CampBuffDataConfig.Get(dto.level);
                float[] param = ReflectionUtil.GetValueByProperty<CampBuff_dataItem, float[]>(key, dataI);
                Debug.Log(key + " = " + param[0] + ", level = " + dto.level);
                return param;
            }
        }

        public new string GetDataStr()
        {
            Dictionary<string, string> jsonData = new Dictionary<string, string>();
            jsonData.Add("npcMap", JsonMapper.ToJson(m_Data.npcMap));
            jsonData.Add("buffMap", JsonMapper.ToJson(m_Data.campBuffMap));
            jsonData.Add("npcCount", GetTotalNum(true).ToString());
            return JsonMapper.ToJson(jsonData);
        }
    }
}
