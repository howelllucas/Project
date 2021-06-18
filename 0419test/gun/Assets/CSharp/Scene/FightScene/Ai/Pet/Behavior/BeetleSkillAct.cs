using EZ.Data;
using EZ.Weapon;
using System;
using UnityEngine;
namespace EZ
{
    public class BeetleSkillAct : PetBaseAct
    {
        [Tooltip("攻击跟随 速度")]
        [SerializeField] protected float FollowSpeed = 7;
        [SerializeField] protected float AtkParam = 1;
        [SerializeField] private float BreakDis = 20;
        public BeetleGun m_BeetleGun;
        public PetGunRotateTools m_RotateTools;
        private PetLockDisTools m_LockTools;
        GameObject m_PlayerGo;
        Action<bool> m_PursureCallBack;
        float m_CheckStateDtTime = 0.15f;
        bool m_ReachPlace = false;
        public override void Init(BasePet pet,PetAtkState controller)
        {
            base.Init(pet, controller);
            m_BeetleGun.SetBeetlePet((pet as BeetlePet));
            m_BeetleGun.Init(null);
            m_BeetleGun.enabled = false;
            m_PursureCallBack = PursueCallBack;
            m_LockTools = new PetLockDisTools(transform,pet);
            m_LockTools.MaxRadio = 1;
            m_LockTools.MinRadio = 1;
            m_LockTools.BreakDis = BreakDis;
            m_LockTools.SetBreakCall(BreakCall);
        }
        public void SetPlayerGo(GameObject playerGo)
        {
            m_PlayerGo = playerGo;
        }
        private  void PursueCallBack(bool reachplace)
        {
            m_ReachPlace = reachplace;
            m_CurTime = 0;

        }
        private void BreakCall()
        {
            m_InBreakState = true;
            m_Pet.PlayAnim(GameConstVal.Run);
            // 检测是否能 进入到其他状态
            if (m_Controller.CheckCanEnterOtherState() && m_InBreakState)
            {
                m_Controller.EndFirstSkill();
            }
        }
        public override void SetLockMonster(Transform monsterTsf, Monster monster, float monsterRadio)
        {
            float radio = monsterRadio + m_Pet.CircleRadio;
            base.SetLockMonster(monsterTsf, monster, monsterRadio);
            m_LockTools.MaxRadio = 10000;
            m_LockTools.MinRadio = 0;
            m_LockTools.SetFollowState(false);
            m_LockTools.SetRotateState(false);
            m_LockTools.ResetCurRadio();
            m_LockTools.SetFollowSpeed(monster.GetMonsterItem().baseSpeed);
        }
        public override void EndEffect()
        {
        }
        public override void EndSkill()
        {
            // SetLockMonster make LockMonsterTsf null, next Start m_LockMonsterTsf will be null
            //SetLockMonster(null, null);
            //m_Bullet.StopAtkAbs();
            m_BeetleGun.enabled = false;
            m_Pet.GetAutoPathComp().SetAutoPathEnable(false, 0.5f);
            m_RotateTools.SetFaceToOri();
        }

        public override void StartSkill()
        {
            m_BeetleGun.enabled = true;
            m_InBreakState = false;
            m_LockTools.SetLockTsf(m_LockMonsterTsf);
            m_LockTools.SetFollowState(false);
            m_LockTools.SetRotateState(false);
            m_Pet.GetAutoPathComp().SetAutoPathEnable(true, 3, FollowSpeed, m_PlayerGo.transform, m_PursureCallBack);
        }
        public override bool CanTriggerSkill()
        {
            return m_AtkTimes > 0;
        }
        private void RotateGun()
        {
            m_RotateTools.SetFaceToMonster(m_LockMonster.transform.position - m_Pet.transform.position);
        }
        public override void MUpdate()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0 && !m_InBreakState)
            {
                m_LockTools.Update();
                RotateGun();
                if (m_ReachPlace)
                {
                    m_CurTime += dtTime;
                    if (m_CurTime > m_CheckStateDtTime)
                    {
                        m_CurTime = 0;
                        m_Pet.PlayAnim(GameConstVal.Idle);
                    }
                }
                else
                {
                    m_CurTime += dtTime;
                    if (m_CurTime > m_CheckStateDtTime)
                    {
                        m_CurTime = 0;
                        m_Pet.PlayAnim(GameConstVal.Run);
                    }
                }
            }
            else
            {
                m_Pet.SetSpeed(Vector2.zero);
            }
        }
    }
}
