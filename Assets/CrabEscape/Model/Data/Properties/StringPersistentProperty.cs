using UnityEngine;

public class StringPersistentProperty : PrefsPersistentProperty<string>
{
    public StringPersistentProperty(string defValue, string key) : base(defValue, key)
    {
        Init();
    }

    protected override void Write(string value)
    {
        PlayerPrefs.SetString(Key, value);
    }

    protected override string Read(string defValue)
    {
        return PlayerPrefs.GetString(Key, defValue);
    }
}
