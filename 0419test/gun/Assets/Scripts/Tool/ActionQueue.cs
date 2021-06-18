using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tool
{
    public class ActionQueue
    {
        Queue<ActionData> queue = new Queue<ActionData>();

        public void PushAction(Action action, float duration, float delay = 0)
        {
            queue.Enqueue(new ActionData(action, duration, delay));
        }

        public void CancelAction(Action action)
        {
            if (action == null)
                return;
            foreach (var item in queue)
            {
                if (item.IsSameAction(action))
                    item.Valid = false;
            }
        }

        public void Update(float deltaTime)
        {
            if (queue.Count <= 0)
                return;
            var cur = queue.Peek();
            if (cur.Run(deltaTime))
                queue.Dequeue();
        }

        private class ActionData
        {
            public bool Valid { get; set; } = true;
            private float delay;
            private Action action;
            private float duration;

            public ActionData(Action action, float duration, float delay)
            {
                this.delay = delay;
                this.action = action;
                this.duration = duration;
            }

            public bool Run(float deltaTime)
            {
                if (!Valid)
                    return true;

                if (delay > 0)
                {
                    delay -= deltaTime;
                    return false;
                }

                action?.Invoke();
                action = null;

                duration -= deltaTime;

                if (duration <= 0)
                {
                    return true;
                }

                return false;
            }

            public bool IsSameAction(Action action)
            {
                return this.action == action;
            }
        }
    }
}