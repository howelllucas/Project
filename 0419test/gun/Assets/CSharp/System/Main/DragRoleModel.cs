using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using System;
namespace EZ
{

    public class DragRoleModel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {

        public Transform m_MainNode;
        public float m_RotateSpeed = 1;

        /*
        public void OnDrag(PointerEventData eventData)
        {
            m_MainNode.Rotate(new Vector3(0, eventData.delta.x * m_RotateSpeed, 0));
        }
        */

        private const float k_f = 3E-3f;
        private const float k_friction = 180f;
        private const float k_max_acc = 7200f;

        public event Action<PointerEventData> OnPointerDownEvent;
        public event Action<PointerEventData> OnPointerUpEvent;
        public event Action<PointerEventData> OnDragEvent;
        public event Action<float> OnUpdateEvent;

        public Quaternion rotation = Quaternion.identity;

        public int mState = 0;
        //是否被拖拽;
        private float mAcc = 0f;

        public float speed = 0f;
        private float mPrevDrag = 0f;
        private Quaternion mDragEndRot;
        private float mDragEndDur;
        private float mDragEndTimer;

        public void Start()
        {
            // rotation = m_MainNode.localRotation;
        }

        //接受鼠标按下的事件;
        public void OnPointerDown(PointerEventData eventData)
        {
            mState = 1;
            mPrevDrag = Time.time;
            if (OnPointerDownEvent != null)
                OnPointerDownEvent.Invoke(eventData);
        }

        public float yVelocity;
        public float smoothTime = 1f;

        public void OnPointerUp(PointerEventData eventData)
        {
            mState = 2;
            if (OnPointerUpEvent != null)
                OnPointerUpEvent.Invoke(eventData);
        }

        //鼠标拖拽时的操作;
        public void OnDrag(PointerEventData eventData)
        {
            float delta = eventData.delta.x / Screen.width;
            const float ffff = 0.06f;
            float ad = Mathf.Abs(delta);
            float t = Mathf.Sign(delta) * (ad * ad + 2f * ad * ffff);
            mAcc = mAcc * 0.2f + 1E4f * t / (Time.time - mPrevDrag);
            if (mAcc >= 0f)
            {
                mAcc = Mathf.Lerp(k_friction, k_max_acc, mAcc / k_max_acc);
            }
            else
            {
                mAcc = Mathf.Lerp(-k_friction, -k_max_acc, -mAcc / k_max_acc);
            }
            //Log.dtf("zw", "acc : {0}", mAcc);
            mPrevDrag = Time.time;
            if (OnDragEvent != null)
                OnDragEvent.Invoke(eventData);
        }

        void Update()
        {
            if (m_MainNode == null)
                return;
            if (mState == 1 || mState == 2)
            {
                rotation = rotation * Quaternion.Euler(0f, -speed * Time.deltaTime, 0f);
                float sign = Sign(speed);
                speed += (mAcc - sign * (k_friction + k_f * speed * speed)) * Time.deltaTime;

                mAcc *= 0.2f;
                if (Mathf.Abs(mAcc) < 5) { mAcc = 0f; }
                if (Sign(speed) * sign < 0f)
                {
                    speed = 0f;
                }
                if (speed == 0f && mState == 2)
                {
                    mState = 3;
                    mDragEndRot = rotation;
                    mDragEndDur = Quaternion.Angle(Quaternion.identity, mDragEndRot) / 90f + 0.3f;
                    mDragEndTimer = 0f;
                }
                if (OnUpdateEvent != null)
                {
                    OnUpdateEvent(Time.deltaTime);
                }
                m_MainNode.localRotation = rotation;
            }
            else if (mState == 3)
            {
                mDragEndTimer += Time.deltaTime;
                float t = Mathf.Clamp01(mDragEndTimer / mDragEndDur);
                rotation = Quaternion.Lerp(mDragEndRot, Quaternion.identity, Mathf.Sin((t - 0.5f) * Mathf.PI) * 0.5f + 0.5f);
                if (t >= 1f)
                {
                    mState = 0;
                }
                if (OnUpdateEvent != null)
                {
                    OnUpdateEvent(Time.deltaTime);
                }
                m_MainNode.localRotation = rotation;
            }

        }

        float Sign(float val)
        {
            return val == 0f ? 0f : (val > 0f) ? 1f : -1f;
       }
    }
}
