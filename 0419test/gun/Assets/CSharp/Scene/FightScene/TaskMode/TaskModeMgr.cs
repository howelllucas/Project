using EZ.Data;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class TaskModeMgr
    {
        private List<BaseTaskMode> m_TaskModes;
        private int m_CurTaskIndex = 0;
        private Transform m_TaskModeNode;
        private PassItem m_CurPassItem;
        private int m_ReviveTimes = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.MAX_REVIVE_TIMES).content);

        private int m_ChipDropTimes = 1;
        private int m_SourceDropTimes = 1;
        private float m_MinTime = 50;
        private float m_MaxTime = 150;
        public TaskModeMgr(Transform mapTsf,GameObject player,PassItem passItem)
        {
            m_TaskModeNode = mapTsf.Find("TaskNode");
            if (m_TaskModeNode == null)
            {
                Debug.Log(" waning   no task Mode ");
                return;
            }
            m_TaskModes = new List<BaseTaskMode>(m_TaskModeNode.GetComponentsInChildren<BaseTaskMode>());
            if(m_TaskModes.Count == 0)
            {
                Debug.Log(" waning   no tasks ");
                return;
            }
            m_TaskModes.Sort();
            int index = 0;
            foreach(BaseTaskMode taskMode in m_TaskModes)
            {
                index++;
                taskMode.SetOrderIndex(index);
                taskMode.Init(this, player);
            }
            m_CurPassItem = passItem;
            InitChipDropMode();
            InitNormalSourceDropMode();
        }
        private void InitChipDropMode()
        {
            if (m_CurPassItem.chipParam != null && m_CurPassItem.chipParam.Length == 2 && m_CurPassItem.chipParam[1] > 0)
            {
                m_ChipDropTimes = m_CurPassItem.chipParam[1];
                if (m_TaskModes.Count == 1)
                {
                    KillMonstersMode killMonstersMode = m_TaskModes[0] as KillMonstersMode;
                    if(killMonstersMode != null)
                    {
                        killMonstersMode.SetWpnChipDropCount(m_CurPassItem.chipParam[0], m_CurPassItem.chipParam[1]);
                    }
                    else
                    {
                        Global.gApp.CurScene.GetTimerMgr().AddTimer(Random.Range(m_MinTime, m_MaxTime), 1, AddWpnChipProp);
                    }
                }
                else
                {
                    Global.gApp.CurScene.GetTimerMgr().AddTimer(Random.Range(m_MinTime, m_MaxTime),1, AddWpnChipProp);
                }
            }
            
        }

        private void InitNormalSourceDropMode()
        {
            if (m_CurPassItem.sourceParam != null && m_CurPassItem.sourceParam.Length == 2 && m_CurPassItem.sourceParam[1] > 0)
            {
                m_SourceDropTimes = m_CurPassItem.sourceParam[1];
                if (m_TaskModes.Count == 1)
                {
                    KillMonstersMode killMonstersMode = m_TaskModes[0] as KillMonstersMode;
                    if (killMonstersMode != null)
                    {
                        killMonstersMode.SetSourceDropCount(m_CurPassItem.sourceParam[0], m_CurPassItem.sourceParam[1]);
                    }
                    else
                    {
                        Global.gApp.CurScene.GetTimerMgr().AddTimer(Random.Range(m_MinTime, m_MaxTime), 1, AddSourceProp);
                    }
                }
                else
                {
                    Global.gApp.CurScene.GetTimerMgr().AddTimer(Random.Range(m_MinTime, m_MaxTime), 1, AddSourceProp);
                }
            }

        }

        public void AddSourceProp(float a, bool b)
        {
            if (AddSourceDropProp(m_CurPassItem.sourceParam[0]))
            {
                m_SourceDropTimes--;
                if (m_SourceDropTimes > 0)
                {
                    Global.gApp.CurScene.GetTimerMgr().AddTimer(Random.Range(m_MinTime, m_MaxTime), 1, AddSourceProp);
                }
            }
            else
            {
                Global.gApp.CurScene.GetTimerMgr().AddTimer(1, 1, AddSourceProp);
            }
        }
        public void AddWpnChipProp(float a,bool b)
        {
            if (AddWpnChipDropProp(m_CurPassItem.chipParam[0]))
            {
                m_ChipDropTimes--;
                if (m_ChipDropTimes > 0)
                {
                    Global.gApp.CurScene.GetTimerMgr().AddTimer(Random.Range(m_MinTime, m_MaxTime), 1, AddWpnChipProp);
                }
            }
            else
            {
                Global.gApp.CurScene.GetTimerMgr().AddTimer(1, 1, AddWpnChipProp);
            }
        }
        private BornNode GetCanAddPropNode()
        {
            BornNode[] roleBornNodes = (Global.gApp.CurScene as FightScene).GetBornNodes();
            int count = roleBornNodes.Length;
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
                BornNode newNode = roleBornNodes[newIndex];
                if (newNode.GetIsOutMap())
                {
                    return newNode;
                }
            }
            return null;
        }
        public bool AddWpnChipDropProp(int dropId)
        {
            //BornNode bornNode = GetCanAddPropNode();
            //if (bornNode != null)
            //{
            //    Global.gApp.CurScene.GetPropMgr().AddWpnChipPropByPass(bornNode.transform.position, dropId);
            //    return true;
            //}
            return false;
        }
        public bool AddSourceDropProp(int dropId)
        {
            //BornNode bornNode = GetCanAddPropNode();
            //if (bornNode != null)
            //{
            //    Global.gApp.CurScene.GetPropMgr().AddProp(bornNode.transform.position, dropId);
            //    return true;
            //}
            return false;
        }
        private void BeginTaskImp(float dt,bool end)
        {
            if (m_TaskModes.Count >= 0)
            {
                m_TaskModes[m_CurTaskIndex].BeginTask();
            }
            else
            {
               Global.gApp.CurScene.GameLose();
            }
        }
        public void BeginTask()
        {
           Global.gApp.CurScene.GetTimerMgr().AddTimer(0.1f, 1, BeginTaskImp);
        }
        public BaseTaskMode GetCurTaskMode()
        {
            return m_TaskModes[m_CurTaskIndex];
        }
        public void TaskCompleted(int taskId)
        {
            BaseTaskMode taskMode = GetCurTaskMode();
            if(taskMode.GetIndex() == taskId)
            {
                int newIndex = m_CurTaskIndex + 1;
                if (newIndex == m_TaskModes.Count)
                {
                   Global.gApp.CurScene.GameWin();
                    return; 
                }
                m_CurTaskIndex = newIndex;
                BaseTaskMode nextTaskMode = GetCurTaskMode();
                nextTaskMode.BeginTask();
            }
            else
            {
                Debug.Log("=== not is cur Task error=== ");
            }
        } 
        public void Destroy()
        {
            foreach(BaseTaskMode taskMode in m_TaskModes)
            {
                taskMode.Destroy();
            }
        }
        public void TimeOver()
        {
            if (m_ReviveTimes > 0)
            {
                Global.gApp.CurScene.Pause();
                FightResultManager.instance.ShowReivePopup((bool tmpR) =>
                {
                    Global.gApp.CurScene.Resume();
                    if (tmpR)
                    {
                        m_ReviveTimes--;
                        BaseTaskMode nextTaskMode = GetCurTaskMode();
                        nextTaskMode.ResetTime();
                    }
                    else
                    {
                        Global.gApp.CurScene.GameLose();
                    }
                });
            }
            else
            {
                Global.gApp.CurScene.GameLose();
            }
        }
    }
}
