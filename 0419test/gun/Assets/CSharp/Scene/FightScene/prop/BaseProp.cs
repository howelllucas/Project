using EZ.Data;
using UnityEngine;
namespace EZ
{
    public class BaseProp : MonoBehaviour
    {
        [SerializeField] protected float DamageCoefficient = 1;
        [SerializeField] protected GameProp m_PropName;
        private bool m_InCameraView = false;
        protected bool m_HasBroadGain = false;
        protected Transform m_NameMesh;
        private Vector3 m_Speed;
        private string PropClipName = "gainprop";
        protected virtual void Awake()
        {
            m_NameMesh = transform.Find(GameConstVal.TextMeshName);
            SceneType sceneType =Global.gApp.CurScene.GetSceneType();
            if (sceneType == SceneType.BreakOutSene)
            {
                m_Speed = new Vector3(0,-6,0);
            }
            else if (sceneType == SceneType.CarScene)
            {
                m_Speed = new Vector3(0,-12,0);
            }
        }
        protected void AdapterName(string itemName)
        {
            if (m_NameMesh == null)
            {
                m_NameMesh = transform.Find(GameConstVal.TextMeshName);
            }
            if (m_NameMesh != null)
            {
                ItemItem itemCfg = Global.gApp.gGameData.GetItemDataByName(itemName);
                if (itemCfg != null)
                {
                    TextMesh textMesh = m_NameMesh.GetComponent<TextMesh>();
                    string lgg = Global.gApp.gSystemMgr.GetMiscMgr().Language;
                    if (lgg == null || lgg.Equals(GameConstVal.EmepyStr))
                    {
                        lgg = UiTools.GetLanguage();
                    }
                    textMesh.text = Global.gApp.gGameData.GetTipsInCurLanguage(itemCfg.sourceLanguage);
                    textMesh.font = Global.gApp.gGameData.GetFont(lgg);
                    MeshRenderer meshRenderer = m_NameMesh.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.sharedMaterial = textMesh.font.material;
                    }
                }
            }
        } 
        public virtual void Update()
        {
            transform.Translate(m_Speed * BaseScene.GetDtTime(),Space.World);
        }
        public bool InCameraView
        {
            get
            {
                return m_InCameraView;
            }

            set
            {
                m_InCameraView = value;
            }
        }
        private void OnBecameInvisible()
        {
            InCameraView = false;
        }
        private void OnBecameVisible()
        {
            InCameraView = true;
        }
        public GameProp GetPropName()
        {
            return m_PropName;
        }
        protected void BroadGainProp()
        {
            if (m_HasBroadGain)
            {
                return;
            }
            Global.gApp.gAudioSource.PlayOneShot(PropClipName);
            m_HasBroadGain = true;
            if (m_NameMesh != null)
            {
                m_NameMesh.gameObject.SetActive(false);
            }
            Global.gApp.gMsgDispatcher.Broadcast<GameProp,GameObject>(MsgIds.GainProp, m_PropName,gameObject);
        }

        protected void BroadCollectingProp()
        {
            Global.gApp.gMsgDispatcher.Broadcast<GameProp, GameObject>(MsgIds.CollectingProp, m_PropName, gameObject);
        }
    }
}
