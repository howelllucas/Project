using System.Collections;
using UnityEngine;

namespace EZ
{
    public class NpcProp : BaseProp
    {
        [Tooltip("解决时间")]
        public float m_RecueTime = 3;
        [SerializeField] FightNpcPlayer.FightDropNPC m_Npc = FightNpcPlayer.FightDropNPC.Npc_boy;
        [SerializeField]private bool m_ForceTrapState = false;
        private bool m_StartRecue = false;
        private bool m_HasRescue = false;
        private bool m_HasGenerateNpc = false;
        private float m_CurTime = 0f;
        private FightNpcPlayer m_NPCPlayer;
        private GameObject m_CircleEffect;
        protected override void Awake()
        {
            if (!m_HasGenerateNpc)
            {
                gameObject.AddComponent<DelayCallBack>().SetAction(GenerateEffect, 0.05f,true);
                gameObject.AddComponent<DelayCallBack>().SetAction(GenerateNpc, 0.1f,true);
            }
            AdapterName(m_Npc.ToString());
        }

        void GenerateEffect()
        {
            GameObject apperaEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Npc_chixuEffect);
            apperaEffect.transform.SetParent(transform);
            apperaEffect.transform.position = transform.position;
        }
        void GenerateNpc()
        {
            m_HasGenerateNpc = true;
            string npcPath = "Prefabs/Campsite/NpcFight/" + m_Npc.ToString();
            GameObject Npc_boy = Global.gApp.gResMgr.InstantiateObj(npcPath);
            FightNpcPlayer npcPlayer = Npc_boy.GetComponent<FightNpcPlayer>();
            if (!m_ForceTrapState)
            {
                Npc_boy.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            }
            m_NPCPlayer = npcPlayer;
            npcPlayer.Init(null);
            m_NPCPlayer.transform.SetParent(transform);
            m_NPCPlayer.transform.position = transform.position;
            m_NPCPlayer.SetBehavior(FightNpcPlayer.NpcBehaviorType.Trapped);
            m_NPCPlayer.FreshProgress(0);

            m_CircleEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.NpcRecycleEffect);
            m_CircleEffect.transform.SetParent(transform, false);
            m_CircleEffect.transform.localPosition = Vector3.zero;
        }
        public FightNpcPlayer GetFightNpcPlayer()
        {
            return m_NPCPlayer;
        }
        public override void Update()
        {
            base.Update();
            if(!m_HasRescue &&m_StartRecue)
            {
                m_CurTime += BaseScene.GetDtTime();
                if(m_RecueTime > 0 && m_NPCPlayer != null)
                {
                    m_NPCPlayer.FreshProgress(m_CurTime / m_RecueTime);
                }
                if (m_CurTime > m_RecueTime)
                {
                    m_NPCPlayer.FreshProgress(m_CurTime / m_RecueTime);
                    m_HasRescue = true;
                    RecuedNpc();
                }
            }
            else if(! m_HasRescue)
            {
                m_CurTime -= BaseScene.GetDtTime();
                m_CurTime = Mathf.Max(m_CurTime, 0);
                if (m_RecueTime > 0 && m_NPCPlayer != null)
                {
                    m_NPCPlayer.FreshProgress(m_CurTime / m_RecueTime);
                }
            }
        }
        private void RecuedNpc()
        {
            m_NPCPlayer.transform.SetParent(Global.gApp.gBulletNode.transform);
            if (!m_ForceTrapState)
            {
                m_NPCPlayer.SetBehavior(FightNpcPlayer.NpcBehaviorType.Rescued);
            }
            m_NPCPlayer.transform.position = transform.position;

            Player player = Global.gApp.CurScene.GetMainPlayerComp();
            if (player != null)
            {
                player.GetPlayerData().AddDropRes(m_Npc.ToString(), 1);
                BroadGainProp();
            }
            Global.gApp.CurScene.GetPropMgr().RemoveProp(gameObject);
        }
        public string GetItemName()
        {
            return m_Npc.ToString();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                m_StartRecue = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!m_HasRescue && m_HasGenerateNpc)
            {
                if (collision.gameObject.CompareTag(GameConstVal.CarrierTag) || collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
                {
                    m_StartRecue = true;
                }
            }
        }
    }
}

