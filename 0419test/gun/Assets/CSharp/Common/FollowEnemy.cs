using UnityEngine;
namespace EZ
{
    public class FollowEnemy : MonoBehaviour
    {
        private Transform m_TargetTsf;
        private void LateUpdate()
        {
            if (m_TargetTsf != null)
            {
                transform.position = m_TargetTsf.position;
                transform.localScale = m_TargetTsf.localScale;
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
        }
        public void SetTargetTsf(Transform tsf)
        {
            m_TargetTsf = tsf; 
            if(m_TargetTsf!= null)
            {
                transform.localScale = Vector3.zero;
            }
        }
    }
}
