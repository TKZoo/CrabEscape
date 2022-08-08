using System;
using UnityEngine;

[Serializable]
public struct StatDef
{
    [SerializeField] private string _name;
    [SerializeField] private statId _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private StatLevel[] _levels;

    public statId Id => _id;
    public string Name => _name;
    public Sprite Icon => _icon;
    public StatLevel[] Levels => _levels;
}

[Serializable]
public struct StatLevel
{
    [SerializeField] private float _value;
    [SerializeField] private ItemWithCount _price;

    public float Value => _value;
    public ItemWithCount Price => _price;
}
public enum statId
{
    Hp,
    Speed,
    RangeDamage
}