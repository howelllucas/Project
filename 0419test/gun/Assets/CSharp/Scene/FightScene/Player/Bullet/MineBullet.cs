using UnityEngine;
namespace EZ
{
    public class MineBullet : BaseBullet
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
            InitBulletPose(damage, firePoint,dtAngleZ,offset);
            InitBulletEffect(firePoint);
            Vector3 newPos = m_BulletEffect.transform.position;
            newPos.z = 0;
            m_BulletEffect.transform.position = newPos;
            transform.eulerAngles = Vector3.zero;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag) && collision.gameObject.layer != GameConstVal.FlyMonsterLayer)
            {
                OnHitted();
            }
        }
        private void OnHitted()
        {
            m_IsInDelayDestroy = true;
            Transform explodeTsf = transform.parent.Find(GameConstVal.ExplodeCollision);
            explodeTsf.GetComponent<RocketExplode>().SetDamage(m_Damage);
            explodeTsf.position = transform.position;
            explodeTsf.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
