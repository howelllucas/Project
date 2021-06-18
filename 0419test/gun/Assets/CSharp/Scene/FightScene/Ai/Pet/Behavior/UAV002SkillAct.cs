using EZ.Data;
using EZ.Weapon;
using UnityEngine;
namespace EZ
{
    public class UAV002SkillAct : PetBaseAct
    {

        [Tooltip("攻击跟随 速度")]
        [SerializeField] protected float FollowSpeed = 7;
        [SerializeField] protected float AtkParam = 1;
        [SerializeField] private float BreakDis = 20;
        public UAV002EleGun m_UAVGun;
        private PetLockDisTools m_LockTools;
        GameObject m_PlayerGo;
        public override void Init(BasePet pet, PetAtkState controller)
        {
            base.Init(pet, controller);
            m_UAVGun.Init(null);
            m_UAVGun.enabled = false;
            m_LockTools = new PetLockDisTools(transform, pet);
            m_LockTools.MaxRadio = 1;
            m_LockTools.MinRadio = 1;
            m_LockTools.BreakDis = BreakDis;
            m_LockTools.SetBreakCall(BreakCall);
        }
        public void SetPlayerGo(GameObject playerGo)
        {
            m_PlayerGo = playerGo;
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
            m_UAVGun.enabled = false;
            m_Pet.GetAutoPathComp().SetAutoPathEnable(false, 0.5f);
            m_Pet.GetAutoPathComp().SetEanbleRotation(true);
        }

        public override void StartSkill()
        {
            m_UAVGun.enabled = true;
            m_InBreakState = false;
            m_LockTools.SetLockTsf(m_LockMonsterTsf);
            m_Pet.GetAutoPathComp().SetAutoPathEnable(true, 3, FollowSpeed, m_PlayerGo.transform);
            m_Pet.GetAutoPathComp().SetEanbleRotation(false);
        }
        public override bool CanTriggerSkill()
        {
            return m_AtkTimes > 0;
        }
        public override void MUpdate()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0 && !m_InBreakState)
            {
                m_LockTools.Update();
                if (m_InBreakState)
                {
                    //m_Pet.SetSpeed(Vector2.zero);
                }
            }
            else
            {
                m_Pet.SetSpeed(Vector2.zero);
            }
        }
    }
}
