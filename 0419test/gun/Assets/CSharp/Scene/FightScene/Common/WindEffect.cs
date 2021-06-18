using System;
using UnityEngine;

namespace EZ
{

    public class WindEffect : MonoBehaviour
    {

        [SerializeField] private GameObject m_Wind01;
        [SerializeField] private GameObject m_Wind02;
        private Action m_CallBack;
        private float m_MinTime = 8;
        private float m_MaxTime = 16;

        private float m_MinRadio = 4f;
        private float m_MaxRadio = 8;
        private int m_MaxCount = 3;
        private int m_MinCount = 1;

        private float m_MinTime2 = 0.5f;
        private float m_MaxTime2 = 3;

        private int m_CurCount = 0;

        private void Start()
        {
            m_CallBack = GenerateWind;
            m_CurCount = UnityEngine.Random.Range(m_MinCount, m_MaxCount + 1);
            float time = UnityEngine.Random.Range(m_MinTime, m_MaxTime);
            gameObject.AddComponent<DelayCallBack>().SetAction(m_CallBack, time);
        }
        void GenerateWind()
        {
            GameObject m_Player = Global.gApp.CurScene.GetMainPlayer();
            if(m_Player == null) { return; }
            m_CurCount--;
            int rate = UnityEngine.Random.Range(0, 101);
            GameObject windGo;
            if (rate < 70)
            {
                windGo = Instantiate(m_Wind02);
            }
            else
            {
                windGo = Instantiate(m_Wind01);
            }
      
            float radio = UnityEngine.Random.Range(m_MinRadio, m_MaxRadio);
            float angle = UnityEngine.Random.Range(0, 360);
            float randVec = UnityEngine.Random.Range(0, 360);
            Vector3 playerPos = m_Player.transform.position;
            Vector3 addPos = new Vector3(Mathf.Sin(angle) * radio, Mathf.Cos(angle) * radio, 0);
            windGo.transform.position = playerPos + addPos;
            windGo.transform.localEulerAngles = new Vector3(randVec, -90, -90);

            if (m_CurCount == 0)
            {
                m_CurCount = UnityEngine.Random.Range(m_MinCount, m_MaxCount + 1);
                float time = UnityEngine.Random.Range(m_MinTime, m_MaxTime);
                gameObject.AddComponent<DelayCallBack>().SetAction(m_CallBack, time);
            }
            else
            {
                float time = UnityEngine.Random.Range(m_MinTime2, m_MaxTime2);
                gameObject.AddComponent<DelayCallBack>().SetAction(m_CallBack, time);
            }
        }
    }
}
