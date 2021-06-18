using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DG.Tweening.Extension
{
    public static class DOTweenExtension
    {
        public static Core.TweenerCore<Vector3, Vector3, Plugins.Options.VectorOptions> DOFollowMove(this Transform target, Vector3 endValue, float duration, Transform followTarget, FollowAxisMask followAxisMask = FollowAxisMask.None, bool ignoreInactive = true, bool snapping = false)
        {
            DOFollowMove followMove = new DOFollowMove(followTarget, followAxisMask, ignoreInactive);
            Core.TweenerCore<Vector3, Vector3, Plugins.Options.VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate (Vector3 x)
            {
                followMove.FollowPosSetter(target, x);
            }, endValue, duration);
            tweenerCore.SetOptions(snapping).SetTarget(target);
            return tweenerCore;
        }
    }

    public enum FollowAxisMask
    {
        None = 0,
        x = 1,
        y = 2,
        z = 4
    }

    public class DOFollowMove
    {
        private Transform followTarget;
        private FollowAxisMask mask;
        private Vector3 targetLastPos;
        private Vector3 followOffset;
        private bool ignoreInactive;
        public DOFollowMove(Transform followTarget, FollowAxisMask mask, bool ignoreInactive = true)
        {
            this.followTarget = followTarget;
            this.mask = mask;
            if (followTarget != null)
                this.targetLastPos = followTarget.position;
            this.followOffset = new Vector3();
            this.ignoreInactive = ignoreInactive;
        }

        internal void FollowPosSetter(Transform moveObj, Vector3 pos)
        {
            Vector3 offset = new Vector3();
            if (followTarget != null)
            {
                if (followTarget.gameObject.activeInHierarchy || !ignoreInactive)
                {
                    offset = followTarget.transform.position - targetLastPos;
                    targetLastPos = followTarget.transform.position;
                    if (mask != FollowAxisMask.None)
                        for (int i = 0; i < 3; i++)
                        {
                            var axis = 1 << i;
                            if (((int)mask & axis) == axis)
                            {
                                offset[i] = 0;
                            }
                        }
                }
                else
                {
                    followTarget = null;
                }
            }
            followOffset += offset;
            moveObj.position = pos + followOffset;
        }
    }

}
