using UnityEngine;

public class SpawnPrefabComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _prefab;

    [ContextMenu("Spawn Prefab")]
    public void SpawnPrefab(GameObject prefab)
    {        
        var instantiate = Instantiate(prefab, _target.position, Quaternion.identity);
        instantiate.transform.localScale = _target.lossyScale;
    }
}
