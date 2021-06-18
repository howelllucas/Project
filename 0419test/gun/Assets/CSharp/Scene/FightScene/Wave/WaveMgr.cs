using EZ.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class WaveMgr
    {
        private Dictionary<int, Wave> m_Wave;
        private GameObject m_MainPlayer;
        private PassItem m_PassData;
        private TimerMgr m_TimerMgr;
        private int m_GuidRef = 0;
        private int m_WaveCount;
        private int m_WaveStartCount = 0;

        private Dictionary<string, float> m_DeadTimes = new Dictionary<string, float>();

        private int m_SpecialWaveId = 888888;
        private int m_SpecialEnemyId = 3004;
        private int m_SpecialGuid = -1;
        private Dictionary<int, int> m_InitCacheMonster = new Dictionary<int, int>();
        private int m_MaxStartCacheCount = 30;
        private PlayerMgr m_PlayerMgr;
        private DynamicCalMst m_DynamicCalMonsterCountMgr;
        private int m_MaxFrameCreateCount = 10;
        private int m_FrameCreateCount = 10;
        // speed up 
        private float m_OriSpeedUpTime = 0.5f;
        private bool m_StartCreateMonster = false;
        private float m_SpeedUpTime = 0.5f;

        private bool m_Has3004 = false;

        bool m_InNormalPass = true;
        public int MaxCreateCount
        {
            get; set;
        }
        private Dictionary<int, int> m_KillMonsterInfo = new Dictionary<int, int>();
        public int CurCreateMonsterCount
        {
            get; set;
        }

        public WaveMgr(PassItem passData, GameObject player)
        {
            m_PassData = passData;
            m_MainPlayer = player;
            m_Wave = new Dictionary<int, Wave>();
            m_DynamicCalMonsterCountMgr = new DynamicCalMst(this);
            m_TimerMgr = new TimerMgr();
            m_PlayerMgr = Global.gApp.CurScene.GetPlayerMgr();
            m_InNormalPass = Global.gApp.CurScene.IsNormalPass();

            ResetFremeCreateCount();
            CreateWaves();
            //CacheSpecialEnemy(3004);
            CacheMonster();
            //m_TimerMgr.AddTimer(30, 5, CreateSpecialWave);
        }

        private void ResetFremeCreateCount()
        {
            if (m_InNormalPass)
            {
                m_FrameCreateCount = m_MaxFrameCreateCount;
            }
            else
            {
                m_FrameCreateCount = 1000;
            }
        }
        public void CacheMonster(int monsterId, int count = 1)
        {
            if (m_InitCacheMonster != null)
            {
                int oriCount;
                if (m_InitCacheMonster.TryGetValue(monsterId, out oriCount))
                {
                    oriCount += count;
                }
                else
                {
                    oriCount = count;
                }
                m_InitCacheMonster[monsterId] = Mathf.Min(oriCount, m_MaxStartCacheCount);
            }
        }
        private void CacheMonster()
        {
            EZ.Data.Monster monsterData = Global.gApp.gGameData.MosterData;
            foreach (KeyValuePair<int, int> keyValuePair in m_InitCacheMonster)
            {
                MonsterItem monsterItem = monsterData.Get(keyValuePair.Key);
                for (int i = 0; i < keyValuePair.Value; i++)
                {
                    Monster monster = m_PlayerMgr.CreateMonsterForceCreate(monsterItem);
                    monster.InitForCache(m_MainPlayer, null, monsterItem);
                }
            }
            m_InitCacheMonster.Clear();
            m_InitCacheMonster = null;
        }
        public void RecordKillMonster(int monsterId)
        {
            int curKillMonster = 0;
            if (m_KillMonsterInfo.TryGetValue(monsterId,out curKillMonster))
            {
                curKillMonster++;
                m_KillMonsterInfo[monsterId] = curKillMonster;
            }
            else
            {
                m_KillMonsterInfo.Add(monsterId, 1);
            }
        }
        public void RemoveMonster()
        {
            CurCreateMonsterCount--;
            if (CurCreateMonsterCount < 0)
            {
                Debug.Log("========= error erro erro ============");
            }
        }
        public Monster CreateMonsterFromCacheInfo(MonsterItem monsterItem, bool forceCreate = false)
        {
            if (m_FrameCreateCount > 0)
            {
                Monster monster = CreateMonster(monsterItem, forceCreate);
                if (monster != null)
                {
                    m_FrameCreateCount--;
                }
                return monster;
            }
            else
            {
                return null;
            }
        }
        public Monster CreateMonster(MonsterItem monsterItem, bool forceCreate = false)
        {
            if (CurCreateMonsterCount < MaxCreateCount || forceCreate)
            {
                CurCreateMonsterCount++;
                if (monsterItem.tag != m_SpecialEnemyId)
                {
                    m_StartCreateMonster = true;
                }
                return m_PlayerMgr.CreateMonster(monsterItem);
            }
            else
            {
                return null;
            }
        }
        public void CacheSpecialEnemy(int id)
        {
            EZ.Data.Monster monsterData = Global.gApp.gGameData.MosterData;
            MonsterItem monsterItem = monsterData.Get(id);
            Monster monster = m_PlayerMgr.CreateMonsterForceCreate(monsterItem);
            monster.InitForCache(m_MainPlayer, null, monsterItem);
        }
        public void CreateSpecialWave(float dt, bool end)
        {
            if (m_SpecialGuid < 0)
            {
                m_SpecialGuid = GetNewGuid();
                Wave wave = new Wave(m_SpecialWaveId, this, m_PassData, m_MainPlayer, m_SpecialGuid);
                m_Wave.Add(m_SpecialGuid, wave);
            }
        }
        public TimerMgr GetTimerMgr()
        {
            return m_TimerMgr;
        }

        public void Update(float dt)
        {
            ResetFremeCreateCount();
            TimerMgrUpdate(dt);
            if(m_StartCreateMonster)
            {
                m_DynamicCalMonsterCountMgr.Update(dt);
            }
        }
        public void SetHas3004(bool has3004)
        {
            m_Has3004 = has3004;
        }
        private void TimerMgrUpdate(float dt)
        {
            // !m_InNormalPass 表示特殊关卡 !m_StartSpeedUp 表示 创建过 怪物 
            //CurCreateMonsterCount > 1 || !m_Has3004 表示 数量大于 1 个
            //如果 是 1 个那么不能是 3004 也ok 否则不可以
            // 如果是 0 个 ，那么不可能 是 3004 了因为没有怪物
            if ((CurCreateMonsterCount > 1 || !m_Has3004) || !m_InNormalPass || !m_StartCreateMonster)
            {
                m_SpeedUpTime = m_OriSpeedUpTime;
                m_TimerMgr.Update(dt);
            }
            else
            {
                m_SpeedUpTime -= dt;
                if (m_SpeedUpTime <= 0)
                {
                    m_SpeedUpTime = m_OriSpeedUpTime;
                    m_TimerMgr.UpdateToRecentOne(dt);
                }
                else
                {
                    m_TimerMgr.Update(dt);
                }
            }
        }
        public Wave GetWavesById(int waveId)
        {
            foreach(KeyValuePair<int,Wave> kv in m_Wave){
                if(kv.Value.GetWaveId() == waveId)
                {
                    return kv.Value;
                }
            }
            return null;
        }
        public Dictionary<int,Wave> GetWaves()
        {
            return m_Wave;
        }
        private void CreateWaves()
        {
            foreach (int waveId in m_PassData.waveID)
            {
                int newGuid = GetNewGuid();
                Wave wave = new Wave(waveId,this,m_PassData, m_MainPlayer,newGuid);
                m_Wave.Add(newGuid, wave);
            }
            m_WaveCount = m_Wave.Count;
        }
        private int GetNewGuid()
        {
            m_GuidRef++;
            return m_GuidRef;
        }

        public void WaveStart(int guid)
        {
            m_WaveStartCount++;
            Global.gApp.gMsgDispatcher.Broadcast<int, int>(MsgIds.WaveStart, m_WaveStartCount, m_WaveCount);
        }
        public void RemoveWave(int guid)
        {
            if(m_SpecialGuid == guid)
            {
                m_SpecialGuid = -1;
            }
            Wave wave; 
            if(m_Wave.TryGetValue(guid,out wave))
            {
                if (wave.Destroy(false))
                {
                    m_Wave.Remove(guid);
                }
            }
            if(m_Wave.Count == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.AllWaveEnded);
            }
        }
        public float GetDeadTime(Animator animator,string path)
        {
            float deadTime = 1;
            if(!m_DeadTimes.TryGetValue(path,out deadTime))
            {
                foreach(AnimationClip clip in animator.runtimeAnimatorController.animationClips)
                {
                    if(clip.name == GameConstVal.Death)
                    {
                        deadTime = clip.length;
                        m_DeadTimes[path] = deadTime;
                        break;
                    }
                }
            }
            return deadTime;
        } 
        public Monster GetMonsterById(int monsterId)
        {
            foreach (KeyValuePair<int, Wave> keyValue in m_Wave)
            {
                Monster monster = keyValue.Value.GetMonserById(monsterId);
                if ( monster != null)
                {
                    return monster;
                }
            }
            return null;
        }
        public Dictionary<int, int> GetCurKillMonsterInfo()
        {
            return m_KillMonsterInfo;
        }
        public void Destroy()
        {
            m_TimerMgr.Destroy();
            foreach(KeyValuePair<int,Wave> keyValue in m_Wave)
            {
                keyValue.Value.Destroy();
            }
            m_Wave.Clear();
        }
    }
}
