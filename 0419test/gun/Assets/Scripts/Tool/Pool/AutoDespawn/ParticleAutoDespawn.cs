using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class ParticleAutoDespawn : MonoBehaviour, IOnPoolSpawn, IOnPoolDespawn
    {
        public ParticleSystem targetParticle;
        public float maxStayTime = 5f;
        private SpawnPool spawnPool;
        private Coroutine timer;

        private void Awake()
        {
            if (targetParticle == null)
                targetParticle = GetComponent<ParticleSystem>();
        }

        public void OnPoolSpawn(SpawnPool spawnPool)
        {
            this.spawnPool = spawnPool;
            if (targetParticle != null)
            {
                timer = StartCoroutine(ListenForEmitDespawn());
            }
        }

        private IEnumerator ListenForEmitDespawn()
        {
            yield return new WaitForSeconds(targetParticle.main.startDelay.constantMax + 0.25f);

            GameObject emitterGO = targetParticle.gameObject;
            float stayTimer = 0;
            while (targetParticle.IsAlive(true) && emitterGO.activeInHierarchy)
            {
                if (stayTimer > maxStayTime)
                    break;
                stayTimer += Time.deltaTime;
                yield return null;
            }
            yield return null;
            timer = null;
            if (emitterGO.activeInHierarchy)
            {
                spawnPool?.Despawn(transform);
                targetParticle.Clear(true);
            }
        }

        public void OnPoolDespawn()
        {
            if (timer != null)
                StopCoroutine(timer);
        }
    }
}