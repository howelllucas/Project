using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class KillMonstersMode : BaseTaskMode
    {
        [SerializeField] private List<int> m_Waves;
        [SerializeField] private List<int> m_ContinuePlotTipsId;
        private List<Wave> m_AllWave;
        private bool m_DirtyFresh = false;
        private int m_OriCount = 1;
        private WaveMgr m_WaveMgr;
        private Monster m_Monster;
        private List<float> m_WpnChipDropProgress;
        private List<float> m_SourceDropProgress;
        private int m_WpnChipDropId = 0;
        private int m_SourceDropId = 0;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_WaveMgr =Global.gApp.CurScene.GetWaveMgr();
            m_FightArrowType = FightTaskUiType.Empty;
            base.Init(mgr, playerGo);
            if (m_Waves.Count == 0)
            {
                int[] waveIds = Global.gApp.CurScene.GetPassData().waveID;
                m_Waves.Capacity = waveIds.Length;
                foreach (int id in waveIds)
                {
                    m_Waves.Add(id);
                }
            }
            m_AllWave = new List<Wave>(m_Waves.Count);
            foreach (int waveId in m_Waves)
            {
                Wave wave = m_WaveMgr.GetWavesById(waveId);
                if (wave != null)
                {
                    m_AllWave.Add(wave);
                }
            }
            RegisterListener();
        }
        public void SetWpnChipDropCount(int dropId,int dropCount)
        {
            m_WpnChipDropId = dropId;
            m_WpnChipDropProgress = new List<float>();
            for (int i = 0;i < dropCount; i++)
            {
                m_WpnChipDropProgress.Add(Random.Range(0.1f, 0.9f));
            }
            m_WpnChipDropProgress.Sort();
        }

        public void SetSourceDropCount(int dropId, int dropCount)
        {
            m_SourceDropId = dropId;
            m_SourceDropProgress = new List<float>();
            for (int i = 0; i < dropCount; i++)
            {
                m_SourceDropProgress.Add(Random.Range(0.1f, 0.9f));
            }
            m_SourceDropProgress.Sort();
        }
        IEnumerator ShowPlotTips(int plotId, float time)
        {
            yield return new WaitForSeconds(time);
            Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowFightPlotByID, plotId);
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
        private void LateUpdate()
        {
            if(m_DirtyFresh)
            {
                m_DirtyFresh = false;
                FreshTarget();
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
                if (!m_Monster.InCameraView && !m_Monster.InDeath)
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
                if (m_Waves.Contains(kv.Value.GetWaveId()))
                {

                    List<Monster> monsters = kv.Value.GetMonsters();
                    foreach(Monster monster in monsters)
                    {
                        if (monster.InCameraView)
                        {
                            SetLockEnemy(null);
                            return;
                        }
                        else if (!onlyCheckMonsterVisible)
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
            float time = 0;
            base.BeginTask();
            FreshTarget();
            if (m_Waves.Count == 0)
            {
                BroadPlotTips(0);
                UnRegisterListener();
                EndTask();
            }
            else
            {
                foreach (int propId in m_ContinuePlotTipsId)
                {
                    time += 3;
                    StartCoroutine(ShowPlotTips(propId, time));
                }
            }
        }
        private void WaveEnded(int guid, int waveId)
        {
            m_Waves.Remove(waveId);
            if (m_Waves.Count == 0) {
                UnRegisterListener();
                if (m_TaskState == TaskState.Begin)
                {
                    BroadPlotTips(0);
                    FreshTarget();
                    gameObject.SetActive(false);
                    EndTask();
                }
            }
        }
        public override void EndTask()
        {
            Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.FightUiProgress,-1);
            base.EndTask();
        }
        private void MonsterDead(int guid, int monsterId, Monster monster)
        {
            m_DirtyFresh = true;
        }
        private void FreshTarget()
        {
            if (m_TaskState == TaskState.Begin)
            {
                int totalCount = 0;
                int leftCount = 0;
                foreach (Wave wave in m_AllWave)
                {
                    totalCount += wave.GetRealTotalMonsterCount();
                    leftCount += wave.GetRealMonsterCount();
                }
                if (totalCount > 0)
                {
                    int percent = (int)(100.0f * (totalCount - leftCount) / totalCount);
                    Progress = percent;
                    BroadTargetTips(0, percent + "%");
                }
                else
                {
                    BroadTargetTips(0, 0 + "%");
                }
                float progress = 1.0f * (totalCount - leftCount) / totalCount;
                TryDropProp(progress);
                Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.FightUiProgress, progress);
            };
        }

        private void TryDropProp(float progress)
        {
            if (m_WpnChipDropProgress != null && m_WpnChipDropProgress.Count > 0)
            {
                if (progress > m_WpnChipDropProgress[0])
                {
                    if (m_TaskModeMgr.AddWpnChipDropProp(m_WpnChipDropId))
                    {
                        m_WpnChipDropProgress.RemoveAt(0);
                    }
                }
            }
            if (m_SourceDropProgress != null && m_SourceDropProgress.Count > 0)
            {
                if (progress > m_SourceDropProgress[0])
                {
                    if (m_TaskModeMgr.AddSourceDropProp(m_SourceDropId))
                    {
                        m_SourceDropProgress.RemoveAt(0);
                    }
                }
            }
        }
        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, int>(MsgIds.WaveEnded, WaveEnded);

            Global.gApp.gMsgDispatcher.RemoveListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int, int>(MsgIds.WaveEnded, WaveEnded);
            Global.gApp.gMsgDispatcher.AddListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
        }
        public override void Destroy()
        {
            UnRegisterListener();
            //gameObject.SetActive(false);
        }
    }
}
