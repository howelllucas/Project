using EZ.Data;
using EZ.Weapon;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class CampsitePerformer : MonoBehaviour, IFightLockEnemyObj
    {
        public Transform m_WeaponNode;
        public float lockRange = 10f;
        private GunCard_TableItem m_GunData;
        private GunCard_TableItem defaultGunData;
        Gun m_Gun;
        List<Monster> m_AllMonster;
        Monster m_LockCampsiteMonster;
        private Animator m_Anim;
        private string m_UAnim = GameConstVal.EmepyStr;
        private Transform m_UITrans;

        private bool isFight;

        private void Awake()
        {
            defaultGunData = TableMgr.singleton.GunCardTable.GetItemByID(TableMgr.singleton.ValueTable.init_weapon_id);
            //m_WeaponNode = transform.Find("ModelNode/hero/weapon_bip");
            m_Anim = transform.Find("ModelNode/hero").GetComponent<Animator>();
            m_UITrans = transform.Find("TaskNode");
        }

        public void SetMonsterList(List<Monster> list)
        {
            m_AllMonster = list;
        }

        private void Update()
        {
            if (isFight)
                SetLockMonster(CheckLockMonster());
        }

        private Monster CheckLockMonster()
        {
            if (m_LockCampsiteMonster != null && !m_LockCampsiteMonster.InDeath)
                return m_LockCampsiteMonster;

            Monster target = null;
            if (m_AllMonster != null)
            {
                float minDis = lockRange;
                for (int i = 0; i < m_AllMonster.Count; i++)
                {
                    var monster = m_AllMonster[i];
                    var dis = Vector3.Distance(monster.transform.position, transform.position);
                    if (monster != null && !monster.InDeath && dis <= minDis)
                    {
                        target = monster;
                        minDis = dis;
                    }
                }
            }

            return target;
        }

        private void SetLockMonster(Monster monster)
        {
            m_LockCampsiteMonster = monster;
            if (monster != null)
            {
                m_Gun.enabled = true;
                Vector3 dir = monster.transform.position - transform.position;
                dir.z = 0;
                float angleZ = EZMath.SignedAngleBetween(dir, Vector3.up);
                Vector3 eulerAngle = transform.eulerAngles;
                float dtAngle = angleZ - eulerAngle.z;
                if (dtAngle > 180f)
                {
                    dtAngle -= 360f;
                }
                if (dtAngle < -180)
                {
                    dtAngle += 360f;
                }
                angleZ = eulerAngle.z + dtAngle * BaseScene.GetDtTime() * 15;
                transform.eulerAngles = new Vector3(0, 0, angleZ);
                PlayFight();
            }
            else
            {
                PlayIdle();
            }
        }

        public GameObject GetLockEnemy()
        {
            if (m_LockCampsiteMonster == null)
                return null;
            return m_LockCampsiteMonster.gameObject;
        }

        public void ChangeGun(GunCard_TableItem gunData)
        {
            if (gunData == null)
                gunData = defaultGunData;

            if (m_GunData != gunData)
            {
                if (m_Gun != null)
                {
                    Destroy(m_Gun.gameObject);
                }

                var gunObj = Global.gApp.gResMgr.InstantiateObj("Prefabs/WeaponNew/" + gunData.prefab);
                gunObj.transform.SetParent(m_WeaponNode, false);
                m_Gun = gunObj.GetComponent<Gun>();
                m_Gun.InitByCardData(gunData);
                m_GunData = gunData;
            }
        }

        public void SetFight()
        {
            PlayFight();
            isFight = true;
        }

        private void PlayFight()
        {
            if (m_Gun != null)
            {
                m_Gun.enabled = true;
            }
            PlayUAnim(GameConstVal.Shoot);
        }

        public void SetIdle()
        {
            PlayIdle();
            isFight = false;
        }

        private void PlayIdle()
        {
            if (m_Gun != null)
            {
                m_Gun.enabled = false;
            }
            PlayUAnim(GameConstVal.Idle);
        }

        private void PlayUAnim(string anim)
        {
            if (anim != null && !m_UAnim.Equals(anim))
            {
                m_UAnim = anim;
                //m_Anim.Play(anim, -1, 0);
            }
        }
    }
}
