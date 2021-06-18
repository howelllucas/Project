using UnityEngine;
namespace EZ
{
    public class RocketBullet : BaseBullet
    {
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {

                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime < m_LiveTime)
                {
                    transform.Translate(Vector3.right * m_Speed * BaseScene.GetDtTime(), Space.Self);
                }
                else
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset,float atkRange = 0)
        {
            transform.parent.SetParent(Global.gApp.gBulletNode.transform, false);
            Transform explodeTsf = transform.parent.Find("ExplodeCollision");
            //explodeTsf.GetComponent<RocketExplode>().SetDamage(m_Damage);
            RocketExplode rocketExplode = explodeTsf.GetComponent<RocketExplode>();
            if (rocketExplode != null)
            {
                rocketExplode.SetDamage(m_Damage);
            }
            else
            {
                explodeTsf.GetComponent<RocketExplodeFireBuff>().SetDamage(m_Damage);
            }

            InitBulletPose(damage,firePoint,dtAngleZ,offset);
            InitBulletEffect(firePoint);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }

            if (collision.gameObject.CompareTag(GameConstVal.MapTag))
            {
                OnHitted();
            }
            else if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                OnHitted();
            }
        }
        private void OnHitted()
        {
            m_IsInDelayDestroy = true;
            Transform explodeTsf = transform.parent.Find("ExplodeCollision");
            //explodeTsf.GetComponent<RocketExplode>().SetDamage(m_Damage);
            RocketExplode rocketExplode = explodeTsf.GetComponent<RocketExplode>();
            if (rocketExplode != null)
            {
                rocketExplode.SetDamage(m_Damage);
            }
            else
            {
                explodeTsf.GetComponent<RocketExplodeFireBuff>().SetDamage(m_Damage);
            }

            explodeTsf.position = transform.position;
            explodeTsf.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            
        }
    }
}
