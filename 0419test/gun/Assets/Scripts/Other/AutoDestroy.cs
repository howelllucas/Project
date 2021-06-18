using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battles
{
    public class AutoDestroy : MonoBehaviour
    {
        public float DespawnTime = 2f;

        public Action onDestroy = null;

        private float pDespawnTime;
        private bool bStart;

        private void OnEnable()
        {
            pDespawnTime = DespawnTime;
            bStart = true;
        }

        public void SetDespawnTime(float value)
        {
            DespawnTime = value;
            pDespawnTime = DespawnTime;
        }

        private void Update()
        {
            if (bStart)
            {
                pDespawnTime -= Time.deltaTime;
                if (pDespawnTime <= 0f)
                {
                    bStart = false;
                    ResourceMgr.singleton.DeleteInstance(base.gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (onDestroy != null)
                onDestroy();
        }
    }
}