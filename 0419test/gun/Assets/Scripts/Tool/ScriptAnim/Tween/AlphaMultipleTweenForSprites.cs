﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class AlphaMultipleTweenForSprites : TweenBase
    {
        [FormerlySerializedAs("m_targets")]
        public SpriteRenderer[] targets;
        [FormerlySerializedAs("m_from")]
        public float from;
        [FormerlySerializedAs("m_to")]
        public float to;
        private float[] oriAlphas;


        protected override void Awake()
        {
            base.Awake();
            if (targets == null || targets.Length <= 0)
            {
                targets = GetComponentsInChildren<SpriteRenderer>();
            }
            oriAlphas = new float[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                oriAlphas[i] = targets[i].color.a;
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
                color.a = alpha * oriAlphas[i];
                targets[i].color = color;
            }

        }
    }
}