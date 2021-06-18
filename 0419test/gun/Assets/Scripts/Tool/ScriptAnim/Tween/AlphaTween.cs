using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Tool
{
    public class AlphaTween : TweenBase
    {
        [FormerlySerializedAs("m_target")]
        public Graphic target;
        [FormerlySerializedAs("m_from")]
        public float from;
        [FormerlySerializedAs("m_to")]
        public float to;

        protected override void Awake()
        {
            base.Awake();
            if (target == null)
                target = GetComponent<Graphic>();
        }

        protected override void DoTween(float t)
        {
            if (target == null)
                return;

            float alpha = Mathf.LerpUnclamped(from, to, t);
            var color = target.color;
            color.a = alpha;
            target.color = color;
        }

    }
}