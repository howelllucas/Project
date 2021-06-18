
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class S_FireTurret006Gun : SecondSupportGun
    {

        private double m_CacheDamage = 0;
        private bool m_CanFire = false;
        private bool m_HassEffect = true;
        private float m_EffectTime = 0;

        BaseTurret m_FireTurret;
        protected override void Awake()
        {
            base.Awake();
            FirePoint = transform.Find(GameConstVal.FirePoint);
            if (FirePoint != null && m_FireEffect != null)
            {
                m_FireEffect.transform.SetParent(FirePoint, false);
                m_FireEffect.SetActive(true);
            }

            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WSFireTurret006);
            m_DtTime = weaponItem.dtime;

            InitDamage();
            m_CurTime = 0;
            m_FireTurret = GetComponentInParent<BaseTurret>();
            m_FireTurret.SetAtkRange(AtkRange);
        }
        //protected override void InitDamage()
        //{
        //    ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WSFireTurret006);
        //    m_DtTime = weaponItem.dtime;

        //    GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
        //    double atk = weaponLevelData.S_Turret006[(int)MWeapon.Atk];
        //    m_Damage = atk;
        //    m_CacheDamage = m_Damage;
        //    m_Damage = 0;
        //}

        protected override void OnEnable()
        {
            base.OnEnable();
            m_HassEffect = true;
        }
        protected override void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                m_CurTime = m_CurTime + dtTime;
                m_EffectTime += dtTime;
                float newDtTime = GetDtTime();
                if (m_CurTime >= newDtTime)
                {
                    m_CurTime = m_CurTime - newDtTime;
                    m_Damage = m_CacheDamage;
                    m_CanFire = true;
                    m_EffectTime = 0;
                    m_HassEffect = false;
                    Fire();
                }
                else
                {
                    if (m_EffectTime > newDtTime / 2)
                    {
                        m_EffectTime = 0;
                        m_HassEffect = true;
                    }
                    if (m_HassEffect)
                    {
                        m_Damage = 1d;
                        m_HassEffect = false;
                    }
                    else
                    {
                        m_Damage = 0;
                    }
                    m_CanFire = false;
                    Fire();
                }
            }
        }
        protected override void Fire()
        {
            if (m_CanFire)
            {
                InstanceFireBulletEffect();
                InstanceNormalBullet(true, true);
                foreach (GameObject go in m_FireBullets)
                {
                    if (go.transform.localScale.z > 0)
                    {
                        base.Fire();
                        break;
                    }
                }
            }
            else
            {
                InstanceFireBulletEffect();
                InstanceNormalBullet(true, true);
            }
        }
    }
}

