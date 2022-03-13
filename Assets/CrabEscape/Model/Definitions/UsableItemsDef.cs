using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/UsableItems", fileName = "UsableItems")]
public class UsableItemsDef : ScriptableObject
{
    [SerializeField] private UsableItemDef[] _items;

    public UsableItemDef Get(string id)
    {
        foreach (var usableItem in _items)
        {
            if (usableItem.Id == id)
            {
                return usableItem;
            }
        }

        return default;
    }
}

[Serializable]
public struct UsableItemDef
{
    [InventoryId] [SerializeField] private string _id;
    public string Id => _id;
}