using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public enum BoxState
    {
        Empty = 1,
        Less = 2,
        Many = 3,
        Full = 4,
    }
    public class CampsiteGoldenBox : MonoBehaviour
    {
        public float boxTime = 5.0f;
        public Animator boxAnim;

        private BoxState state;
        private float stateTime = 0.0f;
        private bool isClcik = false;

        void Update()
        {
            stateTime -= Time.deltaTime;
            if (stateTime <= 0.0f)
            {
                PlayAnim();
                stateTime = boxTime;
            }
        }

        public void SetBoxState(BoxState st)
        {
            if (state == st)
                return;

            state = st;
            stateTime = boxTime;
            PlayAnim();
        }

        void PlayAnim()
        {
            switch (state)
            {
                case BoxState.Empty:
                    {
                        boxAnim.SetTrigger("Empty");
                    }
                    break;
                case BoxState.Less:
                    {
                        boxAnim.SetTrigger("Less");
                    }
                    break;
                case BoxState.Many:
                    {
                        boxAnim.SetTrigger("Many");
                    }
                    break;
                case BoxState.Full:
                    {
                        boxAnim.SetTrigger("Full");
                    }
                    break;
            }
        }



    }
}
