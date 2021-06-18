using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class InternetMgr : Singleton<InternetMgr>
    {
        public event Action<bool> funcOnNetworkStateChange;
        private bool curState;
        private float checkSpace;
        private float timer;

        public void Init(float checkSpace = 1f)
        {
            this.checkSpace = checkSpace;
            curState = IsInternetConnect();
        }

        public bool IsInternetConnect()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= checkSpace)
            {
                timer -= checkSpace;
                var state = IsInternetConnect();
                if (state != curState)
                    funcOnNetworkStateChange?.Invoke(state);
                curState = state;
            }
        }
    }
}