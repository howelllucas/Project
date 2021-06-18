using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class FightFoolAppearBehavior : FightNpcBaseBehavior
    {
        [SerializeField] private float m_FleeSpeed = 10;
        private Action<bool> m_PursueCallBack;
        private Action<bool> m_BackPursueCallBack;
        private bool m_StartBackAct = false;
        private bool m_RealStartBackAct = false;
        float m_CurTime = 0;
        float m_WaitTime = 1;
        float m_DestroyTime = 0.5f;
        private int m_PlotId = 0;

        BornNode[] m_BornNodes;
        BornNode m_CurBornNode;

        public override void Init(FightNpcPlayer npcPlayer, GameObject playerGo)
        {
            base.Init(npcPlayer, playerGo);
            m_PursueCallBack = PursueCallBack;
            m_BackPursueCallBack = PursueCallBack;
            m_BornNodes = (Global.gApp.CurScene as FightScene).GetBornNodes();
        }
        private void Update()
        {
            if (m_StartBackAct)
            {
                float dtTime = BaseScene.GetDtTime();
                if (dtTime > 0)
                {
                    m_CurTime += dtTime;
                    if (m_CurTime >= m_WaitTime)
                    {
                        m_FightNpcPlayer.PlayAnim(GameConstVal.Run);

                        bool hasNewDstNode = GenerateBornNode();
                        if (hasNewDstNode)
                        {
                            m_FightNpcPlayer.GetAutoPathComp().SetAutoPathEnable(true, 0.2f, m_FleeSpeed, m_CurBornNode.transform);
                        }
                        if (!m_FightNpcPlayer.InCameraView)
                        {
                            if (m_CurTime > m_DestroyTime + m_WaitTime)
                            {
                                m_FightNpcPlayer.BroadPlotTips(m_PlotId);
                                m_FightNpcPlayer.GetAutoPathComp().SetAutoPathEnable(false);
                                m_FightNpcPlayer.GetAutoPathComp().SetSpeedScale(1);
                                Global.gApp.gUiMgr.ClosePanel(Wndid.FightNpcPlotUi);
                                m_FightNpcPlayer.DestroySelf();
                            }
                        }
                        else
                        {
                            m_CurTime = m_WaitTime;
                        }
                    }
                    else
                    {
                        m_FightNpcPlayer.PlayAnim(GameConstVal.Idle);
                        m_FightNpcPlayer.SetSpeed(Vector2.zero);
                    }

                }
            }
        }
        private void PursueCallBack(bool reachPlace)
        {
            if (reachPlace && !m_StartBackAct)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.FightNpcPlotUi);
                FightNpcPlotUi plotUi = Global.gApp.gUiMgr.GetPanelCompent<FightNpcPlotUi>(Wndid.FightNpcPlotUi);
                plotUi.SetFollowNode(m_FightNpcPlayer.GetTaskUINode());
                m_StartBackAct = true;
            }
        }
        public bool GenerateBornNode()
        {
            if(m_CurBornNode != null && m_CurBornNode.GetIsOutMap()){
                return false;
            }
            foreach (BornNode bornNode in m_BornNodes)
            {
                if (bornNode.GetIsOutMap())
                {
                    m_CurBornNode = bornNode;
                    return true;
                }
            }
            return false;
        }
        public override void StartBehavior()
        {
            base.StartBehavior();
            m_FightNpcPlayer.GetAutoPathComp().SetAutoPathEnable(true,7, 6, m_PlayerGo.transform, m_PursueCallBack);
            m_FightNpcPlayer.GetAutoPathComp().SetSpeedScale(1.12f);
            m_FightNpcPlayer.PlayAnim(GameConstVal.Run);
            m_StartBackAct = false;
            enabled = true;
        }
        public override void EndBehavior()
        {
            base.EndBehavior();
            m_FightNpcPlayer.GetAutoPathComp().SetAutoPathEnable(false);
            m_FightNpcPlayer.GetAutoPathComp().SetSpeedScale(1);
            m_StartBackAct = true;
            m_FightNpcPlayer.PlayAnim(GameConstVal.Idle);
            m_FightNpcPlayer.SetSpeed(Vector2.zero);
            enabled = false;
        }
        public void SetCurPlotId(int plotId)
        {
            m_PlotId = plotId;
        }
    }
}
