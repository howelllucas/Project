
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class DogGun : Gun
    {
        [SerializeField] private Transform FirePoint1;
        [SerializeField] private Transform FirePoint2;
        private ParticleSystem m_FireEffect1;
        private ParticleSystem m_FireEffect2;
        private DogPet m_DogPet;
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            GameObject effect1Go = Instantiate(FireEffect);
            effect1Go.transform.SetParent(FirePoint1, false);
            m_FireEffect1 = effect1Go.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>();
            GameObject effect2Go = Instantiate(FireEffect);
            effect2Go.transform.SetParent(FirePoint2, false);
            m_FireEffect2 = effect2Go.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>();
            InitDamage();
        }
        public void SetDogPet(DogPet dogPet)
        {
            m_DogPet = dogPet;
        }

        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WDog);

            m_DtTime = weaponItem.dtime;

            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Dog[(int)MWeapon.Atk];

            int curLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(GameConstVal.WDog);
            GunsSub_dataItem levelItem = Global.gApp.gGameData.GunSubDataConfig.Get(curLv);
            double[] weaponParams = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + weaponItem.qualevel, levelItem);

            float campAtkInc = Global.gApp.CurScene.GetMainPlayerComp().GetBuffMgr().CampPetIncAtk;
            m_Damage = atk * weaponParams[0] * (1 + campAtkInc);
        }

        protected override void Fire()
        {
            Global.gApp.gShakeCompt.StartShake(0, 0.1f, 0.03f);
            base.Fire();
            m_DogPet.PlayFireAnim();
            m_FireEffect1.Play();
            BaseBullet bullet1 = GetBullet();
            bullet1.SetLockEnemyObj(m_LockEnemyObj);
            bullet1.Init(m_Damage, FirePoint1, 0, 0);

            m_FireEffect2.Play();
            BaseBullet bullet2 = GetBullet();
            bullet2.SetLockEnemyObj(m_LockEnemyObj);
            bullet2.Init(m_Damage, FirePoint2, 0, 0);
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
        protected override void OnEnable()
        {
            FreshOffsetTime(true);
            m_CurTime = m_DtTime - 0.25f;
        }
        protected override void OnDisable()
        {
            FreshOffsetTime(false);
        }
    }
}

