using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private InventoryData _inventory;

    public InventoryData Inventory => _inventory;
    public int Hp;
}


