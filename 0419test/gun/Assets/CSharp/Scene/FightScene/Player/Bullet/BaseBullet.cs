using UnityEngine;
namespace EZ
{

    public enum BulletId
    {
        None = 0,
        // 1- 1000 主角
        weapon_ak47 = 1,
        weapon_ak47_s = 2,
        weapon_mg = 3,
        weapon_mg_s = 4,
        weapon_laser = 5,
        weapon_laser_s = 6,

        weapon_scatter = 7,
        weapon_scatter_s = 8,

        weapon_firegun = 9,
        weapon_firegun_s = 10,

        weapon_rpg = 11,
        weapon_rpg_s = 12,//rpg 未使用 超级武器 无子弹变化

        weapon_laserX = 13,
        weapon_laserX_s = 14,

        weapon_awp = 15,
        weapon_awp_s = 16,

        weapon_elecgun = 17,
        weapon_elecgun_s = 18,

        weapon_shotgun = 19,
        weapon_shotgun_s = 20,

        weapon_bouncegun = 21,
        weapon_bouncegun_s = 22,//bounce 未使用 超级武器 无子弹变化

        weapon_crossbow = 23,
        weapon_crossbow_s = 24,

        weapon_firecrossbow = 25,
        weapon_firecrossbow_s = 26,

        weapon_icecrossbow = 27,
        weapon_icecrossbow_s = 28,

        weapon_elecrossbow = 29,
        weapon_elecrossbow_s = 30,

        weapon_patriot = 31,
        weapon_patriot_s = 32,//patriot 未使用 超级武器 无子弹变化

        weapon_tornado = 33,
        weapon_tornado_s = 34,

        weapon_boomcrossbow = 35,
        weapon_boomcrossbow_s = 36,

        weapon_bubble = 37,
        weapon_bubble_s = 38,

        // 1001 -2000 副武器
        // 2001 - 3000 宠物
        // 3001 - 4000 机关
        // 4001 -5000 炮塔
        // 5001 -5000 怪物
        Al2014Bullet = 5001
    }
    public abstract class BaseBullet : MonoBehaviour
    {
        public static int MainBulletStartIndex = 1;
        public static int MainBulletEndIndex = 1000;
        [Tooltip("击退效果的起始速度")]
        [SerializeField]private float BackStartSpeed = 1;
        [Tooltip("击退效果的结束速度")]
        [SerializeField]private float BackEndSpeed = 0.5f;
        [Tooltip("击退效果时间")]
        [SerializeField]private float BackTime = 0.2f;
        [Tooltip("子弹类型")]
        [SerializeField]protected BulletId m_BulletId = BulletId.None;

        [Tooltip("敌人被击特效 从cache 里面读取")]
        [SerializeField] protected string HittedEffectName;
        [Tooltip("打到墙被击特效 从cache 里面读取")]
        [SerializeField] protected string HittedWallEffectName;
        [Tooltip("子弹显示特效")]
        [SerializeField] protected GameObject BulletEffect;
        [Tooltip("敌人被击特效go 如果配置 请设置 delayDestroy 优先读取 HittedEffectName 或者 HittedWallEffectName")]
        [SerializeField] protected GameObject HittedEnemyEffect;
        [Tooltip("被击音效")]
        [SerializeField] protected AudioClip HittedEnemyClip;

        [HideInInspector] protected GameObject m_BulletEffect;
        protected ParticleSystem m_BulletEffectParticleSystem;
        [SerializeField] protected float m_LiveTime = 1.0f;
        [SerializeField] protected float m_EffectTime = 0.08f;
        [SerializeField] protected float m_ShowTime = 0.2f;
        [SerializeField] protected float m_Length = 10;
        [SerializeField] protected float m_Speed = 0;
        [Tooltip("伤害加成比例")]
        [SerializeField] protected float DamageCoefficient = 1;
        protected string m_Name = GameConstVal.EmepyStr;
        protected double m_Damage = 0;
        protected float m_CurTime = 0;
        protected bool m_IsInDelayDestroy = false;
        protected Collider2D m_Collider2D;
        protected Rigidbody2D m_Rigidbody2D;
        protected IFightLockEnemyObj m_LockEnemyObj;
        public virtual void Init(double damage, Transform firePoint, float dtAngleZ, float offset,float atkRange = 0)
        {
            m_IsInDelayDestroy = false;
            enabled = true;
            m_CurTime = 0;
            if (m_Collider2D == null)
            {
                m_Collider2D = GetComponent<Collider2D>();
                m_Rigidbody2D = GetComponent<Rigidbody2D>();
            }
            if(m_Collider2D != null)
            {
                m_Collider2D.enabled = true;
                m_Rigidbody2D.simulated = true;
            }
        }

        public void SetLockEnemyObj(IFightLockEnemyObj obj)
        {
            m_LockEnemyObj = obj;
        }

