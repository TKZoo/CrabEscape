using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableItems")]
public class ThrowableItemsDef : ScriptableObject
{
    [SerializeField] private ThrowableItemDef[] _items;

    public ThrowableItemDef Get(string id)
    {
        foreach (var throwableItem in _items)
        {
            if (throwableItem.Id == id)
            {
                return throwableItem;
            }
        }
        return default;
    }
}

[Serializable]
public struct ThrowableItemDef
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private GameObject _projectilePf;

    public string Id => _id;
    public GameObject ProjectilePf => _projectilePf;
}
