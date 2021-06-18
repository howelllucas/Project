using System.Collections;
using UnityEngine;

namespace EZ
{
    public sealed class ActiveTurretMode : BaseTaskMode
    {
        Transform m_TriggerNode;
        GameObject m_PropNode;
        GameObject m_PropEffectNode;
        private BaseProp m_CurProp;
        public override void Init(TaskModeMgr mgr, GameObject playerGo)
        {
            m_FightArrowType = FightTaskUiType.ActiveTurret;
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
            m_PropNode.SetActive(false);

            SetTargetTsf(m_PropNode.transform);
        }
        void Update()
        {
            if (m_TaskState == TaskState.Begin)
            {
                float dtTime = BaseScene.GetDtTime();
                if (dtTime > 0)
                {
                    if (m_TargetTsf && m_CurProp.enabled && !m_CurProp.InCameraView)
                    {
                        UpdatePointArrow(true);
                    }
                    else
                    {
                        UpdatePointArrow(false);
                    }
                    if (m_StartCountDown)
                    {
                        m_CountDownTime = BroadCoolDown(m_CountDownTime); ;
                        if (m_CountDownTime <= 0)
                        {
                            m_StartCountDown = false;
                            Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, GameConstVal.EmepyStr, GameConstVal.EmepyStr);
                            HelicopterArrive();
                        }
                    }
                }
            }
        }

        private void HelicopterArrive()
        {
            m_PropEffectNode.SetActive(false);
            transform.Find("FireTurretNode").gameObject.SetActive(true);
            transform.Find("FireTurretNode").gameObject.GetComponent<Animator>().Play("helidown", -1, 0);
            StartCoroutine(ShowSecondTips());
        }

        IEnumerator ShowSecondTips()
        {
            yield return new WaitForSeconds(1f);
            EndTask();
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

        private void GainProp(GameProp prop, GameObject obj)
        {
            if (obj == m_PropNode)
            {
                BroadPlotTips(2);
                //BroadPlotTips(4);
                m_StartCountDown = true;
                StartCountDown();
                m_TriggerNode.gameObject.SetActive(true);
                m_PropEffectNode.SetActive(true);
                //m_PropNode.SetActive(false);
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
            Global.gApp.gMsgDispatcher.Broadcast<string, string>(MsgIds.FightUiModeCountDownTips, GameConstVal.EmepyStr, GameConstVal.EmepyStr);
            S_FireTurret[] fireTurrets = GetComponentsInChildren<S_FireTurret>();
            foreach(S_FireTurret fireTurret in fireTurrets)
            {
                fireTurret.StartBorn();
            }
            UpdatePointArrow(false);
            enabled = false;
            BroadPlotTips(5);
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

