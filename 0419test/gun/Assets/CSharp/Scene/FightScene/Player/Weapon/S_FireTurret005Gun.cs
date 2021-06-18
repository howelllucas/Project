
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class S_FireTurret005Gun : SecondSupportGun
    {

        BaseTurret m_FireTurret;
        public GameObject m_StopEffect;
        protected override void Awake()
        {
            base.Awake();
            FirePoint = transform.Find(GameConstVal.FirePoint);
            if (FirePoint != null && m_FireEffect != null)
            {
                m_FireEffect.transform.SetParent(FirePoint, false);
                m_FireEffect.SetActive(true);
            }

            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WSFireTurret005);
            m_DtTime = weaponItem.dtime;

            InitDamage();
            m_FireTurret = GetComponentInParent<BaseTurret>();
            m_FireTurret.SetAtkRange(AtkRange);
        }
        //protected override void InitDamage()
        //{
        //    ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WSFireTurret005);
        //    m_DtTime = weaponItem.dtime;


        //    GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
        //    double atk = weaponLevelData.S_Turret005[(int)MWeapon.Atk];
        //    m_Damage = atk;
        //}

        protected override void Fire()
        {
            base.Fire();
            InstanceFireBullet();
        }
        void InstanceFireBullet()
        {
            base.Fire();
            InstanceFireBulletEffect();
            InstanceNormalBullet();
        }
        protected override void OnEnable()
        {
            FreshOffsetTime(true);
        }

        protected override void OnDisable()
        {
            foreach (GameObject go in m_FireBullets)
            {
                GameObject stopEffect = Instantiate(m_StopEffect);
                stopEffect.transform.SetParent(FirePoint, false);
                stopEffect.transform.position = go.transform.position;
                stopEffect.transform.localRotation = go.transform.localRotation;
            }
            base.OnDisable();
        }
    }
}

