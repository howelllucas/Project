using UnityEngine;
namespace EZ
{
    public class LaserBullet : BaseBullet
    {

        [SerializeField]private float m_BaseLength = 5.75f;
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime >= m_EffectTime)
                {
                    m_IsInDelayDestroy = true;
                    GetComponent<Collider2D>().enabled = false;
                    Destroy(gameObject, m_ShowTime - m_CurTime);
                }
            }
        }
        public override void Init(double damage,Transform firePoint, float dtAngleZ, float offset,float atkRange)
        {

            InitBulletPose(damage, firePoint, dtAngleZ,offset);
            InitBulletEffect(firePoint);
            Vector3 direction = transform.right;//射线方向
            float distance = m_Length;//射线检测距离
            Vector3 newPosition = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(newPosition, direction, distance, 1 << GameConstVal.StaticMapLayer);//发射射线，只检测与"Target"层的碰撞
            if(hit.collider != null)
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Vector2 hitPoint = hit.point;
                float newDis = Vector2.Distance(new Vector2(newPosition.x, newPosition.y),hitPoint);
                float scaleX = Mathf.Max(0.1f, newDis / m_BaseLength);
                transform.localScale = new Vector3(scaleX, 1, 1);
                if (!hit.collider.gameObject.CompareTag(GameConstVal.MonsterTag))
                {
                    OnHittedWall(new Vector3(hitPoint.x, hitPoint.y, firePoint.position.z));
                }
            }
            else
            {
                transform.localScale = new Vector3(m_Length / m_BaseLength, 1, 1);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy) { return; }
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster, true);
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
