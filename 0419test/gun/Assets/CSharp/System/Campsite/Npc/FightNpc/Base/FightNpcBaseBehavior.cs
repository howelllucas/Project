using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public abstract class FightNpcBaseBehavior : MonoBehaviour
    {
        protected FightNpcPlayer m_FightNpcPlayer;
        protected GameObject m_PlayerGo;
        protected bool m_StartState = false;
        public virtual void Init(FightNpcPlayer npcPlayer,GameObject playerGo)
        {
            m_FightNpcPlayer = npcPlayer;
            m_PlayerGo = playerGo;
        }
        public virtual void StartBehavior()
        {
            m_StartState = true;
        }
        public virtual void EndBehavior()
        {
            m_StartState = false;
        }
    }
}
