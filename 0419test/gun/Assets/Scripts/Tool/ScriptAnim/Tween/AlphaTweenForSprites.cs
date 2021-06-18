using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class AlphaTweenForSprites : TweenBase
    {
        [FormerlySerializedAs("m_targets")]
        public SpriteRenderer[] targets;
        [FormerlySerializedAs("m_from")]
        public float from;
        [FormerlySerializedAs("m_to")]
        public float to;

        protected override void Awake()
        {
            base.Awake();
            if (targets == null || targets.Length <= 0)
            {
                targets = GetComponentsInChildren<SpriteRenderer>();
            }
        }

        protected override void DoTween(float t)
        {
            if (targets == null)
                return;
            float alpha = Mathf.LerpUnclamped(from, to, t);
            for (int i = 0; i < targets.Length; i++)
            {
                var color = targets[i].color;
                color.a = alpha;
                targets[i].color = color;
            }

        }

    }
}