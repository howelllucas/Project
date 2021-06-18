using UnityEngine;
namespace EZ
{
    public class RepreatEffect : MonoBehaviour
    {
        private Transform m_ParticleTsf;
        private Transform m_Arrow;
        private Material m_ArrowMat;
        private float BaseLength = 0.7f;
        private float m_Scale = 1;
        private void Awake()
        {
            m_ParticleTsf = transform.Find(GameConstVal.ParticleName);
            m_Arrow = transform.Find("Arrow");
            m_ArrowMat = m_Arrow.GetComponent<MeshRenderer>().material;
        }
        public void SetLength(float length)
        {
            float scale = length / BaseLength;
            transform.localScale = new Vector3(scale, 1, 1);
            m_Scale = scale;
        }
        private void Update()
        {
            m_ArrowMat.SetVector(GameConstVal.MainTex_ST, new Vector4(1, m_Scale, 1, 1));
        }
    }
}
