using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

namespace EZ
{
    public class AIPursueAct : AiBase
    {

        // Use this for initialization
 
        [SerializeField] private float HitSpeedScale = 1;
        [SerializeField] private float HitScaleEffectTime = 1;
        private float m_HitScaleEffectTime = -1;
        private float m_HitSpeedScale = 1;
        private float m_BaseSpeed;

        private GameObject m_TargetNode;
        private bool m_AutomaticPath = false;
        [HideInInspector]
        public bool m_PushAutoPath = false;
        private bool m_hasInit = false;
        [HideInInspector]
        public AIPath m_AiPath;
        RVOController m_rvoController;
        Seeker m_seeker;
        CircleCollider2D m_collider;
        SimpleSmoothModifier m_simpleSmooth;

        public override void Init(GameObject player,Wave wave,Monster monster)
        {
            base.Init(player,wave,monster);
            m_BaseSpeed = monster.GetMonsterItem().baseSpeed;

            if (!m_PushAutoPath)
            {
                m_PushAutoPath = true;
                m_hasInit = true;
                PathInit.aiPursueActList.Enqueue(this);
            }
        }

        /// <summary>
        /// 僵尸变聪明
        /// </summary>
        public void BeAutoPath()
        {

            m_AutomaticPath = true;
            // 如果不笨则增加寻路算法
            if (m_AiPath == null)
            {
                // 获取碰撞体
                m_collider = GetComponent<CircleCollider2D>();

                // 增加RVO网格控制
                m_rvoController = gameObject.AddComponent<RVOController>();
                m_rvoController.agentTimeHorizon = 0.4f; // 检测代理碰撞的时间间隔
                m_rvoController.obstacleTimeHorizon = 0.6f; // 检测其他障碍碰撞的时间间隔
                //m_rvoController.collidesWith = (RVOLayer)(-1);
                m_rvoController.collidesWith = RVOLayer.DefaultAgent;
                m_rvoController.maxNeighbours = 2;
                m_rvoController.locked = false;

                // 寻路插件
                m_AiPath = gameObject.AddComponent<AIPath>();
                m_AiPath.radius = GetComponent<CircleCollider2D>().radius;
                m_AiPath.height = 1;
                m_AiPath.slowdownDistance = 0; // 无需减速
                m_AiPath.enableRotation = true;
                m_AiPath.orientation = OrientationMode.YAxisForward;
                m_AiPath.slowWhenNotFacingTarget = false;
                m_AiPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
                m_AiPath.rotationSpeed = 240 * (m_BaseSpeed / 2.5f);
                m_AiPath.pickNextWaypointDist = 1;



                // 增加平滑曲线
                m_simpleSmooth = gameObject.AddComponent<SimpleSmoothModifier>();
                m_simpleSmooth.maxSegmentLength = 1f;
                m_simpleSmooth.iterations = 5;
                RaycastModifier m_raycast = gameObject.AddComponent<RaycastModifier>();
                m_raycast.quality = RaycastModifier.Quality.Low;
                //m_raycast.use2DPhysics = false;
                m_raycast.useRaycasting = false;
                m_raycast.useGraphRaycasting = true;

                m_seeker = GetComponent<Seeker>();
                m_seeker.drawGizmos = false;
                m_seeker.pathCallback += pathCall; // 路径完成后的回调
                Transform targetTsf = GetTargetNode();
                //transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(m_Player.transform.position - transform.position, Vector3.up));
                m_seeker.StartPath(transform.position, targetTsf.position);

                // 设置初始速度
                m_rvoController.velocity = (targetTsf.position - transform.position).normalized * m_BaseSpeed * m_HitSpeedScale; // 直接设置初始速度
                m_rvoController.SetTarget(targetTsf.position, m_rvoController.velocity.magnitude, m_rvoController.velocity.magnitude);
                m_AiPath.velocity2D = m_rvoController.velocity;

                PathInit.curPathNum++;
            } else
            {
                EnablePath();
            }
        }

        // 路径更新完成后的回调
        private bool isNoUsePath = false;
        public void pathCall(Path P)
        {
            if (P.vectorPath.Count <= 0)
            {
                return;
            }
            Vector3 startPoint = P.vectorPath[0];
            float offset = startPoint.magnitude - transform.position.magnitude;
            // 如果路径误差超过0.1，则不用路径
            if (offset > 0.1f)
            {
                m_AiPath.repathRate = 0.8f;
                //Debug.Log("无效" + m_AiPath.maxSpeed);
                isNoUsePath = true;
            } else
            {
                isNoUsePath = false;
            }
        }

        /// <summary>
        /// 寻路生效
        /// </summary>
        public void EnablePath()
        {
            if (m_AiPath.enabled)
            {
                return;
            }
            Transform targetNode = GetTargetNode();
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(targetNode.position - transform.position, Vector3.up));
            m_AiPath.enabled = true;
            m_rvoController.enabled = true;
            m_seeker.StartPath(transform.position, targetNode.position);
            PathInit.curPathNum++;

        }

