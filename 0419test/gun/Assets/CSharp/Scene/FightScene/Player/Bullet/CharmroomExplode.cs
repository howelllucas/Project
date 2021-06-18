using UnityEngine;

namespace EZ
{
    public class CharmroomExplode : BaseBullet
    {
        // 设计 问题。上层 碰撞子弹应该负责 爆炸效果。explode 子弹应该负责被击特效 + 伤害
        [Tooltip("hit effect name create by cache if ExplodeHitEffect = emepty will not create")]
        [SerializeField] string ExplodeHitEffect;
        [Tooltip("hit effect go create by instance live time = 0.5f if ExplodeHitGo = null will not create")]
        [SerializeField] GameObject ExplodeHitGo;

        public void SetDamage(double damage)
        {
            m_Damage = damage;
        }
        private void Start()
        {
            AddExplodeEffectImp();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Pos(m_Damage,transform,true);

                AddHittedEnemyEffect(monster);
            }
        }
        private void AddHittedEnemyEffect(Monster monster)
        {
            if (monster.CheckCanAddHittedEffect())
            {
                GameObject effect = GetHittedEnemyEffect();
                if (effect != null)
                {
                    effect.transform.SetParent(monster.GetBodyNode(), false);
                    effect.transform.position = monster.GetBodyNode().position;
                }
            }
        }
        private GameObject GetExplodHitEffect()
        {
            if (EffectConfig.EffectPath.ContainsKey(ExplodeHitEffect))
            {
                EffectNode effect = Global.gApp.gGameCtrl.EffectCache.GetEffect(ExplodeHitEffect);
                return effect.gameObject;
            }
            else
            {
                if (ExplodeHitGo != null)
                {
                    GameObject explodeObj = Instantiate(ExplodeHitGo);
                    if(explodeObj.GetComponent<DelayDestroy>() == null)
                    {
                        Destroy(explodeObj, 1f);
                    }
                    return explodeObj;
                }
                else
                {
                    return null;
                }
            }
        }
        private void AddExplodeEffectImp()
        {
            if (!m_IsInDelayDestroy)
            {
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                GameObject explodEffect = GetExplodHitEffect();
                if (explodEffect != null)
                {
                    explodEffect.transform.position = transform.position;
                }
                Destroy(transform.parent.gameObject, 0.5f);
            }
        }
    }
}

