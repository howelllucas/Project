using UnityEngine;
namespace EZ
{
    public abstract class AiBase : MonoBehaviour
    {
        protected GameObject m_Player;
        protected Monster m_Monster;
        private Wave m_Wave;
        protected float m_CurTime = 0;
        protected bool m_StartAct = false;
        protected void SetPlayer(GameObject player)
        {
            m_Player = player;
        }
        public virtual void Init(GameObject player, Wave wave, Monster monster)
        {
            m_Monster = monster;
            m_Wave = wave;
            m_CurTime = 0;
            m_StartAct = false;
            SetPlayer(player);
        }
        protected float GetActDtTime()
        {
            if (m_Monster.InCameraView)
            {
                return BaseScene.GetDtTime();
            }
            else
            {
                return 0;
            }
        }

        public virtual void Death()
        {
            m_CurTime = 0;
            m_StartAct = false;
            enabled = false;
            if (m_Monster)
            {
                m_Monster.SetSpeed(Vector2.zero);
            }
        }
    }
}
