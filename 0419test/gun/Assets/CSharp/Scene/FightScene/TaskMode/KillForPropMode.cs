using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class KillForPropMode : BaseTaskMode
    {
        [SerializeField] private int m_AppointMonsterId;
        [SerializeField] private GameProp m_GameProp;
        private bool ShowSecondPlotTips = false;
        private Monster m_Monster;
        private BaseProp m_AppointProp;
        public override void Init(TaskModeMgr mgr,GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.KillApointMonster;
            base.Init(mgr, playerGo);
        }
        public override void BeginTask()
        {
            RegisterListener();
            base.BeginTask();

            BroadTargetTips(0);
            BroadPlotTips(0);
        }
        protected override void SetTargetTsf(Transform transform)
        {
            base.SetTargetTsf(transform);
            if(transform != null)
            {
                m_Monster = transform.GetComponent<Monster>();
            }
            else
            {
                m_Monster = null;
            }
        }
         void Update()
        {
            if (m_TaskState == TaskState.Begin)
            {
                float dtTime = BaseScene.GetDtTime();
                if (dtTime > 0)
                {
                    FindMonsterTargetTsf();
                    if ((m_AppointProp && !m_AppointProp.InCameraView) || (m_Monster && !m_Monster.InCameraView))
                    {
                        UpdatePointArrow(true);
                    }
                    else
                    {
                        UpdatePointArrow(false);
                    }
                    BroadSecondPlotTip();
                    TimeLimitUp();
                }
            }
        }

        private void FindMonsterTargetTsf()
        {
            if (m_TargetTsf == null)
            {
                Monster monster =Global.gApp.CurScene.GetWaveMgr().GetMonsterById(m_AppointMonsterId);
                if (monster != null)
                {
                    SetTargetTsf(monster.transform);
                }
            }
        }
        private  void BroadSecondPlotTip()
        {
            if (!ShowSecondPlotTips && m_Monster && m_Monster.InCameraView)
            {
                ShowSecondPlotTips = true;
                BroadPlotTips(1);
            }
        }
        public override void EndTask()
        {
            UnRegisterListener();
            gameObject.SetActive(false);
            base.EndTask();
        }
        private void GainProp(GameProp prop,GameObject obj)
        {
            if (prop == m_GameProp)
            {
                BroadPlotTips(3);
                EndTask();
            }
        }
        private void MonsterDead(int guid,int monsterId,Monster monster)
        {
            if(monsterId == m_AppointMonsterId)
            {
                Global.gApp.gMsgDispatcher.RemoveListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
                //BroadPlotTips(2);
                //BroadTargetTips(1);
                //GameObject gameObject =Global.gApp.CurScene.GetPropMgr().AddAppointProp(monster.transform.position, m_GameProp.ToString());
                //m_AppointProp = gameObject.GetComponent<BaseProp>();
                //SetTargetTsf(m_AppointProp.transform);
            }
        }

        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, int,Monster>(MsgIds.MonsterDead,MonsterDead);
            Global.gApp.gMsgDispatcher.RemoveListener<GameProp,GameObject>(MsgIds.GainProp, GainProp);
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int, int, Monster>(MsgIds.MonsterDead, MonsterDead);
            Global.gApp.gMsgDispatcher.AddListener<GameProp, GameObject>(MsgIds.GainProp, GainProp);
        }
        public override void Destroy()
        {
            UnRegisterListener();
        }
    }
}
