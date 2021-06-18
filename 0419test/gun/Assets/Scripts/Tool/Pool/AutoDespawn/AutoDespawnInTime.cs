using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class AutoDespawnInTime : MonoBehaviour, IOnPoolSpawn, IOnPoolDespawn
    {
        public float time;
        private SpawnPool spawnPool;
        private Coroutine timer;

        public void OnPoolDespawn()
        {
            if (timer != null)
                StopCoroutine(timer);
        }

        public void OnPoolSpawn(SpawnPool spawnPool)
        {
            this.spawnPool = spawnPool;
            timer = StartCoroutine(DespawnTimer());
        }

        IEnumerator DespawnTimer()
        {
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            this.timer = null;
            spawnPool?.Despawn(transform);
        }
    }
}