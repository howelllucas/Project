using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class AlphaScaleTweenForRender : TweenBase
    {
        [FormerlySerializedAs("m_target")]
        public Renderer target;
        [FormerlySerializedAs("m_colorName")]
        public string colorName;
        [FormerlySerializedAs("m_from")]
        public float from;
        [FormerlySerializedAs("m_to")]
        public float to;
        private Material mat;
        private float oriAlpha;

        protected override void Awake()
        {
            base.Awake();
            if (target == null)
            {
                target = GetComponent<Renderer>();
            }

            if (target != null)
            {
                mat = target.material;
                oriAlpha = mat.GetColor(colorName).a;
            }
        }

        protected override void DoTween(float t)
        {
            if (target == null)
                return;
            float alphaScale = Mathf.LerpUnclamped(from, to, t);
            var color = mat.GetColor(colorName);
            color.a = alphaScale * oriAlpha;
            mat.SetColor(colorName, color);

        }
    }
}