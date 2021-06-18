using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        private Vector3 posOffset;

        public void SetTarget(Transform target)
        {
            if (target != null)
            {
                posOffset = transform.position - target.position;
            }
            this.target = target;
        }

        void Update()
        {
            if (target != null)
            {
                transform.position = target.position + posOffset;
            }
        }
    }
}