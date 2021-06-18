using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tool
{
    public class Rot3DTrackTween : TweenBase
    {
        public Transform target;
        public Vector3 from;
        public Vector3 to;
        public bool local;
        public AnimationCurve trackCurve = AnimationCurve.Linear(0, 0, 1, 1);

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
                target = transform;
        }
        
        protected override void DoTween(float t)
        {
            float t_x = t;
            float t_y = trackCurve.Evaluate(t);
            float t_z = trackCurve.Evaluate(t);
            float x = Mathf.Lerp(from.x, to.x, t_x);
            float y = Mathf.Lerp(from.y, to.y, t_y);
            float z = Mathf.Lerp(from.z, to.z, t_z);
            if (local)
            {
                Vector3 pos = target.localEulerAngles;
                pos.x = x;
                pos.y = y;
                pos.z = z;
                target.localEulerAngles = pos;
            }
            else
            {
                Vector3 pos = target.eulerAngles;
                pos.x = x;
                pos.y = y;
                pos.z = z;
                target.eulerAngles = pos;
            }
        }

    }
}
