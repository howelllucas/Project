using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public interface IOnPoolSpawn
    {
        void OnPoolSpawn(SpawnPool spawnPool);
    }

    public interface IOnPoolDespawn
    {
        void OnPoolDespawn();
    }

    public interface IOverrideSpawn
    {
        void SpawnFunction(GameObject root);
        void DespawnFunction(GameObject root);
    }
}
