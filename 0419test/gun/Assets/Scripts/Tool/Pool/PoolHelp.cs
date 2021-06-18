using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public static class PoolExtension
    {
        public static T AddComponentEx<T>(this GameObject go)
            where T : Component
        {
            PoolItem poolItem = go.GetComponentInParent<PoolItem>();
            if (poolItem != null)
                return poolItem.PoolAddComponent<T>(go);

            return go.AddComponent<T>();
        }

        public static void DestroyComponentEx(this Component component)
        {
            PoolItem poolItem = component.gameObject.GetComponentInParent<PoolItem>();
            if (poolItem != null)
                poolItem.PoolDestroyComponent(component);
            else
                Object.Destroy(component);
        }
    }
}