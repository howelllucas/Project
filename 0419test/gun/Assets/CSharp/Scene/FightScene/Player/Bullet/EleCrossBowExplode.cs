using UnityEngine;

namespace EZ
{
    public class EleCrossBowExplode : BaseBullet
    {
        [SerializeField] private float SpeedEffectTime = 1;
        [SerializeField] private float SpeedEffectVal = -0.5f;

        [SerializeField] float FireBuffTime = 0;
        [SerializeField] double FireBuffDamageParam = 0;
        [Range(1, 99999)]
        [SerializeField] int FireDamageTimes = 1;
        [SerializeField] private GameObject LinkBullet;
        public void SetDamage(double damage)
        {
            m_Damage = damage;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Pos(m_Damage,transform);
                monster.AddBuff(AiBuffType.MoveSpeed, SpeedEffectTime,SpeedEffectVal);
                monster.AddBuff(AiBuffType.FireBuff, FireBuffTime, FireBuffDamageParam * m_Damage, FireBuffTime / FireDamageTimes);
                if (LinkBullet != null)
                {
                    GameObject linkBullet = Instantiate(LinkBullet);
                    linkBullet.GetComponent<ElecPopBullet>().Init(monster.gameObject, m_Damage);
                }
                AddHittedEnemyEffect(monster);
                AddExplodeEffect(monster);

            }
            else if(collision.gameObject.CompareTag(GameConstVal.MapTag))
            {
                AddExplodeEffect(null);
            }
        }
        private void AddHittedEnemyEffect(Monster monster)
        {
            if (monster.CheckCanAddHittedEffect())
            {
                EffectNode effect = Global.gApp.gGameCtrl.EffectCache.GetEffect(EffectConfig.Eleccrossbow_Hit);
                effect.transform.SetParent(monster.GetBodyNode(), false);
                effect.transform.position = monster.GetBodyNode().position;
            }
        }
        private void AddExplodeEffect(Monster monster)
        {
            if (!m_IsInDelayDestroy)
            {
                m_IsInDelayDestroy = true;

                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                if (monster)
                {
                    AddHittedEffect(monster,false,true);
                }
                else
                {
                    AddHittedWallEffect();
                }
                Destroy(transform.parent.gameObject, 0.5f);
            }
        }
        private void OnDestroy()
        {

        }
    }
}

