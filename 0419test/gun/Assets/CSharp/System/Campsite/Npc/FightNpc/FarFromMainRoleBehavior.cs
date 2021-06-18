using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class FarFromMainRoleBehavior : FightNpcBaseBehavior
    {
        [SerializeField] private float m_FleeSpeed = 10;
        private Action<bool> m_PursueCallBack;
        private Action m_PursueExtraCallBack;
        private bool m_ReachedPlaceTag = false;
        private bool m_StartCloseAct = false;
        float m_CurTime = 0;
        float m_CloseMaxTime = 5;
        BornNode[] m_BornNodes;
        BornNode m_CurBornNode;
        public override void Init(FightNpcPlayer npcPlayer, GameObject playerGo)
        {
            base.Init(npcPlayer, playerGo);
            m_PursueCallBack = PursueCallBack;
            m_BornNodes = (Global.gApp.CurScene as FightScene).GetBornNodes();
        }
        private void Update()
        {
            if (m_StartCloseAct && !m_ReachedPlaceTag)
            {
                m_CurTime += BaseScene.GetDtTime();
                if (m_CurTime >= m_CloseMaxTime)
                {
                    PursueCallBack(true);
                }
            }
            if (m_CurBornNode == null)
            {
                bool hasNewDstNode = GenerateBornNode();
                if (hasNewDstNode)
                {
                    m_FightNpcPlayer.PlayAnim(GameConstVal.Run);
                    m_FightNpcPlayer.GetAutoPathComp().SetAutoPathEnable(true, 0.2f, m_FleeSpeed, m_CurBornNode.transform, m_PursueCallBack);
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
        private void PursueCallBack(bool reachPlace)
        {
            if (reachPlace && !m_ReachedPlaceTag)
            {
                m_ReachedPlaceTag = true;
                m_FightNpcPlayer.EndCurBehavior();
                if (m_PursueExtraCallBack != null)
                {
                    m_PursueExtraCallBack();
                }
            }
        }
        public override void StartBehavior()
        {
            base.StartBehavior();
            m_CurBornNode = null;
            m_ReachedPlaceTag = false;
            enabled = true;
        }
        public override void EndBehavior()
        {
            base.EndBehavior();
            m_FightNpcPlayer.GetAutoPathComp().SetAutoPathEnable(false);
            m_FightNpcPlayer.GetAutoPathComp().SetSpeedScale(1);
            m_ReachedPlaceTag = true;
            m_FightNpcPlayer.PlayAnim(GameConstVal.Idle);
            m_FightNpcPlayer.SetSpeed(Vector2.zero);
            enabled = false;
        }
        public void SetPursueCallBack(Action action)
        {
            m_PursueExtraCallBack = action;
        }
    }
}
