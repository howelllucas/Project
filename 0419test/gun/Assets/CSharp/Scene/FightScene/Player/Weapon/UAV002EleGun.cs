
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class UAV002EleGun : Gun
    {

        private double m_CacheDamage = 0;
        private bool m_CanFire = false;
        private bool m_HassEffect = true;
        private float m_EffectTime = 0;

        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            InitDamage();
            FirePoint = transform.parent;
        }

        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WUAV002);
            m_DtTime = weaponItem.dtime;
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.UAV002[(int)MWeapon.Atk];

            int curLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(GameConstVal.WUAV002);
            GunsSub_dataItem levelItem = Global.gApp.gGameData.GunSubDataConfig.Get(curLv);
            double[] weaponParams = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + weaponItem.qualevel, levelItem);

            float campAtkInc = Global.gApp.CurScene.GetMainPlayerComp().GetBuffMgr().CampPetIncAtk;
            m_CacheDamage = atk * weaponParams[0] * (1 + campAtkInc);
            m_Damage = 0;
        }
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

