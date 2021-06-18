using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public abstract class BaseTaskMode : MonoBehaviour, IComparable<BaseTaskMode>
    {
        protected enum TaskState
        {
            Wait,
            Begin,
            Ended,
        }
        [SerializeField] private int m_Index = 0;
        [SerializeField] protected List<int> m_TargetTipsId;
        [SerializeField] protected List<int> m_PlotTipsId;
        [SerializeField] protected float m_CountDownTime = 0;

        public float Progress { get; set; }
        private float m_OriCountDownTime;
        protected bool m_StartCountDown = false;
        protected float m_UpdateDt = 0.067f;
        protected float m_CurTime = 0.33f;

        protected float m_CurClipTime = 0;

        private int m_OrderIndex = 0;
        protected string m_Tips = "";
        protected TaskModeMgr m_TaskModeMgr;
        protected TaskState m_TaskState;
        protected Transform m_TargetTsf;
        protected Transform m_PlayerTsf;
        protected FightTaskUiType m_FightArrowType = FightTaskUiType.Empty;
        private bool m_FirstUpTargetTip = false;
        private bool m_InNormalPass;
        private string CountDownClipName = "countdown";
        public int CompareTo(BaseTaskMode other)
        {
            if (other == null)
            {
                return -1;
            }
            else
            {
                int otherIndex = other.GetIndex();
                if (otherIndex > m_Index)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        public void SetOrderIndex(int index)
        {
            m_OrderIndex = index; 
        }
        public int GetIndex()
        {
            return m_Index;
        }

        protected void BroadTargetTips(string tips)
        {
            Global.gApp.gMsgDispatcher.Broadcast<string, bool>(MsgIds.FightUiTopTips, tips, m_FirstUpTargetTip);
            m_FirstUpTargetTip = true;
        }
     
        protected void BroadTargetTips(int index, string args1 = "", string arg2 = "",string arg3 = "")
        {
            if (m_TargetTipsId.Count >= index + 1)
            {
                string tips = Global.gApp.gGameData.GetTipsInCurLanguage(m_TargetTipsId[index]);
                tips = string.Format(tips, args1, arg2, arg3);
                BroadTargetTips(tips);
            }
        }
        protected void BroadPlotTips(int index, string args)
        {
            if (m_PlotTipsId.Count >= index + 1)
            {
                int strId = m_PlotTipsId[index];
                if (strId > 0)
                {
                    string tips = Global.gApp.gGameData.GetTipsInCurLanguage(strId);
                    tips = string.Format(tips, args);
                    Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowFightPlotByStr, tips);
                }
            }
        }
        protected void BroadPlotTips(int index)
        {
            if (m_PlotTipsId.Count >= index + 1)
            {
                int strId = m_PlotTipsId[index];
                if(strId > 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowFightPlotByID, strId);
                }
            }
        }
        protected virtual void SetTargetTsf(Transform transform)
        {
            m_TargetTsf = transform;
        }
        protected void UpdatePointArrow(bool visible)
        {
            if (!m_InNormalPass) { return; }
            if (m_TaskState == TaskState.Begin)
            {
                if (visible)
                {
                    if (m_TargetTsf != null && m_PlayerTsf != null)
                    {
                        Vector3 pointVec = m_TargetTsf.position - m_PlayerTsf.position;
                        pointVec.z = 0;
                        float newLength = pointVec.magnitude;
                        if (newLength < 500)
                        {
                            float angle = EZMath.SignedAngleBetween(pointVec, Vector3.up);
                            Global.gApp.gMsgDispatcher.Broadcast<float, string, FightTaskUiType>(MsgIds.PointArrowAngle, angle, ((int)pointVec.magnitude).ToString() + "m", m_FightArrowType);
                        }
                        else
                        {
                            Global.gApp.gMsgDispatcher.Broadcast<float, string, FightTaskUiType>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr, m_FightArrowType);
                        }
                    }
                    else
                    {
                        Global.gApp.gMsgDispatcher.Broadcast<float, string, FightTaskUiType>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr, m_FightArrowType);
                    }
                }
                else
                {
                    Global.gApp.gMsgDispatcher.Broadcast<float, string, FightTaskUiType>(MsgIds.PointArrowAngle, -100, GameConstVal.EmepyStr, m_FightArrowType);
                }
            }
        }

        public virtual void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_PlayerTsf = playerGo.transform;
            m_TaskModeMgr = mgr;
            m_TaskState = TaskState.Wait;
            m_InNormalPass =Global.gApp.CurScene.IsNormalPass();
            m_OriCountDownTime = m_CountDownTime;
            Global.gApp.gMsgDispatcher.Broadcast<int, FightTaskUiType>(MsgIds.CreateTaskIcon, m_OrderIndex, m_FightArrowType);
   
        }
        protected  void StartCountDown()
        {
            Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.TaskModeCountDown, m_Index);
        }
        public virtual void BeginTask()
        {
            m_TaskState = TaskState.Begin;
            Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.TaskModeBegin, m_Index);
            Global.gApp.gMsgDispatcher.Broadcast<int, FightTaskUiType>(MsgIds.TaskIconLight, -1001, m_FightArrowType);
            //Global.gApp.CurScene.GetTimerMgr().AddTimer(1,0,(dt,end)=> { EndTask(); });
        }
        public virtual void EndTask()
        {
            if (m_TaskState == TaskState.Begin)
            {
                m_StartCountDown = false;
                UpdatePointArrow(false);
                m_TaskState = TaskState.Ended;
                SetTargetTsf(null);
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.TaskModeEnd, m_Index);
                Global.gApp.gMsgDispatcher.Broadcast<int, FightTaskUiType>(MsgIds.TaskIconLight, m_OrderIndex, m_FightArrowType);
                m_TaskModeMgr.TaskCompleted(m_Index);
            }
        }

        protected void SetTriggerEventEnable(bool enable)
        {
            TriggerEvent[] triggerEvents = GetComponentsInChildren<TriggerEvent>();

            foreach (TriggerEvent trggerEvent in triggerEvents)
            {
                trggerEvent.GetComponent<Collider2D>().enabled = enable;
            }
        }
        protected float BroadCoolDown(float time,bool endBroad = false)
        {
            float dtTime = BaseScene.GetDtTime();
            time -= dtTime;
            m_CurTime += dtTime;
            if (m_CurTime > m_UpdateDt)
            {
                m_CurTime = 0;
                if(time > 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, m_Tips, UiTools.FormatTime(time));
                }
            }
            if (time <= 11f)
            {
                m_CurClipTime += dtTime;
                if (m_CurClipTime >= 1)
                {
                    m_CurClipTime -= 1;
                    Global.gApp.gAudioSource.PlayOneShot(CountDownClipName,true);
                }
            }
            if(time <= 0 && endBroad)
            {
                Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, GameConstVal.EmepyStr, GameConstVal.EmepyStr);
                EndTask();
            }
            return time;
        }

        protected void TimeLimitUp()
        {
            if (m_CountDownTime > 0)
            {
                m_CountDownTime = BroadCoolDown(m_CountDownTime);
                if (m_CountDownTime <= 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, GameConstVal.EmepyStr, GameConstVal.EmepyStr);
                    m_TaskModeMgr.TimeOver();
                }
            }
        }
        protected void ShowNewPlot(int id,Action callBack)
        {
            //if(id > 0)
            //{
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.DialogueUI, id.ToString());
            //    DialogueUI dialogueUI = Global.gApp.gUiMgr.GetPanelCompent<DialogueUI>(Wndid.DialogueUI);
            //    dialogueUI.SetAciton(callBack);
            //}
            //else
            //{
            //    callBack();
            //}
        }
        public void ResetTime()
        {
            m_CountDownTime = m_OriCountDownTime;
            Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, m_Tips, UiTools.FormatTime(m_CountDownTime));
        }
        public abstract void Destroy();
    }
}
