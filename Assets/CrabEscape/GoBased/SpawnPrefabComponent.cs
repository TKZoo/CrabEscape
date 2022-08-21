using UnityEngine;

public class SpawnPrefabComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _prefab;

    [ContextMenu("Spawn Prefab")]
    
    public GameObject SpawnPrefab()
    {
        if (_prefab == null)
        {
            _prefab = gameObject;
        }
        var instantiate = Instantiate(_prefab, _target.position, Quaternion.identity);
        instantiate.transform.localScale = _target.lossyScale;
        return instantiate;
    }

    public void SetAndSpawn(GameObject pf)
    {
        _prefab = pf;
        SpawnPrefab();
    }

    public void SetPrefab(GameObject pf)
    {
        _prefab = pf;
    }
}
