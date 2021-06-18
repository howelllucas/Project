using UnityEngine;

namespace EZ
{
    public class DogPursueState : PetPursueState 
    {
        private float m_StateDtTime = 0.1f;
        private float m_CurStateTime = 0;
        public override void Init(GameObject playerGo,BasePet pet)
        {
            base.Init(playerGo,pet);
        }
        protected override void Update()
        {
            if (m_EnterState)
            {
                float dtTime = BaseScene.GetDtTime();
                m_CurTime += dtTime;
                if (m_CurTime >= AtkCheckTime)
                {
                    m_CurTime = m_CurTime - AtkCheckTime;
                    if (CheckCanEnterOtherState())
                    {
                        return;
                    }
                }
                if (m_ReachPlace)
                {
                    m_CurStateTime += dtTime;
                    if (m_CurStateTime > m_StateDtTime)
                    {
                        m_CurStateTime = 0;
                        m_Pet.PlayAnim(GameConstVal.Idle);
                    }
                }
                else
                {
                    m_CurStateTime += dtTime;
                    if (m_CurStateTime > m_StateDtTime)
                    {
                        m_CurStateTime = 0;
                        m_Pet.PlayAnim(GameConstVal.Run);
                    }
                }
            }
        }
        protected override void PursueCallBack(bool reachPlace)
        {
            base.PursueCallBack(reachPlace);
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
