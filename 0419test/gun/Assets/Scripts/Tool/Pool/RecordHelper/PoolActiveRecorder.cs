using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolActiveRecorder : MonoBehaviour, IOnPoolDespawn
    {
        public GameObject[] recordObjs;
        private ActiveRecordData[] recordDatas;

        public void OnPoolDespawn()
        {
            if (recordDatas == null)
                return;
            for (int i = 0; i < recordDatas.Length; i++)
            {
                var data = recordDatas[i];
                data.target.SetActive(data.isShow);

            }
        }

        void Awake()
        {
            recordDatas = new ActiveRecordData[recordObjs.Length];
            for (int i = 0; i < recordObjs.Length; i++)
            {
                var target = recordObjs[i];
                ActiveRecordData data = new ActiveRecordData()
                {
                    target = target,
                    isShow = target.activeSelf,
                };
                recordDatas[i] = data;
            }
        }

        private struct ActiveRecordData
        {
            public GameObject target;
            public bool isShow;

        }
    }
}   