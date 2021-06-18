using UnityEngine;
using UnityEngine.Serialization;

namespace EZ
{
    public class AwpBullet : BaseBullet
    {
        [Tooltip("子弹宽度缩放")]
        [SerializeField]private float Width = 1.0f;
        [Tooltip("减速时间")]
        [FormerlySerializedAs("EffectTime")]
        [SerializeField] private float ReduceSpeedTime = 0.0f;
        [Tooltip("减速比例")]
        [SerializeField] private float ReduceSpeed = 0.0f;
        [Tooltip("击退几率")]
        [SerializeField] private float BackRate = 1.0f;
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
            transform.localScale = new Vector3(1, Width, 1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy) { return; }
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);

                if (Random.Range(0.0f, 1.0f) < BackRate)
                    monster.OnHit_Vec(m_Damage, transform);
                else
                    monster.OnHit_Simple(m_Damage, transform);

                AddHittedEffect(monster, true);

                if (ReduceSpeedTime > 0.0f)
                    monster.AddBuff(AiBuffType.MoveSpeed, ReduceSpeedTime, ReduceSpeed);
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
