using EZ.Data;
using Game;
using System.Collections.Generic;
using UnityEngine;

namespace EZ.Weapon
{
    public abstract class Gun : MonoBehaviour
    {
        protected int m_Quality = -1;
        protected float m_DtTime = 0.1f;
        [Tooltip("子弹 并排数量")]
        [SerializeField] protected int m_BaseCount = 1;
        [Tooltip("子弹 扇形数量")]
        [SerializeField] protected int m_BaseCurve = 1;
        [Tooltip("子弹 读取 缓存")]
        [SerializeField] protected string BulletName;
        [Tooltip("开火特效 可以不配")]
        [SerializeField] protected GameObject FireEffect;
        [Tooltip("子弹Go 优先读取 BulletName")]
        [SerializeField] protected GameObject Bullet;
        [Tooltip("开火音效 ")]
        [SerializeField] protected AudioClip FireClip;
        [SerializeField] protected AudioClip ReloadClip;

        [Tooltip("开火 特效 一般用于激光 子弹链式常在且根据子弹进行缩放 ")]
        [SerializeField] protected GameObject FireBulletEffect;

        [Tooltip("攻击范围 ")]
        [SerializeField] protected float AtkRange = 5;

        [Tooltip("弹道偏移角度 ")]
        [SerializeField] protected float m_DtAngle = 10;
        [Tooltip("并排 子弹 便宜 距离 ")]
        [SerializeField] protected float m_Offset = 0.15f;

        [Tooltip("额外伤害系数")]
        [SerializeField] private float DamageCoefficient = 1;

        protected Transform FirePoint;
        protected FightWeaponMgr m_WeaponMgr;

        [Tooltip("开火特效 实例 ，在随着枪械 创建删除")]
        [HideInInspector] protected GameObject m_FireEffect;
        protected float m_CurTime = 0;
        protected double m_Damage = 10;
        protected Player m_Player;
        protected IFightLockEnemyObj m_LockEnemyObj;
        protected float m_Scale = 1;
        Transform m_NewFirePoint;
        bool m_SynFirePoint;
        protected List<GameObject> m_FireBullets = new List<GameObject>();
        [Tooltip("枪械 curTime 增长 最大值")]
        [SerializeField] private float FirstShotDtOffset = 0.4f;
        private DelayCallBack m_DelayCallBack;
        protected virtual void Awake()
        {
            m_DelayCallBack = gameObject.AddComponent<DelayCallBack>();
            m_DelayCallBack.SetAction(ChargeTime, 0.033f);
            m_DelayCallBack.SetCallTimes(999999999);
        }
        private void ChargeTime()
        {
            m_CurTime += 0.033f;
        }
        protected virtual void InitBaseInfo(FightWeaponMgr mgr)
        {
            m_SynFirePoint = false;
            m_WeaponMgr = mgr;
            m_NewFirePoint = transform.Find("firePoint");
            FirePoint = transform.parent.parent.Find(GameConstVal.FirePoint);
            if (FirePoint == null)
            {
                FirePoint = new GameObject(GameConstVal.FirePoint).transform;
                FirePoint.SetParent(transform.parent.parent);
                if (m_NewFirePoint != null)
                {
                    FirePoint.transform.position = m_NewFirePoint.position;
                    Vector3 newRotate = m_NewFirePoint.right;
                    newRotate.z = 0;
                    FirePoint.transform.right = newRotate;
                }
            }
            m_Player = GetComponentInParent<Player>();
            m_LockEnemyObj = GetComponentInParent<IFightLockEnemyObj>();
            if (FirePoint != null && m_FireEffect != null)
            {
                m_FireEffect.transform.SetParent(FirePoint, false);
                m_FireEffect.SetActive(true);
            }

        }

        // 开枪的时候必须回调 
        protected void MustCallFire()
        {
            InitFirePos();
            PlayFireAnim();
            AddExternBullet();
        }
        private void AddExternBullet()
        {
            if (m_Player != null)
            {
                BaseBullet baseBullet = Global.gApp.gGameCtrl.BulletCache.GetBullet(BulletConfig.Bullet_Extern);
                baseBullet.SetLockEnemyObj(m_LockEnemyObj);
                baseBullet.Init(m_Damage, null, 0, 0);
            }
        }
        protected void InitFirePos()
        {
            //20210218 Shuang 修改
            if (!m_SynFirePoint)
            //if (m_Player != null && !m_SynFirePoint)
            {
                //m_SynFirePoint = true;
                if (m_NewFirePoint != null)
                {
                    FirePoint.transform.position = m_NewFirePoint.position;
                    Vector3 newRotate = m_NewFirePoint.right;
                    newRotate.z = 0;
                    FirePoint.transform.right = newRotate;
                }
                else
                {
                    if (m_Player != null)
                        m_Player.GetFight().ResetFirepointPos();
                }
            }

        }
        public virtual void Init(FightWeaponMgr mgr)
        {
            InitBaseInfo(mgr);
            if (mgr != null)
            {
                SetDtTime(0);
                SetDamage(0);
            }
            if (m_Player != null)
            {
                m_Player.GetFight().SetRadius(AtkRange);
                m_Scale = 1 / m_Player.transform.lossyScale.x;
            }
        }

