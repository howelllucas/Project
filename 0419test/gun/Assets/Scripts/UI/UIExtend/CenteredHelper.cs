using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class CenteredHelper : MonoBehaviour
    {
        public Transform centerFlag;
        public Transform target;
        public Transform leftFlagInTarget;
        public Transform rightFlagInTarget;


        private void Update()
        {
            var targetCenter = 0.5f * (leftFlagInTarget.position + rightFlagInTarget.position);
            var offset = centerFlag.position - targetCenter;
            offset.y = 0;
            offset.z = 0;
            if (offset.x > 0.01f)
                target.position += offset;
        }

    }
}