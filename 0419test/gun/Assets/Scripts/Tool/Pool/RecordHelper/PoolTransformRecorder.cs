using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolTransformRecorder : MonoBehaviour, IOnPoolDespawn
    {
        public Transform[] recordTrans;
        private TransformRecordData[] recordDatas;

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
            }
        }

        private void Awake()
        {
            recordDatas = new TransformRecordData[recordTrans.Length];
            for (int i = 0; i < recordTrans.Length; i++)
            {
                Transform target = recordTrans[i];
                TransformRecordData data = new TransformRecordData()
                {
                    target = target,
                    parent = target.parent,
                    siblingIndex = target.GetSiblingIndex(),
                    localPos = target.localPosition,
                    localEuler = target.localEulerAngles,
                    localScale = target.localScale
                };
                recordDatas[i] = data;
            }
        }

        private struct TransformRecordData
        {
            public Transform target;
            public Transform parent;
            public int siblingIndex;
            public Vector3 localPos;
            public Vector3 localEuler;
            public Vector3 localScale;
        }
    }
}

