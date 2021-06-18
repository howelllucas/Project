
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public interface ISingleton
    {
        void ClearSingleton();
    }
    public class Singleton<T> : ISingleton where T : Singleton<T>
    {
        static T s_singleton;
        static public T singleton { get { return s_singleton; } }

        //public static string ext_unity = ".unity";
        //public static string ext_prefab = ".prefab";
        public Singleton()
        {
            if(s_singleton!=null)
            {
                Debug.LogErrorFormat("{0} repeat s_singleton error!", typeof(T) );
            }
            s_singleton = this as T;
        }

        public virtual void ClearSingleton()
        {
            s_singleton = null;
        }
    }
}
