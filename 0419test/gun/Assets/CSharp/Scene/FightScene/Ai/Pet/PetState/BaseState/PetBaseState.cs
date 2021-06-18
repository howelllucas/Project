using UnityEngine;

namespace EZ
{
    public abstract class PetBaseState : MonoBehaviour
    {

        protected float m_CurTime;
        protected bool m_EnterState;
        protected BasePet m_Pet;
        protected GameObject m_PlayerGo;
        public abstract bool CheckState();
        public abstract void StartState();
        public abstract void EndState();
        public virtual void Init(GameObject playerGo, BasePet pet)
        {
            m_PlayerGo = playerGo;
            m_Pet = pet;
        }
        public virtual bool CheckCanEnterOtherState()
        {
            return false;
        }
    }
}
