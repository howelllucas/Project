using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tool
{
    public class Pos2DTrackTweenMix : ScriptAnimBase
    {
        public Transform target;
        public bool local;
        public Pos2DMixData[] mixDatas;

        protected override void Awake()
        {
            base.Awake();
            if (target == null)
                target = transform;
        }

        protected override void DoOnPlay()
        {
            base.DoOnPlay();
            if (mixDatas == null)
                return;
            duration = 0;
            for (int i = 0; i < mixDatas.Length; i++)
            {
                duration = Mathf.Max(mixDatas[i].delay + mixDatas[i].duration);
            }
        }

        protected override void DoOnUpdate()
        {
            if (mixDatas == null)
                return;
            float sumWeight = 0;
            Vector2 pos2D = Vector2.zero;
            for (int i = 0; i < mixDatas.Length; i++)
            {
                var pos = mixDatas[i].CalcPos(PlayTime);
                float weight = mixDatas[i].CalcWeight(PlayTime);
                pos *= weight;
                sumWeight += weight;
                pos2D += pos;
            }
            if (sumWeight == 0)
                return;

            pos2D /= sumWeight;
            if (local)
            {
                Vector3 pos = target.localPosition;
                pos.x = pos2D.x;
                pos.y = pos2D.y;
                target.localPosition = pos;
            }
            else
            {
                Vector3 pos = target.position;
                pos.x = pos2D.x;
                pos.y = pos2D.y;
                target.position = pos;
            }
        }

        [System.Serializable]
        public class Pos2DMixData
        {
            public Vector2 from;
            public Vector2 to;
            public float duration;
            public float delay;
            public AnimationCurve timeCurve;
            public AnimationCurve trackCurve;
            public AnimationCurve weightCurve;

            public Vector2 CalcPos(float time)
            {
                float t = (time - delay) / duration;
                t = Mathf.Clamp01(t);
                t = timeCurve.Evaluate(t);
                float t_x = t;
                float t_y = trackCurve.Evaluate(t);
                float x = Mathf.Lerp(from.x, to.x, t_x);
                float y = Mathf.Lerp(from.y, to.y, t_y);
                return new Vector2(x, y);
            }
            public float CalcWeight(float time)
            {
                if (time <= delay || time > delay + duration)
                {
                    return 0;
                }
                float t = (time - delay) / duration;
                return weightCurve.Evaluate(t);
            }
        }
    }
}
