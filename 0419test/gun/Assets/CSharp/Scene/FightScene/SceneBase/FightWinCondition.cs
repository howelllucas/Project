
using EZ.Data;
using System.Collections.Generic;

namespace EZ
{
    public enum GameWinType
    {
        KillAllMonster = 1,
        KillAppointMonster = 2,
        KeepLive = 3,
        KillAppointWave = 4,
        ActiveProps = 5,
    }
    public class FightWinCondition
    {
        private FightScene m_FightScene;
        private PassItem m_PassData;
        private GameWinType m_GameType = GameWinType.KillAllMonster;
        private Dictionary<int, int> m_ActiveProps = new Dictionary<int, int>();
        public FightWinCondition(PassItem passData, FightScene scene)
        {
            m_PassData = passData;
            m_FightScene = scene;
            //m_GameType = (GameWinType)m_PassData.winCondition[0];
            RegisterListener();
        }

        private void ActiveProp(int propId)
        {
            if (m_GameType == GameWinType.ActiveProps)
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
                        m_FightScene.GameWin();
                    }
                }
            }
        }

        private void CoolDown(int curTime)
        {
            //if (curTime >= m_PassData.winCondition[1])
            //{
            //    m_FightScene.GameWin();
            //}
        }
        private void WaveEnd(int guid, int waveId)
        {
            //if (waveId == m_PassData.winCondition[1])
            //{
            //    m_FightScene.GameWin();
            //}
        }
        private void AllWaveEnd()
        {
            m_FightScene.GameWin();
        }
        private void MonsterDead(int guid, int monsterId, Monster monster)
        {
            //    if (monsterId == m_PassData.winCondition[1])
            //    {
            //        m_FightScene.GameWin();
            //    }
        }
        private void RegisterListener()
        {
            if (m_GameType == GameWinType.KillAllMonster)
            {
                Global.gApp.gMsgDispatcher.AddListener(MsgIds.AllWaveEnded, AllWaveEnd);
            }
            else if (m_GameType == GameWinType.KillAppointMonster)
            {
                Global.gApp.gMsgDispatcher.AddListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
            }
            else if (m_GameType == GameWinType.ActiveProps)
            {
                Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.ActiveProp, ActiveProp);
                //int[] winCondition = m_PassData.winCondition;
                //for (int i = 1; i <= (winCondition.Length - 1) / 2; i++)
                //{
                //    m_ActiveProps.Add(winCondition[2 * (i - 1) + 1], winCondition[2 * i]);
                //}
            }
            else if (m_GameType == GameWinType.KeepLive)
            {
                Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TimeCoolDown, CoolDown);
            }
            else if (m_GameType == GameWinType.KillAppointWave)
            {
                Global.gApp.gMsgDispatcher.AddListener<int, int>(MsgIds.WaveEnded, WaveEnd);
            }
        }
        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TimeCoolDown, CoolDown);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.AllWaveEnded, AllWaveEnd);
            Global.gApp.gMsgDispatcher.RemoveListener<int, int,Monster>(MsgIds.MonsterDead, MonsterDead);
            Global.gApp.gMsgDispatcher.RemoveListener<int, int>(MsgIds.WaveEnded, WaveEnd);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.ActiveProp, ActiveProp);
        }
        public void Destroy()
        {
            UnRegisterListener();
        }
        public GameWinType GetPlayType()
        {
            return m_GameType;
        }
    }
}
