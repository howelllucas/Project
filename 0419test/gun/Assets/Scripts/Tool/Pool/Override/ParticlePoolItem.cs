using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class ParticlePoolItem : MonoBehaviour, IOverrideSpawn
    {
        private ParticleSystem[] particles;

        private void Awake()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
        }

        public void DespawnFunction(GameObject root)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        public void SpawnFunction(GameObject root)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }
    }
}