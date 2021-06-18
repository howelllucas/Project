using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class FightRescuedBehavior : FightNpcBaseBehavior
    {
        [SerializeField] private float m_FleeSpeed = 10;
        float m_CurTime = 0;
        float m_WaitTime = 0.3f;
        float m_DestroyTime = 0.5f;
        private int m_PlotId = 0;

        BornNode[] m_BornNodes;
        BornNode m_CurBornNode;
        public override void Init(FightNpcPlayer npcPlayer, GameObject playerGo)
        {
            base.Init(npcPlayer, playerGo);
            m_BornNodes = (Global.gApp.CurScene as FightScene).GetBornNodes();
        }
        private void Update()
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
        public bool GenerateBornNode()
        {
            if (m_CurBornNode != null && m_CurBornNode.GetIsOutMap())
            {
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

            //Global.gApp.gUiMgr.OpenPanel(Wndid.FightNpcPlotUi);
            //FightNpcPlotUi plotUi = Global.gApp.gUiMgr.GetPanelCompent<FightNpcPlotUi>(Wndid.FightNpcPlotUi);
            //plotUi.SetFollowNode(m_FightNpcPlayer.GetTaskUINode());
            m_FightNpcPlayer.PlayAnim(GameConstVal.Run);
            GameObject disapperaEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Npc_xiaoshiEffect);
            disapperaEffect.AddComponent<DelayDestroy>().SetLiveTime(1.5f);
            disapperaEffect.transform.position = transform.position;
        }

        public void SetCurPlotId(int plotId)
        {
            m_PlotId = plotId;
        }
    }
}
