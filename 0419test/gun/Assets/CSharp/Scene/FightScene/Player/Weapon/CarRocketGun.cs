using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

namespace EZ.Weapon
{
    public class CarRocketGun : Gun
    {

        public Transform m_RoateNode;
        public Animator m_Animator;
        private float m_CheckDtTime = 1.3f;
        private float m_CurCheckTime = 1.3f;
        private Monster m_LockGameEnemy;
        private WaveMgr m_WaveMgr;
        private float m_RadiusSqr;
        public override void Init(FightWeaponMgr mgr)
        {
            m_RadiusSqr = AtkRange * AtkRange;
            m_WaveMgr = m_RoateNode.parent.GetComponentInChildren<Player>().GetWaveMgr();

            FirePoint = transform.Find("paota/FirePoint");
            m_Player = GetComponentInParent<Player>();
            if (FirePoint != null && m_FireEffect != null)
            {
                m_FireEffect.transform.SetParent(FirePoint, false);
                m_FireEffect.SetActive(true);
            }
            InitDamage();
            m_CurTime = GetDtTime() * 0.8f;
        }
        protected override void InitDamage()
        {

            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WTank_turret);
            m_DtTime = weaponItem.dtime;
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();


            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Tank_turret[(int)MWeapon.Atk];
            m_Damage = atk;
        }
        protected override void Update()
        {
            FaceToEnemy();
            if (m_LockGameEnemy != null)
            {
                base.Update();
            }
        }
        private void FaceToEnemy()
        {
            if (m_LockGameEnemy == null || m_LockGameEnemy.InDeath)
            {
                FindLockEnemy();
                m_CurCheckTime = 0;
            }
            else
            {
                m_CurCheckTime = m_CurCheckTime + BaseScene.GetDtTime();
                if (m_CurCheckTime >= m_CheckDtTime)
                {
                    m_CurCheckTime = m_CurCheckTime - m_CheckDtTime;
                    FindLockEnemy();
                }
            }
            FaceToEnemyImp();
        }
        private void FaceToEnemyImp()
        {
            if (m_LockGameEnemy)
            {
                float angleZ = EZMath.SignedAngleBetween(m_LockGameEnemy.transform.position - transform.position, Vector3.up);
                CalcAnglesWorld(angleZ);
            }
            else
            {
                CalcAnglesLocal(0);
            }
        }
        private void CalcAnglesLocal(float angleZ)
        {
            Vector3 eulerAngle = m_RoateNode.localEulerAngles;
            float dtAngle = angleZ - eulerAngle.z;
            if (dtAngle > 180)
            {
                dtAngle = (dtAngle) - 360;
            }
            else if (dtAngle < -180)
            {
                dtAngle = (dtAngle) + 360;
            }
            dtAngle = eulerAngle.z + dtAngle * BaseScene.GetDtTime() * 5;
            m_RoateNode.localEulerAngles = new Vector3(0, 0, dtAngle);
        }
        private void CalcAnglesWorld(float angleZ)
        {
            Vector3 eulerAngle = m_RoateNode.eulerAngles;
            float dtAngle = angleZ - eulerAngle.z;
            if (dtAngle > 180)
            {
                dtAngle = (dtAngle) - 360;
            }
            else if (dtAngle < -180)
            {
                dtAngle = (dtAngle) + 360;
            }
            dtAngle = eulerAngle.z + dtAngle * BaseScene.GetDtTime() * 8;
            m_RoateNode.eulerAngles = new Vector3(0,0, dtAngle);
        }

        protected override void Fire()
        {
            InstanceBullet();
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
        private void SetLockEnemy(Monster lockEnemy)
        {
            m_LockGameEnemy = lockEnemy;
        }
        private void FindLockEnemy()
        {
            Vector3 m_position = transform.position;
            SetLockEnemy(null);
            float m_lastPositionSqure = 0;
            Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
            foreach (KeyValuePair<int, Wave> kv in waves)
            {
                List<Monster> monsters = kv.Value.GetMonsters();
                foreach (Monster monster in monsters)
                {
                    Vector3 postion = monster.transform.position;
                    Vector3 dtPosition = m_position - postion;
                    float sqrMagnitude = dtPosition.sqrMagnitude;
                    if (sqrMagnitude <= m_RadiusSqr)
                    {
                        if (m_LockGameEnemy != null)
                        {
                            if (sqrMagnitude < m_lastPositionSqure)
                            {
                                m_lastPositionSqure = sqrMagnitude;
                                SetLockEnemy(monster);
                                if (sqrMagnitude <= 25f)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            SetLockEnemy(monster);
                            m_lastPositionSqure = (m_LockGameEnemy.transform.position - m_position).sqrMagnitude;
                            if (m_lastPositionSqure <= 25)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        void InstanceBullet()
        {
            m_Animator.Play(GameConstVal.Attack,-1,0);
            InstanceNormalBullet();
            Global.gApp.gAudioSource.PlayOneShot(FireClip);
            if (m_FireEffect != null)
            {
                m_FireEffect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().Play();
            }
        }
    }
}


