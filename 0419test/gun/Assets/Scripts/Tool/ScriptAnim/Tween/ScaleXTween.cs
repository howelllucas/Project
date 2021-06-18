using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class ScaleXTween : TweenBase
    {
        [FormerlySerializedAs("m_from")]
        public float from;
        [FormerlySerializedAs("m_to")]
        public float to;

        protected override void DoTween(float t)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.LerpUnclamped(from, to, t);
            transform.localScale = scale;
        }

    }
}