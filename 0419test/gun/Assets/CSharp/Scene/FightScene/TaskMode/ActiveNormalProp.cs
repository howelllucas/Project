using UnityEngine;

namespace EZ
{
    public sealed class ActiveNormalProp : BaseTaskMode
    {
        Transform m_TriggerNode;
        GameObject m_PropNode;
        GameObject m_PropEffectNode;
        private BaseProp m_CurProp;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.CollectProp;
            base.Init(mgr, playerGo);
            if (m_PlotTipsId.Count >= 4)
            {
                string tips = Global.gApp.gGameData.GetTipsInCurLanguage(m_PlotTipsId[3]);
                m_Tips = tips;
            }
            m_TriggerNode = transform.Find("TriggerNode");
            m_PropNode = transform.Find("NormalProp").gameObject;
            m_PropEffectNode = transform.Find("NormalPropEffect").gameObject;
            m_CurProp = m_PropNode.GetComponent<BaseProp>();
            SetTargetTsf(m_PropNode.transform);
        }
        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                if (m_TargetTsf && m_CurProp && !m_CurProp.InCameraView)
                {
                    UpdatePointArrow(true);
                }
                else
                {
                    UpdatePointArrow(false);
                }
                if (m_StartCountDown)
                {
                    m_CountDownTime = BroadCoolDown(m_CountDownTime);
                    if (m_CountDownTime <= 0)
                    {
                        EndTask();
                    }
                }
            }
        }
        public override void BeginTask()
        {
            RegisterListener();
            base.BeginTask();
            BroadTargetTips(0);
            BroadPlotTips(0);
            m_PropNode.GetComponent<Collider2D>().enabled = true;
        }
        private void CollectingProp(GameProp prop, GameObject obj)
        {
            if (obj == m_PropNode)
            {
                BroadPlotTips(1);
            }
        }
        private void GainProp(GameProp prop, GameObject obj)
        {
            if (obj == m_PropNode)
            {
                BroadPlotTips(2);
                //BroadPlotTips(4);
                m_StartCountDown = true;
                m_TriggerNode.gameObject.SetActive(true);
                m_PropEffectNode.SetActive(true);
            }
        }
        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<GameProp, GameObject>(MsgIds.GainProp, GainProp);
            Global.gApp.gMsgDispatcher.RemoveListener<GameProp, GameObject>(MsgIds.CollectingProp, CollectingProp);
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<GameProp, GameObject>(MsgIds.GainProp, GainProp);
            Global.gApp.gMsgDispatcher.AddListener<GameProp, GameObject>(MsgIds.CollectingProp, CollectingProp);
        }
        public override void EndTask()
        {
            BroadPlotTips(5);
            UnRegisterListener();
            Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips,GameConstVal.EmepyStr, GameConstVal.EmepyStr);
            base.EndTask();
        }
        public override void Destroy()
        {
            UnRegisterListener();
        }
    }
}