        protected void InitBulletPose(double damage, Transform firePoint, float dtAngleZ, float offset)
        {
            m_Damage = damage * DamageCoefficient;
            Vector3 rotation = firePoint.eulerAngles;
            rotation.z = rotation.z + dtAngleZ;
            transform.eulerAngles = rotation;
            Vector3 position = transform.up * offset + firePoint.position;
            transform.position = new Vector3(position.x, position.y, 0);
        }
        protected void InitBulletEffect(Transform firePoint)
        {
            if (BulletEffect != null)
            {
                if (m_BulletEffect == null)
                {
                    m_BulletEffect = Instantiate(BulletEffect);
                    m_BulletEffect.transform.SetParent(transform, false);
                    if (m_BulletEffect.transform.Find(GameConstVal.ParticleName))
                    {
                        m_BulletEffectParticleSystem = m_BulletEffect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>();
                    }

                }
                Vector3 pos = transform.position;
                pos.z = firePoint.position.z;
                m_BulletEffect.transform.position = pos;
                if(m_BulletEffectParticleSystem != null)
                {
                    m_BulletEffectParticleSystem.Play();
                }
            }
        }

        public float GetBackStartSpeed()
        {
            return BackStartSpeed;
        }
        public float GetBackEndSpeed()
        {
            return BackEndSpeed;
        }
        public float GetBackTime()
        {
            return BackTime;
        }
        public BulletId GetBulletId()
        {
            return m_BulletId;
        }
        protected void AddHittedWallEffect()
        {
            GameObject effect = GetHittedWallEnemyEffect(EffectConfig.HittedEffectLimitCount,true);
            if (effect != null)
            {
                if (m_BulletEffect != null)
                {
                    effect.transform.position = m_BulletEffect.transform.position;
                }
                else
                {
                    effect.transform.position = transform.position;
                }
            }
        }
        protected void AddHittedEffect(Monster monster,bool forceToEmeny = false,bool forceToBullet = false)
        {
            if (forceToBullet)
            {
                GameObject effect = GetHittedEnemyEffect();
                if (effect != null)
                {
                    if (m_BulletEffect != null)
                    {
                        effect.transform.position = m_BulletEffect.transform.position;
                    }
                    else
                    {
                        effect.transform.position = transform.position;
                    }
                }
            }
            else if (monster.CheckCanAddHittedEffect())
            {
                GameObject effect = GetHittedEnemyEffect();
                if (effect != null)
                {
                    if (m_BulletEffect != null && !forceToEmeny)
                    {
                        effect.transform.position = m_BulletEffect.transform.position;
                    }
                    else if (monster != null)
                    {
                        effect.transform.position = monster.GetBodyNode().position;
                    }
                    else
                    {
                        effect.transform.position = transform.position;
                    }
                }
            }
        }
        protected virtual GameObject GetHittedWallEnemyEffect(int limitCount,bool limtReturnNull)
        {
            if (EffectConfig.EffectPath.ContainsKey(HittedWallEffectName))
            {
                EffectNode effect = Global.gApp.gGameCtrl.EffectCache.GetEffect(HittedWallEffectName, limitCount,limtReturnNull);
                if (effect != null)
                {
                    return effect.gameObject;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (HittedEnemyEffect != null)
                {
                    //return Instantiate(HittedEnemyEffect);
                    EffectNode effect = Global.gApp.gGameCtrl.EffectCache.GetEffect(HittedEnemyEffect, limitCount, limtReturnNull);
                    if (effect == null)
                        return null;
                    return effect.gameObject;
                }
                else
                {
                    return null;
                }
            }
        }
        protected virtual GameObject GetHittedEnemyEffect()
        {
            if(EffectConfig.EffectPath.ContainsKey(HittedEffectName))
            {
                EffectNode effect = Global.gApp.gGameCtrl.EffectCache.GetEffect(HittedEffectName);
                return effect.gameObject;
            }
            else
            {
                if (HittedEnemyEffect != null)
                {
                    //return Instantiate(HittedEnemyEffect);
                    EffectNode effect = Global.gApp.gGameCtrl.EffectCache.GetEffect(HittedEnemyEffect);
                    if (effect != null)
                        return effect.gameObject;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
        }
        public virtual void Recycle()
        {
            enabled = false;
            if (m_Collider2D != null)
            {
                m_Collider2D.enabled = false;
                m_Rigidbody2D.simulated = false;
            }
            if (m_BulletEffectParticleSystem != null)
            {
                m_BulletEffectParticleSystem.Stop();
            }
            Global.gApp.gGameCtrl.BulletCache.Recycle(m_Name, this);
        }
        public virtual void InitForCache()
        {
            enabled = false;
            if (m_Collider2D == null)
            {
                m_Collider2D = GetComponent<Collider2D>();
                m_Rigidbody2D = GetComponent<Rigidbody2D>();
            }
            if (m_Collider2D != null)
            {
                m_Collider2D.enabled = false;
                m_Rigidbody2D.simulated = false;
            }
        }
        public void SetName(string name)
        {
            m_Name = name;
        }
        public void SetLiveTime(float liveTime)
        {
            m_LiveTime = liveTime;
        }
    }
}
