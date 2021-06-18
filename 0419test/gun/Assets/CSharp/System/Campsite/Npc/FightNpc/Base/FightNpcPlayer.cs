using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public abstract class FightNpcPlayer : MonoBehaviour
    {
        public enum FightDropNPC
        {
            bread = 90001,
            wood = 90002,
            stone = 90003,
            iron = 90004,
            water = 90005,
            medicalKits = 90006,
            Npc_boy = 90007,
            Npc_cook = 90008,
            Npc_doctor01 = 90009,
            Npc_doctor03 = 90010,
            Npc_human00 = 90011,
            Npc_human01 = 90012,
            Npc_human02 = 90013,
            Npc_human03 = 90014,
            Npc_human04 = 90015,
            Npc_human05 = 90016,
            Npc_oldwoman = 90017,
            Npc_police01 = 90018,
            Npc_police02 = 90019,
            Npc_police03 = 90020,
            Npc_rich = 90021,
            Npc_worker = 90022,
            Npc_drstrange = 90023,
            Npc_recycle = 90024,
            Npc_worker01 = 90025,
            photo1 = 90026,
            photo2 = 90027,
            photo3 = 90028,
            photo4 = 90029,
            photo5 = 90030,
            photo6 = 90031,
            Npc_human06 = 90038,
            Npc_human07 = 90039,
            Npc_human08 = 90040,
            Npc_human09 = 90041,
            Npc_human10 = 90042,
            Npc_human11 = 90043,
            Npc_human12 = 90044,
            Npc_human13 = 90045,
            Npc_human14 = 90046,
            Npc_human15 = 90047,
        }
        public enum NpcBehaviorType
        {
            None = 0,
            Trapped = 1,// 受困需要解救
            Rescuing = 2,// 解救中
            Rescued = 3,// 已解救
            FoolAppear = 4, // 傻逼呵呵的 突然出现 
            CloseToRole = 5, //  接近主角
            FarAwayRole = 6 //  远离主角
        }
        [SerializeField] protected NpcBehaviorType m_BehaviorType = NpcBehaviorType.None;
        protected Animator m_Animator;
        private Rigidbody2D m_Rigidbody2D;
        private Collider2D m_Collider2D;
        protected PetAutoPath m_AutoPath;
        private string m_CurAnimName = string.Empty;
        protected Player m_Player;
        protected GameObject m_PlayerGo;
        protected Transform m_TaskUiNode;
        
        protected FightNpcBaseBehavior m_CurBehavior;

        public bool InCameraView
        {
            get; private set;
        }
        protected virtual void Awake()
        {
            m_Animator = GetComponentInChildren<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Collider2D = GetComponent<Collider2D>();
        }
        public virtual void Init(GameObject playerGo)
        {
            if(playerGo == null)
            {
                playerGo = Global.gApp.CurScene.GetMainPlayer();
            }
            if (playerGo != null)
            {
                m_PlayerGo = playerGo;
                m_Player = playerGo.GetComponent<Player>();
            }
        }
        public void SetSpeed(Vector2 speed)
        {
            m_Rigidbody2D.velocity = speed;
        }
        public virtual void FreshProgress(float progress)
        {

        }
        public void PlayAnim(string name)
        {
            if (!m_CurAnimName.Equals(name))
            {
                m_Animator.Play(name, -1, 0);
                m_CurAnimName = name;
            } 
        }
        public void BroadPlotTips(int plotId)
        {
            if (plotId > 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowFightPlotByID, plotId);
            }
        }
        public Transform GetTaskUINode()
        {
            if (m_TaskUiNode == null)
            {
                m_TaskUiNode = transform.Find("TaskNode");
            }
            return m_TaskUiNode;
        }
        public PetAutoPath GetAutoPathComp()
        {
            if (m_AutoPath == null)
            {
                m_AutoPath = gameObject.AddComponent<PetAutoPath>();
            }
            return m_AutoPath;
        }
        protected abstract void InitBehavior();
        public virtual void SetBehavior(NpcBehaviorType npcBehaviorType)
        {
            m_BehaviorType = npcBehaviorType;
            InitBehavior();
        }
        public void EndCurBehavior()
        {
            if(m_CurBehavior != null)
            {
                m_CurBehavior.EndBehavior();
                m_CurBehavior = null;
            }
        }
        public void SetCollionEnable(bool enable)
        {
            m_Collider2D.enabled = enable;
            m_Rigidbody2D.velocity = Vector2.zero;
        }
        public void SetCollisionType(RigidbodyType2D rigidbodyType2D)
        {
            m_Rigidbody2D.bodyType = rigidbodyType2D;
        }
        private void OnBecameVisible()
        {
            InCameraView = true;
        }

        private void OnBecameInvisible()
        {
            InCameraView = false;
        }
        public int GetNpcId()
        {
            return 1;
        }
        public virtual void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
