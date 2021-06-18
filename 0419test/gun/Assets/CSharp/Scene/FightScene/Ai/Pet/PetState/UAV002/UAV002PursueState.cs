using UnityEngine;

namespace EZ
{
    public class UAV002PursueState : PetPursueState 
    {

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
            }
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
