using EZ.Data;
using EZ.Weapon;
using Game;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class Player : MonoBehaviour, IFightLockEnemyObj
    {
        public float m_Weight = 1f;
        public Transform RoleNode;
        public Transform EffectNode;
        public Transform HPNode;
        public GameObject DangerEffect1;
        public GameObject DangerEffect2;
        public GameObject SafeEffect1;
        public GameObject SafeEffect2;

        [SerializeField] private float m_Speed = 10f;
        [SerializeField] private float m_BaseSpeed = 6f;
        private Rigidbody2D m_Rigidbody2D;
        private CircleCollider2D m_CircleCollider2D;

        private WaveMgr m_WaveMgr;
        private CarrieMgr m_Carrier;
        private FightWeaponMgr m_WeaponMgr;
        private FightPetMgr m_PetMgr;
        private BuffMgr m_BuffMgr;
        private PlayerData m_PlayerData;

        private BeatBackAct m_BackAct;
        private GameObject m_DamageRange;


        private Fight m_Fight;
        private bool m_LockMove = false;
        private int m_DodgeVal = -100;
        private int m_CritVal = -100;
        private float m_CritDamage = -1.0f;
        private float m_Angle = -1f;
        private bool m_EnableVIT = false;
        private string m_InitCar;
        private bool m_InNormalScene = true;
        private float m_GoldRate = 1.0f;
        public float GoldRate
        {
            get { return m_GoldRate; }
        }
        public bool Idle { get { return m_Angle < 0f; } }
        public float angle { get { return m_Angle; } set { m_Angle = value; } }

        public float MonsterDamage { get; set; } = 0;
        private void Start()
        {

            m_WaveMgr = Global.gApp.CurScene.GetWaveMgr();
            m_DamageRange = transform.Find("ModelNode/Damage").gameObject;
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_BackAct = GetComponent<BeatBackAct>();
            m_BackAct.Init(this, m_Weight);
            m_Carrier = new CarrieMgr(this);
            m_WeaponMgr = new FightWeaponMgr(this);
            m_PetMgr = new FightPetMgr(this);
            m_BuffMgr = new BuffMgr(this);
            m_PlayerData = new PlayerData(this, m_BuffMgr);
            m_PlayerData.Init();
            m_Fight = new Fight(this);
            m_Fight.SetRotateNode(RoleNode);
            m_WeaponMgr.InitGun();

            //int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExSpeed);
            //Skill_dataItem skillLevelData = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
            //float addParam = (skillLevelData == null) ? 1f : skillLevelData.skill_exspeed[0];
            float addParam = 1.0f + 0.2f * (m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.MoveSpeed) /
                            (m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.MoveSpeed) + 5.0f *
                            PlayerDataMgr.singleton.GetPlayerLevel() + 1.0f));

            m_Speed *= addParam;

            Debug.Log("m_Speed " + m_Speed);

            m_GoldRate = 1.0f + m_WeaponMgr.GetCombatAttrValue(Game.CombatAttr.CombatAttrType.Gold_Add);

            Debug.Log("m_GoldRate " + m_GoldRate);
            //m_EnableVIT = (Global.gApp.CurScene.GetPassData().enableVIT != 0);
            //if (m_EnableVIT)
            //{
            //    m_BuffMgr.AddBuff(BuffType.PlayerEnergy, 10, 10);
            //}
            m_InNormalScene = Global.gApp.CurScene.IsNormalPass();
            if (m_InNormalScene)
            {
                m_WeaponMgr.SetCurMainWeaponEnabled(false);
                //m_PetMgr.ChangePet(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet());
            }
            //if (m_InitCar != null)
            //{
            //    GameObject CarNode = Global.gApp.gResMgr.InstantiateObj(m_InitCar);
            //    SetCarrier(CarNode, 999999999, 6);
            //}
            SetSafe(true, false);

            //m_Damage = 25 * (1 + 0.25f) ^ (怪攻击等级 - 1);
            MonsterDamage = Mathf.Ceil(25 * Mathf.Pow((1 + 0.25f), (PlayerDataMgr.singleton.StageAtkLevel - 1)));


            Global.gApp.gMsgDispatcher.Broadcast<int, Transform>(MsgIds.AddPlayerHpUi, 0, HPNode);

        }
        public void SetInitCar(string carPath)
        {
            m_InitCar = carPath;
        }
        private void Update()
        {
            float time = BaseScene.GetDtTime();
            m_Fight.Update();
            m_WeaponMgr.Update();
            //m_PetMgr.Update(time);
            m_BuffMgr.Update(time);
            m_PlayerData.Update();
        }
        public void SetSafe(bool isSafe, bool varySafe = false)
        {
            if (!m_InNormalScene)
            {
                varySafe = true;
            }
            if (varySafe)
            {
                SafeEffect1.SetActive(false);
                SafeEffect2.SetActive(false);
                DangerEffect1.SetActive(false);
                DangerEffect2.SetActive(false);
            }
            else if (isSafe)
            {
                SafeEffect1.SetActive(true);
                SafeEffect2.SetActive(true);
                DangerEffect1.SetActive(false);
                DangerEffect2.SetActive(false);
            }
            else
            {
                SafeEffect1.SetActive(false);
                SafeEffect2.SetActive(false);
                DangerEffect1.SetActive(true);
                DangerEffect2.SetActive(true);
            }
        }

        public Fight GetFight()
        {
            return m_Fight;
        }
        public void Move(float sx, float sy)
        {
            if (!m_LockMove)
            {
                m_Fight.Move(sx, sy);
            }
        }
        public void SetColliderState(bool mEnable)
        {
            m_CircleCollider2D.enabled = mEnable;
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.simulated = mEnable;
            m_DamageRange.SetActive(mEnable);
        }
        public void SetCircleCollider2DEnable(bool mEnable, GameObject carrierNode = null)
        {
            m_Fight.AdaptFireAngle();
            SetColliderState(mEnable);
            m_Fight.SetLockEnemy(null);
            EndBackAct();
            if (mEnable)
            {
                m_Fight.ClearAnimName();
                m_Fight.SetRotateNode(RoleNode);
            }
            else
            {
                m_Fight.SetRotateNode(carrierNode.transform);
            }
        }

        public CarrieMgr GetCarrier()
        {
            return m_Carrier;
        }
        public WaveMgr GetWaveMgr()
        {
            return m_WaveMgr;
        }

        public float GetSpeed()
        {
            float newSpeed = m_Speed * (1 + m_BuffMgr.GetIncMoveSpeed());

            if (m_EnableVIT)
            {
                return (m_BuffMgr.HasEnergy() ? newSpeed : newSpeed / 2);
            }
            else
            {
                return newSpeed;
            }
        }

        public FightWeaponMgr GetWeaponMgr()
        {
            return m_WeaponMgr;
        }

        public BuffMgr GetBuffMgr()
        {
            return m_BuffMgr;
        }
        public void DestroyCarrier(bool stopAudio)
        {
            m_Carrier.DestroyCarrier(stopAudio);
        }
        public void SetCarrier(GameObject cardNode, float liveTime, float speed, float damageCoefficient = 1)
        {
            m_Carrier.SetCarrier(cardNode, liveTime, speed, damageCoefficient);
        }
        public int GetCritVal()
        {
            if (m_CritVal < 0)
            {
                //int critLv = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExCrit);
                //if (critLv > 0)
                //{
                //    float[] skill_excrit = Global.gApp.gGameData.SkillDataConfig.Get(critLv).skill_excrit;
                //    if (skill_excrit != null)
                //    {
                //        m_CritVal = (int)(skill_excrit[0] * 100);
                //    }
                //    else
                //    {
                //        m_CritVal = 0;
                //    }
                //}
                //else
                //{
                //    m_CritVal = 0;
                //}

                float addParam = 0.5f * ( m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.Crit_Rate) /
                                (m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.Crit_Rate) +
                                10.0f * PlayerDataMgr.singleton.GetPlayerLevel() + 1.0f) );

                m_CritVal = (int)(addParam * 100);

                Debug.Log("m_CritVal " + m_CritVal);
            }
            return m_CritVal;
        }

        public float GetCritDamage()
        {
            if (m_CritDamage < 0.0f)
            {
                m_CritDamage = 1.0f + 4.0f * (m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.Crit_Damage) /
                                (m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.Crit_Damage) +
                                5.0f * PlayerDataMgr.singleton.GetPlayerLevel() + 1.0f));

                Debug.Log("m_CritDamage " + m_CritDamage);

            }
            return m_CritDamage;
        }

        private int GetDodgeVal()
        {
            if (m_DodgeVal < 0)
            {
                //int dodgeLv = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExDodge);
                //if (dodgeLv > 0)
                //{
                //    float[] skill_exdodge = Global.gApp.gGameData.SkillDataConfig.Get(dodgeLv).skill_exdodge;
                //    if (dodge > 0.0f)
                //    {
                //        m_DodgeVal = (int)(dodge * 100);
                //    }
                //    else
                //    {
                //        m_DodgeVal = 0;
                //    }
                //}
                //else
                //{
                //    m_DodgeVal = 0;
                //}

                float dodge = 0.3f * (m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.Dodge) /
                            (m_WeaponMgr.GetCombatAttrValue(CombatAttr.CombatAttrType.Dodge) + 
                            10.0f * PlayerDataMgr.singleton.GetPlayerLevel() + 0.0f));


                m_DodgeVal = (int)(dodge * 100);

                Debug.Log("m_DodgeVal " + m_DodgeVal);
            }
            return m_DodgeVal;
        }
        public void LockMove(float time)
        {
            if (time > 0)
            {
                m_Fight.Move(0, 0);
                SetLockState(true);
                Transform lockTsf = transform.Find("lockCallBack");

                if (lockTsf == null)
                {
                    GameObject emepty = new GameObject();
                    emepty.transform.SetParent(transform);
                    emepty.name = "lockCallBack";
                    DelayCallBack delayCallBack = emepty.AddComponent<DelayCallBack>();
                    delayCallBack.SetAction(() =>
                    {
                        Destroy(emepty);
                        SetLockState(false);
                    }, time);
                }
                else
                {
                    lockTsf.GetComponent<DelayCallBack>().ResteTime();
                }
            }
        }
        private void SetLockState(bool lockState)
        {
            m_LockMove = lockState;
            if (!lockState)
            {
                DestroyLockEffect();
            }
        }
        private void DestroyLockEffect()
        {
            Transform elecEffect = transform.Find(GameConstVal.LockEffect);
            if (elecEffect != null)
            {
                Destroy(elecEffect.gameObject);
            }
        }
        public void OnHitted(float damage)
        {
            if (m_PlayerData.CanBeHitted())
            {
                if (m_BuffMgr.CheckHasBuff(BuffType.NaNoboostBuff))
                {
                    return;
                }
                int dodgeVal = GetDodgeVal();
                if (dodgeVal > 0)
                {
                    int randVal = Random.Range(0, 101);
                    if (randVal < dodgeVal)
                    {
                        GameObject missObj = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Fighttips_miss);
                        missObj.transform.SetParent(transform, false);
                        return;
                    }
                }
                if (m_BuffMgr.CheckHasBuff(BuffType.ShieldBuff))
                {
                    m_BuffMgr.RemoveBuff(BuffType.ShieldBuff);
                    m_PlayerData.ResetProtectTime();
                    return;
                }
                m_PlayerData.OnHit(damage);
            }
        }
        public void ChangeThirdWeapon(string weaponName, float keepTime, float damageCoefficient)
        {
            m_WeaponMgr.ChangeThirdWeapon(weaponName, keepTime, damageCoefficient);
        }
        public void ChangeWeapon(string weaponName, float keepTime, float damageCoefficient)
        {
            m_WeaponMgr.ChangeWeapon(weaponName, keepTime, damageCoefficient);
        }
        public void ChangePet(string petName, float keepTime, float damageCoefficient)
        {
            //m_PetMgr.ChangeSecondPet(petName, keepTime);
        }
        public PlayerData GetPlayerData()
        {
            return m_PlayerData;
        }
        public bool CanBackAct()
        {
            if (m_PlayerData.CanBeHitted() && !m_BuffMgr.CheckHasBuff(BuffType.NaNoboostBuff)
                && !m_BuffMgr.CheckHasBuff(BuffType.ShieldBuff))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void StartBackActOnVec(Transform transform)
        {
            if (CanBackAct())
            {
                if (m_Carrier.HasCarrier())
                {
                    return;
                }
                SetLockState(true);
                m_BackAct.enabled = true;
                m_BackAct.OnHit_Vec(transform);
            }
        }
        public void StartBackActToPos(Vector3 position, float time)
        {
            {
                if (m_Carrier.HasCarrier() || time <= 0)
                {
                    return;
                }
                SetLockState(true);
                m_BackAct.enabled = true;
                m_BackAct.StartBackActToPos(position, time);
            }
        }
        public void StartBackActOnPos(Transform transform)
        {
            if (CanBackAct())
            {
                if (m_Carrier.HasCarrier() || m_BackAct.enabled)
                {
                    return;
                }
                SetLockState(true);
                m_BackAct.enabled = true;
                m_BackAct.OnHit_Pos(transform);
            }
        }
        public void OnHit_RealVec(Transform bulletTsf, Vector3 hitVec)
        {
            if (CanBackAct())
            {
                if (m_Carrier.HasCarrier() || m_BackAct.enabled)
                {
                    return;
                }
                SetLockState(true);
                m_BackAct.enabled = true;
                m_BackAct.OnHit_RealVec(bulletTsf, hitVec);
            }
        }
        public void SetSpeed(Vector2 speedVec)
        {
            float speed = speedVec.magnitude;
            float rate = speed / m_BaseSpeed;
            m_Rigidbody2D.velocity = speedVec;
            if (m_InNormalScene)
            {
                m_Fight.SetMoveAnimSpeed(rate);
            }
        }
        public void EndBackAct()
        {
            SetLockState(false);
            m_BackAct.enabled = false;
        }
        public FightPetMgr GetFightPetMgr()
        {
            return m_PetMgr;
        }
        public void OnDestroy()
        {
            m_PlayerData.Destroy();
            m_Fight.Destroy();
            m_PetMgr.Destroy();
        }

        public GameObject GetLockEnemy()
        {
            var enemyGo = GetFight().GetLockEnemy();
            return enemyGo;
        }
    }
}
