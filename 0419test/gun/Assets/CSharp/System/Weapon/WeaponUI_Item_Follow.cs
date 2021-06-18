using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{

    public class WeaponUI_Item_Follow : MonoBehaviour
    {
        private Transform m_FollowNode;
        public void SetFollowNode(Transform followNode)
        {
            m_FollowNode = followNode;
        }
        private void LateUpdate()
        {
            if(m_FollowNode != null)
            {
                transform.position = m_FollowNode.transform.position;
            }
        }
    }
}
