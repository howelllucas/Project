using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolRectTransformRecorder : MonoBehaviour, IOnPoolDespawn
    {
        public RectTransform[] recordRects;
        private RectRecordData[] recordDatas;

        public void OnPoolDespawn()
        {
            if (recordDatas == null)
                return;
            for (int i = 0; i < recordDatas.Length; i++)
            {
                var data = recordDatas[i];
                data.target.SetParent(data.parent);
                data.target.SetSiblingIndex(data.siblingIndex);
                data.target.localPosition = data.localPos;
                data.target.localEulerAngles = data.localEuler;
                data.target.localScale = data.localScale;
                data.target.anchorMin = data.anchorMin;
                data.target.anchorMax = data.anchorMax;
                data.target.pivot = data.pivot;
                data.target.anchoredPosition = data.anchoredPos;
                data.target.sizeDelta = data.sizeDelta;
            }
        }

        void Awake()
        {
            recordDatas = new RectRecordData[recordRects.Length];
            for (int i = 0; i < recordRects.Length; i++)
            {
                RectTransform target = recordRects[i];
                RectRecordData data = new RectRecordData()
                {
                    target = target,
                    parent = target.parent,
                    siblingIndex = target.GetSiblingIndex(),
                    localPos = target.localPosition,
                    localEuler = target.localEulerAngles,
                    localScale = target.localScale,
                    anchorMin = target.anchorMin,
                    anchorMax = target.anchorMax,
                    pivot = target.pivot,
                    anchoredPos = target.anchoredPosition,
                    sizeDelta = target.sizeDelta,
                };
                recordDatas[i] = data;
            }
        }

        private struct RectRecordData
        {
            public RectTransform target;
            public Transform parent;
            public int siblingIndex;
            public Vector3 localPos;
            public Vector3 localEuler;
            public Vector3 localScale;
            public Vector2 anchorMin;
            public Vector2 anchorMax;
            public Vector2 pivot;
            public Vector2 anchoredPos;
            public Vector2 sizeDelta;
        }
    }
}

