using UnityEngine;
namespace EZ
{
    public class AIActiveRange : MonoBehaviour
    {
        [SerializeField]private float m_ActiveRange = 3;
        [SerializeField]private float m_CloseRange = 4;
        private bool m_Active = false;
        private CircleCollider2D m_Collision2d;
        private Monster m_Monster;
        private void Awake()
        {
            m_Collision2d = GetComponent<CircleCollider2D>();
            m_Monster = GetComponentInParent<Monster>();
            m_Collision2d.radius = m_ActiveRange;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!m_Active)
            {
                if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
                {
                    m_Active = true;
                    m_Collision2d.radius = m_CloseRange;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_Active)
            {
                if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
                {
                    m_Active = false;
                    m_Collision2d.radius = m_ActiveRange;
                }
            }
        }
    }
}
