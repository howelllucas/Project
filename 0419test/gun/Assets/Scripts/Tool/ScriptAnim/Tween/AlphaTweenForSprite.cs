using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class AlphaTweenForSprite : TweenBase
    {
        [FormerlySerializedAs("m_target")]
        public SpriteRenderer target;
        [FormerlySerializedAs("m_from")]
        public float from;
        [FormerlySerializedAs("m_to")]
        public float to;

        protected override void Awake()
        {
            base.Awake();
            if (target == null)
                target = GetComponent<SpriteRenderer>();
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