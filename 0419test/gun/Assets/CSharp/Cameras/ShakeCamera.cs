using UnityEngine;
namespace EZ
{

    public class ShakeCamera : MonoBehaviour
    {

        private float m_ShakeTime = 0.0f;
        private float m_DtTime = 0.033f;
        private float m__CurTime = 0.0f;
        [SerializeField] private float m_Shakeamp = 0.005f;

        [SerializeField] private bool m_IsShakeCamera = false;

        public float Shakeamp
        {
            get
            {
                return m_Shakeamp;
            }

            set
            {
                m_Shakeamp = value;
            }
        }

        void Start()
        {
            m_ShakeTime = 0.1f;
            m_DtTime = 0.033f;
            m__CurTime = 0.01f;
        }
        void Update()
        {
            if (m_IsShakeCamera)
            {
                if (m_ShakeTime > 0)
                {
                    m_ShakeTime -= BaseScene.GetDtTime();
                    if (m_ShakeTime <= 0)
                    {
                        transform.localPosition = Vector3.zero;
                        m_IsShakeCamera = false;
                        m_ShakeTime = 0.1f;
                        m__CurTime = 0.01f;
                    }
                    else
                    {
                        m__CurTime += BaseScene.GetDtTime();

                        if (m__CurTime >= m_DtTime)
                        {
                            m__CurTime = 0;
                            transform.localPosition = new Vector3(Shakeamp * (-1.0f + 2.0f * Random.value), Shakeamp * (-1.0f + 2.0f * Random.value), 0);
                        }
                    }
                }
            }
        }
        public void StartShakeCamera()
        {
            m_IsShakeCamera = true;
        }
    }
}