using UnityEngine;

namespace EZ
{
    public class DeadthPursueState : PetPursueState
    {
        private float m_StateDtTime = 0.2f;
        private float m_CurStateTime = 0;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
        }
        protected override void Update()
        {
            if (m_EnterState)
            {
                // 走路 跟随的时候隔一段时间 检测一下 攻击
                float dtTime = BaseScene.GetDtTime();
                if (m_CurTime >= AtkCheckTime)
                {
                    m_CurTime = m_CurTime - AtkCheckTime;
                    if (CheckCanEnterOtherState())
                    {
                        return;
                    }
                }
                // 跟随到达目标点 隔一会 在切换idle 放在 宠物 来回切换动作
                if (m_ReachPlace)
                {
                    m_CurTime += dtTime;
                    m_CurStateTime += dtTime;
                    if (m_CurStateTime > m_StateDtTime)
                    {
                        m_CurStateTime = 0;
                        m_Pet.PlayAnim(GameConstVal.Idle);
                    }
                }
                else
                {
                    // 不在目标点 隔一会再切Run 防止宠物 来回切换动作。当前动作是Run 或者Idle 不会再次播放
                    m_CurStateTime += dtTime;
                    if (m_CurStateTime > m_StateDtTime)
                    {
                        m_CurStateTime = 0;
                        m_Pet.PlayAnim(GameConstVal.Run);
                    }
                }
            }
        }
        protected override void PursueCallBack(bool repachplace)
        {
            base.PursueCallBack(repachplace);
            m_CurStateTime = 0;
        }
        public override void StartState()
        {
            base.StartState();
        }

        public override bool CheckState()
        {
            return base.CheckState();
        }

        public override void EndState()
        {
            base.EndState();
        }
    }
}
