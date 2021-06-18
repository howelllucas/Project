using UnityEngine;
namespace EZ
{
    public class WarningEffect : MonoBehaviour
    {

        private Transform m_ParticleTsf;
        private Transform m_Arrow;
        private Material m_ArrowMat;
        [SerializeField] private float BaseLength = 1;
        private float m_CurTime = 0;
        private float m_LiveTime = 1;
        private void Awake()
        {
            m_ParticleTsf = transform.Find(GameConstVal.ParticleName);
            m_Arrow = transform.Find("01/Arrow");
            m_ArrowMat = m_Arrow.GetComponent<MeshRenderer>().material;
            m_ArrowMat.SetVector("_MainTex_ST", new Vector4(5, 1, 1, 1));
        }
        public void Init(float liveTime, float length, Transform tsf)
        {
            float scale = length / BaseLength;
            Vector3 localScale = m_ParticleTsf.localScale;
            localScale.x = scale;
            m_ParticleTsf.localScale = localScale;

            m_LiveTime = liveTime;

            transform.position = tsf.position;
            transform.rotation = tsf.rotation;
        }
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime >= m_LiveTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
