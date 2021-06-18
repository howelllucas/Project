using EZ.Data;
using UnityEngine;
namespace EZ
{
    public class ManiacSkill2Act : PetBaseAct
    {
        enum ManiacSkillStep
        {
            None = 0,
            Step1 = 1,
            Step2 = 2,
            Step3 = 3,
            step4 = 4,
        }

        private ManiacSkillStep m_CurSkillStep = ManiacSkillStep.None;
        [SerializeField] protected float RealDamageTime = 0.3f;
        [SerializeField] protected int AtkTimes = 1;
        [SerializeField] protected float AtkParam = 1;
        [SerializeField] private float BreakDis = 20;
        [SerializeField] private GameObject JumpBullet;



        private PetLockDisTools m_LockTools;
        private float m_AnimTime = 2f;
        private float m_DtTime;
        private Vector3 m_LockPos;
        private Vector3 m_StartPos;
        // step val

        private float m_SkillTime;
        private float m_SkillStep1Time = 0.80f;
        private float m_SkillStep2Time = 1.10f;

        private bool m_EndSkill = false;
        //private float m_SkillStep3Time = 0.7f;
        //private float m_SkillStep4Time = 1.5f;

        public override void Init(BasePet pet, PetAtkState controller)
        {
            base.Init(pet, controller);
            m_AtkTimes = AtkTimes;
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WManiac);
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Maniac[(int)MWeapon.Atk];

            int curLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(GameConstVal.WManiac);
            GunsSub_dataItem levelItem = Global.gApp.gGameData.GunSubDataConfig.Get(curLv);
            double[] weaponParams = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + weaponItem.qualevel, levelItem);

            float campAtkInc = Global.gApp.CurScene.GetMainPlayerComp().GetBuffMgr().CampPetIncAtk;
            m_Damage = atk * AtkParam * weaponParams[0] * (1 + campAtkInc);

            m_DtTime = weaponItem.dtime + m_AnimTime;
            m_LockTools = new PetLockDisTools(transform, pet);
            m_LockTools.MaxRadio = 1;
            m_LockTools.MinRadio = 1;
            m_LockTools.BreakDis = BreakDis;
            m_LockTools.SetBreakCall(BreakCall);
            m_LockTools.SetFollowState(false);
        }
        private void BreakCall()
        {
            m_InBreakState = true;
            m_Pet.PlayAnim(GameConstVal.Run);
            if (m_Controller.CheckCanEnterOtherState() && m_InBreakState)
            {
                m_Controller.EndFirstSkill();
            }
        }
        public override void SetLockMonster(Transform monsterTsf, Monster monster, float monsterRadio)
        {
            float radio = monsterRadio + m_Pet.CircleRadio;
            base.SetLockMonster(monsterTsf, monster, monsterRadio);
            m_LockTools.MaxRadio = radio + MaxOffset;
            m_LockTools.MinRadio = radio + MinOffset;
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
            m_CurSkillStep = ManiacSkillStep.None;
       
        }

        public override void StartSkill()
        {
            m_EndSkill = false;
            m_InBreakState = false;
            m_CurTime = m_DtTime - 0.01f;
            m_LockTools.SetLockTsf(m_LockMonsterTsf);
            m_CurSkillStep = ManiacSkillStep.None;
        }
        public override bool CanTriggerSkill()
        {
            return m_AtkTimes > 0;
        }
        private void AddBullet()
        {
            GameObject go = Instantiate(JumpBullet);
            ManiacJumpBullet bulletComp = go.GetComponent<ManiacJumpBullet>(); ;
            bulletComp.Init(m_Damage,m_LockPos);
        }
        private void ExecuteStep(float dt)
        {
            m_Pet.SetSpeed(Vector2.zero);
            m_SkillTime += dt;
            if (m_CurSkillStep == ManiacSkillStep.Step1)
            {
                m_Pet.SetColliderTriggerEnable(true);
                float ratio = m_SkillTime / m_SkillStep1Time;
                if (m_SkillTime > m_SkillStep1Time)
                {
                    m_SkillTime = 0;
                    m_CurSkillStep = ManiacSkillStep.Step2;
                    AddBullet();
                }
                transform.position = Vector3.Lerp(m_StartPos, m_LockPos, Mathf.Min(1, ratio));
 
            }
            else if (m_CurSkillStep == ManiacSkillStep.Step2) // fly
            {
                m_Pet.SetColliderTriggerEnable(false);
                if (m_SkillTime > m_SkillStep2Time)
                {
                    m_CurSkillStep = ManiacSkillStep.None;
                    m_SkillTime = 0;
                    m_Pet.PlayAnim(GameConstVal.Idle);
                }
            }
        }
        public override void MUpdate()
        {
            float dtTime = BaseScene.GetDtTime();
            m_CurTime += dtTime;
            if (dtTime > 0 && !m_InBreakState)
            {
                if (!m_EndSkill)
                {
                    m_LockTools.Update();
                }
                if (!m_InBreakState)
                {
                    ExecuteStep(dtTime);
                    if(m_LockMonster != null && m_LockMonster.InDeath)
                    {
                        m_EndSkill = true;
                    }
                    if (m_LockMonster != null && !m_LockMonster.InDeath && !m_EndSkill)
                    {
                        // 技能执行中 
                        if (m_AtkTimes > 0 && m_CurTime > m_DtTime)
                        {
                            m_Pet.PlayAnim(GameConstVal.Skill02);
                            m_CurTime = m_CurTime - m_DtTime;
                            m_CurSkillStep = ManiacSkillStep.Step1;
                            m_SkillTime = 0;
                            m_AtkTimes--;
                            m_LockPos = m_LockMonster.transform.position;
                            m_StartPos = m_Pet.transform.position;
                        }
                        else if (m_AtkTimes == 0 && m_CurTime > m_AnimTime)
                        {
                            m_AtkTimes = AtkTimes;
                            m_Pet.PlayAnim(GameConstVal.Run);
                            m_Controller.EndSecondSkill();
                            m_Controller.TriggerFirstSkill();
                        }
                    }
                    else if(m_CurTime > m_AnimTime) // 必须等待动作执行完毕
                    {
                        if (m_AtkTimes == 0)
                        {
                            m_AtkTimes = AtkTimes;
                            m_Controller.EndSecondSkill();
                        }
                        else
                        {
                            m_Controller.EndSkill();
                        }
                        m_Pet.PlayAnim(GameConstVal.Idle);
                    }
                }
                else
                {
                    m_Pet.SetSpeed(Vector2.zero);
                }
            }
            else
            {
                m_Pet.SetSpeed(Vector2.zero);
            }
        }
    }
}