        public virtual void InitByCardData(GunCard_TableItem cardData)
        {
            InitBaseInfo(null);
            if (cardData != null)
            {
                m_DtTime = 1 / cardData.atkSpeed;
                m_Damage = cardData.atk;
            }
            if (m_Player != null)
            {
                m_Player.GetFight().SetRadius(AtkRange);
                m_Scale = 1 / m_Player.transform.lossyScale.x;
            }
        }

        public virtual void SetDamageCoefficient(float damageCoefficient)
        {
            m_Damage *= damageCoefficient;
        }
        // Update is called once per frame
        protected virtual void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            float newDtTime = GetDtTime();
            if (m_CurTime >= newDtTime)
            {
                m_CurTime = m_CurTime - newDtTime;
                Fire();
            }
        }

        protected virtual int GetBulletCount()
        {
            if (m_Player != null)
            {
                int bulletCount = (int)(m_BaseCount + m_Player.GetBuffMgr().GetIncFireCountTimes());
                return bulletCount;
            }
            else
            {
                return m_BaseCount;
            }
        }
        protected virtual int GetBulletCurves()
        {
            if (m_Player != null)
            {
                int bulletCurve = (int)(m_BaseCurve + m_Player.GetBuffMgr().GetIncFireCurveInc());
                return bulletCurve;
            }
            else
            {
                return m_BaseCurve;
            }
        }

