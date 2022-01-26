using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnRandomPrefabComponent : MonoBehaviour
{
    [SerializeField] private PrefabListToSpawn[] _prefabs;
    [SerializeField] private Transform _target;
    [SerializeField] private int _totalDiffPrefabs = 0;
    
    private int totalWeight = -1;
    
    public int TotalWeight
    {
        get
        {
            if (totalWeight == -1)
            {
                CalculateWeight();
            }
            return totalWeight;
        }
    }
    
    private int CalculateWeight()
    {
        totalWeight = 0;
        for(int i = 0; i < _prefabs.Length; i++)
        { 
            totalWeight += _prefabs[i].weight;
        }
        return totalWeight;
    }

    public void SpawnPrefabs()
    {
        totalWeight = CalculateWeight();
        int roll = Random.Range(0, TotalWeight);
        int prefabsAmount = 0;
        for(int i = 0; i < _prefabs.Length; i++)
        {
            if (roll <= _prefabs[i].weight)
            {
                for(int j = 0; j < _prefabs[i].amount; j++)
                {
                    Instantiate(_prefabs[i].prefab, _target.position, Quaternion.identity);
                }
                prefabsAmount++;
                if (prefabsAmount >= _totalDiffPrefabs)
                {
                    return;
                }
            }
            else
            {
                roll -= _prefabs[i].weight;
            }
        }
    }

    [Serializable]
    public class PrefabListToSpawn
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _weight;
        [SerializeField] private int _amount;

        public GameObject prefab => _prefab;
        public int weight => _weight;
        public int amount => _amount;
    }
    
}
