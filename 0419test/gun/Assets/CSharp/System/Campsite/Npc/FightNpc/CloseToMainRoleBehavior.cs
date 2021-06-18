using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class CloseToMainRoleBehavior : FightNpcBaseBehavior
    {
        [SerializeField] private float m_FleeSpeed = 10;
        private Action<bool> m_PursueCallBack;
        private Action m_PursueExtraCallBack;
        private bool m_ReachedPlaceTag = false;
        private bool m_StartCloseAct = false;
        float m_CurTime = 0;
        float m_CloseMaxTime = 5;
        public override void Init(FightNpcPlayer npcPlayer, GameObject playerGo)
        {
            base.Init(npcPlayer, playerGo);
            m_PursueCallBack = PursueCallBack;
        }
        private void Update()
        {
            if (m_StartCloseAct && !m_ReachedPlaceTag)
            {
                m_CurTime += BaseScene.GetDtTime();
                if(m_CurTime >= m_CloseMaxTime)
                {
                    PursueCallBack(true);
                }
            }
        }
        private void PursueCallBack(bool reachPlace)
        {
            if (reachPlace && !m_ReachedPlaceTag)
            {
                m_ReachedPlaceTag = true;
                if(m_PursueExtraCallBack != null)
                {
                    m_PursueExtraCallBack();
                }
                m_FightNpcPlayer.EndCurBehavior();
            }
        }
        public override void StartBehavior()
        {
            base.StartBehavior();
            m_FightNpcPlayer.GetAutoPathComp().SetAutoPathEnable(true,7, 6, m_PlayerGo.transform, m_PursueCallBack);
            m_FightNpcPlayer.GetAutoPathComp().SetSpeedScale(1.12f);
            m_FightNpcPlayer.PlayAnim(GameConstVal.Run);
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