        /// <summary>
        /// 寻路失效
        /// </summary>
        public void DisablePath()
        {
            if (!m_AiPath.enabled)
            {
                return;
            }
            m_PushAutoPath = false;
            m_AutomaticPath = false;
            m_collider.isTrigger = false;
            m_AiPath.maxSpeed = 0;
            m_AiPath.enabled = false;
            m_rvoController.enabled = false;
            PathInit.curPathNum--;
        }

        public void SetHittedSpeedScaleDisable()
        {
            m_HitScaleEffectTime = -1;
            m_HitSpeedScale = 1;
        }
        public void SetHittedSpeedScaleEnable()
        {
            m_HitScaleEffectTime = HitScaleEffectTime;
            m_HitSpeedScale = HitSpeedScale;
        }
        public bool TriggerScalePursue()
        {
            if(HitSpeedScale < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                m_HitScaleEffectTime = m_HitScaleEffectTime - dtTime;
                if (m_HitScaleEffectTime <= 0)
                {
                    m_HitSpeedScale = 1;
                }
                MoveToRole();
            }
            else
            {
                if(m_AutomaticPath)
                {
                    m_AiPath.maxSpeed = 0;
                } else
                {
                    m_Monster.SetSpeed(Vector2.zero);
                }
                
            }
        }
        private void OnDisable()
        {
            if (m_AutomaticPath)
            {
                DisablePath();
            }
        }
        private void OnEnable()
        {
            SetHittedSpeedScaleDisable();
            if(BaseScene.GetDtTime() > 0)
            {
                if (!m_PushAutoPath && m_hasInit)
                {
                    m_PushAutoPath = true;
                    PathInit.aiPursueActList.Enqueue(this);
                }

                MoveToRole();
            }
        }
        public override void Death()
        {
            base.Death();
            if (m_AutomaticPath)
            {
                DisablePath();
            }
        }
        private void MoveToRole()
        {
            Transform targetNode = GetTargetNode();
            if (targetNode != null)
            {
                Vector3 posStart = transform.position;
                Vector3 targetPos = targetNode.position;
                Vector2 velocity = new Vector2(targetPos.x - posStart.x, targetPos.y - posStart.y);
                // 僵尸是否愚蠢
                if (!m_AutomaticPath || (m_AutomaticPath && !m_AiPath.enabled))
                {
                    if (velocity.sqrMagnitude > 0.25f)
                    {
                        Vector2 velocity2 = velocity.normalized * m_BaseSpeed * m_HitSpeedScale;
                        m_Monster.SetSpeed(velocity2 * BaseScene.TimeScale);
                        transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(targetPos - posStart, Vector3.up));
                    }
                    else
                    {
                        m_Monster.SetSpeed(Vector2.zero);
                    }
                } else
                {
                    // 是否在外面,并根据是否在屏幕外做优化
                    if (m_Monster.InShadowView)
                    {
                        m_collider.isTrigger = false;
                        if (!isNoUsePath)
                        {
                            m_AiPath.repathRate = 0.2f;
                        }
                        m_rvoController.agentTimeHorizon = 0.4f; // 检测代理碰撞的时间间隔
                        m_rvoController.obstacleTimeHorizon = 0.6f; // 检测其他障碍碰撞的时间间隔
                        m_rvoController.maxNeighbours = 2;

                    } else
                    {
                        m_collider.isTrigger = true;

                        if (!isNoUsePath)
                        {
                            m_AiPath.repathRate = 0.5f;
                        }
                        m_rvoController.agentTimeHorizon = 1f; // 检测代理碰撞的时间间隔
                        m_rvoController.obstacleTimeHorizon = 1f; // 检测其他障碍碰撞的时间间隔
                        m_rvoController.maxNeighbours = 2;
                    }


                    // 插件的目标
                    if (m_seeker.IsDone())
                    {
                        m_AiPath.destination = targetNode.position;
                    }

                    if (velocity.sqrMagnitude > 0.25f)
                    {
                        //Vector2 velocity2 = velocity.normalized * m_BaseSpeed * m_HitSpeedScale;
                        float velocity2Val = m_BaseSpeed * m_HitSpeedScale;
                        // 修改为插件速度
                        float speedVal = velocity2Val * BaseScene.TimeScale;
                        float newSpeed = 1;
                        if (m_Monster.m_AiBuffMgr != null)
                        {
                            newSpeed += m_Monster.m_AiBuffMgr.GetIncMoveSpeed();
                        }
                        speedVal *= newSpeed;

                        m_AiPath.maxSpeed = speedVal;
                    }
                    else
                    {
                        //修改为插件速度
                        m_AiPath.maxSpeed = 0;
                    }
                }
                
            }
        }
        private Transform GetTargetNode()
        {
            if (m_TargetNode != null)
            {
                return m_TargetNode.transform;
            }
            else
            {
                if (m_Player != null)
                {
                    return m_Player.transform;
                }
                else
                {
                    return null;
                }
            }
        }
        public void SetTargetNode(GameObject go)
        {
            m_TargetNode = go; 
        }
    }
}
