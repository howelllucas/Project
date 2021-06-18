using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class ActiveLoopTween : TweenBase
    {
        [FormerlySerializedAs("m_target")]
        public GameObject target;

        protected override void Awake()
        {
            base.Awake();
            if (target == null)
                target = gameObject;
            loop = true;
        }

        protected override void DoTween(float t)
        {
            target.SetActive(t <= 0.5f);
        }

    }
}