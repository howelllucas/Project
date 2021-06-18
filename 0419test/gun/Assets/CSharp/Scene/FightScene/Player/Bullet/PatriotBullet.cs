using UnityEngine;
namespace EZ
{
    public class PatriotBullet : BaseBullet
    {

        public override void Init(double damage, Transform firePoint, float radio, float offAngle, float atkRange = 0)
        {
            transform.parent.SetParent(Global.gApp.gBulletNode.transform, false);
            Transform explodeTsf = transform.parent.Find("ExplodeCollision");
            explodeTsf.GetComponent<RocketExplode>().SetDamage(m_Damage);

            InitBulletPose(damage);
            InitBulletEffect(firePoint, offAngle, radio);
        }
        protected void InitBulletEffect(Transform firePoint, float offAngle, float radio)
        {
            if (m_BulletEffect == null)
            {
                m_BulletEffect = Instantiate(BulletEffect);
                m_BulletEffect.transform.SetParent(transform.transform.parent, false);
            }
            var patriotRocketFly = m_BulletEffect.GetComponent<PatriotRocketFly>();
            patriotRocketFly.Init(firePoint, offAngle, radio, m_LiveTime, m_Speed, ReachPlace);
            if (m_LockEnemyObj != null)
            {
                patriotRocketFly.SetLockMonster(m_LockEnemyObj.GetLockEnemy());
            }
        }
        public void ReachPlace()
        {
            OnHitted();
        }
        private void InitBulletPose(double damage)
        {
            m_Damage = damage;
        }
        private void LateUpdate()
        {
            if (!m_IsInDelayDestroy)
            {

                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime < m_LiveTime)
                {
                    Vector3 newPos = m_BulletEffect.transform.position;
                    newPos.z = 0;
                    transform.position = newPos;
                }
                else
                {
                    OnHitted();
                }
            }
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
            m_BulletEffect.SetActive(false);
            Transform explodeTsf = transform.parent.Find("ExplodeCollision");
            explodeTsf.GetComponent<RocketExplode>().SetDamage(m_Damage);
            explodeTsf.position = transform.position;
            explodeTsf.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
