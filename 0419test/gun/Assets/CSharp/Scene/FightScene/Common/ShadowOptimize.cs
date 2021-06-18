using UnityEngine;

namespace EZ
{
    public class ShadowOptimize : MonoBehaviour
    {
        [SerializeField] private GameObject m_SkineNode;
        Monster m_Monster;
        private void Start()
        {
            m_Monster = GetComponentInParent<Monster>();
            if (!m_Monster.InCameraView)
            {
                m_SkineNode.layer = 0;
                m_Monster.InShadowView = false;
            }
        }
        private void OnBecameVisible()
        {
            m_SkineNode.layer = GameConstVal.MonsterLayer;
            m_Monster.InShadowView = true;
        }
        private void OnBecameInvisible()
        {
            m_SkineNode.layer = 0;
            m_Monster.InShadowView = false;
        }
    }
}
