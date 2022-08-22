using System;
using UnityEngine;

public class QuickInventoryModel : IDisposable
{
    private readonly PlayerData _playerData;

    public InventoryItemData[] Inventory { get; private set; }
    public readonly IntProperty SelectedIndex = new IntProperty();
    private readonly CompositeDisposable _trash = new CompositeDisposable();

    public event Action OnChanged;

    public InventoryItemData SelectedItem
    {
        get
        {
            if (Inventory.Length > 0 && Inventory.Length > SelectedIndex.Value)
                return Inventory[SelectedIndex.Value];
                
            return null;
        }
    }

    public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);
    
    public QuickInventoryModel(PlayerData playerData)
    {
        _playerData = playerData;
        Inventory = _playerData.Inventory.GetAll(ItemTag.Throwable, ItemTag.Usable, ItemTag.Consumable);
        
        _playerData.Inventory.OnChanged += OnInventoryChange;
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
            Inventory = _playerData.Inventory.GetAll(ItemTag.Throwable, ItemTag.Usable, ItemTag.Consumable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            OnChanged?.Invoke();
        }
    }

    public void SetNextItem()
    {
        SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
    }

    public void Dispose()
    {
        _playerData.Inventory.OnChanged -= OnInventoryChange;
    }
}