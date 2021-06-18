using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class LockRotation : MonoBehaviour
    {

        private Transform m_LockTsf;
        public void SetLockTsf(Transform lockTsf)
        {
            m_LockTsf = lockTsf;
        }
        private void Update()
        {
            transform.position = m_LockTsf.position;
        }
    }
}
