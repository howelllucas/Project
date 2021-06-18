 
using EZ.Data;
using EZ.Util;
using UnityEngine;

namespace EZ
{
    public enum ModeEnum
    {
        NORMAL = 0,
        RE_NORMAL = 1,
        SPECIAL = 2,
    }

    public class FightScene : BaseScene
    {
        private PlayerMgr m_PlayerMgr;
        private Player m_MainPlayerComp;
        private GameObject m_MainPlayer;
        private GameObject m_BornNode;
        private BornNode[] m_BornNodes;
        private WaveMgr m_WaveMgr;
        private PropMgr m_PropMgr;
        private PassItem m_PassData;

        private GameObject m_MapGo;
        protected bool m_Ended;
        private TimerMgr m_TimerMgr;
        private FightWinCondition m_GameWinCondition;
        private FightGamePlay m_GamePlay;
        private TaskModeMgr m_TaskModeMgr;
        private float m_CurTime = 0;
        private SceneType m_SceneType;
        private bool m_InNormalPassType;

        //0正常关卡 1重玩正常关卡 2特殊关卡
        private ModeEnum m_ModeEnum;
    
        //private Map m_Map;
        public FightScene(PassItem passData)
        {
            m_TimerMgr = new TimerMgr();
            m_Ended = false;
            m_PassData = passData;
            m_SceneType = (SceneType)passData.sceneType;
            m_InNormalPassType = (m_SceneType == SceneType.NormalScene);
            GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
            int type = passData.id / System.Convert.ToInt32(initPassIdConfig.content);
            if (type == 1)
            {
                if (Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId() != m_PassData.id || Global.gApp.gSystemMgr.GetPassMgr().GetHasPassedMaxPass())
                {
                    m_ModeEnum = ModeEnum.RE_NORMAL;
                } else
                {
                    m_ModeEnum = ModeEnum.NORMAL;
                }
            } else
            {
                m_ModeEnum = ModeEnum.SPECIAL;
            }
            Global.gApp.gLightCompt.enabled = true;
            //m_Map = new Map(90, 90, m_MainPlayer);
        }
        private void CreateColliderMap()
        {
            m_MapGo = Global.gApp.gResMgr.InstantiateObj(m_PassData.collider);
            m_MapGo.transform.SetParent(Global.gApp.gKeepNode.transform);
            GameObject go = Global.gApp.gResMgr.InstantiateObj("Prefabs/MapData/" + m_PassData.scene + "_BornNode");
            go.transform.SetParent(m_MapGo.transform);
        }
        private void CreateMainPlayer()
        {
            m_MainPlayer = m_PlayerMgr.CreatePlayer();
            if (m_PassData.scene.Equals("city_02") || m_PassData.scene.Equals("lianyouchang_04"))
            {
                GameObject go = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Xiayu_01);
                go.transform.SetParent(m_MainPlayer.transform,false);
            }
            m_MainPlayerComp = m_MainPlayer.GetComponent<Player>();
            //m_MainPlayer.GetComponent<Player>().SetHp(1000);
            Transform bornNode = m_MapGo.transform.Find("BornPos");
            if (bornNode)
            {
                int childCount = bornNode.childCount;
                if(childCount == 0)
                {
                    m_MainPlayer.transform.position = new Vector3(bornNode.position.x, bornNode.position.y, 0);
                }
                else
                {
                    int posIndex = UnityEngine.Random.Range(0, childCount + 1);
                    if(posIndex < childCount)
                    {
                        Transform newPosNode = bornNode.GetChild(posIndex);
                        m_MainPlayer.transform.position = new Vector3(newPosNode.position.x, newPosNode.position.y, 0);
                    }
                    else
                    {
                        m_MainPlayer.transform.position = new Vector3(bornNode.position.x, bornNode.position.y, 0);
                    }
                }
            }
            if (m_SceneType == SceneType.CarScene)
            {
                m_MainPlayer.GetComponent<Player>().SetInitCar("Prefabs/Carrie/Car_002");
            }
            else if(m_SceneType == SceneType.BreakOutSene)
            {
                m_MainPlayer.transform.localScale = m_MainPlayer.transform.localScale * 1.6f;
            }
        }
        public override Player GetMainPlayerComp()
        {
            return m_MainPlayerComp;
        }
        public override GameObject GetMainPlayer()
        {
            return m_MainPlayer;
        }
        public override void Init()
        {
            base.Init();
            CreateColliderMap();
            m_PlayerMgr = new PlayerMgr();
            CreateMainPlayer();

            if (m_SceneType == SceneType.NormalScene)
            {
                Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>().Reset();
                base.InitCamera("Prefabs/MapData/Camera/MainCamera");
                QualitySettings.shadowDistance = 35;
                InitBornNodeInfo();
                AddTipNpc(40);
            }
            else if (m_SceneType == SceneType.CarScene)
            {
                Global.gApp.gCamCompt.transform.position = Vector3.zero;
                Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>().Reset();
                Global.gApp.gCamCompt.enabled = false;
                base.InitCamera("Prefabs/MapData/Camera/CarCamera2");
                QualitySettings.shadowDistance = 50;
            }
            else
            {
                Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>().Reset();
                QualitySettings.shadowDistance = 50;
                Global.gApp.gCamCompt.transform.position = Vector3.zero;
                Global.gApp.gCamCompt.enabled = false;
                base.InitCamera("Prefabs/MapData/Camera/CarCamera3");
            }
            Global.gApp.gUiMgr.OpenPanel(Wndid.FightPanel);
            Global.gApp.gUiMgr.OpenPanel(Wndid.RewardEffectUi);
            m_PropMgr = new PropMgr();

            m_WaveMgr = new WaveMgr(m_PassData, m_MainPlayer);
            // lastInit 
            m_TaskModeMgr = new TaskModeMgr(m_MapGo.transform, m_MainPlayer, m_PassData);

            m_TaskModeMgr.BeginTask();
            //m_GamePlay.Init(m_PassData, this, m_MapGo,m_MainPlayer.transform);
            //m_GamePlay.BroadMsg();


            DarkEffect de = m_MainCamera.GetComponent<DarkEffect>();
            if (de != null)
            {
                de.enabled = (m_PassData.limitView != 0);
                if (de.enabled)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<GameObject, bool, int>(MsgIds.FocusGameObject, m_MainPlayer, true, 355);
                }
            }

