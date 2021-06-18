using UnityEngine;

namespace EZ
{
    public class FightNormalNpcPlayer :FightNpcPlayer 
    {

        FightTrappedBehavior m_TrappedBehavior;
        FightRescuingBehavior m_RescuingBehavior;
        FightRescuedBehavior m_RescuedBehavior;
        FightFoolAppearBehavior m_FoolAppearBehavior;

        FightNpcProgress m_FightNpcProgress = null;
        public CloseToMainRoleBehavior FightCloseToRoleBehavior
        {
            get; private set;
        }
        public FarFromMainRoleBehavior FightFarAwayRoleBehavior
        {
            get; private set;
        }

        public override void FreshProgress(float progress)
        {
            FightUI fightUI = Global.gApp.gUiMgr.GetPanelCompent<FightUI>(Wndid.FightPanel);
            if(fightUI == null)
            {
                return;
            }
            if (m_FightNpcProgress == null)
            {
                GameObject go = Global.gApp.gResMgr.InstantiateObj(Wndid.FightNpcProgress);
                go.transform.SetParent(fightUI.NpcProgress.rectTransform, false);
                m_FightNpcProgress = go.GetComponent<FightNpcProgress>();
                go.GetComponent<FollowNode>().SetFloowNode(GetTaskUINode());
            }
            m_FightNpcProgress.Received.image.fillAmount = progress;
            if(progress >= 1)
            {
                DG.Tweening.DOTweenAnimation[] dOTweenAnimations = m_FightNpcProgress.GetComponents<DG.Tweening.DOTweenAnimation>();
                foreach(DG.Tweening.DOTweenAnimation dOTweenAnimation in dOTweenAnimations)
                {
                    dOTweenAnimation.enabled = true;
                    dOTweenAnimation.DORestart();
                }
                Destroy(m_FightNpcProgress.gameObject,3);
                m_FightNpcProgress = null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_TrappedBehavior = GetComponent<FightTrappedBehavior>();
            m_RescuingBehavior = GetComponent<FightRescuingBehavior>();
            m_RescuedBehavior = GetComponent<FightRescuedBehavior>();
            m_FoolAppearBehavior = GetComponent<FightFoolAppearBehavior>();
            FightCloseToRoleBehavior = GetComponent<CloseToMainRoleBehavior>();
            FightFarAwayRoleBehavior = GetComponent<FarFromMainRoleBehavior>();
            InitBehavior();
        }
        public override void Init(GameObject playerGo = null)
        {
            base.Init(playerGo);
            m_TrappedBehavior.Init(this, m_PlayerGo);
            m_RescuingBehavior.Init(this, m_PlayerGo);
            m_RescuedBehavior.Init(this, m_PlayerGo);
            m_FoolAppearBehavior.Init(this, m_PlayerGo);
            if(FightCloseToRoleBehavior != null)
            {
                FightCloseToRoleBehavior.Init(this, m_PlayerGo);
            }
            if (FightFarAwayRoleBehavior != null)
            {
                FightFarAwayRoleBehavior.Init(this, m_PlayerGo);
            }
        }

        protected override void InitBehavior()
        {
            EndCurBehavior();
            m_TrappedBehavior.enabled = false;
            m_RescuingBehavior.enabled = false;
            m_RescuedBehavior.enabled = false;
            m_FoolAppearBehavior.enabled = false;
            if(FightCloseToRoleBehavior != null)
            {
                FightCloseToRoleBehavior.enabled = false;
            }
            if (FightFarAwayRoleBehavior != null)
            {
                FightFarAwayRoleBehavior.enabled = false;
            }
            if(m_BehaviorType == NpcBehaviorType.Trapped)
            {
                m_TrappedBehavior.enabled = true;
                m_CurBehavior = m_TrappedBehavior;
            }
            else if(m_BehaviorType == NpcBehaviorType.Rescuing)
            {
                m_RescuingBehavior.enabled = true;
                m_CurBehavior = m_RescuingBehavior;
            }
            else if(m_BehaviorType == NpcBehaviorType.Rescued)
            {
                m_RescuedBehavior.enabled = true;
                m_CurBehavior = m_RescuedBehavior;
            }
            else if(m_BehaviorType == NpcBehaviorType.FoolAppear)
            {
                m_FoolAppearBehavior.enabled = true;
                m_CurBehavior = m_FoolAppearBehavior;
            }
            else if(m_BehaviorType == NpcBehaviorType.CloseToRole)
            {
                FightCloseToRoleBehavior.enabled = true;
                m_CurBehavior = FightCloseToRoleBehavior;
            }
            else if(m_BehaviorType == NpcBehaviorType.FarAwayRole)
            {
                FightFarAwayRoleBehavior.enabled = true;
                m_CurBehavior = FightFarAwayRoleBehavior;
            }
            if (m_CurBehavior != null)
            {
                m_CurBehavior.StartBehavior();
            }
        }
        public void SetFoolAppearActPlotId(int plotId)
        {
            m_FoolAppearBehavior.SetCurPlotId(plotId);
        }
        public override void DestroySelf()
        {
            if(m_FightNpcProgress != null)
            {
                Destroy(m_FightNpcProgress.gameObject);
                m_FightNpcProgress = null;
            }
            base.DestroySelf();
        }
    }
}
