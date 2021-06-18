using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scenes
{
    public enum BoxState
    {
        Empty = 1,
        Less = 2,
        Many = 3,
        Full = 4,
    }
    public class CityBox : MonoBehaviour
    {
        public float boxTime = 5.0f;
        public Animator boxAnim;
        public Camera worldCamera;

        public Action OnBoxClick;

        private BoxState state;
        private float stateTime = 0.0f;
        private bool isClcik = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            stateTime -= Time.deltaTime;
            if (stateTime <= 0.0f)
            {
                PlayAnim();
                stateTime = boxTime;
            }

            //UpdateRay();
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

        private void OnMouseDown()
        {
            Debug.Log("OnMouseDown");
        }

        void UpdateRay()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClcik = false;
                Debug.Log("点击鼠标左键");
                RaycastHit hit;
                var ray = worldCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    var obj = hit.collider.gameObject;
                    //通过名字
                    if (obj.name.Equals("BaoXiang"))
                    {
                        isClcik = true;
                        Debug.Log("点中" + obj.name);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!isClcik)
                    return;

                Debug.Log("点击鼠标左键");
                RaycastHit hit;
                var ray = worldCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    var obj = hit.collider.gameObject;
                    //通过名字
                    if (obj.name.Equals("BaoXiang"))
                    {
                        if (OnBoxClick != null)
                            OnBoxClick();
                    }
                }
            }
        }
    }
}