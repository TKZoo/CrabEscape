using System;
using UnityEngine;

[Serializable]
public class Cooldown
{
    [SerializeField] private float _value;

    private float _timeUp;

    public bool IsReady => _timeUp <= Time.time;

    public void Reset()
    {
        _timeUp = Time.time + _value;
    }
}