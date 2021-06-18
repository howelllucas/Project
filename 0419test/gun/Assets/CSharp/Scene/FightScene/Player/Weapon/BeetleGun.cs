
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class BeetleGun : Gun
    {
        private BeetlePet m_BeetlePet;
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            InitDamage();
            FirePoint = transform.Find(GameConstVal.FirePoint);
        }
        public void SetBeetlePet(BeetlePet beetlePet)
        {
            m_BeetlePet = beetlePet;
        }

        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WBeetle);

            m_DtTime = weaponItem.dtime;
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Beetle[(int)MWeapon.Atk];

            int curLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(GameConstVal.WBeetle);
            GunsSub_dataItem levelItem = Global.gApp.gGameData.GunSubDataConfig.Get(curLv);
            double[] weaponParams = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + weaponItem.qualevel, levelItem);

            float campAtkInc = Global.gApp.CurScene.GetMainPlayerComp().GetBuffMgr().CampPetIncAtk;
            m_Damage = atk * weaponParams[0] * ( 1 + campAtkInc);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_CurTime = m_DtTime - 0.25f;
        }
        protected override void Fire()
        {
            m_BeetlePet.PlayFireAnim();
            InstanceBullet();
        }
        void InstanceBullet()
        {
            InstanceNormalBullet();
            Global.gApp.gAudioSource.PlayOneShot(FireClip);
            if (m_FireEffect != null)
            {
                m_FireEffect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().Play();
            }
        }
    }
}

