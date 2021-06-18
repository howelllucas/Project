using System.Collections;
using UnityEngine;

namespace EZ
{
    public sealed class RetreatAMode : BaseTaskMode
    {
        private bool m_HelicopterArrived;
        private TriggerEvent m_HelicopterTriggerEvent;
        Transform m_TriggerNode1;
        Transform m_TriggerNode2;
        GameObject m_PropNode;
        GameObject m_PropEffectNode;
        private BaseProp m_CurProp;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.ArriveHelicopter;
            base.Init(mgr, playerGo);
            if (m_PlotTipsId.Count >= 4)
            {
                string tips = Global.gApp.gGameData.GetTipsInCurLanguage(m_PlotTipsId[3]);
                m_Tips = tips;
            }
            m_TriggerNode1 = transform.Find("TriggerNode1");
            m_TriggerNode2 = transform.Find("HelicopterNode/TriggerNode2");
            m_PropNode = transform.Find("NormalProp").gameObject;
            m_PropNode.SetActive(false);
            m_PropEffectNode = transform.Find("NormalPropEffect").gameObject;
            m_CurProp = m_PropNode.GetComponent<BaseProp>();

            SetTargetTsf(m_PropNode.transform);
        }
        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                if (!m_HelicopterArrived)
                {
                    if (m_TargetTsf && m_CurProp && !m_CurProp.InCameraView)
                    {
                        UpdatePointArrow(true);
                    }
                    else
                    {
                        UpdatePointArrow(false);
                    }
                }
                else
                {
                    if(!m_HelicopterTriggerEvent.InCameraView)
                    {
                        UpdatePointArrow(true);
                    }
                    else
                    {
                        UpdatePointArrow(false);
                    }
                }
                if (m_StartCountDown)
                {
                    m_CountDownTime = BroadCoolDown(m_CountDownTime);
                    if (m_CountDownTime <= 0)
                    {
                        m_StartCountDown = false;
                        Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, GameConstVal.EmepyStr, GameConstVal.EmepyStr);
                        HelicopterArrive();
                    }
                }
            }
        }

        private void HelicopterArrive()
        {
            m_PropEffectNode.SetActive(false);
            transform.Find("HelicopterNode").gameObject.SetActive(true);
            transform.Find("HelicopterNode").gameObject.GetComponent<Animator>().Play("helidown",-1,0);
            SetTargetTsf(m_TriggerNode2);
            m_HelicopterTriggerEvent = m_TriggerNode2.GetComponent<TriggerEvent>();
            StartCoroutine(ShowSecondTips());
        }
        IEnumerator ShowSecondTips()
        {
            yield return new WaitForSeconds(1f);
            m_TriggerNode2.gameObject.SetActive(true);
            BroadPlotTips(5);
        }
        public override void BeginTask()
        {
            RegisterListener();
            base.BeginTask();
            BroadTargetTips(0);
            BroadPlotTips(0);
            m_PropNode.SetActive(true);
            m_PropNode.GetComponent<Collider2D>().enabled = true;
        }
        private void CollectingProp(GameProp prop, GameObject obj)
        {
            if (obj == m_PropNode)
            {
                BroadPlotTips(1);
            }
        }
        public void TriggerCollider(int triggerId, Transform eventNode)
        {
            if (eventNode == m_TriggerNode2)
            {
                BroadPlotTips(6);
                EndTask();
            }
        }
        private void GainProp(GameProp prop, GameObject obj)
        {
            if (obj == m_PropNode)
            {
                BroadPlotTips(2);
                //BroadPlotTips(4);
                m_StartCountDown = true;
                StartCountDown();
                m_TriggerNode1.gameObject.SetActive(true);
                m_PropEffectNode.SetActive(true);
            }
        }
        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<GameProp, GameObject>(MsgIds.GainProp, GainProp);
            Global.gApp.gMsgDispatcher.RemoveListener<GameProp, GameObject>(MsgIds.CollectingProp, CollectingProp);
            Global.gApp.gMsgDispatcher.RemoveListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<GameProp, GameObject>(MsgIds.GainProp, GainProp);
            Global.gApp.gMsgDispatcher.AddListener<GameProp, GameObject>(MsgIds.CollectingProp, CollectingProp);
            Global.gApp.gMsgDispatcher.AddListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);

        }
        public override void EndTask()
        {
            UnRegisterListener();
            Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, GameConstVal.EmepyStr, GameConstVal.EmepyStr);
            base.EndTask();
        }
        public override void Destroy()
        {
            UnRegisterListener();
        }
    }
}
