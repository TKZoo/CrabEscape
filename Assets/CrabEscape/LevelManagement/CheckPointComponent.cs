using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpawnPrefabComponent))]

public class CheckPointComponent : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private UnityEvent _setChecked;
    [SerializeField] private UnityEvent _setUnchecked;
    [SerializeField] private SpawnPrefabComponent _heroSpawner;

    private GameSession _session;

    public string Id => _id;

    

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        if (_session.IsChecked(_id))
        {
            _setChecked?.Invoke();
        }
        else
        {
            _setUnchecked?.Invoke();
        }
    }

    public void Check()
    {
        _setChecked?.Invoke();
        _session.SetChecked(_id);
    }

    public void SpawnHero()
    {
        _heroSpawner.SpawnPrefab();
    }
    
}


