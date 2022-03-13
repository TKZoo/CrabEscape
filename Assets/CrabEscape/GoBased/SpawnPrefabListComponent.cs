using System;
using UnityEngine;

public class SpawnPrefabListComponent : MonoBehaviour
{
    [SerializeField] private SpawnPrefabData[] _pfSpawners;

    public void Spawn(string id)
    {
        foreach (var data in _pfSpawners)
        {
            if (data.id == id)
            {
                data.Component.SetAndSpawn(data.pfSpawn);
                break;
            }
        }
    }
    
    [Serializable]
    public class SpawnPrefabData
    {
        public string id;
        public SpawnPrefabComponent Component;
        public GameObject pfSpawn;
    }
}
