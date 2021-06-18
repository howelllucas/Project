using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{

    public class TrainMove : MonoBehaviour {

        Animator animator;
        [SerializeField] private float m_MaxDtTime;
        [SerializeField] private float m_MinDtTime;
        private float m_DtTime;
        private float m_CurTime;
        private float m_AnimTime;
        void Start() {
            m_DtTime = Random.Range(m_MinDtTime, m_MaxDtTime);
            animator = GetComponent<Animator>();
            m_AnimTime = GetDeadTime(animator);
        }
        private float GetDeadTime(Animator animator)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "huoche")
                {
                    return clip.length;
                }
            }
            return 1;
        }
        void Update() {
            m_CurTime += BaseScene.GetDtTime();
            if(m_CurTime > m_DtTime)
            {
                animator.Play("huoche",-1,0);
                m_CurTime = 0;
                m_DtTime = Random.Range(m_MinDtTime, m_MaxDtTime) + m_AnimTime;
            }
        }
    }
}
