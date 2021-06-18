using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tool
{
    public class Pos2DTrackTween : TweenBase
    {
        public Transform target;
        public Vector2 from;
        public Vector2 to;
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
            float x = Mathf.Lerp(from.x, to.x, t_x);
            float y = Mathf.Lerp(from.y, to.y, t_y);
            if (local)
            {
                Vector3 pos = target.localPosition;
                pos.x = x;
                pos.y = y;
                target.localPosition = pos;
            }
            else
            {
                Vector3 pos = target.position;
                pos.x = x;
                pos.y = y;
                target.position = pos;
            }
        }

    }
}
