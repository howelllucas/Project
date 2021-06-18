using UnityEngine;
namespace EZ
{
    public class AIHook : MonoBehaviour
    {
        public Transform m_TargetTsf;
        private AIThrowHookAct m_ThrowHookAct;
        private void Start()
        {
            m_ThrowHookAct = GetComponentInParent<AIThrowHookAct>();
        }
        private void LateUpdate()
        {
            if (m_TargetTsf != null)
            {
                transform.position = m_TargetTsf.position;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MapTag))
            {
                m_ThrowHookAct.StartBackAct(false);
            }
            else if(collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                m_ThrowHookAct.StartBackAct(true);
            }
        }
    }
}
