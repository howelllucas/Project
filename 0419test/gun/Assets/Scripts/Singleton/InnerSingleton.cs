//不受单例管理器管理的内部单例基类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tool
{
    public class InnerSingleton<T> where T : InnerSingleton<T>, new()
    {
        static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.InitSingleton();
                }
                return instance;
            }
        }

        protected virtual void InitSingleton() { }
    }

    public class InnerMonoSingleton<T> : MonoBehaviour where T : InnerMonoSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null && !appIsQuit)
                {
                    instance = Instantiate();
                }

                return instance;
            }
        }

        protected static T Instantiate()
        {
            var instance = (T)FindObjectOfType(typeof(T));

            if (instance == null)
            {
                GameObject singleton = new GameObject("[Singleton]" + typeof(T).Name);
                instance = singleton.AddComponent<T>();
            }
            else
            {
                instance.InitSingleton();
            }

            return instance;
        }

        private static bool appIsQuit;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                InitSingleton();
            }
        }

        protected virtual void InitSingleton()
        {

        }

        public virtual void OnApplicationQuit()
        {
            instance = null;
            appIsQuit = true;
        }
    }
}



