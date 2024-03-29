using System.Collections.Generic;
using UnityEngine;

public class QuickInventoryController : MonoBehaviour
{
    [SerializeField] private Transform _inventorySlots;
    [SerializeField] private InventoryItemWidget _prefab;

    private GameSession _session;
    private readonly PlayerData _playerData;

    private readonly CompositeDisposable _trash = new CompositeDisposable();
    private List<InventoryItemWidget> _createdItems = new List<InventoryItemWidget>();

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
        Rebuild();
    }

    private void Rebuild()
    {
        var _inventory = _session.QuickInventory.Inventory;

        //create required items
        for (int i = _createdItems.Count; i < _inventory.Length; i++)
        {
            var item = Instantiate(_prefab, _inventorySlots);
            _createdItems.Add(item);
        }

        // update data and activate item
        for (int i = 0; i < _inventory.Length; i++)
        {
            _createdItems[i].SetData(_inventory[i], i);
            _createdItems[i].gameObject.SetActive(true);
        }

        // hide unused items
        for (int i = _inventory.Length; i < _createdItems.Count; i++)
        {
            _createdItems[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
