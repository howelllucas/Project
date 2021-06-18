using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using Game.Pool;

namespace Game.Tool
{
    public abstract class ScriptAnimBase : MonoBehaviour, IOnPoolDespawn
    {
        [FormerlySerializedAs("m_autoStart")]
        public bool autoStart;
        [FormerlySerializedAs("m_loop")]
        public bool loop;
        [FormerlySerializedAs("m_pingPang")]
        public bool pingPang;
        [FormerlySerializedAs("m_reverse")]
        public bool reverse;
        [FormerlySerializedAs("m_duration")]
        public float duration;
        [FormerlySerializedAs("m_delay")]
        public float delay;
        [FormerlySerializedAs("m_resetOnPlay")]
        public bool resetOnPlay = true;
        protected bool isPlay { get; private set; }
        private float clock;
        private float thisTimeDelay;
        protected bool dir { get; private set; }
        protected Action funcOnFinish;
        public bool isFinish { get; protected set; }

        public float PlayTime
        {
            get
            {
                return Mathf.Max(0, clock - thisTimeDelay);
            }
        }

        public float NormalizeTime
        {
            get
            {
                return duration == 0 ? 1f : Mathf.Clamp01(PlayTime / duration);
            }
        }

        protected virtual void Awake()
        {
            thisTimeDelay = delay;
            dir = !reverse;
        }

        protected virtual void Start()
        {
            
        }

        private void OnEnable()
        {
            DoOnEnable();
            if (autoStart)
                Play();
        }

        protected virtual void DoOnEnable() { }

        public void Play()
        {
            Play(delay);
        }

        public void Play(float delay)
        {
            Play(delay, null);
        }

        public void Play(Action onFinish)
        {
            Play(delay, onFinish);
        }

        public void Play(float delay, Action onFinish)
        {
            if (resetOnPlay)
                ResetParam();
            isPlay = true;
            isFinish = false;
            clock = 0;
            thisTimeDelay = delay;
            dir = !reverse;
            this.funcOnFinish = onFinish;

            DoOnPlay();
        }

        protected virtual void DoOnPlay() { }

        public void Stop()
        {
            isPlay = false;
            DoOnStop();
        }

        protected virtual void DoOnStop() { }

        private void Update()
        {
            if (!isPlay)
                return;
            clock += Time.deltaTime;
            if (clock - thisTimeDelay >= 0)
            {
                DoOnUpdate();
                if (clock - thisTimeDelay >= duration)
                {
                    if (loop)
                    {
                        clock -= duration;
                        if (pingPang)
                        {
                            dir = !dir;
                        }
                    }
                    else if (pingPang)
                    {
                        if (dir == !reverse)
                        {
                            clock -= duration;
                            dir = !dir;
                        }
                        else
                        {
                            isPlay = false;
                            isFinish = true;
                            DoOnFinish();
                        }
                    }
                    else
                    {
                        isPlay = false;
                        isFinish = true;
                        DoOnFinish();
                    }
                }
            }
        }

        protected virtual void DoOnFinish()
        {
            funcOnFinish?.Invoke();
        }

        protected virtual void DoOnUpdate() { }

        protected virtual void ResetParam()
        {
            isPlay = false;
            isFinish = false;
            clock = 0;
            thisTimeDelay = delay;
            dir = !reverse;
            funcOnFinish = null;
        }

        public void OnPoolDespawn()
        {
            ResetParam();
        }
    }
}