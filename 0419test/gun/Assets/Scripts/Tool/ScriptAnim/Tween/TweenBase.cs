using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public abstract class TweenBase : ScriptAnimBase
    {
        [FormerlySerializedAs("m_curve")]
        public AnimationCurve curve = new AnimationCurve
            (
                new Keyframe(0, 0, 1, 1),
                new Keyframe(1, 1, 1, 1)
            );
        [FormerlySerializedAs("m_startForce0")]
        public bool startForce0;

        protected override void Start()
        {
            base.Start();
            if (startForce0)
                ForceTo(0);
        }

        public void ForceTo(float t)
        {
            DoTween(ConvertT(t));
        }

        protected override void DoOnUpdate()
        {
            float t = NormalizeTime;

            DoTween(ConvertT(t));
        }

        private float ConvertT(float t)
        {
            t = Mathf.Clamp01(t);
            if (!dir)
                t = 1 - t;
            float curveT = t;
            if (curve != null)
            {
                curveT = curve.Evaluate(t);
            }

            return curveT;
        }

        protected abstract void DoTween(float t);

        protected override void ResetParam()
        {
            base.ResetParam();
            ForceTo(0);
        }
    }
}