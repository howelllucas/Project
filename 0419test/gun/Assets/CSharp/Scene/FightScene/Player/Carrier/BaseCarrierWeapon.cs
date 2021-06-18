using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ
{
    public class BaseCarrierWeapon : MonoBehaviour
    {
        protected double m_Damage;
        protected Rigidbody2D m_ParentRigidbody2D;
        protected bool m_InNormalPass;
        public virtual void init(float damageCoefficient)
        {
            m_ParentRigidbody2D = transform.parent.GetComponent<Rigidbody2D>();
            m_InNormalPass = Global.gApp.CurScene.IsNormalPass();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                if (m_InNormalPass)
                {
                    Vector2 velocity = m_ParentRigidbody2D.velocity;
                    if (velocity.sqrMagnitude > 0.2f)
                    {
                        collision.gameObject.GetComponent<Monster>().OnHitByCarrier(m_Damage);
                    }
                }
                else
                {
                    collision.gameObject.GetComponent<Monster>().HitDeadth();
                }
            }
        }
    }
}
