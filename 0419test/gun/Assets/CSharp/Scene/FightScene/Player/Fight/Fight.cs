using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class Fight
    {

        private string m_UAnim = GameConstVal.EmepyStr;
        private string m_DAnim = GameConstVal.EmepyStr;
        private float m_CurTime = 0;
        private float m_CheckDtTime = 0.3f;
        private float m_BaseRadius = 5.0f;
        private float m_Radius = 5.0f;
        private float m_RadiusSqr = 5.0f;

        private Player m_Player;
        private Animator m_Anim;
        private Transform m_RotateRoleNode;
        private Transform m_Transform;
        private GameObject m_LockGameEnemy;
        private FightWeaponMgr m_WeaponMgr;
        private WaveMgr m_WaveMgr;
        private CarrieMgr m_Carrier;
        private Transform m_FootEffectNode;
        private Transform m_GndNode;
        private FollowEnemy m_LockEnemyEffect;
        private bool m_InNormalPassType;
        private SceneType m_PassType;
        private Vector3 m_OriFirePointPos;
        private Transform m_FirePoint;
        public Fight(Player player )
        {
            m_FootEffectNode = player.transform.Find("ModelNode/FootEffect");
            m_GndNode = player.transform.Find("GndNode");
            m_WeaponMgr = player.GetWeaponMgr();
            m_WaveMgr = player.GetWaveMgr();
            m_Carrier = player.GetCarrier();
            m_Transform = player.transform;
            m_Player = player;
            m_Anim = m_Player.transform.Find("ModelNode/hero").GetComponent<Animator>();
            PlayAnim(GameConstVal.Idle, GameConstVal.Idle);
            m_RadiusSqr = m_Radius * m_Radius;

            GameObject lockEffect = Global.gApp.gResMgr.InstantiateObj("Prefabs/Effect/common/chooseenemy");
            m_LockEnemyEffect = lockEffect.GetComponent<FollowEnemy>();
            lockEffect.transform.SetParent(Global.gApp.gRoleNode.transform, false);
            m_InNormalPassType = Global.gApp.CurScene.IsNormalPass();
            m_PassType = Global.gApp.CurScene.GetSceneType();
            AdaptFireAngle();
        }
        public void AdaptFireAngle()
        {
            if (!m_InNormalPassType)
            {
                Transform firePoint = m_Player.transform.Find("ModelNode/hero/FirePoint");
                Vector3 localAngle = firePoint.localEulerAngles;
                localAngle.y = -90;
                firePoint.localEulerAngles = localAngle;
                m_OriFirePointPos = firePoint.transform.localPosition;
                m_FirePoint = firePoint;
            }
        }
        public void ResetFirepointPos()
        {
            if (m_FirePoint != null)
            {
                m_FirePoint.transform.localPosition = m_OriFirePointPos;
            }
        }
        public void Update()
        {
            if (!m_Carrier.HasCarrier())
            {
                if (m_InNormalPassType)
                {
                    FaceToEnemy();
                }
            }
            else
            {
                m_Carrier.Update(BaseScene.GetDtTime());
                m_Player.SetSafe(true, true);
            }
        }
        private void FaceToEnemy()
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
            if (m_LockGameEnemy != null)
            {
                m_Player.SetSafe(false);
            }
            else
            {
                m_Player.SetSafe(true);
            }
        }
        public Transform GetLockEnemyTsf()
        {
            if (m_LockGameEnemy != null)
            {
                return m_LockGameEnemy.transform;
            }
            else
            {
                return null;
            }
        }
        public GameObject GetLockEnemy()
        {
            return m_LockGameEnemy;
        }
        public void SetLockEnemy(GameObject lockEnemy)
        {
            m_LockGameEnemy = lockEnemy;
            if (lockEnemy != null)
            {
                m_LockEnemyEffect.SetTargetTsf(lockEnemy.transform);
            }
            else
            {
                m_LockEnemyEffect.SetTargetTsf(null);
            }
        }

        private void FindLockEnemy()
        {
            // 非普通模式不需要锁定怪物
            if (!m_InNormalPassType) { return; }
            Vector3 m_position = m_Transform.position;
            //if (m_LockGameEnemy != null) 
            //{
            //	Vector3 postion1 = m_LockGameEnemy.transform.position;
            //	Vector3 dtPosition1 = m_position - postion1;
            //	if (dtPosition1.sqrMagnitude <= m_RadiusSqr) {
            //		return;
            //	}
            //}

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
                                SetLockEnemy(monster.gameObject);
                                if (sqrMagnitude <= 0.8f)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            SetLockEnemy(monster.gameObject);
                            m_lastPositionSqure = (m_LockGameEnemy.transform.position - m_position).sqrMagnitude;
                            if (m_lastPositionSqure <= 0.8f)
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
                m_WeaponMgr.SetCurMainWeaponEnabled(true);
            }
            else
            {
                m_WeaponMgr.SetCurMainWeaponEnabled(false);
                //CalcAngles(0);
            }
        }
        public void ForceFaceToEnemy()
        {
            if (m_LockGameEnemy)
            {
                float angleZ = EZMath.SignedAngleBetween(m_LockGameEnemy.transform.position - m_RotateRoleNode.position, Vector3.up);
                CalcAngles(angleZ, true);
            }
        }
        private void CalcAngles(float angleZ, bool foceFace = false)
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
            if (!foceFace)
            {
                angleZ = eulerAngle.z + dtAngle * BaseScene.GetDtTime() * 10;
            }
            else
            {
                angleZ = eulerAngle.z + dtAngle;
            }
            m_RotateRoleNode.localEulerAngles = new Vector3(0, 0, angleZ);
        }

        public void PlayShotAnim(string anim)
        {
            if (anim != null)
            {
                m_UAnim = anim;
                m_Anim.Play(anim, -1, 0);
            }
        }
        public void PlayUAnim(string anim)
        {
            if (anim != null && !m_UAnim.Equals(anim))
            {
                m_UAnim = anim;
                m_Anim.Play(anim, -1, 0);
            }
        }
        public void SetAnimSpeed(float speed)
        {
            m_Anim.speed = speed;
        }
        public void SetMoveAnimSpeed(float speed)
        {
            m_Anim.SetFloat(GameConstVal.SpeedSymbolStr, speed);
        }
        public void PlayAnim(string uAnim = null, string dAnim = null)
        {
            PlayUAnim(uAnim);
            PlayDAnim(dAnim);
        }
        public void PlayDAnim(string anim)
        {
            if (anim != null && !m_DAnim.Equals(anim))
            {
                m_DAnim = anim;
                m_Anim.Play(anim, 1, 0);
            }
        }
        public void ClearAnimName()
        {
            m_DAnim = GameConstVal.EmepyStr;
            m_UAnim = GameConstVal.EmepyStr;
        }
        public void SetRotateNode(Transform rotateNode)
        {
            m_RotateRoleNode = rotateNode;
        }
        public void CalcCurAnim(float sx, float sy)
        {
            if (BaseScene.GetDtTime() <= 0)
            {
                return;
            }
            if (sx != 0 || sy != 0)
            {

                Vector3 speedV = new Vector3(sx, sy, 0);
                float angle = EZMath.SignedAngleBetween(speedV, Vector3.up);

                m_Player.angle = angle;

                // Debug.Log(angle);

                Vector3 roleEulerAngle = m_RotateRoleNode.eulerAngles;
                float roleAngleZ = roleEulerAngle.z;
                float relativeAngleZ = angle - roleAngleZ;
                relativeAngleZ = relativeAngleZ >= 0 ? relativeAngleZ : relativeAngleZ + 360;
                if (relativeAngleZ <= 45 || relativeAngleZ >= 315)
                {
                    PlayDAnim(GameConstVal.Run);
                }
                else if (relativeAngleZ >= 45 && relativeAngleZ <= 135)
                {
                    PlayDAnim(GameConstVal.Run_left);

                }
                else if (relativeAngleZ >= 135 && relativeAngleZ <= 215)
                {

                    PlayDAnim(GameConstVal.Run_back);
                }
                else
                {
                    PlayDAnim(GameConstVal.Run_right);
                }
            }
            else
            {
                m_Player.angle = -1f;
                PlayDAnim(GameConstVal.Idle);
            }
        }
        public void Move(float sx, float sy)
        {
            // 有载具 就先 走载具
            if (!m_Carrier.HasCarrier())
            {
                if (m_InNormalPassType)
                {
                    Vector2 velocity = new Vector2(sx, sy);
                    m_Player.SetSpeed(velocity.normalized * m_Player.GetSpeed() * BaseScene.TimeScale);
                    CalcCurAnim(sx, sy);
                    MoveImp(sx, sy);
                }
                else
                {
                    sy = 0;
                    Vector2 velocity = new Vector2(sx, sy);
                    m_Player.SetSpeed(velocity.normalized * m_Player.GetSpeed() * BaseScene.TimeScale);
                    CalcCurAnim(0, 1);
                    MoveImp(0, 1);
                }
            }
            else
            {
                if (m_InNormalPassType)
                {
                    m_Carrier.Move(sx, sy);
                    MoveImp(sx, sy);
                }
                else
                {
                    sy = Mathf.Abs(sx) * 3;
                    m_Carrier.Move(sx, 0);
                    if (sx == 0 && sy == 0)
                    {
                        sy = 1;
                    }
                    float angleZ = EZMath.SignedAngleBetween(new Vector3(sx, sy, 0), Vector3.up);
                    CalcAngles(angleZ);
                }
            }

        }
        public void MoveImp(float sx, float sy)
        {
            // Move the character

            if (m_LockGameEnemy == null)
            {
                if (sx != 0 || sy != 0)
                {
                    float angleZ = EZMath.SignedAngleBetween(new Vector3(sx, sy, 0), Vector3.up);
                    CalcAngles(angleZ);

                }
                if (!m_Carrier.HasCarrier())
                {
                    PlayUAnim(GameConstVal.Idle);
                }
            }
            else
            {
                if (!m_Carrier.HasCarrier())
                {

                }
            }
        }

        public bool HasCarrier()
        {
            return m_Carrier.HasCarrier();
        }
        public void SetRadius(float radius)
        {
            m_Radius = radius;
            m_RadiusSqr = m_Radius * m_Radius;
            float scale = radius / m_BaseRadius;
            if (radius < 22)
            {
                m_FootEffectNode.localScale = new Vector3(scale, scale, scale);
                m_GndNode.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                m_FootEffectNode.localScale = Vector3.zero;
                m_GndNode.localScale = Vector3.zero;
            }
        }
        public void Destroy()
        {
            m_Carrier.DestroyCarrier(true);
            if (m_LockEnemyEffect != null)
            {
                Object.Destroy(m_LockEnemyEffect.gameObject);
            }
        }
    }

}