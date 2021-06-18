using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public sealed class CollectOneNpcMode : BaseTaskMode
    {
        NpcProp m_NpcProp;
        [Tooltip(" 新剧情剧情id ")]
        public int m_NewPlotId = -1;
        private int m_TargetPropCount = 0;
        private FightNpcPlayer m_CollectFightNpcPlayer = null;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.CollectProp;
            gameObject.SetActive(false);
            base.Init(mgr, playerGo);
            m_NpcProp = GetComponentInChildren<NpcProp>();
            m_NpcProp.gameObject.GetComponent<Collider2D>().enabled = false;
        }
        public override void BeginTask()
        {
            RegisterListener();
            m_NpcProp.GetComponent<Collider2D>().enabled = true;
            gameObject.SetActive(true);
            base.BeginTask();
            BroadTargetTips(0);
            BroadPlotTips(0);
            SetTsf();
        }
        private void SetTsf()
        {
            SetTargetTsf(m_NpcProp.transform);
        }
        private void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                if (m_TargetTsf && m_NpcProp && !m_NpcProp.InCameraView)
                {
                    UpdatePointArrow(true);
                }
                else
                {
                    UpdatePointArrow(false);
                }
                TimeLimitUp();
            }
        }
        private void GainProp(GameProp prop, GameObject obj)
        {
            if(m_NpcProp.gameObject == obj)
            {
                m_CollectFightNpcPlayer = m_NpcProp.GetFightNpcPlayer();
                ShowNewPlot(m_NewPlotId, PlotCallBack);
            }
        }
        private void PlotCallBack()
        {
            //m_CollectFightNpcPlayer.SetBehavior(FightNpcPlayer.NpcBehaviorType.Rescued);
            EndTask();
        }
        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<GameProp, GameObject>(MsgIds.GainProp, GainProp);
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<GameProp, GameObject>(MsgIds.GainProp, GainProp);
        }
        public override void EndTask()
        {
            UnRegisterListener();
            base.EndTask();
        }
        public override void Destroy()
        {
            UnRegisterListener();
        }
    }
}
