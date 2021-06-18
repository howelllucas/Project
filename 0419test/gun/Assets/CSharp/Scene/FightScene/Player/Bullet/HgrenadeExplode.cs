using UnityEngine;

namespace EZ
{
    public class HgrenadeExplode : BaseBullet
    {
        // 设计 问题。上层 碰撞子弹应该负责 爆炸效果。explode 子弹应该负责被击特效 + 伤害
        [Tooltip("hit effect name create by cache if ExplodeHitEffect = emepty will not create")]
        [SerializeField] string ExplodeHitEffect;
        [Tooltip("hit effect go create by instance live time = 0.5f if ExplodeHitGo = null will not create")]
        [SerializeField] GameObject ExplodeHitGo;

        [Tooltip("爆炸延迟时间")]
        public float DelayExplodeTime = 1;
        [Tooltip("爆炸 最大位置半径")]
        public float MaxRadio = 2;
        [Tooltip("爆炸 最小位置半径")]
        public float MinRadio = 1;
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
                    Destroy(explodeObj, 0.5f);
                    return explodeObj;
                }
                else
                {
                    return null;
                }
            }
        }
        public void AddExplodeEffect(double damage,Vector3 startPos)
        {
            gameObject.SetActive(false);
            m_Damage = damage;
            float radio = Random.Range(MinRadio, MaxRadio);
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            float offsetX = Mathf.Cos(angle) * radio;
            float offsetY = Mathf.Sin(angle) * radio;
            transform.position = startPos + new Vector3(offsetX, offsetY, 0);
            transform.parent.gameObject.AddComponent<DelayCallBack>().SetAction(
                AddExplodeEffectImp,Random.Range(0f,DelayExplodeTime)
                );
        }
        private void AddExplodeEffectImp()
        {
            if (!m_IsInDelayDestroy)
            {
                gameObject.SetActive(true);
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

