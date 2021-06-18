using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class FrameDestroy : MonoBehaviour
    {

        [SerializeField] int DestroyFrame = 1;
        private float m_CurFrame = 0;
        private void Update()
        {
            m_CurFrame++;
            if (m_CurFrame >= DestroyFrame)
            {
                Destroy(gameObject);
            }
        }
    }
}
