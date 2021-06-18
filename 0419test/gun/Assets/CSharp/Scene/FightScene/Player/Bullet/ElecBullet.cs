using UnityEngine;
namespace EZ
{
    public class ElecBullet : BaseBullet
    {
        [SerializeField] private float SpeedBuffTime = 1;
        [SerializeField] private float SpeedBuffVal = -0.5f;
        [SerializeField] private GameObject LinkBullet;
        private float m_BaseLength = 5.75f;
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime >= m_EffectTime)
            {
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject);
            }
        }
        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange)
        {
            InitBulletPose(damage, firePoint, dtAngleZ, offset);

            Vector3 direction = transform.right;//射线方向
            float distance = m_Length;//射线检测距离
            Vector3 newPosition = transform.position;

            int mask = GameConstVal.LasserMask;
            RaycastHit2D hit = Physics2D.Raycast(newPosition, direction, distance, mask);//发射射线，只检测与"Target"层的碰撞
            if (hit.collider != null)
            {
                if (m_Damage > 0)
                {
                    Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                }
                if (hit.collider.gameObject.CompareTag(GameConstVal.MonsterTag))
                {
                    Vector2 hitPoint = hit.collider.gameObject.transform.position;
                    float newDis = Vector2.Distance(new Vector2(newPosition.x, newPosition.y), hitPoint);
                    float scaleX = Mathf.Max(0.1f, newDis / m_BaseLength);
                    transform.localScale = new Vector3(scaleX, 1, 1);
                }
                else
                {
                    Vector2 hitPoint = hit.point;
                    float newDis = Vector2.Distance(new Vector2(newPosition.x, newPosition.y), hitPoint);
                    float scaleX = Mathf.Max(0.1f, newDis / m_BaseLength);
                    transform.localScale = new Vector3(scaleX, 1, 1);
                }
            }
            else
            {
                transform.localScale = new Vector3(0, 1, 1);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy) { return; }
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                if (m_Damage > 0.1d)
                {
                    OnHitted(collision.transform, monster);
                    Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                    monster.OnHit_Vec(m_Damage, transform);
                    monster.AddBuff(AiBuffType.MoveSpeed, SpeedBuffTime, SpeedBuffVal);
                }
                if (monster.CheckCanAddHittedEffect())
                {
                    GameObject effect = GetHittedEnemyEffect();
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.SetParent(monster.GetBodyNode(), false);
                }
                m_IsInDelayDestroy = true;
            }
        }
        private void OnHitted(Vector3 position)
        {
            GameObject effect = Instantiate(HittedEnemyEffect);
            effect.transform.position = position;
            Destroy(effect, 0.2f);
        }
        private void OnHitted(Transform monsterTsf, Monster monster)
        {
            var m_WaveMgr = Global.gApp.CurScene.GetWaveMgr();
            if (m_WaveMgr != null)
            {
                GameObject linkBullet = Instantiate(LinkBullet);
                linkBullet.GetComponent<ElecPopBullet>().Init(monster.gameObject, m_Damage);
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
