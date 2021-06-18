using EZ.Data;
using UnityEngine;

namespace EZ
{
    public class CampPhotoProp : BaseProp
    {
        [Tooltip("延迟删除时间")]
        public float DelayRemoveTime = 1.5f;
        [SerializeField] FightNpcPlayer.FightDropNPC m_ResProp = FightNpcPlayer.FightDropNPC.photo1;
        private bool m_IsDeath = false;
        private float m_CurTime = 0;
        private GameObject m_Appear;
        private GameObject m_Disappear;
        private int m_DropCount = 1;

        protected override void Awake()
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            base.Awake();
            DelayCallBack callBack = gameObject.AddComponent<DelayCallBack>();
            callBack.SetAction(GenerateRes, 0.1f, true);
            callBack.SetCallTimes(2);
            AdapterName(m_ResProp.ToString());
        }
        public override void Update()
        {
            base.Update();
            if (m_IsDeath)
            {
                m_CurTime = m_CurTime + Time.deltaTime;
                if (m_CurTime > DelayRemoveTime)
                {
                    Global.gApp.CurScene.GetPropMgr().RemoveProp(gameObject);
                }
            }
        }
        private void GenerateRes()
        {
            if (m_Appear == null)
            {
                string npcPath = "Prefabs/Effect/prop/photo_chixu";
                m_Appear = Global.gApp.gResMgr.InstantiateObj(npcPath);
                m_Appear.transform.SetParent(transform, false);
                m_Appear.SetActive(true);
            }
            else
            {
                string npcPath = "Prefabs/Effect/prop/photo_xiaoshi";
                m_Disappear = Global.gApp.gResMgr.InstantiateObj(npcPath);
                m_Disappear.transform.SetParent(transform, false);
                m_Disappear.SetActive(false);
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
            }

        }
        public string GetItemName()
        {
            return m_ResProp.ToString();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsDeath)
            {
                return;
            }
            if ((collision.gameObject.CompareTag(GameConstVal.CarrierTag) || collision.gameObject.CompareTag(GameConstVal.DamageRangeTag)) && 
                m_Disappear != null && m_Appear != null)
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                Player player = collision.gameObject.GetComponentInParent<Player>();
                if(player == null)
                {
                    player = collision.gameObject.GetComponentInChildren<Player>();
                }
                if (player != null)
                {
                    m_IsDeath = true;
                    if (m_Disappear != null)
                    {
                        m_Appear.SetActive(false);
                        m_Disappear.SetActive(true);
                    }
                    player.GetPlayerData().AddDropRes(m_ResProp.ToString(), m_DropCount);
                    BroadGainProp();
                }
            }
        }
    }
}

