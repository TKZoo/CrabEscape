using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Usable", fileName = "Usable")]
public class UsableRepository : DefRepository<UsableItemDef>
{
}

[Serializable]
public struct UsableItemDef : IHaveId
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private float _value;
    [SerializeField] private float _effectTime;
    public string Id => _id;
    public float Value => _value;
    public float EffectTime => _effectTime;
}
