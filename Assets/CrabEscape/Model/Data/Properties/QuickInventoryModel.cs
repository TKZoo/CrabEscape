using System;
using UnityEngine;

public class QuickInventoryModel
{
    private readonly PlayerData _playerData;

    public InventoryItemData[] Inventory { get; private set; }
    public readonly IntProperty SelectedIndex = new IntProperty();

    public event Action OnChanged;

    public InventoryItemData SelectedItem => Inventory[SelectedIndex.Value];

    public QuickInventoryModel(PlayerData playerData)
    {
        _playerData = playerData;
        Inventory = _playerData.Inventory.GetAll(ItemTag.Throwable, ItemTag.Usable);
        
        _playerData.Inventory.OnChanged += OnInventoryChange;
        Debug.Log(Inventory.Length);
    }

    public IDisposable Subscribe(Action call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }

    private void OnInventoryChange(string id, int value)
    {
        var indexFound = Array.FindIndex(Inventory, x => x.Id == id);
        if (indexFound != -1)
        {
            Inventory = _playerData.Inventory.GetAll(ItemTag.Throwable, ItemTag.Usable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            OnChanged?.Invoke();
        }
    }

    public void SetNextItem()
    {
        SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
    }
}