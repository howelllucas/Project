using Pathfinding;
using Pathfinding.RVO;
using System;
using UnityEngine;

namespace EZ
{

    public class PetAutoPath : MonoBehaviour
    {
        AIPath m_AiPath;
        RVOController m_RvoController;
        Seeker m_Seeker;
        SimpleSmoothModifier m_SimpleSmooth;

        Transform m_LockTsf;
        GameObject m_PlayerGo;
        Player m_PlayerCompt;
        Action<bool> m_EndCallBack;
        private bool m_ReachPlace = false;
        private bool m_SynPlayerSpeed = false;
        float m_RotationSpeed = 360;
        float m_MoveSpeed = 6;
        float m_MoveSpeedScale = 1;
        float m_DisSqrOffset = 0.25f;
        private void Awake()
        {
            BeAutoPath();
            SetAutoPathEnable(false);
        }
        private void BeAutoPath()
        {
            // 增加RVO网格控制
            m_RvoController = gameObject.AddComponent<RVOController>();
            m_RvoController.agentTimeHorizon = 0.4f; // 检测代理碰撞的时间间隔
            m_RvoController.obstacleTimeHorizon = 0.6f; // 检测其他障碍碰撞的时间间隔
            m_RvoController.collidesWith = RVOLayer.DefaultAgent;
            m_RvoController.maxNeighbours = 2;
            m_RvoController.locked = false;

            // 寻路插件
            m_AiPath = gameObject.AddComponent<AIPath>();
            m_AiPath.radius = GetComponent<CircleCollider2D>().radius;
            m_AiPath.height = 1;
            m_AiPath.slowdownDistance = 0; // 无需减速
            m_AiPath.enableRotation = true;
            m_AiPath.orientation = OrientationMode.YAxisForward;
            m_AiPath.slowWhenNotFacingTarget = false;
            m_AiPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
            m_AiPath.rotationSpeed = m_RotationSpeed ;
            m_AiPath.pickNextWaypointDist = 1;
            m_AiPath.repathRate = 0.2f;
            m_AiPath.whenCloseToDestination = CloseToDestinationMode.Stop;
            // 增加平滑曲线
            m_SimpleSmooth = gameObject.AddComponent<SimpleSmoothModifier>();
            m_SimpleSmooth.maxSegmentLength = 1f;
            m_SimpleSmooth.iterations = 5;
            RaycastModifier m_raycast = gameObject.AddComponent<RaycastModifier>();
            m_raycast.quality = RaycastModifier.Quality.Low;
            m_raycast.useRaycasting = false;
            m_raycast.useGraphRaycasting = true;

            m_Seeker = GetComponent<Seeker>();
            m_Seeker.drawGizmos = true;

            // 设置初始速度
            m_AiPath.velocity2D = m_RvoController.velocity;

            m_PlayerCompt = Global.gApp.CurScene.GetMainPlayerComp();
            m_PlayerGo = Global.gApp.CurScene.GetMainPlayer();

        }
        public void Update()
        {
            if (m_Seeker.IsDone())
            {
                m_AiPath.destination = m_LockTsf.position;
            }
            Vector3 posStart = transform.position;
            Vector3 targetPos = m_LockTsf.transform.position;
            Vector2 velocity = new Vector2(targetPos.x - posStart.x, targetPos.y - posStart.y);
            if (velocity.sqrMagnitude > m_DisSqrOffset)
            {
                if (!m_SynPlayerSpeed)
                {
                    m_AiPath.maxSpeed = m_MoveSpeed * m_MoveSpeedScale;
                }
                else
                {
                    m_AiPath.maxSpeed = m_PlayerCompt.GetSpeed() * m_MoveSpeedScale;
                }
                SetReachPlaceState(false);
            }
            else
            {
                //修改为插件速度
                m_AiPath.maxSpeed = 0;
                //m_AiPath.rotationSpeed = 0;
                SetReachPlaceState(true);
            }
        }
        // 甲壳虫 狗 无人机 等调用 设置 寻路逻辑
        public void SetAutoInfoTypeOne()
        {
            m_AiPath.radius = 0.01f;
            m_AiPath.maxAcceleration = 100;
            m_RvoController.maxNeighbours = 0;
            m_RvoController.collidesWith = 0;
        }
        public void SetAutoPathEnable(bool enable,float disOffset = 0.5f,float maxSpeed = 6,Transform lockTsf = null,Action<bool> callBack = null)
        {
            SetReachPlaceState(false);
            // resetMoveSpeed Scale To One 
            m_MoveSpeedScale = 1;
            m_MoveSpeed = maxSpeed;
            m_AiPath.maxSpeed = 0;
            m_AiPath.enabled = enable;
            m_RvoController.enabled = enable;
            m_LockTsf = lockTsf;
            m_EndCallBack = callBack;
            this.enabled = enable;
            m_DisSqrOffset = disOffset * disOffset;
            m_AiPath.endReachedDistance = disOffset;
            if(lockTsf == m_PlayerGo.transform)
            {
                m_SynPlayerSpeed = true;
            }
            else
            {
                m_SynPlayerSpeed = false;
            }
            if (enable)
            {
                //m_RvoController.velocity = (lockTsf.position - transform.position).normalized * m_MoveSpeed;
                //m_AiPath.velocity2D = m_RvoController.velocity;
                //m_RvoController.SetTarget(lockTsf.position, m_MoveSpeed, m_MoveSpeed);
            }
        }
        private void SetReachPlaceState(bool reachPlace)
        {
            if(reachPlace != m_ReachPlace)
            {
                m_ReachPlace = reachPlace;
                if (m_EndCallBack != null)
                {
                    m_EndCallBack(reachPlace);
                }
            }
        }
        public void SetSpeedScale(float speedScale)
        {
            m_MoveSpeedScale = speedScale;
        }
        public void SetEanbleRotation(bool enableRotation)
        {
            m_AiPath.enableRotation = enableRotation;
        }
    }
}
