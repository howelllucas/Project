using UnityEngine;
namespace EZ
{
    public abstract class PetFlashToFollowState : PetBaseState
    {
        [SerializeField]protected float m_InvisibleThreadHoldTime = 1;
        protected float m_InvisibleTime;
        protected Transform m_LockNode;
        protected BornNode m_LockCompt;
        public override void Init(GameObject playerGo,BasePet pet)
        {
            base.Init(playerGo, pet);
            m_LockNode = playerGo.transform.Find("ModelNode/PetNode");
            m_LockCompt = m_LockNode.GetComponent<BornNode>();
        }

        public virtual void Update()
        {
           if(!m_Pet.InCameraView)
            {
                m_InvisibleTime += BaseScene.GetDtTime();
            }
           else
            {
                m_InvisibleTime = 0;
            }
        }
        public override void StartState()
        {
            transform.position = m_LockNode.position;
            m_InvisibleTime = 0;
            m_Pet.ChangeToPursueState();
        }
        public override bool CheckState()
        {
            if (m_Pet.InCameraView || !m_LockCompt.GetIsOutMap() || m_InvisibleTime < m_InvisibleThreadHoldTime)
            {
                return false;
            }
            return false;
            m_Pet.ChangeToFlashState();
            return true;
        }
        public override void EndState()
        {
        }
    }
}

