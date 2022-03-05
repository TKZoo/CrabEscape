using System;
using UnityEngine;

[Serializable]
public class FloatPersistentProperty : PrefsPersistentProperty<float>
{
    public FloatPersistentProperty(float defValue, string key) : base(defValue, key)
    {
        Init();
    }

    protected override void Write(float value)
    {
        PlayerPrefs.SetFloat(Key, value);
        PlayerPrefs.Save();
    }

    protected override float Read(float defValue)
    {
       return PlayerPrefs.GetFloat(Key, defValue);
    }
}
