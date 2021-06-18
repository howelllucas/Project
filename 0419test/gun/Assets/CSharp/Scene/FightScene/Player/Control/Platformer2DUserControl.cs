using System;
using UnityEngine;
namespace EZ
{
    public class Platformer2DUserControl : MonoBehaviour
    {

        public Player mPlayer;
        private float DtAngle = -30;
        private float m_SinVal = 0;
        private float m_CosVal = 0;
        private bool m_InNormalScene;
        private void Start()
        {
            m_InNormalScene =Global.gApp.CurScene.IsNormalPass();
            DtAngle = (float)(DtAngle * Mathf.Deg2Rad);
            m_CosVal = Mathf.Cos(DtAngle);
            m_SinVal = Mathf.Sin(DtAngle);
        }
        private void Update()
        {

            BroadInput();
        }
        private void BroadInput()
        {
            // Read the inputs.
            if(Time.timeScale == 0)
            {
                return;
            }
            float sx = ETCInput.GetAxis("Horizontal"); 
            float sy = ETCInput.GetAxis("Vertical");
            if(sx == 0 && sy == 0)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    sx = -1;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    sx = 1;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    sy = 1;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    sy = -1;
                }
            }
            if(m_InNormalScene && (sx != 0 || sy != 0))
            {
                float nSx = sx * m_CosVal - sy * m_SinVal;
                float nSy = sy * m_CosVal + sx * m_SinVal;
                sx = nSx;
                sy = nSy;
            }
            // Pass all parameters to the character control script.
            mPlayer.Move(sx, sy);
        } 
    }
}
