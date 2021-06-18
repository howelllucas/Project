using UnityEngine;
namespace EZ
{

    public class AIShieldEvent : MonoBehaviour
    {

        public GameObject m_EffectNode;
        public ParticleSystem m_Particle;
        public void AddHittedEffect(Vector3 pos)
        {
            m_Particle.Play();
        }
    }
}
