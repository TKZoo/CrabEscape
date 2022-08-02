using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Throwable", fileName = "Throwable")]
public class ThrowableRepository : DefRepository<ThrowableItemDef>
{
}

[Serializable]
public struct ThrowableItemDef : IHaveId
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private GameObject _projectilePf;

    public string Id => _id;
    public GameObject ProjectilePf => _projectilePf;
}
