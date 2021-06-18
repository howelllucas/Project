using EZ.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class CtrlEleFireTurret : BaseTurret, ICtrlElementNode
    {
        public Animator m_AnimTurret;
        public Animator m_AnimTurretBase;
        public Transform m_AtkRange;

        private GameWeapon m_WeaponName = GameWeapon.S_FireTurret;
        private float m_CurTime = 0;
        private float m_CheckDtTime = 0.5f;
        private float m_BaseRadius = 3.571f;
        private float m_Radius = 5.0f;
        private float m_RadiusSqr = 25f;
        private string m_TurretAnim = GameConstVal.EmepyStr;
        private string m_TurretBaseAnim = GameConstVal.EmepyStr;
        private Transform m_RotateRoleNode = null;
        private GameObject m_LockGameEnemy = null;
        private Gun m_Gun;
        private WaveMgr m_WaveMgr;
        private float m_EndedTime = 1.5f;
        private bool m_Borned = false;
        CElement.EleState m_CurEleState = CElement.EleState.None;

        void Start()
        {
            //GetComponent<DelayDestroy>().AddLiveTime(m_EndedTime);
            //GetComponent<DelayDestroy>().enabled = false;
            m_Gun.enabled = false;
            m_RotateRoleNode = transform.Find(GameConstVal.RotNode);
            m_WaveMgr = Global.gApp.CurScene.GetWaveMgr();
        }
        void Update()
        {
            if (m_Borned)
            {
                if (m_LockGameEnemy == null || m_LockGameEnemy.GetComponent<Monster>().InDeath)
                {
                    SetLockEnemy(null);
                    FindLockEnemy();
                    m_CurTime = 0;
                }
                else
                {
                    m_CurTime = m_CurTime + BaseScene.GetDtTime();
                    if (m_CurTime >= m_CheckDtTime)
                    {
                        m_CurTime = m_CurTime - m_CheckDtTime;
                        FindLockEnemy();
                    }
                }
                FaceToEnemyImp();
            }
        }
        public void SetLockEnemy(GameObject lockEnemy)
        {
            m_LockGameEnemy = lockEnemy;
        }

        private void FindLockEnemy()
        {
            Vector3 m_position = transform.position;
            SetLockEnemy(null);
            float m_lastPositionSqure = 0;
            Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
            foreach (Wave wave in waves.Values)
            {
                List<Monster> monsters = wave.GetMonsters();
                foreach (Monster monster in monsters)
                {
                    Vector3 postion = monster.transform.position;
                    Vector3 dtPosition = m_position - postion;
                    float curDtSqure = dtPosition.sqrMagnitude;
                    if (curDtSqure <= m_RadiusSqr)
                    {
                        if (m_LockGameEnemy != null)
                        {
                            if (curDtSqure < m_lastPositionSqure)
                            {
                                m_lastPositionSqure = curDtSqure;
                                SetLockEnemy(monster.gameObject);
                                if (curDtSqure <= 3.2f)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            SetLockEnemy(monster.gameObject);
                            m_lastPositionSqure = (m_LockGameEnemy.transform.position - m_position).sqrMagnitude;
                            if (m_lastPositionSqure <= 3.2f)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void FaceToEnemyImp()
        {
            if (m_LockGameEnemy)
            {
                float angleZ = EZMath.SignedAngleBetween(m_LockGameEnemy.transform.position - m_RotateRoleNode.position, Vector3.up);

                CalcAngles(angleZ);
                if (!m_Gun.enabled)
                {
                    m_Gun.enabled = true;
                }
            }
            else
            {
                if (m_Gun.enabled)
                {
                    m_Gun.enabled = false;
                }
                PlayTurretAnim(GameConstVal.Idle_Deploy);
            }
        }
        private void CalcAngles(float angleZ)
        {
            Vector3 eulerAngle = m_RotateRoleNode.localEulerAngles;
            float dtAngle = angleZ - eulerAngle.z;
            if (dtAngle > 180)
            {
                dtAngle = (dtAngle) - 360;
            }
            else if (dtAngle < -180)
            {
                dtAngle = (dtAngle) + 360;
            }
            angleZ = eulerAngle.z + dtAngle * BaseScene.GetDtTime() * 10;
            m_RotateRoleNode.localEulerAngles = new Vector3(0, 0, angleZ);
        }

        public override void PlayTurretAnim(string anim, bool forcePlay = false)
        {
            if (forcePlay || (anim != null && !m_TurretAnim.Equals(anim)))
            {
                m_TurretAnim = anim;
                m_AnimTurret.Play(anim);
            }
        }
        private void PlayAnim(string turretAnim, string baseAnim)
        {
            PlayTurretAnim(turretAnim);
            PlayTurretBaseAnim(baseAnim);
        }
        private void PlayTurretBaseAnim(string anim)
        {
            if (anim != null && !m_TurretBaseAnim.Equals(anim))
            {
                m_TurretBaseAnim = anim;
                if (m_AnimTurretBase != null)
                {
                    m_AnimTurretBase.Play(anim);
                }
            }
        }
        private void StartBorn()
        {
            PlayTurretAnim(GameConstVal.Born);
            gameObject.AddComponent<DelayCallBack>().SetAction(TurretBorn, 1.34f);
        }
        void TurretEnded()
        {
            PlayTurretAnim(GameConstVal.Idle);
            m_Gun.enabled = false;
        }
        void TurretBorn()
        {
            PlayTurretAnim(GameConstVal.Idle_Deploy);
            m_Gun.enabled = true;
            m_Borned = true;
            m_AtkRange.gameObject.SetActive(true);
            //GetComponent<DelayDestroy>().enabled = true;
            //float liveTime = GetComponent<DelayDestroy>().GetLiveTime();
            m_Ref++;
            //gameObject.AddComponent<DelayCallBack>().SetAction(TurretEnded, liveTime - m_EndedTime);
            //Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.AddFightUICountItem, GameConstVal.WSFireTurret + m_Ref, GameConstVal.WSFireTurret, liveTime - m_EndedTime);
        }
        public override void SetAtkRange(float atkRange)
        {
            if (m_CurEleState == CElement.EleState.Open)
            {
                m_AtkRange.gameObject.SetActive(true);
            }
            m_Radius = atkRange;
            m_RadiusSqr = atkRange * atkRange;
            float scale = atkRange / m_BaseRadius;
            m_AtkRange.localScale = new Vector3(scale, scale, 1);
        }

        void SetChildActive(bool active)
        {
            int childNum = transform.childCount;
            for (int i = 0; i < childNum; i++)
            {
                transform.GetChild(i).gameObject.SetActive(active);
            }
        }
        void ICtrlElementNode.SetEleOpen()
        {
            m_CurEleState = CElement.EleState.Open;
            m_AtkRange.gameObject.SetActive(true);
            PlayTurretAnim(m_TurretAnim, true);
            StartBorn();
        }

        void ICtrlElementNode.SetEleClose()
        {
            m_Borned = false;
            m_CurEleState = CElement.EleState.Close;
            m_AtkRange.gameObject.SetActive(false);
            TurretEnded();
        }

        void ICtrlElementNode.SyncStartEleState(CElement.EleState eleState)
        {
            m_Gun = GetComponentInChildren<Gun>();
            if (eleState == CElement.EleState.Open)
            {
                SetChildActive(false);
                gameObject.AddComponent<DelayCallBack>().SetAction(() =>
                {
                    SetChildActive(true);
                    PlayTurretAnim(m_TurretAnim, true);
                    StartBorn();
                }, 0.1f);
                m_CurEleState = CElement.EleState.Open;
                m_AtkRange.gameObject.SetActive(true);
            }
            else if(eleState == CElement.EleState.Close)
            {
                ((ICtrlElementNode)this).SetEleClose();
            }
        }
    }
}

