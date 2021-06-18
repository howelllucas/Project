using EZ.Weapon;
using UnityEngine;
using Game;
using Game.Data;

namespace EZ
{

    public class FightWeaponMgr
    {
        private Player m_Player;
        private GameObject m_FirePoint;
        private bool m_DirtyRocketGun = false;
        private Transform m_WeaponNodeTsf;

        private GunCard_TableItem m_WeaponRes;
        public GunCard_TableItem WeaponRes
        {
            get { return m_WeaponRes; }
        }

        private GunCardData m_GunCardData = null;
        public GunCardData GunCardData
        {
            get { return m_GunCardData; }
        }

        #region First Gun info
        private bool m_ChangingFirstWeapon = false;
        private GameObject m_FirstWeapon;
        private Gun m_FirstGun;
        private GameObject m_CurFirstWeapon;
        private bool m_FirstGunIsEnable = true;
        private float m_FirstGunTime = 0;
        private float m_FirstGunDuration = 0;
        private string m_FirstWeaponName = GameConstVal.EmepyStr;
        #endregion

        #region Second Gun info
        private GameObject m_SecondWeapon;
        private GameObject m_CurSecondWeapon;
        private Gun m_SecondGun;

        #endregion

        #region third Gun info
        private bool m_ChangingThirdWeapon = false;
        private float m_ThirdGunDuration = 0;
        private float m_ThirdRunTime = 0;
        private string m_ThirdWeaponName = GameConstVal.EmepyStr;
        private GameObject m_CurThirdWeapon;
        private Gun m_CurThirdGun;
        #endregion
        public FightWeaponMgr(Player player, int defaultWeapon = 0)
        {
            m_GunCardData = PlayerDataMgr.singleton.GetUseWeapon();
            if (m_GunCardData == null)
                return;

            m_WeaponRes = TableMgr.singleton.GunCardTable.GetItemByID(m_GunCardData.cardID);
            if (m_WeaponRes == null)
            {
                Debug.LogError("m_WeaponData = null");
                return;
            }
            //string firstGunName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon();//GameConstVal.WLaserX;
            string firstGunName = m_WeaponRes.prefab;
            string newWpnName = firstGunName;
            //if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(firstGunName) > 0)
            //{
            //    newWpnName = newWpnName + "_s";
            //}
            Debug.Log("firstGunName " + firstGunName);
            m_Player = player;
            m_WeaponNodeTsf = player.transform.Find("ModelNode/hero/weapon_bip_real");
            m_FirstWeapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/WeaponNew/" + newWpnName);
            m_FirePoint = m_WeaponNodeTsf.parent.Find(GameConstVal.FirePoint).gameObject;
            m_FirstWeapon.transform.SetParent(m_WeaponNodeTsf, false);
            m_FirstGun = m_FirstWeapon.GetComponent<Gun>();
            m_CurFirstWeapon = m_FirstWeapon;
            m_FirstWeaponName = firstGunName;

            InitCombatAttr();
            //ReviseCombatAttrValue(CombatAttr.CombatAttrType.Attack, m_WeaponRes.atk);
            //ReviseCombatAttrValue(CombatAttr.CombatAttrType.Attack_Speed_Add, m_WeaponRes.atkSpeed);

            var skills = PlayerDataMgr.singleton.GetFuseSkills();
            for (int i = 0;i < skills.Count;++i)
            {
                var data = TableMgr.singleton.FuseGunSkillTable.GetItemByID(skills[i]);
                if (data == null)
                    continue;

                var type = (CombatAttr.CombatAttrType)(data.id / 100);
                ReviseCombatAttrValue(type, data.value);
            }

            //if (Global.gApp.CurScene.IsNormalPass())
            //{
            //    string secondGunName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurSubWeapon();//GameConstVal.WLaserX;
            //    if (secondGunName != null && !secondGunName.Equals(GameConstVal.EmepyStr))
            //    {
            //        m_SecondWeapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/Weapon/" + secondGunName);
            //        m_SecondWeapon.transform.SetParent(m_WeaponNodeTsf, false);
            //        m_SecondGun = m_SecondWeapon.GetComponent<Gun>();
            //        m_CurSecondWeapon = m_SecondWeapon;
            //    }
            //}
        }
        public void InitGun()
        {
            m_FirstGun.Init(this);
            if(m_SecondGun != null)
            {
                m_SecondGun.Init(this);
            }
        }
        public void Update()
        {
            //CheckIdleWeapon();
            if (m_ChangingFirstWeapon)
            {
                m_FirstGunTime = m_FirstGunTime + BaseScene.GetDtTime();
                if (m_FirstGunTime >= m_FirstGunDuration)
                {
                    m_ChangingFirstWeapon = false;
                    m_FirstGunDuration = 0;
                    ResetFirstWeapon();
                }
            }
            if(m_ChangingThirdWeapon)
            {
                m_ThirdRunTime = m_ThirdRunTime + BaseScene.GetDtTime();
                if(m_ThirdRunTime >= m_ThirdGunDuration)
                {
                    m_ThirdGunDuration = 0;
                    m_ChangingThirdWeapon = false;
                    ResetThirdWeapon();
                }
            }
        }
        private void CheckIdleWeapon()
        {
            if (!m_DirtyRocketGun && ETCJoystick.GetTouchCount() == 0 && !Input.anyKey)
            {
                m_DirtyRocketGun = true;
                m_ChangingFirstWeapon = false;
                m_FirstGunDuration = 0;
                Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, "Weapon", GameConstVal.WRPG, -1);
                ChangeWeapon(GameConstVal.WRPG, 99999999);
            }
            else if (m_DirtyRocketGun && (ETCJoystick.GetTouchCount() > 0 || Input.anyKey))
            {
                m_DirtyRocketGun = false;
                m_ChangingFirstWeapon = false;
                m_FirstGunDuration = 0;
                ResetFirstWeapon();
            }
        }
        public void ResetFirstWeapon()
        {
            if (m_CurFirstWeapon != m_FirstWeapon)
            {
                if(m_FirstGun != null)
                {
                    m_FirstGun.ChangeGun(false);
                }
                Object.Destroy(m_CurFirstWeapon);
                m_CurFirstWeapon = m_FirstWeapon;
                m_CurFirstWeapon.SetActive(true);
                m_FirstGun = m_FirstWeapon.GetComponent<Gun>();
                m_FirstGun.Init(this);
                m_FirstGun.ChangeGun(true);
                SetCurMainWeaponEnabled(m_FirstGunIsEnable);
                //m_FirstWeaponName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon();
                m_FirstWeaponName = Game.PlayerDataMgr.singleton.GetUseWeaponName();
            }
        }