        protected void SetDtTime(float atkSpeed)
        {
            m_DtTime = atkSpeed;
            if (m_WeaponMgr != null)
            {
                m_DtTime = m_WeaponMgr.GunCardData.GetAtkSpeed();

                Debug.Log("m_DtTime " + m_DtTime);
                float speed = 1.0f + 0.5f * ( m_WeaponMgr.GetCombatAttrValue(Game.CombatAttr.CombatAttrType.Attack_Speed) /
                                                ( m_WeaponMgr.GetCombatAttrValue(Game.CombatAttr.CombatAttrType.Attack_Speed) +
                                                    10.0f * Game.PlayerDataMgr.singleton.GetPlayerLevel() + 1.0f ) );

                Debug.Log("speed " + speed);
                m_DtTime *= speed;

                m_DtTime = 1.0f / m_DtTime;

                Debug.Log("m_DtTime " + m_DtTime);
            }
        }
        protected virtual float GetDtTime()
        {
            //if (m_Player != null)
            //{
            //    float newDtTime = m_DtTime / (1 + m_Player.GetBuffMgr().GetIncFireSpeed());
            //    return Mathf.Max(newDtTime, 0.034f);
            //}
            //else
            //{
            return m_DtTime;
            //}
        }
        protected void SetDamage(double damage)
        {
            //int roleLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            ////LevelData levelData = Global.gApp.gGameData.GetLevelData().Find(l => l.level == roleLevel);
            //Hero_dataItem roleLevelData = Global.gApp.gGameData.HeroDataConfig.Get(roleLevel);

            //float atkParam = roleLevelData.atkParams;

            //int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExAtk);
            //Skill_dataItem skillLevelData = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
            //float skillParam = (skillLevelData == null) ? 1f : skillLevelData.skill_exatk[0];
            m_Damage = damage;
            if (m_WeaponMgr != null)
            {
                //m_Damage = m_WeaponMgr.GunCardData.GetAtk();
                m_Damage = PlayerDataMgr.singleton.GetUseWeaponAtk();
                m_Damage += m_WeaponMgr.GetCombatAttrValue(Game.CombatAttr.CombatAttrType.Attack);
                m_Damage *= (1.0f + m_WeaponMgr.GetCombatAttrValue(Game.CombatAttr.CombatAttrType.Attack_Add));

                Debug.Log("m_Damage " + m_Damage);
            }
        }
        protected virtual double GetDamage()
        {
            //if (m_Player != null)
            //{
            //    float damageInc = m_Player.GetBuffMgr().GetIncFireAtk();
            //    return m_Damage * (1 + damageInc);
            //}
            //else
            //{
                return m_Damage;
            //}
        }
        protected void PlayFireAnim()
        {
            if (m_Player != null)
            {
                m_Player.GetFight().PlayShotAnim(GameConstVal.Shoot);
            }
        }
        protected virtual void Fire()
        {
            MustCallFire();
            Global.gApp.gAudioSource.PlayOneShot(FireClip);
        }
        protected void FreshOffsetTime(bool enable)
        {
            if (enable)
            {
                m_DelayCallBack.enabled = false;
                m_CurTime = Mathf.Min(FirstShotDtOffset, m_CurTime);
                m_CurTime = Mathf.Min(m_CurTime, m_DtTime);
            }
            else
            {
                m_DelayCallBack.enabled = true;
            }
        }
        // when gun enable fresh offset time 并且 创新创建 开火特效等
        protected virtual void OnEnable()
        {
            FreshOffsetTime(true);
            if (FireEffect != null && m_FireEffect == null)
            {
                m_FireEffect = Instantiate(FireEffect);
                if (FirePoint != null && m_FireEffect != null)
                {
                    m_FireEffect.transform.SetParent(FirePoint, false);
                }
                else
                {
                    m_FireEffect.gameObject.SetActive(false);
                }
            }
        }
        protected virtual void OnDisable()
        {
            FreshOffsetTime(false);
            if (m_FireEffect != null)
            {
                Destroy(m_FireEffect);
                m_FireEffect = null;
            }
            foreach (GameObject go in m_FireBullets)
            {
                Destroy(go);
            }
            m_FireBullets.Clear();
        }
        public virtual void ChangeGun(bool enable)
        {
            if (!enable && m_FireEffect != null)
            {
                Destroy(m_FireEffect);
                m_FireEffect = null;
            }
        }
        protected void InstanceFireBulletEffect()
        {
            int bulletCount = GetBulletCount();
            int bulletCurves = GetBulletCurves();
            if (bulletCount * bulletCurves != m_FireBullets.Count)
            {
                foreach (GameObject go in m_FireBullets)
                {
                    Destroy(go);
                }
                m_FireBullets.Clear();
                float dtAngleZ = -m_DtAngle * (bulletCurves - 1) / 2;
                for (int index = 0; index < bulletCurves; index = index + 1)
                {
                    float Offset = -m_Offset * (bulletCount - 1) / 2; ;
                    for (int pIndex = 0; pIndex < bulletCount; pIndex = pIndex + 1)
                    {
                        GameObject effect = Instantiate(FireBulletEffect);
                        effect.transform.SetParent(FirePoint.transform, false);

                        Vector3 rotation = FirePoint.eulerAngles;
                        rotation.z = rotation.z + dtAngleZ;
                        effect.transform.eulerAngles = rotation;
                        Vector3 position = effect.transform.up * Offset + FirePoint.position;
                        effect.transform.position = position;

                        m_FireBullets.Add(effect);
                        Offset = Offset + m_Offset;
                    }
                    dtAngleZ = dtAngleZ + m_DtAngle;
                }
            }
        }
        protected void InstanceNormalBullet(bool synScaleToEffect = false, bool x2Z = false)
        {
            int bulletCount = GetBulletCount();
            int bulletCurves = GetBulletCurves();
            float dtAngleZ = -m_DtAngle * (bulletCurves - 1) / 2; ;
            int mIndex = 0;
            double damage = GetDamage();
            for (int index = 0; index < bulletCurves; index = index + 1)
            {
                float Offset = -m_Offset * (bulletCount - 1) / 2; ;
                for (int pIndex = 0; pIndex < bulletCount; pIndex = pIndex + 1)
                {
                    BaseBullet bullet = GetBullet();
                    bullet.SetLockEnemyObj(m_LockEnemyObj);
                    bullet.Init(damage, FirePoint, dtAngleZ, Offset);
                    Offset = Offset + m_Offset;
                    if (synScaleToEffect && m_FireBullets[mIndex] != null)
                    {
                        if (!x2Z)
                        {
                            m_FireBullets[mIndex].transform.localScale = bullet.transform.localScale * m_Scale;
                        }
                        else
                        {
                            Vector3 localScale = bullet.transform.localScale;
                            float x = localScale.x;
                            localScale.x = localScale.z;
                            localScale.z = x;
                            m_FireBullets[mIndex].transform.localScale = localScale * m_Scale;
                        }
                    }
                    mIndex++;
                }
                dtAngleZ = dtAngleZ + m_DtAngle;
            }
        }
        public virtual double GetBaseAtkTime()
        {
            return m_DtTime;
        }
        public virtual double GetBaseDamage()
        {
            return m_Damage;
        }
        protected virtual void InitDamage()
        {

        }
        public virtual void SetQuality(int quality)
        {
            m_Quality = quality;
            ////InitDamage();
        }
        protected int GetRealQuality(ItemItem weaponItem)
        {
            if (m_Quality < 0)
            {
                return Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(weaponItem);
            }
            return m_Quality;
        }
        protected virtual BaseBullet GetBullet()
        {
            if (BulletConfig.BulletPath.ContainsKey(BulletName))
            {
                BaseBullet baseBullet = Global.gApp.gGameCtrl.BulletCache.GetBullet(BulletName);
                return baseBullet;
            }
            else
            {
                GameObject bullet = Instantiate(Bullet);
                return bullet.GetComponentInChildren<BaseBullet>();
            }
        }
    }

}
