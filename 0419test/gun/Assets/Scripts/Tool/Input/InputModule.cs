using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace Game.Tool
{
    public struct InputData
    {
        public int touchId;
        public Vector2 screenPos;
        public bool isInUI;
    }

    public interface IInputDataDeal
    {
        void OnTouchDown(InputData data);
        void OnTouch(InputData data);
        void OnTouchUp(InputData data);
        void ForceUp();
    }

    public class InputDataDeal_Single : IInputDataDeal
    {
        private InputData? current;
        public Action<InputData> funcOnTouchDown;
        public Action<InputData> funcOnTouch;
        public Action<InputData> funcOnTouchUp;
        public Action<InputData> funcOnForceUp;

        public void OnTouchDown(InputData data)
        {
            if (current.HasValue)
                return;
            current = data;
            funcOnTouchDown?.Invoke(data);
        }

        public void OnTouch(InputData data)
        {
            if (!current.HasValue || current.Value.touchId != data.touchId)
                return;
            current = data;
            funcOnTouch?.Invoke(data);
        }

        public void OnTouchUp(InputData data)
        {
            if (!current.HasValue || current.Value.touchId != data.touchId)
                return;
            funcOnTouchUp?.Invoke(data);
            current = null;
        }

        public void ForceUp()
        {
            if (current.HasValue)
            {
                funcOnForceUp(current.Value);
            }
            current = null;
        }
    }

    public class InputDataDeal_Multi : IInputDataDeal
    {
        private Dictionary<int, InputData> inputDic = new Dictionary<int, InputData>();
        public event Action<InputData> funcOnTouchDown;
        public event Action<InputData> funcOnTouch;
        public event Action<InputData> funcOnTouchUp;
        public event Action<InputData> funcOnForceUp;

        public void OnTouchDown(InputData data)
        {
            if (inputDic.ContainsKey(data.touchId))
                return;
            inputDic[data.touchId] = data;
            funcOnTouchDown?.Invoke(data);
        }

        public void OnTouch(InputData data)
        {
            if (!inputDic.ContainsKey(data.touchId))
                return;
            inputDic[data.touchId] = data;
            funcOnTouch?.Invoke(data);
        }

        public void OnTouchUp(InputData data)
        {
            if (!inputDic.ContainsKey(data.touchId))
                return;
            funcOnTouchUp?.Invoke(data);
            inputDic.Remove(data.touchId);
        }

        public void ForceUp()
        {
            foreach (var data in inputDic.Values)
            {
                funcOnForceUp?.Invoke(data);
            }
            inputDic.Clear();
        }
    }

    public class InputModule
    {
        private bool valid;
        private IInputDataDeal dataDeal;

        public InputModule(IInputDataDeal dataDeal)
        {
            this.valid = true;
            this.dataDeal = dataDeal;
        }

        public void Update(float deltaTime)
        {
            if (!valid || dataDeal == null)
                return;
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                OnTouchDown(new InputData()
                {
                    touchId = 0,
                    screenPos = Input.mousePosition,
                    isInUI = IsPointerOverGameObject(Input.mousePosition)
                });
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnTouchUp(new InputData()
                {
                    touchId = 0,
                    screenPos = Input.mousePosition,
                    isInUI = IsPointerOverGameObject(Input.mousePosition)
                });
            }
            else if (Input.GetMouseButton(0))
            {
                OnTouch(new InputData()
                {
                    touchId = 0,
                    screenPos = Input.mousePosition,
                    isInUI = IsPointerOverGameObject(Input.mousePosition)
                });
            }
#else
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnTouchDown(new InputData()
                        {
                            touchId = touch.fingerId,
                            screenPos = touch.position,
                            isInUI = IsPointerOverGameObject(touch.position)
                        });
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        OnTouch(new InputData()
                        {
                            touchId = touch.fingerId,
                            screenPos = touch.position,
                            isInUI = IsPointerOverGameObject(touch.position)
                        });
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        OnTouchUp(new InputData()
                        {
                            touchId = touch.fingerId,
                            screenPos = touch.position,
                            isInUI = IsPointerOverGameObject(touch.position)
                        });
                        break;
                    default:
                        break;
                }
            }
#endif
        }

        public void SetValid(bool valid)
        {
            if (valid != this.valid)
            {
                if (!valid)
                {
                    ForceUp();
                }
                this.valid = valid;
            }
        }

        private void OnTouchDown(InputData data)
        {
            if (!valid || dataDeal == null)
                return;
            dataDeal.OnTouchDown(data);
        }

        private void OnTouch(InputData data)
        {
            if (!valid || dataDeal == null)
                return;
            dataDeal.OnTouch(data);
        }

        private void OnTouchUp(InputData data)
        {
            if (!valid || dataDeal == null)
                return;
            dataDeal.OnTouchUp(data);
        }

        private void ForceUp()
        {
            if (!valid || dataDeal == null)
                return;
            dataDeal.ForceUp();
        }

        public static bool IsPointerOverGameObject(Vector2 mousePoint)
        {
            if (EventSystem.current == null)
                return false;
            //创建一个点击事件
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePoint;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            //向点击位置发射一条射线，检测是否点击到UI
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults.Count > 0;
        }
    }
}