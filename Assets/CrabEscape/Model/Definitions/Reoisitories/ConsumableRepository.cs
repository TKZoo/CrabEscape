using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Consumable", fileName = "Consumable")]
public class ConsumableRepository : DefRepository<ConsumableItemDef>
{
}

[Serializable]
public struct ConsumableItemDef : IHaveId
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private ConsumableItemType _consumableItemType;
    [SerializeField] private float _value;
    [SerializeField] private float _effectTime;
    public string Id => _id;
    public float Value => _value;
    public float EffectTime => _effectTime;
    public ConsumableItemType ConsumableItemType => _consumableItemType;
}

public enum ConsumableItemType
{
    HealthPotion,
    SpeedPotion
}