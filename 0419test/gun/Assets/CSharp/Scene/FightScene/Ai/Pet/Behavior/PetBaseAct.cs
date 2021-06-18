using UnityEngine;

namespace EZ
{
    public abstract class PetBaseAct : MonoBehaviour
    {
        [Tooltip(" 碰撞框 之间的 最小距离")]
        public float MinOffset = 0.05f;
        [Tooltip(" 碰撞框 之间的 最大距离")]
        public float MaxOffset = 0.15f;
        protected double m_Damage;
        protected bool m_InBreakState;
        protected float m_ = 0.3f;
        protected float m_CurTime;
        protected int m_AtkTimes;
        protected float m_MonsterRadioSqr = 1;
        protected Transform m_LockMonsterTsf;
        protected Monster m_LockMonster;
        protected BasePet m_Pet;
        protected PetAtkState m_Controller;

        public abstract void MUpdate();
        public abstract void StartSkill();
        public abstract void EndSkill();
        public abstract void EndEffect();
        public abstract bool CanTriggerSkill();

        public virtual void Init(BasePet pet, PetAtkState controller)
        {
            m_Pet = pet;
            m_Controller = controller;
        }
    
        public virtual void SetLockMonster(Transform monsterTsf,Monster monster,float monsterRadio)
        {
            if (monsterTsf != null)
            {
                m_LockMonsterTsf = monsterTsf;
                m_LockMonster = monster;
            }
            else
            {
                m_LockMonsterTsf = null;
                m_LockMonster = null;
            }
            m_MonsterRadioSqr = Mathf.Pow(monsterRadio + m_Pet.CircleRadio,2);
        }
    }
}
