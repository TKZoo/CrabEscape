using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Perks", fileName = "Perks")]
public class PerkRepository : DefRepository<PerkDef>
{
}

[Serializable]
public struct  PerkDef : IHaveId
{
    [SerializeField] private string _id;
    [SerializeField] private string _info;
    [SerializeField] private Sprite _icon;
    [SerializeField] private float _cooldown;
    [SerializeField] private ItemWithCount _price;
    
    public string Id => _id;
    public string Info => _info;
    public Sprite Icon => _icon;
    public float Cooldown => _cooldown;
    public ItemWithCount Price => _price;
}

[Serializable]
public struct ItemWithCount
{
    [InventoryId] [SerializeField] private string _itemId;
    [SerializeField] private int _count;

    public string ItemId => _itemId;
    public int Count => _count;
}