        private void ResetThirdWeapon()
        {
            if (m_CurThirdWeapon != null)
            {
                Object.Destroy(m_CurThirdWeapon);
                m_CurThirdWeapon = null;
            }
            m_ThirdWeaponName = GameConstVal.EmepyStr;
        }

        public void ChangeThirdWeapon(string weaponName, float keepTime,float damageCoefficient = 1)
        {
            if (!weaponName.Equals(m_ThirdWeaponName))
            {
                m_ThirdGunDuration = keepTime;
            }
            else
            {
                m_ThirdGunDuration = Mathf.Max(keepTime, m_ThirdGunDuration);
            }
            ResetThirdWeapon();

            m_CurThirdWeapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/Weapon/" + weaponName);
            m_CurThirdWeapon.transform.SetParent(m_WeaponNodeTsf, false);
            m_CurThirdGun = m_CurThirdWeapon.GetComponent<Gun>();
            m_CurThirdGun.Init(this);
            m_CurThirdGun.SetDamageCoefficient(damageCoefficient);
            m_ChangingThirdWeapon = true;
            m_ThirdRunTime = 0;
            m_ThirdWeaponName = weaponName;
        }
     
        private void DestroyFirstWpnToOri()
        {
            if (m_CurFirstWeapon != m_FirstWeapon)
            {
                if (m_FirstGun != null)
                {
                    m_FirstGun.ChangeGun(false);
                }
                Object.Destroy(m_CurFirstWeapon);
                m_CurFirstWeapon = m_FirstWeapon;
                m_FirstGun = m_FirstWeapon.GetComponent<Gun>();
                //m_FirstWeaponName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon();
                m_FirstWeaponName = Game.PlayerDataMgr.singleton.GetUseWeaponName();
            }
            if (m_FirstGun != null)
            {
                m_FirstGun.ChangeGun(false);
            }
        }
        public void ChangeWeapon(string weaponName,float keepTime,float damageCoefficient = 1)
        {
            if (!weaponName.Equals(m_FirstWeaponName))
            {
                m_FirstGunDuration = keepTime;
            }
            else
            {
                m_FirstGunDuration = Mathf.Max(keepTime, m_FirstGunDuration);
            }
            DestroyFirstWpnToOri();
            m_CurFirstWeapon.SetActive(false);
            m_CurFirstWeapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/Weapon/" + weaponName);
            m_CurFirstWeapon.transform.SetParent(m_WeaponNodeTsf, false);
            m_FirstGun = m_CurFirstWeapon.GetComponent<Gun>();
            m_FirstGun.Init(this);
            m_FirstGun.SetDamageCoefficient(damageCoefficient);
            m_FirstGun.ChangeGun(true);
            SetCurMainWeaponEnabled(m_FirstGunIsEnable);
            m_ChangingFirstWeapon = true;
            m_FirstGunTime = 0;
            m_FirstWeaponName = weaponName;
        }
        public Gun GetFirstGun()
        {
            return m_FirstGun;
        }
        public void SetCurMainWeaponEnabled(bool isEnabled)
        {
            m_FirstGunIsEnable = isEnabled;
            if (isEnabled)
            {
                if (!m_FirstGun.enabled)
                {
                    m_FirstGun.enabled = true;
                }
            }
            else
            {
                if (m_FirstGun.enabled)
                {
                    m_FirstGun.enabled = false;
                }
            }
        }

        ////////////////////////////////////////////////////////////////

        private CombatAttr combatAttrs = new CombatAttr();
        public bool InitCombatAttr()
        {
            if (!combatAttrs.Init())
                return false;
            return true;
        }

        public float GetCombatAttrValue(CombatAttr.CombatAttrType propertyID)
        {
            return combatAttrs.GetValue(propertyID);
        }


        public void ReviseCombatAttrValue(CombatAttr.CombatAttrType propertyID, float value)
        {
            combatAttrs.Revise(propertyID, value);
        }
    }
}
