using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public sealed class ReachAssignedPointMode : BaseTaskMode
    {
        private Transform m_TriggerEventTsf;
        private TriggerEvent m_TriggerEvent;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.ArrivePoint;
            base.Init(mgr, playerGo);
        }
        private void Start()
        {
            m_TriggerEventTsf = transform.Find("TriggerNode");
            m_TriggerEvent = m_TriggerEventTsf.GetComponent<TriggerEvent>(); 
            SetTargetTsf(m_TriggerEventTsf);
            gameObject.SetActive(false);
        }
        public override void BeginTask()
        {
            gameObject.SetActive(true);
            RegisterListener();

            base.BeginTask();

            BroadTargetTips(0);
            BroadPlotTips(0);
            StartCoroutine(ShowSecondTips());
        }

        public override void EndTask()
        {
            UnRegisterListener();
            gameObject.SetActive(false);
            base.EndTask();
        }

        public void TriggerCollider(int triggerId, Transform eventNode)
        {
            if(eventNode == m_TriggerEventTsf)
            {
                BroadPlotTips(2);
                EndTask();
            }
        }
        private void Update()
        {
            if (m_TriggerEvent && !m_TriggerEvent.InCameraView)
            {
                UpdatePointArrow(true);
            }
            else
            {

                UpdatePointArrow(false);
            }
        }
        IEnumerator ShowSecondTips()
        {
            yield return new WaitForSeconds(5);
            BroadPlotTips(1);
        }
        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
        }
        private void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, Transform>(MsgIds.TriggerCollider, TriggerCollider);
        }
        public override void Destroy()
        {
            UnRegisterListener();
        }
    }
}
