using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class BubbleBullet : BaseBullet
    {
        [Tooltip("爆炸伤害系数")]
        public float ExplodeDamageParam = 0.5f;

        [SerializeField] private float EffectTime = 1;
        [SerializeField] private float ReduceSpeed = -1.0f;

        public override void Init(double damage, Transform firePoint, float radio, float offAngle, float atkRange = 0)
        {
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage);
            InitBulletEffect(firePoint, offAngle, radio);
        }
        protected void InitBulletEffect(Transform firePoint, float offAngle, float radio)
        {
            if (m_BulletEffect == null)
            {
                m_BulletEffect = Instantiate(BulletEffect);
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
            DestroyAll();
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
                    if (m_BulletEffect != null)
                    {
                        Vector3 newPos = m_BulletEffect.transform.position;
                        newPos.z = 0;
                        transform.position = newPos;
                    }
                }
                else
                {
                    DestroyAll();
                }
            }
        }
        private void DestroyAll()
        {
            Destroy(m_BulletEffect);
            Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.MapTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                m_IsInDelayDestroy = true;
                AddHittedWallEffect();
                DestroyAll();
            }
            else if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                monster.AddBuff(AiBuffType.MoveSpeed, EffectTime, ReduceSpeed);
                if (!monster.InDeath && monster.CheckCanAddHittedEffect())
                {
                    Transform effectNode = monster.GetBodyNode().Find(GameConstVal.BubbleEffect);
                    BubbleEffectNode bubbleEffectNode = null;
                    if (effectNode != null)
                    {
                        bubbleEffectNode = effectNode.GetComponent<BubbleEffectNode>();
                        bubbleEffectNode.Restart();
                    }
                    else
                    {
                        GameObject effect = GetHittedEnemyEffect();
                        if (effect != null)
                        {
                            bubbleEffectNode = effect.GetComponent<BubbleEffectNode>();
                            effect.name = GameConstVal.BubbleEffect;
                            effect.transform.SetParent(monster.GetBodyNode(), false);
                            effect.transform.localPosition = Vector3.zero; ;
                            effect.transform.localScale = new Vector3(1, 1, 1);
                        }
                    }
                    if (bubbleEffectNode != null)
                        bubbleEffectNode.SetDamage(m_Damage * ExplodeDamageParam);
                }
                m_IsInDelayDestroy = true;
                DestroyAll();
            }
            else if (obj.CompareTag(GameConstVal.ShieldTag))
            {
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                collision.gameObject.GetComponent<AIShieldEvent>().AddHittedEffect(transform.position);
                DestroyAll();
            }
        }
    }
}
