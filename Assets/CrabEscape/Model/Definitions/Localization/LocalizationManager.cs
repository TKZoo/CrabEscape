using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    public readonly static LocalizationManager I;

    private StringPersistentProperty _localeKey = new StringPersistentProperty("eng", "localization/current");
    private Dictionary<string, string> _localization;

    public event Action OnLocaleChanged;
    
    static LocalizationManager()
    {
        I = new LocalizationManager();
    }

    public LocalizationManager()
    {
        LoadLocale(_localeKey.Value);
    }

    public string LocalKey => _localeKey.Value;

    private void LoadLocale(string localizationToLoad)
    {
        var def = Resources.Load<LocalizationDef>($"Localization/{localizationToLoad}");
        _localization = def.GetData();
        _localeKey.Value = localizationToLoad;
        OnLocaleChanged?.Invoke();
    }

    public string Localize(string key)
    {
        return _localization.TryGetValue(key, out var value) ? value : $"%%%{key}$$$";
    }

    public void SetLocale(string localeKey)
    {
        LoadLocale(localeKey);
    }
}
