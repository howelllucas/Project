using UnityEngine;

namespace EZ
{
    public class MonsterVisibleListener : MonoBehaviour
    {
        Monster m_Monster;
        private void Start()
        {
            m_Monster = GetComponentInParent<Monster>();
        }
        private void OnBecameVisible()
        {
            m_Monster.InCameraView = true;
        }
        private void OnBecameInvisible()
        {
            m_Monster.InCameraView = false;
        }
    }
}
