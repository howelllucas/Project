using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolItem : MonoBehaviour
    {
        /// <summary>
        /// 若对池内对象做出了一些超出重置范围内的操作 则设置此值为true 该对象回收时直接删除
        /// </summary>
        public bool disabledItem { get; private set; }
        /// <summary>
        /// 额外动态添加的组件
        /// </summary>
        List<Component> extensionComponents = new List<Component>();

        List<IOnPoolSpawn> onSpawnInterfaces = new List<IOnPoolSpawn>();

        List<IOnPoolDespawn> onDespawnInterfaces = new List<IOnPoolDespawn>();

        IOverrideSpawn overrideSpawnInterface;

        TrailRenderer[] subTrails;

        private void Awake()
        {
            subTrails = GetComponentsInChildren<TrailRenderer>();
            overrideSpawnInterface = GetComponent<IOverrideSpawn>();
            onSpawnInterfaces.AddRange(GetComponentsInChildren<IOnPoolSpawn>());
            onDespawnInterfaces.AddRange(GetComponentsInChildren<IOnPoolDespawn>());
        }

        public T PoolAddComponent<T>(GameObject target = null)
            where T : Component
        {
            if (target == null)
                target = gameObject;

            T component = target.AddComponent<T>();
            extensionComponents.Add(component);

            return component;
        }

        public void PoolDestroyComponent(Component component)
        {
            if (extensionComponents.Contains(component))
            {
                extensionComponents.Remove(component);
                Destroy(component);
            }
            else
            {
                Debug.LogWarning("对象池内对象组件被删除！！");
                disabledItem = true;
                Destroy(component);
            }
        }

        public void OnSpawn(SpawnPool spawnPool)
        {
            for (int i = 0; i < onSpawnInterfaces.Count; i++)
            {
                onSpawnInterfaces[i].OnPoolSpawn(spawnPool);
            }

        }

        public void OnDespawn()
        {
            if (subTrails != null)
                for (int i = 0; i < subTrails.Length; i++)
                {
                    subTrails[i].Clear();
                }
            for (int i = 0; i < onDespawnInterfaces.Count; i++)
            {
                onDespawnInterfaces[i].OnPoolDespawn();
            }
            for (int i = 0; i < extensionComponents.Count; i++)
            {
                var component = extensionComponents[i];
                if (component == null)
                    continue;

                var onDespawn = component as IOnPoolDespawn;
                if (onDespawn != null)
                    onDespawn.OnPoolDespawn();

                Destroy(component);
            }
            extensionComponents.Clear();
        }

        public void SetSpawn()
        {
            if (overrideSpawnInterface != null)
                overrideSpawnInterface.SpawnFunction(gameObject);
            else
                gameObject.SetActive(true);
        }

        public void SetDespawn()
        {
            if (overrideSpawnInterface != null)
                overrideSpawnInterface.DespawnFunction(gameObject);
            else
                gameObject.SetActive(false);
        }

    }
}

