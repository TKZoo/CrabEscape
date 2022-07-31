using System.Collections.Generic;
using UnityEngine;

public class LocalizationWindow : AnimatedWindow
{
    [SerializeField] private Transform _container;
    [SerializeField] private LocalizationItemWidget _prefab;
    
    private DataGroup<LocalizationItemWidget.LocaleInfo, LocalizationItemWidget> _dataGroup;

    private string[] _supportedLocalizations = {"rus", "lat", "eng"};

    protected override void Start()
    {
        base.Start();
        _dataGroup = new DataGroup<LocalizationItemWidget.LocaleInfo, LocalizationItemWidget>(_prefab, _container);
        _dataGroup.SetData(ComposeData());
    }
    
    public void OnShowSettings()
    {
        var canvas = FindObjectOfType<Canvas>();
        var window = Resources.Load<GameObject>("UI/SettingsWindow");
        Instantiate(window, canvas.transform);
        Close();
    }
    
    private List<LocalizationItemWidget.LocaleInfo> ComposeData()
    {
        var data = new List<LocalizationItemWidget.LocaleInfo>();
        foreach (var localization in _supportedLocalizations)
        {
            data.Add(new LocalizationItemWidget.LocaleInfo{LocaleId = localization});
        }

        return data;
    }

    public void OnSelected(string selectedLocale)
    {
        LocalizationManager.I.SetLocale(selectedLocale);
    }
}
