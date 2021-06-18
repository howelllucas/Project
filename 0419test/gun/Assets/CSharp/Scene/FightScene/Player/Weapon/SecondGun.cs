using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ.Weapon
{
    public abstract class SecondGun : Gun
    {
        public enum TriggerType
        {
            None = 0,
            Automatic = 1,
            Passiveness = 2,
            PassivenessNoLockMonsterNeeded = 3,
        }

        public enum PowerIncreaseTpye
        {
            None = 0,
            Move = 1,
            Always = 2,
        }
        public enum ShowPowerIncreaseIcon
        {
            UnShow = 0,
            Show = 1,
        }
        [Tooltip("能力增长方式 Move 移动增长 always 一直增长")]
        [SerializeField] protected PowerIncreaseTpye m_PowerTriggerType = PowerIncreaseTpye.Always;

        [Tooltip("触发方式 Automatic 主动 Passiveness 被动 ,PassivenessNoLockMonsterNeeded 暂时无效,None 兼容  0")]
        [SerializeField] protected TriggerType m_TriggerType = TriggerType.Automatic;


        [Tooltip("是否显示 power 图标")]
        [SerializeField] protected ShowPowerIncreaseIcon m_PowerShowType = ShowPowerIncreaseIcon.Show;

        [Tooltip("被动技能最多攒几个")]
        [SerializeField] protected float m_MaxFireTimes = 1;
        [Tooltip("攒够了之后 再次释放间隔，比如攒了 2个 m_FireTime = 0.15f,那么释放完第一个 在释放第二个需要等待 0.15f秒 ")]
        [SerializeField] protected float m_FireTime;
        [Tooltip("set 0  not use ")]
        protected float m_CurFireTime;
        protected float m_Progress;
        protected Fight m_Fight;
        protected ItemItem m_WeaponItem;
        private double m_CampSecondWpnIncAtk = -100;
        public override void Init(FightWeaponMgr mgr)
        {
            InitBaseInfo(mgr);
            if (m_TriggerType == TriggerType.Automatic)
            {
                InitAutomatic();
            }
            else if (m_TriggerType == TriggerType.Passiveness || m_TriggerType == TriggerType.PassivenessNoLockMonsterNeeded)
            {

            }
            InitePowerShowInfo();
        }
        protected double GetCampDamageInc()
        {
            if(m_CampSecondWpnIncAtk < 0)
            {
                m_CampSecondWpnIncAtk = Global.gApp.CurScene.GetMainPlayerComp().GetBuffMgr().CampSecondWpnIncAtk;
            }
            return 1 + m_CampSecondWpnIncAtk;
        }
        protected override void InitBaseInfo(FightWeaponMgr mgr)
        {
            m_Player = GetComponentInParent<Player>();
            m_Fight = m_Player.GetFight();
            base.InitBaseInfo(mgr);
            if (m_Player != null)
            {
                m_Scale = 1 / m_Player.transform.lossyScale.x;
            }
            m_CurFireTime = 0;
            m_CurTime = 0;
        }

        protected virtual void InitAutomatic()
        {
            m_CurTime = GetDtTime();
        }

        protected virtual void InitePowerShowInfo()
        {
            if (m_PowerShowType == ShowPowerIncreaseIcon.Show)
            {
                Global.gApp.gMsgDispatcher.Broadcast<float, ItemItem>(MsgIds.FreshGunPower, 0, m_WeaponItem);
            }
        }
        protected override void Update()
        {
            if (m_TriggerType == TriggerType.Passiveness || m_TriggerType == TriggerType.PassivenessNoLockMonsterNeeded)
            {
                UpdatePassiveness();
            }
            else if(m_TriggerType == TriggerType.Automatic)
            {
                base.Update();
                UpdateAutoMatic();
            }
            BroadPowerIconInfo();
        }
        protected override void Fire()
        {
            base.Fire();
            InstanceNormalBullet();
            if (m_TriggerType == TriggerType.Passiveness || m_TriggerType == TriggerType.PassivenessNoLockMonsterNeeded)
            {
                m_CurFireTime = m_FireTime;
            }
        }
        protected void UpdateAutoMatic()
        {

        }
        private void BroadPowerIconInfo()
        {
            if (m_PowerShowType == ShowPowerIncreaseIcon.Show)
            {
                float newDtTime = GetDtTime();
                Global.gApp.gMsgDispatcher.Broadcast<float, ItemItem>(MsgIds.FreshGunPower, Mathf.Min(m_CurTime / newDtTime, 1), m_WeaponItem);
            }
        }
        protected override int GetBulletCount()
        {
            return m_BaseCount;
        }
        protected override int GetBulletCurves()
        {
            return m_BaseCount;
        }
        protected override void OnEnable()
        {
            FreshOffsetTime(true);
        }
        protected override void OnDisable()
        {
            if (m_PowerShowType == ShowPowerIncreaseIcon.Show)
            {
                Global.gApp.gMsgDispatcher.Broadcast<float,ItemItem>(MsgIds.FreshGunPower, -1, m_WeaponItem);
            }
        }
        private void OnDestroy()
        {
            if (m_PowerShowType == ShowPowerIncreaseIcon.Show)
            {

                Global.gApp.gMsgDispatcher.Broadcast<float,ItemItem>(MsgIds.FreshGunPower, -1, m_WeaponItem);
            }
        }
        protected  void UpdatePassiveness()
        {
            float dt = BaseScene.GetDtTime();
            if (dt <= 0) { return; }
            m_CurFireTime -= dt;
            float newDtTime = GetDtTime();
            if (m_CurFireTime <= 0)
            {
                if(m_PowerTriggerType == PowerIncreaseTpye.Always)
                {
                    if (m_CurTime < m_MaxFireTimes * newDtTime)
                    {
                        m_CurTime = m_CurTime + dt;
                    }
                }
                else if(m_PowerTriggerType == PowerIncreaseTpye.Move)
                {
                    if (ETCJoystick.GetTouchCount() > 0 || Input.anyKey)
                    {
                        if (m_CurTime < m_MaxFireTimes * newDtTime)
                        {
                            m_CurTime = m_CurTime + dt;
                        }
                    }
                }
         
                if (ETCJoystick.GetTouchCount() == 0 && !Input.anyKey && !m_Player.GetCarrier().HasCarrier())
                {
                    if (m_TriggerType == TriggerType.Passiveness && m_Fight.GetLockEnemy() != null)
                    {
                        if (m_CurTime >= newDtTime)
                        {
                            m_Fight.ForceFaceToEnemy();
                            m_CurTime = m_CurTime - newDtTime;
                            m_CurFireTime = m_FireTime;
                            Fire();
                        }
                    }
                    else if(m_TriggerType == TriggerType.PassivenessNoLockMonsterNeeded)
                    {
                        if (m_CurTime >= newDtTime)
                        {
                            m_Fight.ForceFaceToEnemy();
                            m_CurTime = m_CurTime - newDtTime;
                            m_CurFireTime = m_FireTime;
                            Fire();
                        }
                    }
                }
            }
        }
    }
}
