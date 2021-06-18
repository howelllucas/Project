using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class KeepRotation : MonoBehaviour
    {

        Quaternion m_Rotation;
        private void Awake()
        {
            m_Rotation = transform.rotation;
        }
        private void LateUpdate()
        {
            transform.rotation = m_Rotation;
        }
    }
}
