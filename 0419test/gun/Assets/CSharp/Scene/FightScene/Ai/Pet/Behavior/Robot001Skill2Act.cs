using EZ.Data;
using UnityEngine;
namespace EZ
{
    public class Robot001Skill2Act : PetBaseAct
    {

        [Tooltip(" 技能触发  跟随怪物的速度 系数")]
        [SerializeField] protected float FollowMonsterSpeedParam = 1;

        [SerializeField] protected int AtkTimes = 1;
        [SerializeField] protected float AtkParam = 1;
        [SerializeField] protected float AtkDtTimeParam = 1;
        [SerializeField] private float BreakDis = 20;
        private PetLockDisTools m_LockTools;
        private float m_AnimTime = 1f;
        private float m_DtTime;
        public ParticleSystem m_ChuXianEffect;
        public ParticleSystem m_XiaoShiEffect;
        public bool m_ChiXuSate = false;
        public bool m_XiaoShiState = false;

        private bool m_ForceEndSkill = false;
        private bool m_EndSkill = false;
        private void Awake()
        {
            SetEffectState(false,false,true);
        }
        public override void Init(BasePet pet, PetAtkState controller)
        {
            base.Init(pet, controller);
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WRobot);
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Robot[(int)MWeapon.Atk];
            m_DtTime = weaponItem.dtime * AtkDtTimeParam;

            int curLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(GameConstVal.WRobot);
            GunsSub_dataItem levelItem = Global.gApp.gGameData.GunSubDataConfig.Get(curLv);
            double[] weaponParams = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + weaponItem.qualevel, levelItem);

            float campAtkInc = Global.gApp.CurScene.GetMainPlayerComp().GetBuffMgr().CampPetIncAtk;
            m_Damage = atk * AtkParam * weaponParams[0] * (1 + campAtkInc);

            m_LockTools = new PetLockDisTools(transform, pet);
            m_LockTools.MaxRadio = 1;
            m_LockTools.MinRadio = 1;
            m_LockTools.BreakDis = BreakDis;
            m_LockTools.SetBreakCall(BreakCall);
            m_AtkTimes = AtkTimes;
        }
        private void BreakCall()
        {
            m_ForceEndSkill = true;
            m_InBreakState = true;
            m_Pet.PlayAnim(GameConstVal.Run);
            if (m_Controller.CheckCanEnterOtherState() && m_InBreakState)
            {
                m_Controller.EndSecondSkill();
            }
        }
        public override bool CanTriggerSkill()
        {
            return m_AtkTimes > 0;
        }
        public override void EndSkill()
        {
            //SetLockMonster(null, null);
            SetEffectState(false, true);
        }
        public override void EndEffect()
        {
            SetEffectState(false, true);
        }
        public void SetEffectState(bool chuxian,bool xiaoshi,bool forceEffect = false)
        {
            if (forceEffect || chuxian != m_ChiXuSate)
            {
                if (chuxian)
                {
                    m_ChuXianEffect.Play();
                }
                else
                {
                    m_ChuXianEffect.Stop();
                }
                m_ChiXuSate = chuxian;
            }
            if (forceEffect || xiaoshi != m_XiaoShiState)
            {
                if (xiaoshi)
                {
                    m_XiaoShiEffect.Play();
                }
                else
                {
                    m_XiaoShiEffect.Stop();
                }
            }
        }
        public override void StartSkill()
        {
            m_ForceEndSkill = false;
            m_InBreakState = false;
            m_EndSkill = false;
            m_CurTime = m_DtTime - 0.5f;
            m_Pet.PlayAnim(GameConstVal.Skill02, -1, 0.1f);
            m_LockTools.SetLockTsf(m_LockMonsterTsf);
        }
        public override void SetLockMonster(Transform monsterTsf, Monster monster, float monsterRadio)
        {
            float radio = monsterRadio + m_Pet.CircleRadio;
            base.SetLockMonster(monsterTsf, monster, monsterRadio);
            m_LockTools.MaxRadio = radio + MaxOffset;
            m_LockTools.MinRadio = radio + MinOffset;
            m_LockTools.ResetCurRadio();
            m_LockTools.SetFollowSpeed(monster.GetMonsterItem().baseSpeed * FollowMonsterSpeedParam);
        }
        public void ResetInfo()
        {
            m_ForceEndSkill = false;
            m_EndSkill = false;
            m_LockTools.SetLockTsf(m_LockMonsterTsf);
        }
        public override void MUpdate()
        {
            float dtTime = BaseScene.GetDtTime();
            m_CurTime += dtTime;
            if (dtTime > 0 && !m_InBreakState)
            {
                if (!m_EndSkill && m_LockMonster != null && !m_LockMonster.InDeath)
                {
                    m_LockTools.Update();
                }
                if (!m_InBreakState)
                {
                    if (m_LockMonster != null && m_LockMonster.InDeath)
                    {
                        m_EndSkill = true;
                        EndEffect();
                    }
                    if (m_LockMonster != null && !m_LockMonster.InDeath && !m_EndSkill && !m_ForceEndSkill)
                    {
                        if (m_AtkTimes > 0 && m_CurTime > m_DtTime)
                        {
                            SetEffectState(true, false);
                            m_CurTime = m_CurTime - m_DtTime;
                            m_AtkTimes--;
                            InstanceBullet();
                        }
                        else if (m_AtkTimes == 0 && m_CurTime > m_AnimTime)
                        {
                            m_AtkTimes = AtkTimes;
                            m_Pet.PlayAnim(GameConstVal.Run);
                            m_Controller.EndSecondSkill();
                            m_Controller.TriggerFirstSkill();
                        }
                    }
                    else if (m_CurTime > m_AnimTime) // 必须等待动作执行完毕
                    {
                        m_Pet.PlayAnim(GameConstVal.Idle);
                        m_Controller.EndSkill();
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
        private void InstanceBullet()
        {
            BaseBullet skillBullet = Global.gApp.gGameCtrl.BulletCache.GetBullet(BulletConfig.Bullet_RobotSkill02);
            skillBullet.transform.localScale = new Vector3(1, 1, 1);
            skillBullet.transform.SetParent(transform, false);
            skillBullet.Init(m_Damage, transform, 0, 0);
        }
    }
}
