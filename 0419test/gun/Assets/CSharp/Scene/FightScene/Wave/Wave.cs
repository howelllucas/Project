using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    enum BornNodeType
    {
        Scene = 1,
        MainRole = 2,
        Area = 3,
    }
    enum RandomType
    {
        RandCreateInfo,
        RandMonster
    }
    enum TriggerType
    {
        TriggerByTime = 1,
        TriggerByWaveEnd = 2,
        TriggerByActiveProps = 3,
        TriggerByCollider = 4,
        TriggerByTaskModeBegin = 5,
        TriggerByTaskModeEnd = 6,
        TriggerByTaskModeCountDown = 7,
    }
    enum ApperEffectType
    {
        NoEffect = 0,
        WarningEffect = 1,
    }
    public class Wave
    {
        //-----
        private GameObject m_MainPlayer;
        private List<Monster> m_Monsters;
        private WaveMgr m_WaveMgr;
        private PassItem m_PassData;
        private WaveItem m_WaveData;

        private BornNodeType m_BornNodeType;
        private TriggerType m_TriggerType;
        private int m_MonsterCount = 0;
        private int m_RealTotalMonsterCount = 0;
        private int m_RealCountCount = 0;

        private int m_Big20071 = 121;
        private int m_Big20072 = 40;
        private int m_Big2007 = 13;
        private int m_Big2008 = 4;

        private List<GameObject> m_BornNodeList;

        private List<MonsterItem> m_CreateInfo;
        private List<MonsterItem> m_RoundRoleCreateInfo;
        private BornNode[] m_RoundRoleBornNodes;
        private int m_Guid;
        private bool m_StartWave;
        private int m_TimeId = -100;
        private int m_WaveId = 0;
        private int m_DtCreateCount = 1;
        private RandomType m_RandomType; 
        private bool m_NormalScene; 

        private Dictionary<int, int> m_ActiveProps = new Dictionary<int, int>();
        public Wave(int waveId, WaveMgr waveMgr, PassItem passData, GameObject player, int guid)
        {
            m_WaveId = waveId;
            m_StartWave = false;
            m_Guid = guid;
            m_CreateInfo = new List<MonsterItem>();
            m_RoundRoleCreateInfo = new List<MonsterItem>();
            m_BornNodeList = new List<GameObject>();
            m_RoundRoleBornNodes = (Global.gApp.CurScene as FightScene).GetBornNodes();
            m_NormalScene = Global.gApp.CurScene.IsNormalPass();
            m_WaveMgr = waveMgr;
            //m_WaveData = Global.gApp.gGameData.GetWaveData().Find(l => l.id == waveId);
            m_WaveData = Global.gApp.gGameData.WaveData.Get(waveId);
            m_PassData = passData;
            m_MainPlayer = player;
            m_Monsters = new List<Monster>();

            m_TriggerType = (TriggerType)m_WaveData.trigMode[0];
            m_DtCreateCount = Mathf.Max(m_DtCreateCount,m_WaveData.NumTime);
            if(m_WaveData.enemyNum.Length == 1 && m_WaveData.enemyID.Length > 1)
            {
                m_RandomType = RandomType.RandMonster;
            }
            else
            {
                m_RandomType = RandomType.RandCreateInfo;
            }
            InitMonsterInfo();
            RegisterListeners();
            StartWave();
        }
        public float GetDeadTime(Animator animator, EZ.Data.MonsterItem monster)
        {
            return m_WaveMgr.GetDeadTime(animator, monster.path);
        }
        private void FindBornNode()
        {
            string nodeName = m_WaveData.nodeID[0];
            if (nodeName.IndexOf("RNode") >= 0)
            {
                m_BornNodeType = BornNodeType.MainRole;
            }
            else if (nodeName.IndexOf("S") >= 0)
            {
                m_BornNodeType = BornNodeType.Scene;
            }
            else
            {
                m_BornNodeType = BornNodeType.Area;
            }
            m_BornNodeList.Capacity = m_WaveData.nodeID.Length;
            if (m_BornNodeType != BornNodeType.MainRole)
            {
                foreach (string name in m_WaveData.nodeID)
                {
                    GameObject bornNode = GameObject.Find(name);
                    if (bornNode != null)
                    {
                        m_BornNodeList.Add(bornNode);
                    }
                }
            }
        }
        private void DelayCreateMonster(float dt, bool isEnd)
        {
            UnRegisterListeners();
            StartCreateMonster();
        }
        private void StartWave()
        {
            if (m_TriggerType == TriggerType.TriggerByTime)
            {
                m_TimeId = m_WaveMgr.GetTimerMgr().AddTimer(m_WaveData.trigMode[1], 1, DelayCreateMonster);
            }
        }
        private void StartCreateMonster()
        {
            if (m_StartWave)
            {
                return;
            }
            m_StartWave = true;
            if((ApperEffectType)m_WaveData.warning == ApperEffectType.WarningEffect)
            {
                AddAppearWarningEffect();
            }
            if (m_WaveData.delayTime > 0)
            {
                m_TimeId = m_WaveMgr.GetTimerMgr().AddTimer(m_WaveData.delayTime, 1, StartCreateMonsterImp);
            }
            else
            {
                StartCreateMonsterImp(0, true);
            }
        }
        private void InitMonsterInfo()
        {
            GenerateCreateInfo();
            foreach(MonsterItem monsterItem in m_CreateInfo)
            {
                int enemyId = monsterItem.tag;
                if (enemyId == 2007)
                {
                    m_RealTotalMonsterCount +=  m_Big2007;
                }
                else if (enemyId == 2008)
                {
                    m_RealTotalMonsterCount +=  m_Big2008;
                }
                else if (enemyId == 20071)
                {
                    m_RealTotalMonsterCount += m_Big20071;
                }
                else if (enemyId == 20072)
                {
                    m_RealTotalMonsterCount += m_Big20072;
                }
                else
                {
                    m_RealTotalMonsterCount ++;

                }
            }
            m_RealCountCount = m_RealTotalMonsterCount;
            m_MonsterCount = m_CreateInfo.Count;
        }
        private void StartCreateMonsterImp(float dt, bool end)
        {
            UnRegisterListeners();
            m_WaveMgr.WaveStart(m_Guid);

            FindBornNode();
            if (m_MonsterCount == 0)
            {
                //  开始就结束了
                Global.gApp.gMsgDispatcher.Broadcast<int, int>(MsgIds.WaveEnded, m_Guid, m_WaveData.id);
                m_WaveMgr.RemoveWave(m_Guid);
                return;
            }
            m_TimeId = m_WaveMgr.GetTimerMgr().AddTimer(m_WaveData.dtTime, 0, CreateMonster);
            CreateMonster(0, false);
        }

        private void GenerateCreateInfo()
        {
            m_CreateInfo.Capacity = Mathf.Min(m_CreateInfo.Capacity, m_MonsterCount);
            m_Monsters.Capacity = Mathf.Max(m_Monsters.Capacity, m_MonsterCount / 2);
            EZ.Data.Monster monsterData = Global.gApp.gGameData.MosterData;
            if (m_RandomType == RandomType.RandCreateInfo)
            {
                int countIndex = 0;
                foreach (int enemyId in m_WaveData.enemyID)
                {
                    int eId = enemyId;
                    if (DebugMgr.GetInstance().MonsterId > 0)
                    {
                        eId = DebugMgr.GetInstance().MonsterId;
                    }
                    MonsterItem monsterItem = monsterData.Get(eId);
                    if (monsterItem == null)
                    {
                        Debug.LogError("EnemyId " + eId + " does not exist!");
                        continue;
                    }

                    var count = Mathf.CeilToInt(m_WaveData.enemyNum[countIndex] * Game.PlayerDataMgr.singleton.StageWaveFactor);
                    for (int i = 0; i < count; i++)
                    {
                        m_CreateInfo.Add(monsterItem);
                    }
                    m_WaveMgr.CacheMonster(eId, count);
                    countIndex++;
                }
            }
            else if(m_RandomType == RandomType.RandMonster)
            {
                int enemyIdCount = m_WaveData.enemyID.Length;
                int enemyCount = Mathf.CeilToInt(m_WaveData.enemyNum[0] * Game.PlayerDataMgr.singleton.StageWaveFactor);
                for(int i = 0;i < enemyCount; i++)
                {
                    int enemyIndex = Random.Range(0, enemyIdCount);
                    int enemyId = m_WaveData.enemyID[enemyIndex];
                    if (DebugMgr.GetInstance().MonsterId > 0)
                    {
                        enemyId = DebugMgr.GetInstance().MonsterId;
                    }
                    MonsterItem monsterItem = monsterData.Get(enemyId);
                    if (monsterItem == null)
                    {
                        Debug.LogError("EnemyId " + enemyId + " does not exist!");
                        continue;
                    }
                    m_CreateInfo.Add(monsterItem);
                    m_WaveMgr.CacheMonster(enemyId,1);
                }
            }
            //CacheMonster();
        }
        private void CacheMonster()
        {
            return;
            EZ.Data.Monster monsterData = Global.gApp.gGameData.MosterData;
            foreach (int enemyId in m_WaveData.enemyID)
            {
                MonsterItem monsterItem = monsterData.Get(enemyId);
                Monster monster = m_WaveMgr.CreateMonster(monsterItem);
                monster.InitForCache(m_MainPlayer, this, monsterItem);
            }
        }
        private void CreateMonster(float dt, bool ended)
        {
            if (m_CreateInfo.Count == 0 && m_RoundRoleCreateInfo.Count == 0)
            {
                RemoveTimer();
                return;
            }
            int dtCreateCount = m_DtCreateCount;
            CreateRoundRoleMonster(ref dtCreateCount);
            CreatNormalTypeMonster(dtCreateCount);
        }
        private void CreateRoundRoleMonster(ref int creatCount)
        {
            if (m_RoundRoleCreateInfo.Count > 0)
            {
                int newBornIndex = Random.Range(0, m_RoundRoleBornNodes.Length);
                BornNode newBornNode = m_RoundRoleBornNodes[newBornIndex];
                int dtCreateCount = creatCount;
                for (int i = 0; i < dtCreateCount; i++)
                {
                    int index = Random.Range(0, m_RoundRoleCreateInfo.Count);
                    if (CreateRoundMonsterImp(m_RoundRoleCreateInfo[index], newBornNode,true))
                    {
                        creatCount--;
                        m_RoundRoleCreateInfo.RemoveAt(index);
                        if (m_RoundRoleCreateInfo.Count == 0)
                        {
                            if (m_CreateInfo.Count == 0)
                            {
                                RemoveTimer();
                                return;
                            }
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        private void CreatNormalTypeMonster(int creatCount)
        {
            // same logic bug bornNode from m_RoundRoleBornNodes
            if(m_CreateInfo.Count == 0)
            {
                if (m_RoundRoleCreateInfo.Count == 0)
                {
                    RemoveTimer();
                }
                return;
            }
            if (m_BornNodeType == BornNodeType.MainRole)
            {
                int bornIndex = Random.Range(0, m_RoundRoleBornNodes.Length);
                BornNode bornNode = m_RoundRoleBornNodes[bornIndex];
                for (int i = 0; i < creatCount; i++)
                {
                    int index = Random.Range(0, m_CreateInfo.Count);
                    bool createResult = CreateRoundMonsterImp(m_CreateInfo[index], bornNode);
                    m_CreateInfo.RemoveAt(index);
                    if (createResult || m_CreateInfo.Count == 0)
                    {
                        if (m_CreateInfo.Capacity > 64 && m_CreateInfo.Capacity > m_CreateInfo.Count * 3)
                        {
                            m_CreateInfo.TrimExcess();
                        }
                        if (m_CreateInfo.Count == 0)
                        {
                            if (m_RoundRoleCreateInfo.Count == 0)
                            {
                                RemoveTimer();
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                int bornIndex = Random.Range(0, m_BornNodeList.Count);
                GameObject bornNode = m_BornNodeList[bornIndex];
                for (int i = 0; i < creatCount; i++)
                {
                    int index = Random.Range(0, m_CreateInfo.Count);
                    bool createResult = CreateMonsterImp(m_CreateInfo[index], bornNode, m_BornNodeType);
                    m_CreateInfo.RemoveAt(index);
                    if (createResult || m_CreateInfo.Count == 0)
                    {
                        if (m_CreateInfo.Capacity > 64 && m_CreateInfo.Capacity > m_CreateInfo.Count * 3)
                        {
                            m_CreateInfo.TrimExcess();
                        }
                        if (m_CreateInfo.Count == 0)
                        {
                            if (m_RoundRoleCreateInfo.Count == 0)
                            {
                                RemoveTimer();
                            }
                            break;
                        }
                    }
                }
            }
        }
        private void RemoveTimer()
        {
            if (m_TimeId > 0)
            {
                m_WaveMgr.GetTimerMgr().RemoveTimer(m_TimeId);
                m_TimeId = -100;
            }
        }
        public void CreateFassionMonster(int monsterId, Vector3 position)
        {
            EZ.Data.MonsterItem monsterItem = Global.gApp.gGameData.MosterData.Get(monsterId);
            Monster monster = m_WaveMgr.CreateMonster(monsterItem,true);
            monster.transform.position = position;
            m_Monsters.Add(monster);
            monster.Init(m_MainPlayer, this, monsterItem);
            m_MonsterCount++;
        }
        private void AddRoundRoleMonsterInfo(MonsterItem monsterItem)
        {
            if (m_NormalScene)
            {
                m_RoundRoleCreateInfo.Add(monsterItem);
            }
        }
        private bool CreateMonsterImp(MonsterItem monsterItem, GameObject bornNode, BornNodeType bornType)
        {
            if (bornType == BornNodeType.Scene)
            {
                return CreateScenePointMonsterImp(monsterItem, bornNode);
            }
            else if (bornType == BornNodeType.Area)
            {
                return CreateSceneAreatMonsterImp(monsterItem, bornNode);
            }
            return false;
        }
        //lockExterAdd = true lock push item to RoundRoleMonsterInfo and call CreateMonsterFromCacheInfo  limit by frameCreate count
        private bool CreateRoundMonsterImp(MonsterItem monsterItem, BornNode bornNode,bool lockExterAdd = false)
        {
            if (bornNode.GetIsOutMap())
            {
                Monster monster = null;
                if (!lockExterAdd)
                {
                    monster = m_WaveMgr.CreateMonster(monsterItem);
                }
                else
                {
                    monster = m_WaveMgr.CreateMonsterFromCacheInfo(monsterItem);
                }
                if (monster != null)
                {
                    Vector3 position = bornNode.transform.position;
                    position.z = 0;
                    monster.transform.position = position;
                    m_Monsters.Add(monster);
                    monster.Init(m_MainPlayer, this, monsterItem);
                    return true;
                }
                else
                {
                    if (!lockExterAdd)
                    {
                        AddRoundRoleMonsterInfo(monsterItem);
                        return false;
                    }
                }
            }
            else
            {
                int count = m_RoundRoleBornNodes.Length;
                int symbol = Random.Range(0, 1000);
                int bornIndex = Random.Range(0, count);
                if (symbol > 500)
                {
                    symbol = 1;
                }
                else
                {
                    symbol = -1;
                    bornIndex += count;
                }
                for (int i = 0; i < count; i++)
                {
                    int newIndex = bornIndex % count;
                    BornNode newNode = m_RoundRoleBornNodes[newIndex];
                    if (newNode.GetIsOutMap())
                    {
                        Monster monster = null;
                        if (!lockExterAdd)
                        {
                            monster = m_WaveMgr.CreateMonster(monsterItem);
                        }
                        else
                        {
                            monster = m_WaveMgr.CreateMonsterFromCacheInfo(monsterItem);
                        }
                        if (monster != null)
                        {
                            Vector3 position = newNode.transform.position;
                            position.z = 0;
                            monster.transform.position = position;
                            m_Monsters.Add(monster);
                            monster.Init(m_MainPlayer, this, monsterItem);
                            return true;
                        }
                        else
                        {
                            if (!lockExterAdd)
                            {
                                AddRoundRoleMonsterInfo(monsterItem);
                            }
                            return false;
                        }
                    }
                    bornIndex += symbol;
                }
            }

            if (!lockExterAdd)
            {
                AddRoundRoleMonsterInfo(monsterItem);
            }
            return false;
        }
        private bool CreateScenePointMonsterImp(MonsterItem monsterItem, GameObject bornNode)
        {
            Monster monster = m_WaveMgr.CreateMonster(monsterItem);
            if (monster != null)
            {
                Vector3 position = bornNode.transform.position;
                position.z = 0;
                monster.transform.position = position;
                m_Monsters.Add(monster);
                monster.Init(m_MainPlayer, this, monsterItem);
                return true;
            }
            else
            {
                AddRoundRoleMonsterInfo(monsterItem);
                return false;
            }
        }
        private bool CreateSceneAreatMonsterImp(MonsterItem monsterItem, GameObject bornNode)
        {
            Monster monster = m_WaveMgr.CreateMonster(monsterItem);
            if (monster != null)
            {
                Vector3 localScale = bornNode.transform.localScale;
                float x = Random.Range(-0.5f, 0.5f);
                float y = Random.Range(-0.5f, 0.5f);
                Vector3 posOffset = new Vector3(localScale.x * x, localScale.y * y, 0);

                Vector3 position = bornNode.transform.position + posOffset;
                position.z = 0;
                monster.transform.position = position;
                m_Monsters.Add(monster);
                monster.Init(m_MainPlayer, this, monsterItem);
                return true;
            }
            else
            {
                AddRoundRoleMonsterInfo(monsterItem);
                return false;
            }
        }

        public PassItem GetPassData()
        {
            return m_PassData;
        }

        public Monster GetMonserById(int monsterId)
        {
            foreach (Monster monster in m_Monsters)
            {
                if (monster.GetMonsterId() == monsterId)
                {
                    return monster;
                }
            }
            return null;
        }
        public List<Monster> GetMonsters()
        {
            return m_Monsters;
        }
        public int GetRealMonsterCount()
        {
            return m_RealCountCount;
        }
        public int GetRealTotalMonsterCount()
        {
            return m_RealTotalMonsterCount;
        }
        public void RemoveMonster(Monster monsterCom, bool ingoreBroad = false)
        {
            m_Monsters.Remove(monsterCom);
            m_MonsterCount--;
            m_RealCountCount--;
            m_WaveMgr.RemoveMonster();
            int guid = monsterCom.GetGuid();
            int monsterId = monsterCom.GetMonsterId();
            if (!ingoreBroad)
            {
                m_WaveMgr.RecordKillMonster(monsterId);
                Global.gApp.gMsgDispatcher.Broadcast<int, int, Monster>(MsgIds.MonsterDead, guid, monsterId, monsterCom);
            }
            if (m_MonsterCount <= 0)// m_RealCountCount == 0 is also ok
            {
                Global.gApp.gMsgDispatcher.Broadcast<int, int>(MsgIds.WaveEnded, m_Guid, m_WaveData.id);
                m_WaveMgr.RemoveWave(m_Guid);
            }
        }
        public bool Destroy(bool forceDestroy = true)
        {
            if (!forceDestroy)
            {
                if(m_Monsters.Count > 0)
                {
                    Debug.LogError(" 创建怪物数量 与删除的 不匹配 wave id " + m_WaveData.id);
                    return false;
                }
            }
            UnRegisterListeners();
            RemoveTimer();
            foreach (Monster monster in m_Monsters)
            {
                monster.DestroySelf();
                Object.Destroy(monster.gameObject);
            }
            m_Monsters.Clear();
            return true;
        }

        public int GetWaveId()
        {
            return m_WaveId;
        }
        public void WaveEnded(int guid, int waveId)
        {
            if (waveId == m_WaveData.trigMode[1])
            {
                StartCreateMonster();
            }
        }

        public void TriggerCollider(int triggerId, Transform eventNode)
        {
            if (triggerId == m_WaveData.trigMode[1])
            {
                StartCreateMonster();
            }
        }

        public void TriggerActiveProp(int propId)
        {
            int curPropCount;
            if (m_ActiveProps.TryGetValue(propId, out curPropCount))
            {
                curPropCount--;
                m_ActiveProps[propId] = curPropCount;

                if (curPropCount == 0)
                {
                    foreach (int count in m_ActiveProps.Values)
                    {
                        if (count > 0)
                        {
                            return;
                        }
                    }
                    StartCreateMonster();
                }
            }
        }

        private void AddAppearWarningEffect()
        {
            Global.gApp.CurScene.Pause();
            GameObject BossWarning = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/BossWarning");
            DelayCallBack delayCallBack = BossWarning.AddComponent<DelayCallBack>();
            delayCallBack.SetAction(() => { Global.gApp.CurScene.Resume();Object.Destroy(BossWarning); }, 1.2f, true);
            BossWarning.GetComponent<DelayDestroy>().enabled = false;
            BossWarning.transform.SetParent(Global.gApp.gUiMgr.GetUiCanvasTsf(), false);
            BossWarning.transform.SetAsFirstSibling();
        }

        public void TaskModeEnd(int modeIndex)
        {
            if (modeIndex == m_WaveData.trigMode[1])
            {
                StartCreateMonster();
            }
        }

        public void TaskModeBegin(int modeIndex)
        {
            if (modeIndex == m_WaveData.trigMode[1])
            {
                StartCreateMonster();
            }
        }
        public void TaskModeCountDown(int modeIndex)
        {
            if (modeIndex == m_WaveData.trigMode[1])
            {
                StartCreateMonster();
            }
        }
        private void RegisterListeners()
        {
            if (m_TriggerType == TriggerType.TriggerByWaveEnd)
            {
                Global.gApp.gMsgDispatcher.AddListener<int, int>(MsgIds.WaveEnded, WaveEnded);
            }
            else if (m_TriggerType == TriggerType.TriggerByActiveProps)
            {
                Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.ActiveProp, TriggerActiveProp);

                int[] trigModeInfo = m_WaveData.trigMode;
                for (int i = 1; i <= (trigModeInfo.Length - 1) / 2; i++)
                {
                    m_ActiveProps.Add(trigModeInfo[2 * (i - 1) + 1], trigModeInfo[2 * i]);
                }
            }
            else if (m_TriggerType == TriggerType.TriggerByCollider)
            {
                Global.gApp.gMsgDispatcher.AddListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
            }
            else if (m_TriggerType == TriggerType.TriggerByTaskModeBegin)
            {
                Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TaskModeBegin, TaskModeBegin);
            }
            else if (m_TriggerType == TriggerType.TriggerByTaskModeEnd)
            {
                Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TaskModeEnd, TaskModeEnd);
            }
            else if(m_TriggerType == TriggerType.TriggerByTaskModeCountDown)
            {
                Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TaskModeCountDown, TaskModeCountDown);
            }
        }
        public float GetHpParam()
        {
            float hpParam = 1;
            if(m_WaveData.HpParam > 0)
            {
                hpParam = m_WaveData.HpParam;
            }
            return hpParam;
        }
        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, int>(MsgIds.WaveEnded, WaveEnded);
            Global.gApp.gMsgDispatcher.RemoveListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.ActiveProp, TriggerActiveProp);

            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TaskModeBegin, TaskModeBegin);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TaskModeEnd, TaskModeEnd);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TaskModeCountDown, TaskModeCountDown);
        }
    }
}