            if (m_PassData.enableVIT != 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, "PlayerEnergyProp", "PlayerEnergyProp", 10);
            }
            if (m_SceneType != SceneType.CarScene)
            {
                Global.gApp.gGameCtrl.EffectCache.CacheEffect(EffectConfig.DeadEffect, 20);
            }
            else
            {
                Global.gApp.gGameCtrl.EffectCache.CacheEffect(EffectConfig.DeadEffect, 5);
                Global.gApp.gGameCtrl.EffectCache.CacheEffect(EffectConfig.DeadEffect1, 20);
            }
            InitSpecialSceneSpeed();

        }

        public override TimerMgr GetTimerMgr()
        {
            return m_TimerMgr;
        }
        public override void Update(float dt)
        {
            dt = dt * BaseScene.TimeScale;
            m_CurTime = m_CurTime + dt;
            m_WaveMgr.Update(dt);
            m_TimerMgr.Update(dt);

#if GAME_DEBUG
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameWin();
            }
#endif
        }
        public override WaveMgr GetWaveMgr()
        {
            return m_WaveMgr;
        }
        public override PropMgr GetPropMgr()
        {
            return m_PropMgr;
        }
        public override PlayerMgr GetPlayerMgr()
        {
            return m_PlayerMgr;
        }

        public override PassItem GetPassData()
        {
            return m_PassData;
        }
        public override Map GetMap()
        {
            return null;
        }
        public override void GameLose()
        {
            if (m_Ended) { return; }
            Pause();
            m_Ended = true;
			// log
            FightResultManager.instance.KillProgress = m_TaskModeMgr.GetCurTaskMode().Progress;
			FightResultManager.instance.SetFightState(FightResultManager.FightState.FAIL);
            //int revive = m_MainPlayer.GetComponent<Player>().GetPlayerData().GetReviveTimes();
            //Global.gApp.gSystemMgr.GetPassMgr().GameLose(m_PassData.id, revive, (int)m_CurTime);
            //Player tmp = m_MainPlayer.GetComponent<Player>();
            Global.gApp.gUiMgr.ClosePanel(Wndid.FightPanel);
            //FightResultManager.instance.ShowLosePopup(tmp == null ? 0 : tmp.GetPlayerData().GetGold());

            Game.PlayerDataMgr.singleton.RequestFinishStage(false, 0, (b) =>
            {
                if (b)
                {
                    FightResultManager.instance.ShowLosePopup(0);
                }
            });
            //int enterTimes = Global.gApp.gSystemMgr.GetPassMgr().GetPassEnterTimes();
        }
        public override void GameWin()
        {
            if (m_Ended) { return; }
            m_Ended = true;
            // log
            FightResultManager.instance.KillProgress = 100;
            FightResultManager.instance.SetFightState(FightResultManager.FightState.SUCCESS);
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //int revive = m_MainPlayer.GetComponent<Player>().GetPlayerData().GetReviveTimes();
            //Global.gApp.gSystemMgr.GetPassMgr().GameSucess(m_PassData.id, revive, (int)m_CurTime);
            Global.gApp.gUiMgr.ClosePanel(Wndid.FightPanel);
            Player ply = m_MainPlayer.GetComponent<Player>();
            ply.GetPlayerData().ResetProtectTime(-100000);
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            //int enterTimes = Global.gApp.gSystemMgr.GetPassMgr().GetPassEnterTimes();

            Game.PlayerDataMgr.singleton.RequestFinishStage(true, (int)(Game.PlayerDataMgr.singleton.StageGold *
                                                            ply.GoldRate), (b) =>
                                                            {

                                                                if (b)
                                                                {
                                                                    var curLife = ply.GetPlayerData().GetHp();
                                                                    var maxLife = ply.GetPlayerData().GetMaxHp();
                                                                    var reviveCount = ply.GetPlayerData().GetReviveTimes();
                                                                    Game.PlayerDataMgr.singleton.GetStageStarList(true, curLife / maxLife, reviveCount);

                                                                    m_TimerMgr.AddTimer(1.5f, 1, DelayWin);
                                                                }
                                                            });
            //m_TimerMgr.AddTimer(1.5f, 1, DelayWin);
        }
        private void DelayWin(float dt,bool end)
        {
            Player tmp = m_MainPlayer.GetComponent<Player>();
            //FightResultManager.instance.ShowWinPopup(tmp == null ? 0 : tmp.GetPlayerData().GetGold());
            FightResultManager.instance.ShowWinPopup(Game.PlayerDataMgr.singleton.StageGold * tmp.GoldRate);
            Pause();
        }
        public override void OnDestroy()
        {
            if (!m_Ended)
            {
                FightResultManager.instance.SetFightState(FightResultManager.FightState.QUIT);
                //ELK 日志打点 主动退出
                int revive = m_MainPlayer.GetComponent<Player>().GetPlayerData().GetReviveTimes();
                ELKLogMgr.GetInstance().SendELKLog4Pass(ELKLogMgr.PASS_QUIT, revive, (int)m_CurTime);
            }
            //记录次数
            Global.gApp.gSystemMgr.GetPassMgr().PassTimesChange(m_PassData.id);
            //Debug.Log("passId = " + m_PassData.id + ", m_ModeEnum = " + (int)m_ModeEnum);

            Debug.Log("m_Ended=" + m_Ended);
            //infoc 日志
            //InfoCLogUtil.instance.SendStageLog(m_PassData.id, (int)m_CurTime, 0f, m_MainPlayer.GetComponent<Player>().GetPlayerData().GetHitHp(), (int)m_ModeEnum);
            //af 日志
            AfLog(m_PassData.id);
            
            //重置暂存数据
            FightResultManager.instance.SetFightState(FightResultManager.FightState.FAIL);
            FightResultManager.instance.SetRetryType(FightResultManager.RetryType.NONE);
            FightResultManager.instance.SetCoin(0);
            FightResultManager.instance.SetRewardType(FightResultManager.RewardType.NONE);

            base.OnDestroy();
            m_MapGo.SetActive(false);
            if(m_BornNode!= null)
            {
                Object.Destroy(m_BornNode);
            }
            m_TaskModeMgr.Destroy();
            m_PropMgr.Destroy();
            m_WaveMgr.Destroy();
            m_PlayerMgr.Destroy();
            //m_GameWinCondition.Destroy();
            //m_GamePlay.Destroy();
            Object.Destroy(m_MapGo);
            DestroyBullet();
            Global.gApp.gUiMgr.ClosePanel(Wndid.FightPanel);
        }
        private void AddTipNpc(float checkTime)
        {
            int curPass = m_PassData.id;
            int plotId = -1;
            string[] passInfo = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.NPC_BOY_APPEAR_PASS).contents;
            string[] plotInfo = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.NPC_BOY_APPEAR_WORD).contents;
            for (int i = 0; i < passInfo.Length; i++)
            {
                int newPassId = int.Parse(passInfo[i]);
                if (newPassId == curPass)
                {
                    if (plotInfo.Length >= i)
                    {
                        plotId = int.Parse(plotInfo[i]);
                    }
                    break;
                }
            }
            if (plotId > 0)
            {
                m_TimerMgr.AddTimer(checkTime, 1, (float a, bool b) =>
                {
                    foreach (BornNode bornNode in m_BornNodes)
                    {
                        if (bornNode.GetIsOutMap()){
                            GameObject Npc_boy = Global.gApp.gResMgr.InstantiateObj("Prefabs/Campsite/NpcFight/Npc_boy");
                            Npc_boy.transform.position = bornNode.transform.position;
                            FightNormalNpcPlayer npcPlayer = Npc_boy.GetComponent<FightNormalNpcPlayer>();
                            npcPlayer.Init(m_MainPlayer);
                            npcPlayer.SetFoolAppearActPlotId(plotId);
                            npcPlayer.SetBehavior(FightNpcPlayer.NpcBehaviorType.FoolAppear);
                            return;
                        }
                    }
                    AddTipNpc(2);
                });
            }
        }
        private void InitBornNodeInfo()
        {
            m_BornNode = Global.gApp.gResMgr.InstantiateObj("Prefabs/MapData/BornNode");
            m_BornNode.GetComponent<LockRotation>().SetLockTsf(m_MainPlayer.transform);
            m_BornNode.transform.SetParent(Global.gApp.gRoleNode.transform, false);
            m_BornNodes = m_BornNode.GetComponentsInChildren<BornNode>();
        }
        public BornNode[] GetBornNodes()
        {
            return m_BornNodes;
        }
        private void DestroyBullet()
        {
            GameObject bulletNode = Global.gApp.gBulletNode;
            GameObject newBulletNode = new GameObject();
            newBulletNode.transform.SetParent(bulletNode.transform.parent, false);
            newBulletNode.name = bulletNode.name;
            Global.gApp.gBulletNode = newBulletNode;
            Object.Destroy(bulletNode);
        }
        public override void Resume()
        {
            base.Resume();
            if (m_PauseRef == 0)
            {
                Animator[] allAnim = Global.gApp.gRoleNode.GetComponentsInChildren<Animator>();
                foreach (Animator anim in allAnim)
                {
                    anim.speed = 1;
                }
                ParticleSystem[] system = m_MainPlayer.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem anim in system)
                {
                    var main = anim.main;
                    main.simulationSpeed = 1;
                }
                Global.gApp.gAudioSource.UnPause();
            }
        }
        public override void Pause()
        {
            base.Pause();
            m_MainPlayerComp.GetPlayerData().PausePrtect();
            if (m_PauseRef > 0)
            {
                Animator[] allAnim = Global.gApp.gRoleNode.GetComponentsInChildren<Animator>();
                foreach (Animator anim in allAnim)
                {
                    anim.speed = 0;
                }
                ParticleSystem[] system = m_MainPlayer.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem anim in system)
                {
                    var main = anim.main;
                    main.simulationSpeed = 0;
                }
                Global.gApp.gAudioSource.Pause();
            }
        }
        
        public void ResumeRoleAnim()
        {
            Animator[] allAnim = GetMainPlayer().GetComponentsInChildren<Animator>();
            foreach (Animator anim in allAnim)
            {
                anim.speed = 1;
            }
            FightPetMgr fightPetMgr = GetMainPlayerComp().GetFightPetMgr();
            GameObject petGo = fightPetMgr.GetCurPet();
            if (petGo != null)
            {
                Animator[] allAnimPet = petGo.GetComponentsInChildren<Animator>();
                foreach (Animator anim in allAnim)
                {
                    anim.speed = 1;
                }
            }

            petGo = fightPetMgr.GetSecondCurPet();
            if (petGo != null)
            {
                Animator[] allAnimPet = petGo.GetComponentsInChildren<Animator>();
                foreach (Animator anim in allAnim)
                {
                    anim.speed = 1;
                }
            }
        }
        
        private void InitSpecialSceneSpeed()
        {
            if (m_SceneType == SceneType.BreakOutSene)
            {
                m_TimerMgr.AddTimer(0.1f, 1, (dt, ended) => { InitSpecialSceneSpeedImp(0.7f); });
            }
            else if(m_SceneType == SceneType.CarScene)
            {
                m_TimerMgr.AddTimer(0.1f, 1, (dt, ended) => { InitSpecialSceneSpeedImp(0.85f); });
            }
        }
        private void InitSpecialSceneSpeedImp(float scale)
        {
            GameObject sceneRootNode = GameObject.Find("SpecialRootNode");
            if (sceneRootNode != null)
            {
                LoopRoll[] loopRolls = sceneRootNode.GetComponentsInChildren<LoopRoll>();
                foreach (LoopRoll loopRoll in loopRolls)
                {
                    loopRoll.SetSpeedScale(scale);
                }
            }
        }
        public override SceneType GetSceneType()
        {
            return m_SceneType;
        }
        public override bool IsNormalPass()
        {
            return m_InNormalPassType;
        }
        public float GetCurTime()
        {
            return m_CurTime;
        }

        private void AfLog(int passId)
        {
            int passIndex = passId % 100000;
            if (Global.gApp.gSystemMgr.GetMiscMgr().FirstMain == 0 && passIndex == 1)
            {
                //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_new_guide);
            }
            //成功过关
            if (FightResultManager.instance.GetFightState() == (int)FightResultManager.FightState.SUCCESS)
            {
                
                //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_level_achieve);
                if (passIndex % 5 == 0)
                {
                    //SDKDSAnalyticsManager.trackEvent(string.Format(AFInAppEvents.af_mz_lv, passIndex));
                }

                //首日
                if (Global.gApp.gSystemMgr.GetMiscMgr().GetCreateDays() == 1)
                {
                    if (passIndex % 5 == 0)
                    {
                        //SDKDSAnalyticsManager.trackEvent(string.Format(AFInAppEvents.af_mz_1day_stage, passIndex));
                    }
                }
                //7日
                if (Global.gApp.gSystemMgr.GetMiscMgr().GetCreateDays() <= 6)
                {
                    if (passIndex % 5 == 0)
                    {
                        //SDKDSAnalyticsManager.trackEvent(string.Format(AFInAppEvents.af_mz_7day_stage, passIndex));
                    }
                }
            }
        }
    }
}
