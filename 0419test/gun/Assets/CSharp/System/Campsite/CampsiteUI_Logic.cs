
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EZ
{

    public class TaskItem
    {
        public int taskId = -1;
        public int taskState = -1;
        public int npcId = 1;
    }
    struct NpcPosStruct
    {
        public string NPCId;
        public Vector3 Pos;
    }

    public partial class CampsiteUI
    {
        Camera m_MainCamera;
        GameObject m_ShowNode;
        GameObject m_ControllerNode;
        private float m_DtTime = 0.25f;
        private int m_MaxCount = 5;
        private int m_CreateCount = 1;
        private List<NpcBehavior> m_NpcBehav = new List<NpcBehavior>();
        private List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        private int m_TaskNpcBornNodeIndex = 0;
        private Transform m_NoTaskNpcBornNode;
        private Transform m_TaskNpcBronNode;
        private Transform m_NpcNode;
        private Transform m_FightNode;
        private List<RedHeartEle> m_BillBoardRedHeartList = new List<RedHeartEle>();
        private List<CampsiteUI_ResourceItemUI> m_ResourceItemList = new List<CampsiteUI_ResourceItemUI>();
        private string[] m_EnemyPath = new string[]
        {
            "Prefabs/Campsite/Role/Enemy10000001",
        };
        private int m_EnemyTypeCount = 1;

        List<NpcQuestItemDTO> m_CurTaskList;
        private int m_CreateIndex = 0;

        private GuardRewardNode m_GuardRewardNodeComp;
        private float m_CurGuandTime = 0;
        private float m_CurFreshTime = 5;
        private NpcMgr m_NpcMgr;
        private bool m_HasStepGuid = false;
        private NpcBehavior m_OldWomanBav = null;
        private NpcBehavior m_BoyBav = null;
        private int m_AdHeartTotalTimes = 0;
        private int m_AdHeartWatchTimes = 0;

        private int m_PosEffectIndex = -1;

        RedHeartPickRecord m_RedHeartPickRecord;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            //SetNPCEnabled(true);
            m_ControllerNode.SetActive(true);
            //FreshNodeInfo();
            //m_NpcMgr.FreshCampInfo();
            //m_CurTaskList = m_NpcMgr.NpcQuestList;
            //if (Global.gApp.gSystemMgr.GetMiscMgr().GetIsFirstOpenCampUi())
            //{
            //    m_NpcMgr.FirstEnterFresh();
            //}
            //m_AdHeartWatchTimes = Global.gApp.gSystemMgr.GetNpcMgr().GetCampHeartAdWatchTimes();
            //m_AdHeartTotalTimes = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_ADLOOK_MAXNUM).content);

            //FreshBillboardText();
            //Global.gApp.gLightCompt.enabled = false;
            //QualitySettings.shadowDistance = 100;
            //int campGuidStep = Global.gApp.gSystemMgr.GetCampGuidMgr().GetCurGuidStep();
            //if (campGuidStep >= 0)
            //{
            //    // 第六步需要隐藏 npc boy
            //    if(campGuidStep == 5)
            //    {
            //        if (m_NpcMgr.GetShowBoyNpcState())
            //        {
            //            m_NpcMgr.SetShowBoyNpc(false);
            //        }
            //    }
            //    else if (!m_NpcMgr.GetShowBoyNpcState())
            //    {
            //        m_NpcMgr.SetShowBoyNpc(true);
            //    }
            //    m_HasStepGuid = true;
            //    Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //    if (m_BoyBav != null)
            //    {
            //        m_BoyBav.gameObject.SetActive(m_NpcMgr.GetShowBoyNpcState());
            //    }
            //}
            //else
            //{
            //    m_HasStepGuid = false;
            //}
            //int campLevel = Global.gApp.gSystemMgr.GetNpcMgr().CalCampLevel();
            //if (campLevel > Global.gApp.gSystemMgr.GetNpcMgr().CampShowLevel)
            //{
            //    m_PosEffectIndex = 0;

            //    Global.gApp.gSystemMgr.GetNpcMgr().CampShowLevel = campLevel;
            //    Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //    gameObject.AddComponent<DelayCallBack>().SetAction(()=> {
            //        m_CurGuandTime = -1001;
            //        Global.gApp.gUiMgr.OpenPanel(Wndid.CampToUpgrade);
            //        Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            //    }, 1.1f, true);

            //    Global.gApp.gAudioSource.PlayOneShot("campUp");
            //} else
            //{
            //    m_CurGuandTime = -1001;
            //}
            //m_RedHeartPickRecord.Flush();
            //RegisterListener();
            base.ChangeLanguage();
        }
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            m_RedHeartPickRecord = gameObject.AddComponent<RedHeartPickRecord>();
            m_NpcMgr = Global.gApp.gSystemMgr.GetNpcMgr();
            m_EnemyTypeCount = m_EnemyPath.Length;
            m_ShowNode = GameObject.Find("binansuo_03");
            m_ControllerNode = m_ShowNode.transform.Find("ControllerNode").gameObject;
            m_ControllerNode.SetActive(true);
            m_FightNode = m_ControllerNode.transform.Find("FightNode");
            m_FightNode.gameObject.SetActive(true);
            m_MainCamera = Camera.main;
            Global.gApp.gGameAdapter.AdaptCamera(ref m_MainCamera);
            m_NoTaskNpcBornNode = m_ControllerNode.transform.Find("FightNode/NPCBornNode/Dynamic");
            m_TaskNpcBronNode = m_ControllerNode.transform.Find("FightNode/NPCBornNode/Static");
            m_NpcNode = m_ControllerNode.transform.Find("FightNode/NpcNode");
            m_DrstrangeIconBtn.button.onClick.AddListener(OpenWeaponRaiseUi);
            m_RecycleIconBtn.button.onClick.AddListener(OpenMatBg);

            Transform adFollowNode = m_ControllerNode.transform.Find("FightNode/AdNode");

            AdIconBtn.button.onClick.AddListener(RespondBillboardClick);
            AdIcon.gameObject.GetComponent<FollowNode>().SetFloowNode(adFollowNode);

            m_NameBg.button.onClick.AddListener(OpenDetail);
            m_ResourceBtn.button.onClick.AddListener(OpenResourceDetail);
            m_OldWamonIconBtn.button.onClick.AddListener(OpenCampTaskDetails);
            m_Buff.button.onClick.AddListener(OpenCampBuff);
            m_BadgeIconBtn.button.onClick.AddListener(OpenBadge);

            //检测npc是否离开
            m_NpcMgr.NpcLeave();

            //获取营地点位配置list
            var campsitePoints = m_FightNode.GetComponentsInChildren<CampsiteNpcPoint>(true);
            for (int i = 0; i < campsitePoints.Length; i++)
            {
                var point = campsitePoints[i];
                int linkDataIndex = point.PointId - 1;
                if (CampsiteMgr.singleton.HasPoint(linkDataIndex))
                {
                    point.SetValid(linkDataIndex);
                }
                else
                {
                    point.SetInvalid();
                }
            }

            DelayCallBack delayCallBack = gameObject.AddComponent<DelayCallBack>();
            delayCallBack.SetCallTimes(9999999);
            delayCallBack.SetAction(FreshDropRedHeartData, 30, true);
        }

        private void FreshDropRedHeartData()
        {
            if (Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(1))
            {
                m_RedHeartPickRecord.Flush(false);
                foreach (NpcBehavior npcBehavior in m_NpcBehav)
                {
                    npcBehavior.FreshDropRedHeartData();

                }
                // Flush 成功会 会save data
                m_NpcMgr.SaveData();
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampRedTips);
            }
        }
        private void FreshNodeInfo()
        {
            string[] curConsume = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_DAY_COST).contents;
            string maxDayStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_MAX_STORE_DAY).content;
            double maxDayD = double.Parse(maxDayStr);
            int curId = 0;
            SortedDictionary<int, double> maxCousumeCountMap = new SortedDictionary<int, double>();
            Dictionary<string, ItemDTO> npcMap = m_NpcMgr.NpcMap;
            double totlaNpcNum = 0;
            double totalNotFreshNpcNum = 0;
            foreach (KeyValuePair<string, ItemDTO> kvValue in npcMap)
            {
                totlaNpcNum += kvValue.Value.num;
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(kvValue.Value.itemId);
                CampNpcItem cfg = Global.gApp.gGameData.CampNpcConfig.Get(itemCfg.name);
                if (cfg.notFresh == 1)
                {
                    totalNotFreshNpcNum += kvValue.Value.num;
                }
            }
            for (int i = 0; i < curConsume.Length; i += 2)
            {
                curId = int.Parse(curConsume[i]);
                double curMat = totlaNpcNum * double.Parse(curConsume[i + 1]) * maxDayD;
                maxCousumeCountMap[curId] = curMat;
            }

            // npc人数
            CurNpcNum.text.text = ((int)totlaNpcNum).ToString();
            string[] maxNpcNum = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_MAX_NUM).contents;

            int campLv = m_NpcMgr.CalCampLevel();
            MaxNpcNum.text.text = (int.Parse(maxNpcNum[2 * campLv - 1]) + Global.gApp.gGameData.NotFreshNpcList.Count).ToString();

            FreshBuild();
            FreshCakeBuild();
            FreshCampLvState();
            FreshRecycleAndResState();
            FreshTaskIconState();
            string[] levelNames = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_LEVELS).contents;
            CampName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(int.Parse(levelNames[campLv - 1]));



            double totalCount = 0d;
            double totalMaxCount = 0d;
            ResourceItemUI.gameObject.SetActive(false);
            ClearResourceList();
            foreach (int id in maxCousumeCountMap.Keys)
            {
                double curV = GameItemFactory.GetInstance().GetItem(id);
                double maxV = maxCousumeCountMap[id];
                totalCount += curV;
                totalMaxCount += maxV;
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(id);

                CampsiteUI_ResourceItemUI itemUI = ResourceItemUI.GetInstance();
                itemUI.gameObject.SetActive(true);
                itemUI.Materials.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(itemCfg.image_grow);
                itemUI.Amount.text.text = System.Math.Ceiling(curV).ToString("0.##") + "/" + System.Math.Ceiling(maxV).ToString("0.##");
                itemUI.transform.SetSiblingIndex(id);

                m_ResourceItemList.Add(itemUI);
            }

            int index = -1;
            if (totalMaxCount > 0)
            {
                double rata = (totalCount / (totalMaxCount)) * 100;
                string[] stateJudge = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_HEATH_DEFINITION).contents;
                for (int i = 0; i < stateJudge.Length; i++)
                {
                    if (rata <= double.Parse(stateJudge[i]))
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0)
                {
                    index = stateJudge.Length;
                }
            }
            else
            {
                index = 0;
            }
            string[] stateIds = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_HEATH_NAME).contents;
            CmpState.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(int.Parse(stateIds[index]));
            if (index == 0)
            {

                CmpState.text.color = new Color(255.0f / 255, 39.0f / 255, 39.0f / 255);
            }
            else if (index == 1)
            {

                CmpState.text.color = new Color(255.0f / 255, 234.0f / 255, 99.0f / 255);
            }
            else if (index == 2)
            {
                CmpState.text.color = new Color(183.0f / 255, 245.0f / 255, 60.0f / 255);
            }
        }
        private void CreateNpc()
        {
            NpcQuestItemDTO questItem = m_CurTaskList[m_CreateIndex];
            Transform bornNode;
            bool showOldWomanPlot = false;
            bool IsOldWoman = false;
            bool IsBoy = false;
            GameObject flolowNode = null;
            if (questItem.npcId == "Npc_boy")
            {
                bornNode = m_ControllerNode.transform.Find("FightNode/NPCBornNode/BoyBornNode");
                IsBoy = true;
            }
            else if (questItem.npcId == "Npc_oldwoman")
            {
                if (!Global.gApp.gSystemMgr.GetMiscMgr().GetAndResetHasOpenCampState())
                {
                    showOldWomanPlot = true;
                }
                bornNode = m_ControllerNode.transform.Find("FightNode/NPCBornNode/OldWomanBornNode");
                flolowNode = OldWamonIcon.gameObject;
                IsOldWoman = true;
            }
            else if (questItem.npcId == "Npc_drstrange")
            {
                bornNode = m_ControllerNode.transform.Find("FightNode/NPCBornNode/DrStrangeBornNode");
                flolowNode = DrstrangeIcon.gameObject;
            }
            else if (questItem.npcId == "Npc_recycle")
            {
                bornNode = m_ControllerNode.transform.Find("FightNode/NPCBornNode/RecycleBornNode");
                flolowNode = RecycleIcon.gameObject;
            }
            else
            {
                int index = Random.Range(0, m_NoTaskNpcBornNode.childCount);
                bornNode = m_NoTaskNpcBornNode.GetChild(index);
            }
            CreateNpcImp(bornNode, questItem, m_CreateIndex, flolowNode);
            if (showOldWomanPlot)
            {
                //m_NpcBehav[m_CreateIndex].ShowOldWomanTaskUi(100203);
            }
            if (IsOldWoman)
            {
                m_OldWomanBav = m_NpcBehav[m_CreateIndex];
                m_OldWomanBav.transform.position = bornNode.transform.position;
            }
            else if (IsBoy)
            {
                m_NpcBehav[m_CreateIndex].gameObject.SetActive(m_NpcMgr.GetShowBoyNpcState());
                m_BoyBav = m_NpcBehav[m_CreateIndex];
            }
            m_CreateIndex++;
        }
        private void CreateNpcImp(Transform bornNode, NpcQuestItemDTO taskItem, int taskIndex, GameObject followNode = null)
        {
            string npcId = taskItem.npcId;

            CampNpcItem campNpcItem = Global.gApp.gGameData.CampNpcConfig.Get(npcId);
            GameObject npcNode = Global.gApp.gResMgr.InstantiateObj(campNpcItem.NpcPath);
            npcNode.transform.SetParent(m_NpcNode, false);
            if (taskItem.state != NpcState.None)
            {
                Transform newBornNode = m_TaskNpcBronNode.transform.GetChild(m_TaskNpcBornNodeIndex);
                npcNode.transform.position = newBornNode.position;
                m_TaskNpcBornNodeIndex++;
            }
            else
            {
                Vector3 localScale = bornNode.localScale;
                float x = Random.Range(-0.5f, 0.5f);
                float y = Random.Range(-0.5f, 0.5f);
                Vector3 posOffset = new Vector3(localScale.x * x, localScale.y * y, 0);

                Vector3 position = bornNode.transform.position + posOffset;
                position.z = 0;
                npcNode.transform.position = position;
            }
            NpcBehavior npcBehavior = npcNode.GetComponent<NpcBehavior>();
            npcBehavior.Init(taskItem, taskIndex);
            if (followNode != null)
            {
                FollowNode followComp = followNode.GetComponent<FollowNode>();
                followComp.SetFloowNode(npcBehavior.GetTaskUINode());
                npcBehavior.SetForceHasOutlineEffect();
            }
            m_NpcBehav.Add(npcBehavior);
        }
        private void Update()
        {
            if (m_PosEffectIndex >= 0 && m_PosEffectIndex < Global.gApp.gSystemMgr.GetCampGuidMgr().GetBuidPoss().Length)
            {
                foreach (Vector3 v in Global.gApp.gSystemMgr.GetCampGuidMgr().GetBuidPoss())
                {
                    GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Binansuo_shengji);
                    effect.transform.position = v;
                }
                m_PosEffectIndex = -1;
            }
            // 下一帧需要刷新的东西
            //if (m_CurGuandTime < 0)
            //{
            //    int index = 0;
            //    foreach (NpcBehavior npcBehavior in m_NpcBehav)
            //    {
            //        if (m_CurTaskList[index] != null)
            //        {
            //            npcBehavior.FreshDropInfo();
            //            npcBehavior.FreshTaskState(true);
            //        }
            //        index++;
            //    }
            //    FreshBillboardHeart();
            //    GenerateGuardUi();
            //    FreshGuardInfo();
            //    m_CurGuandTime = 0;
            //}
            //if (m_CreateIndex < m_CurTaskList.Count)
            //{
            //    CreateNpc();
            //}
            //if (m_CreateIndex == m_CurTaskList.Count && m_HasStepGuid)
            //{
            //    Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            //    Global.gApp.gSystemMgr.GetCampGuidMgr().StartCurGuidStep();
            //    m_HasStepGuid = false;
            //}

            //FreshBillboardText();
            //m_CurGuandTime += BaseScene.GetDtTime();
            //if (m_CurGuandTime > m_CurFreshTime)
            //{
            //    m_CurGuandTime -= m_CurFreshTime;
            //    FreshGuardInfo();
            //}
            CheckClick();
        }
        private void CheckClick()
        {
            bool hasToucEvent = false;
            Vector3 screenPosition = Vector3.zero; ;
#if ((UNITY_ANDROID || UNITY_IOS || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR)
            int count = Input.touchCount;
            if (count > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    hasToucEvent = true;
                    screenPosition = Input.GetTouch(0).position;
                }
            }
          
