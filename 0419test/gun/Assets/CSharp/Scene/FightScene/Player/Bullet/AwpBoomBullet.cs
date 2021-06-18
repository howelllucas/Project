using UnityEngine;
namespace EZ
{
    public class AwpBoomBullet : BaseBullet
    {

        [SerializeField]private float m_BaseLength = 5.75f;
        [SerializeField]private GameObject m_ExplodeObj = null;

        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime >= m_EffectTime)
                {
                    m_IsInDelayDestroy = true;
                    GetComponent<Collider2D>().enabled = false;
                    Destroy(transform.parent.gameObject, m_ShowTime - m_CurTime);
                }
            }
        }
        public override void Init(double damage,Transform firePoint, float dtAngleZ, float offset,float atkRange)
        {
            InitBulletPose(damage, firePoint, dtAngleZ,offset);
            InitBulletEffect(firePoint);
            transform.localScale = new Vector3(m_Length / m_BaseLength, 1, 1);

            //Transform explodeTsf = transform.parent.Find("ExplodeCollision");
            //RocketExplode rocketExplode = explodeTsf.GetComponent<RocketExplode>();
            //if (rocketExplode != null)
            //{
            //    rocketExplode.SetDamage(m_Damage);
            //}

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy) { return; }
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster, true);

                if (m_ExplodeObj != null && monster.InDeath)
                {
                    var obj = Instantiate(m_ExplodeObj);
                    //Transform explodeTsf = transform.parent.Find("ExplodeCollision");
                    RocketExplode rocketExplode = obj.GetComponent<RocketExplode>();
                    if (rocketExplode != null)
                    {
                        rocketExplode.SetDamage(m_Damage);

                        obj.transform.position = monster.transform.position;
                        obj.transform.gameObject.SetActive(true);
                    }
                }
            }
        }
        private void OnHittedWall(Vector3 position)
        {
            GameObject effect = GetHittedWallEnemyEffect(EffectConfig.HittedEffectLimitCount, true);
            if (effect != null)
            {
                effect.transform.position = position;
            }
        }
    }
}
