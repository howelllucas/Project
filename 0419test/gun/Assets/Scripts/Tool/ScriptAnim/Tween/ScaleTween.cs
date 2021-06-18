using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class ScaleTween : TweenBase
    {
        [FormerlySerializedAs("m_from")]
        public Vector3 from = Vector3.zero;
        [FormerlySerializedAs("m_to")]
        public Vector3 to = Vector3.one;

        protected override void DoTween(float t)
        {
            transform.localScale = Vector3.LerpUnclamped(from, to, t);
        }

    }
}