using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public sealed class CollectPropModeB : BaseTaskMode
    {
        private List<GameObject> m_Props = new List<GameObject>();
        private int m_TargetPropCount = 0;
        private BaseProp m_CurProp;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.CollectProp;
            gameObject.SetActive(false);
            base.Init(mgr, playerGo);
            BaseProp[] props = GetComponentsInChildren<BaseProp>();
            foreach (BaseProp prop in props)
            {
                m_Props.Add(prop.gameObject);
                prop.GetComponent<Collider2D>().enabled = false;
            }
            SetTriggerEventEnable(false);
            m_TargetPropCount = props.Length;
        }
        public override void BeginTask()
        {
            RegisterListener();
            foreach (GameObject prop in m_Props)
            {
                prop.GetComponent<Collider2D>().enabled = true;
            }
            SetTriggerEventEnable(true);
            gameObject.SetActive(true);
            base.BeginTask();
            BroadTargetTips(0, "0", m_TargetPropCount.ToString());
            BroadPlotTips(0);
            StartCoroutine(DelayTips());
            SetTsf();
        }
        IEnumerator DelayTips()
        {
            yield return new WaitForSeconds(1);
            BroadPlotTips(1);
        }
        private void SetTsf()
        {
            if (m_Props.Count >= 1 && m_Props[0] != null)
            {
                SetTargetTsf(m_Props[0].transform);
                m_CurProp = m_Props[0].GetComponent<BaseProp>();
            }
        }
        private void Update()
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
                TimeLimitUp();
            }
        }
        private void CollectingProp(GameProp prop, GameObject obj)
        {
            int index = m_Props.FindIndex(l => l == obj);
            if (index >= 0)
            {
                BroadPlotTips(2);
            }
        }
        private void GainProp(GameProp prop, GameObject obj)
        {
            if (m_Props.Contains(obj))
            {
                m_Props.Remove(obj);
                int leftCount = m_TargetPropCount - m_Props.Count;
                BroadTargetTips(0, leftCount.ToString(), m_TargetPropCount.ToString());

                if (m_Props.Count == 0)
                {
                    BroadPlotTips(4);
                    EndTask();
                }
                else
                {
                    BroadPlotTips(3);
                    SetTsf();
                }
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
            UnRegisterListener();
            base.EndTask();
        }
        public override void Destroy()
        {
            UnRegisterListener();
        }
    }
}