#else
            if (Input.GetMouseButtonDown(0))
            {
                hasToucEvent = true;
                screenPosition = Input.mousePosition;
            }
#endif
            if (hasToucEvent)
            {
                GameObject overGO = GetFirstUIElement(screenPosition);
                if (overGO != null)
                {
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                RaycastHit hit;
                bool isHit = Physics.Raycast(ray, out hit, 10000, 1 << GameConstVal.BossLayer | 1 << GameConstVal.MainRoleLayer);
                if (isHit)
                {
                    //if (hit.collider.gameObject.CompareTag(GameConstVal.NPCTag))
                    //{
                    //    // 奇异博士需要特殊处理
                    //    if (hit.collider.gameObject.name == "Npc_drstrange")
                    //    {
                    //        bool finishStep5 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(4);
                    //        if (finishStep5)
                    //        {
                    //            OpenWeaponRaiseUi();
                    //            return;
                    //        }
                    //    }
                    //    else if (hit.collider.gameObject.name == "Npc_recycle")
                    //    {
                    //        bool finishStep4 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(3);
                    //        if (finishStep4)
                    //        {
                    //            OpenMatBg();
                    //            return;
                    //        }
                    //    }
                    //    else if (hit.collider.gameObject.name == "Npc_oldwoman")
                    //    {
                    //        bool finishStep5 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(4);
                    //        if (finishStep5)
                    //        {
                    //            OpenCampTaskDetails();
                    //            return;
                    //        }
                    //    }
                    //    NpcBehavior monster = hit.collider.gameObject.GetComponentInParent<NpcBehavior>();
                    //    if (monster != null)
                    //    {
                    //        monster.RespondClick();
                    //    }
                    //}
                    //else if (hit.collider.gameObject.name == "BadgeNode")
                    //{
                    //    OpenBadge();
                    //}
                    //点击 主角 的处理
                    //else if (hit.collider.gameObject.CompareTag(GameConstVal.MainRoleTag))
                    //{
                    //    if (m_GuardRewardNodeComp != null)
                    //    {
                    //        m_GuardRewardNodeComp.AddReward();
                    //    }
                    //}

                    if (hit.collider.gameObject.CompareTag(GameConstVal.NPCTag))
                    {
                        CampsiteNpcPoint point = hit.collider.gameObject.GetComponent<CampsiteNpcPoint>();
                        point.RespondClick();
                    }
                }
            }
        }
        private GameObject GetFirstUIElement(Vector2 position)
        {
            EventSystem uiEventSystem = EventSystem.current;
            if (uiEventSystem != null)
            {
                PointerEventData uiPointerEventData = new PointerEventData(uiEventSystem);
                uiPointerEventData.position = position;

                uiEventSystem.RaycastAll(uiPointerEventData, uiRaycastResultCache);
                if (uiRaycastResultCache.Count > 0)
                {
                    return uiRaycastResultCache[0].gameObject;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public override void Recycle()
        {
            base.Recycle();
            if (m_RedHeartPickRecord != null)
            {
                m_RedHeartPickRecord.Flush();
            }
            Global.gApp.gLightCompt.enabled = true;
            SetRedHeartShow(false);
            SetNPCEnabled(false);
            if (m_ControllerNode != null)
            {
                m_ControllerNode.SetActive(false);
            }
            UnRegisterListener();
        }
        private void SetNPCEnabled(bool enable)
        {
            foreach (NpcBehavior npcBehavior in m_NpcBehav)
            {
                npcBehavior.SetEnable(enable);
            }
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int, int>(MsgIds.TaskStateChanged, TaskStateChanged);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, FreshNodeInfo);
        }
        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, int>(MsgIds.TaskStateChanged, TaskStateChanged);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, FreshNodeInfo);
        }
        private void TaskStateChanged(int taskIndex, int taskState)
        {
            foreach (NpcBehavior npc in m_NpcBehav)
            {
                if (npc.GetTaskIndex() == taskIndex)
                {
                    npc.FreshTaskState();
                }
            }
        }
        public override void Release()
        {
            if (m_RedHeartPickRecord != null)
            {
                m_RedHeartPickRecord.Flush();
            }
            base.Release();
            Global.gApp.gLightCompt.enabled = true;
            SetRedHeartShow(false);
            if (m_ControllerNode != null)
            {
                m_ControllerNode.SetActive(false);
            }
            else
            {
                m_ShowNode = GameObject.Find("binansuo");
                if (m_ShowNode != null)
                {
                    m_ShowNode.gameObject.SetActive(false);
                }
            }
            UnRegisterListener();
        }

        private void GenerateGuardUi()
        {
            if (m_GuardRewardNodeComp == null)
            {
                double addCount = m_NpcMgr.GetGuardRewardCount();
                if (addCount >= 1)
                {
                    GameObject guardUiGo = Global.gApp.gResMgr.InstantiateObj(Wndid.GuardRewardNode);
                    m_GuardRewardNodeComp = guardUiGo.GetComponent<GuardRewardNode>();
                    //m_GuardRewardNodeComp.Init(m_CampsitePerformer.Find("TaskNode"));
                    FreshGuardInfo();
                }
            }
        }
        private void RespondBillboardClick()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.CAMP_TV);
            if (!m_NpcMgr.HasAdTimes())
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1007);
                return;
            }
            if (m_NpcMgr.GetHasHeartAd())
            {
                //CompleteAds(true);
                //AdManager.instance.ShowRewardVedio(CompleteAds, AdShowSceneType.CAMP_GET_HEART, 0, 0, 0);				
                Global.gApp.gUiMgr.OpenPanel(Wndid.ConfirmLookAdUI);
            }
        }


        public void CompleteAds(bool ended)
        {
            //Global.gApp.gMsgDispatcher.RemoveListener<bool>(MsgIds.ViewAdCallBack, CompleteAds);
            if (ended)
            {

                m_NpcMgr.HeartAdComplet();
                m_AdHeartWatchTimes = Global.gApp.gSystemMgr.GetNpcMgr().GetCampHeartAdWatchTimes();
                FreshBillboardText();
                FreshBillboardHeart();
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampRedTips);
            }
            else
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3040);
            }
        }
        private void FreshBillboardHeart()
        {

            int dropCount = (int)m_NpcMgr.GetAdLeftRedHeartCount();
            if (dropCount > 0)
            {
                float dtDeg = Mathf.PI / dropCount;
                float radio = 100f;

                for (int i = 0; i < dropCount; i++)
                {
                    RedHeartEle redHeartEle;
                    if (m_BillBoardRedHeartList.Count < i + 1)
                    {
                        GameObject redHeartGo = Global.gApp.gResMgr.InstantiateObj(Wndid.ReadHeart);
                        redHeartEle = redHeartGo.GetComponent<RedHeartEle>();
                        m_BillBoardRedHeartList.Add(redHeartEle);
                        redHeartEle.transform.SetParent(RedHeartNode.rectTransform, false);
                    }
                    else
                    {
                        redHeartEle = m_BillBoardRedHeartList[i];
                    }
                    float posX = radio * Mathf.Cos(dtDeg * i);
                    float posY = radio * Mathf.Sin(dtDeg * i);
                    redHeartEle.SetInfo(this, 1);
                    Vector2 worldPos = m_AdIcon.rectTransform.anchoredPosition + new Vector2(posX, posY);
                    redHeartEle.SetUiPos(worldPos);
                }
            }
            while (dropCount < m_BillBoardRedHeartList.Count)
            {
                int curIndex = m_BillBoardRedHeartList.Count - 1;
                Destroy(m_BillBoardRedHeartList[curIndex].gameObject);
                m_BillBoardRedHeartList.RemoveAt(curIndex);
            }
        }
        public void PickRedHeart(RedHeartEle redHeartEle, double num)
        {
            double addCount = m_NpcMgr.PickAdRedHeart(num, redHeartEle.transform.position);
            if (addCount > 0)
            {
                m_BillBoardRedHeartList.Remove(redHeartEle);
                Destroy(redHeartEle.gameObject);
            }
        }
        private void FreshBillboardText()
        {
            if (m_AdHeartWatchTimes >= m_AdHeartTotalTimes)
            {
                m_AdCountDown.text.text = string.Empty;
                m_AdIconBtn.button.interactable = true;
                return;
            }
            if (!m_NpcMgr.GetHasHeartAd())
            {
                double dtTime = m_NpcMgr.GetAdDtTime() / 1000;
                int realLeft = 360 - (int)dtTime;
                m_AdCountDown.text.text = EZMath.FormateTimeMMSS(realLeft);
                m_AdIconBtn.button.interactable = false;
            }
            else
            {
                m_AdCountDown.text.text = string.Empty;
                m_AdIconBtn.button.interactable = true;
            }
        }
        public Transform GetTaskStateNodeTsf()
        {
            return TaskStateNode.rectTransform;
        }
        public Transform GetRedHeartNodeTsf()
        {
            return RedHeartNode.rectTransform;
        }
        private void FreshGuardInfo()
        {
            if (m_GuardRewardNodeComp != null)
            {
                m_GuardRewardNodeComp.Fresh(m_NpcMgr.GetGuardRewardCount());
            }
        }
        private void OpenWeaponRaiseUi()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.CAMP_WEAPON);
            Global.gApp.gUiMgr.OpenPanel(Wndid.WeaponRaiseUI);
        }
        private void OpenMatBg()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.CAMP_MAT);
            Global.gApp.gUiMgr.OpenPanel(Wndid.CampMatBag);
        }
        private void OpenDetail()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.CAMP_DETAIL);
            Global.gApp.gUiMgr.OpenPanel(Wndid.CampDetailUI);
        }

        private void OpenResourceDetail()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.CAMP_RESOURCE);
            Global.gApp.gUiMgr.OpenPanel(Wndid.CampResourcesDetailUI);
        }

        private void OpenCampTaskDetails()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.CAMP_TASK);
            Global.gApp.gUiMgr.OpenPanel(Wndid.CampTaskDetails);
        }
        private void OpenCampBuff()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.CAMP_BUFF);
            Global.gApp.gUiMgr.OpenPanel(Wndid.CampBUFF);
        }

        private void ClearResourceList()
        {
            foreach (CampsiteUI_ResourceItemUI obj in m_ResourceItemList)
            {
                ResourceItemUI.CacheInstance(obj);
            }
            m_ResourceItemList.Clear();

        }
        private void SetRedHeartShow(bool enable)
        {
            CommonUI commonUI = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            if (commonUI != null)
            {
                //commonUI.SetHeartNodeEnable(enable);
            }
        }
        public void FreshBuild(bool withEffect = false)
        {
            int campLv = m_NpcMgr.CalCampLevel();
            bool finishStep2 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(1);
            if (!finishStep2)
            {
                campLv = -100;
                AdIcon.gameObject.SetActive(false);
                m_BadgeIcon.gameObject.SetActive(false);
            }
            else
            {
                if (withEffect)
                {
                    m_BadgeIcon.gameObject.SetActive(false);
                }
                else
                {
                    m_BadgeIcon.gameObject.SetActive(true);
                }
                m_BadgeIcon.gameObject.GetComponent<FollowNode>().SetFloowNode(m_ControllerNode.transform.Find("buildNode/binansuo/BadgeNode"));
                AdIcon.gameObject.SetActive(true);
            }
            m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/Lv1").gameObject.SetActive(campLv == 1);
            m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/Lv2").gameObject.SetActive(campLv == 2);
            m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/Lv3").gameObject.SetActive(campLv == 3);

            m_ControllerNode.transform.Find("FightNode/Collider1").gameObject.SetActive(campLv == 1);
            m_ControllerNode.transform.Find("FightNode/Collider2").gameObject.SetActive(campLv == 2);
            m_ControllerNode.transform.Find("FightNode/Collider3").gameObject.SetActive(campLv == 3);
            if (withEffect)
            {
                GameObject effect1 = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Ui_anniuchuxian_1);
                effect1.transform.position = AdIcon.rectTransform.position;
            }
            bool finishStep1 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(1);
            SetRedHeartShow(finishStep1);
        }
        public void FreshBadgeByGuid()
        {
            m_BadgeIcon.gameObject.SetActive(true);
            m_BadgeIcon.gameObject.GetComponent<FollowNode>().SetFloowNode(m_ControllerNode.transform.Find("buildNode/binansuo/BadgeNode"));
            GameObject effect2 = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Ui_anniuchuxian_1);
            effect2.transform.position = m_BadgeIconBtn.rectTransform.position;
        }
        public void FreshCakeBuild(bool forceShow = false)
        {
            bool finishStep8 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(7);
            finishStep8 = finishStep8 || forceShow;
            m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/Lv1/Bns_fanmaiche01").gameObject.SetActive(finishStep8);
            m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/Lv2/Bns_fanmaiche02").gameObject.SetActive(finishStep8);
            m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/Lv3/Bns_fanmaiche03").gameObject.SetActive(finishStep8);
        }
        public void ResetOriAnim()
        {
            foreach (NpcBehavior npcBehavior in m_NpcBehav)
            {
                npcBehavior.ResetOriAnim();
            }
        }
        public void PlayNPCheerAnimAndFreshHeart()
        {
            foreach (NpcBehavior npcBehavior in m_NpcBehav)
            {
                NpcQuestItemDTO npcQuestItemDTO = npcBehavior.GetQuestItem();
                bool addHeart = m_NpcMgr.ForceAddHeart(npcQuestItemDTO);
                npcBehavior.PlayCheerAnim(addHeart);
            }
            m_NpcMgr.SaveData();
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampRedTips);
        }
        public void FreshCampLvState(bool forceShow = false)
        {
            if (forceShow)
            {
                NameBg.gameObject.SetActive(true);
                GameObject effect2 = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Ui_anniuchuxian_2);
                effect2.transform.position = m_NameEffectNode.rectTransform.position;
                m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/jianzhu_fuxu").gameObject.SetActive(false);
                m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/jianzhu_wanzheng").gameObject.SetActive(true);

            }
            else
            {
                bool finishStep3 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(2);
                NameBg.gameObject.SetActive(finishStep3);
                m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/jianzhu_fuxu").gameObject.SetActive(!finishStep3);
                m_ControllerNode.transform.Find("buildNode/binansuo/LightMapNode/jianzhu_wanzheng").gameObject.SetActive(finishStep3);
            }
        }
        public void FreshRecycleAndResState(bool forceShow = false)
        {
            if (forceShow)
            {
                m_RecycleIcon.gameObject.SetActive(true);
                m_ResourceNode.gameObject.SetActive(true);
                m_ResState.gameObject.SetActive(true);
                ResourceBtn.gameObject.SetActive(true);

                GameObject effect1 = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Ui_anniuchuxian_1);
                effect1.transform.position = m_RecycleIconBtn.rectTransform.position;
            }
            else
            {
                bool finishStep4 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(3);
                m_RecycleIcon.gameObject.SetActive(finishStep4);
                m_ResourceNode.gameObject.SetActive(finishStep4);
                m_ResState.gameObject.SetActive(finishStep4);
                ResourceBtn.gameObject.SetActive(finishStep4);
            }
        }
        public void FreshTaskIconState(bool forceShow = false)
        {
            if (forceShow)
            {
                OldWamonIcon.gameObject.SetActive(true);
                DrstrangeIcon.gameObject.SetActive(true);


                GameObject effect1 = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Ui_anniuchuxian_1);
                effect1.transform.position = OldWamonIconBtn.rectTransform.position;

                GameObject effect2 = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Ui_anniuchuxian_1);
                effect2.transform.position = DrstrangeIconBtn.rectTransform.position;

                m_Buff.gameObject.SetActive(true);

                GameObject effect3 = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Ui_anniuchuxian_1);
                effect3.transform.position = m_Buff.rectTransform.position;
            }
            else
            {
                bool finishStep5 = Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(4);
                OldWamonIcon.gameObject.SetActive(finishStep5);
                DrstrangeIcon.gameObject.SetActive(finishStep5);
                m_Buff.gameObject.SetActive(finishStep5);
            }
        }

        public Vector3 GetOldWomanTaskNodeUiPos()
        {
            if (m_OldWomanBav != null)
            {
                return m_OldWomanBav.GetCalcRightAdaptNodePos();
            }
            else
            {
                return Vector3.zero;
            }
        }
        public void FreshAllNpcTaskState()
        {
            m_CurTaskList = m_NpcMgr.NpcQuestList;
            List<NpcPosStruct> npcBehaviorPos = new List<NpcPosStruct>();
            foreach (NpcBehavior npcBehavior in m_NpcBehav)
            {
                npcBehaviorPos.Add(new NpcPosStruct()
                {
                    NPCId = npcBehavior.GetNpcId(),
                    Pos = npcBehavior.transform.position,
                });
                npcBehavior.DestroyNpc();
            }
            m_NpcBehav.Clear();
            m_CreateIndex = 0;
            int curIndex;
            m_CurGuandTime = -100;
            while (m_CreateIndex < m_CurTaskList.Count)
            {
                curIndex = m_CreateIndex;
                CreateNpc();
                NpcBehavior newNpcBehavior = m_NpcBehav[curIndex];
                string npcId = newNpcBehavior.GetNpcId();
                int index = 0;
                foreach (NpcPosStruct npcPosStruct in npcBehaviorPos)
                {
                    if (npcId == npcPosStruct.NPCId)
                    {
                        newNpcBehavior.transform.position = npcPosStruct.Pos;
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }
                npcBehaviorPos.RemoveAt(index);
            }
        }
        public void FreshOldWomanTaskState()
        {
            if (m_OldWomanBav != null)
            {
                m_OldWomanBav.ForceFreshTaskState();
            }
        }
        public RedHeartPickRecord GetHeartRecordTool()
        {
            return m_RedHeartPickRecord;
        }
        private void OpenBadge()
        {
            if (Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(1))
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampBadgeUI);
            }
        }
    }
}
