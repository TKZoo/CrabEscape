using System;
using UnityEngine;

public class InventoryAddComponent : MonoBehaviour
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private int _count;
    
    public event Action OnChanged;
    
    private GameSession _session;

    private void Awake()
    {
        _session = FindObjectOfType<GameSession>();
    }
    
    public IDisposable Subscribe(Action call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }

    public void Add(GameObject go)
    {
        var hero = go.GetComponent<Hero>();
        if (hero != null)
        {
            _session.PlayerData.Inventory.Add(_id, _count);
            OnChanged?.Invoke();
        }
    }
}