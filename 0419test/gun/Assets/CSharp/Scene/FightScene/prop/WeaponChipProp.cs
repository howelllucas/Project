using UnityEngine;

namespace EZ
{
    public class WeaponChipProp : BaseProp
    {
        public enum ChipGainType
        {
            None = 0,
            AutoGain = 1,
            TriggerGain = 2,
        }
        [SerializeField] private ChipGainType m_ChipGainType = ChipGainType.AutoGain;
        private float m_StartTime = 0.3f;
        private float m_Time = 0.5f;
        private float m_Wait = 0.5f;
        private float m_CurTime = 0;
        private bool m_IsDeath = false;
        private float m_EndedTime = 1;
        private int m_DropCount = 1;
        private GameObject m_Appear;
        private GameObject m_AppearTail;
        private GameObject m_Disappear;
        GameWeapon m_ChipName = GameWeapon.weaponChip;
        private Transform m_MainPlayerTsf;
        private Player m_Player;
        private bool m_HasAddChip = false;
        private Vector3 m_DestPos;
        private Vector3 m_StartPos;
        protected override void Awake()
        {
            base.Awake();
            AdapterName(GameWeapon.weaponChip.ToString());
        }
        private void Start()
        {
            if (Global.gApp.gUiMgr.CheckPanelExit(Wndid.FightPanel))
            {
                m_MainPlayerTsf = Global.gApp.CurScene.GetMainPlayer().transform;
                m_Player = Global.gApp.CurScene.GetMainPlayerComp();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public override void Update()
        {
            CreateAppear();
            base.Update();
            if (!m_IsDeath)
            {
                if (m_ChipGainType == ChipGainType.AutoGain)
                {
                    AddChip();
                    m_CurTime = m_CurTime + Time.deltaTime;
                    if (m_CurTime > m_StartTime)
                    {
                        float ratio = (m_CurTime - m_StartTime) / m_Time;
                        ratio = Mathf.Min(ratio, 1);
                        Vector3 newPos = Vector3.Lerp(m_StartPos, m_DestPos, ratio);
                        transform.position = newPos;
                        if (ratio >= 1f)
                        {
                            BroadGainChip();
                            if(m_Appear != null)
                            {
                                Destroy(m_Appear);
                                m_Appear = null;
                            }
                            if (m_CurTime > m_StartTime + m_Time + m_Wait)
                            {
                                Global.gApp.CurScene.GetPropMgr().RemoveProp(gameObject);
                            }
                        }
                    }
                }
            }
            else
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime >= m_EndedTime)
                {
                    Global.gApp.CurScene.GetPropMgr().RemoveProp(gameObject);
                }
            }
        }
        private void BroadGainChip()
        {
            if (!m_HasBroadGain)
            {
                BroadGainProp();
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.WpnChipCountChanged);
            }
        }
        private void AddChip()
        {
            if (!m_HasAddChip)
            {
                m_HasAddChip = true;
                m_Player.GetPlayerData().AddDropRes(m_ChipName.ToString(), m_DropCount);
            }
        }

        private void StartDisapear()
        {
            if (m_Disappear == null)
            {
                if (m_Appear != null)
                {
                    m_Appear.SetActive(false);
                }
                m_Disappear = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[EffectConfig.WpnChipDisappearTriggerGain]);
                m_Disappear.transform.position = transform.position;
            }
        }

        public void SetChipGainTypeTrigger(int dropCount)
        {
            TextMesh nameText = transform.Find(GameConstVal.TextMeshName).GetComponent<TextMesh>();
            nameText.text = nameText.text + " X " + dropCount.ToString();
            m_DropCount = dropCount;
            m_ChipGainType = ChipGainType.TriggerGain;
        }
        private void CreateAppear()
        {
            if (m_Appear == null)
            {
                if (m_ChipGainType == ChipGainType.AutoGain)
                {
                    m_Appear = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[EffectConfig.WpnChipAppearAutoGain]);
                    m_AppearTail = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[EffectConfig.WpnChipAppearAutoGainTail]);
                    m_AppearTail.transform.SetParent(transform, false);
                    transform.Find(GameConstVal.TextMeshName).gameObject.SetActive(false);
                    FightUI fightUi = Global.gApp.gUiMgr.GetPanelCompent<FightUI>(Wndid.FightPanel);
                    if (fightUi != null)
                    {
                        RectTransform rext = fightUi.GetChipIconRectTsf();
                        m_DestPos = rext.position;
                    }
                    else
                    {
                        m_DestPos = Vector3.zero;
                    }
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                    transform.position = UiTools.ScreenToUiWorld(screenPoint);
                    m_StartPos = transform.position;
                    AddChip();
                }
                else
                {
                    m_Appear = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[EffectConfig.WpnChipAppearTriggerGain]);
                }
                m_Appear.transform.SetParent(transform, false);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_ChipGainType == ChipGainType.TriggerGain)
            {
                if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
                {
                    StartDisapear();
                    m_ChipGainType = ChipGainType.AutoGain;
                    Destroy(m_Appear);
                    m_Appear = null;
                    CreateAppear();
                }
            }
        }
    }
}

