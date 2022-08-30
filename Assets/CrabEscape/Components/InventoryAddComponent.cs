using UnityEngine;

public class InventoryAddComponent : MonoBehaviour
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private int _count;
  
    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }

    public void Add(GameObject go)
    {
        _session.PlayerData.Inventory.Add(_id, _count);
    }
}