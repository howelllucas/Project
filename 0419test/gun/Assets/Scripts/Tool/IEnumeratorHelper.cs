using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;

namespace Game
{
    public class IEnumeratorHelper : InnerMonoSingleton<IEnumeratorHelper>
    {
        protected override void InitSingleton()
        {
            base.InitSingleton();
            DontDestroyOnLoad(gameObject);
        }

        public static Coroutine Start(IEnumerator func)
        {
            return Instance.StartCoroutine(func);
        }

        public static void Stop(Coroutine corotine)
        {
            Instance.StopCoroutine(corotine);
        }

        public static void StopAll()
        {
            Instance.StopAllCoroutines();
        }
    }
}