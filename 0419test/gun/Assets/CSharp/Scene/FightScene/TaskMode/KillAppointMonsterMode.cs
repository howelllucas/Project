using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class KillAppointMonsterMode : BaseTaskMode
    {
        [SerializeField] private List<int> m_MonsterIds;
        [SerializeField] private int m_KillCount = 1;
        private int m_OriCount = 1;
        private WaveMgr m_WaveMgr;
        private Monster m_Monster;

        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.KillApointMonster;
            m_OriCount = m_KillCount;
            m_WaveMgr =Global.gApp.CurScene.GetWaveMgr();
            base.Init(mgr, playerGo);
        }
        private void Update()
        {
            if (m_TaskState == TaskState.Begin)
            {
                float dtTime = BaseScene.GetDtTime();
                if (dtTime > 0)
                {
                    FindGuidEnemy();
                    UpdatePointArrow(true);
                    TimeLimitUp();
                }
            }
        }
        private void SetLockEnemy(Transform lockEnemy)
        {
            if (lockEnemy != null)
            {
                m_Monster = lockEnemy.GetComponent<Monster>();
            }
            else
            {
                m_Monster = null;
            }
            SetTargetTsf(lockEnemy);
        }
        private void FindGuidEnemy()
        {
            bool onlyCheckMonsterVisible = false;
            if (m_TargetTsf)
            {
                if(!m_Monster.InCameraView && !m_Monster.InDeath)
                {
                    onlyCheckMonsterVisible = true;
                }
                else
                {

                    SetLockEnemy(null);
                }
            }
            Vector3 m_position = m_PlayerTsf.position;
            float m_lastPositionSqure = 0;
            Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
            foreach (KeyValuePair<int, Wave> kv in waves)
            {
                List<Monster> monsters = kv.Value.GetMonsters();
                foreach(Monster monster in monsters)
                {
                    int monsterId = monster.GetMonsterId();
                    if (m_MonsterIds.Contains(monsterId))
                    {
                        if (monster.InCameraView)
                        {
                            SetLockEnemy(null);
                            return;
                        }
                        else if(!onlyCheckMonsterVisible)
                        {
                            Vector3 postion = monster.transform.position;
                            Vector3 dtPosition = m_position - postion;
                            float sqrMagnitude = dtPosition.sqrMagnitude;
                            if (sqrMagnitude > 4)
                            {
                                if (m_TargetTsf != null)
                                {
                                    if (sqrMagnitude < m_lastPositionSqure)
                                    {
                                        m_lastPositionSqure = sqrMagnitude;
                                        SetLockEnemy(monster.transform);
                                    }
                                }
                                else
                                {
                                    SetLockEnemy(monster.transform);
                                    m_lastPositionSqure = (m_TargetTsf.transform.position - m_position).sqrMagnitude;
                                }
                            }
                        }
                    }
                }
            }
        }
        public override void BeginTask()
        {
            RegisterListener();
            base.BeginTask();

            BroadTargetTips(0, m_OriCount.ToString(), (m_OriCount - m_KillCount).ToString(), m_OriCount.ToString());
            BroadPlotTips(0,m_KillCount.ToString());
        }

        private void MonsterDead(int guid, int monsterId, Monster monster)
        {
            if(m_MonsterIds.Contains(monsterId))
            {
                m_KillCount--;
                BroadTargetTips(0, m_OriCount.ToString(), (m_OriCount - m_KillCount).ToString(), m_OriCount.ToString());
                if (m_KillCount == 0)
                {
                    BroadTargetTips(1);
                    EndTask();
                }
            }
        }

        public override void EndTask()
        {
            BroadPlotTips(1);
            UnRegisterListener();
            gameObject.SetActive(false);
            base.EndTask();
        }

        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
        }
        public override void Destroy()
        {
            UnRegisterListener();
        }
    }
}
