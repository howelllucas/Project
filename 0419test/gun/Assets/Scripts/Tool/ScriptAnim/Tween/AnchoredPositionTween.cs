using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class AnchoredPositionTween : TweenBase
    {
        [FormerlySerializedAs("m_target")]
        public RectTransform target;
        [FormerlySerializedAs("m_from")]
        public Vector2 from;
        [FormerlySerializedAs("m_to")]
        public Vector2 to;

        protected override void Awake()
        {
            base.Awake();
            if (target == null)
                target = transform as RectTransform;
        }

        protected override void DoTween(float t)
        {
            if (target == null)
                return;

            Vector2 pos = Vector2.LerpUnclamped(from, to, t);
            target.anchoredPosition = pos;
        }

    }
}