using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Defs/LocalizationDef", fileName = "LocalizationDef")]
public class LocalizationDef : ScriptableObject
{
    [SerializeField] private string _url;
    [SerializeField] private List<LocalizationItem> _localizationItems;

    private UnityWebRequest _request;

    public Dictionary<string, string> GetData()
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var localizationItem in _localizationItems)
        {
            dictionary.Add(localizationItem.Key, localizationItem.Value);
        }

        return dictionary;
    }

    [ContextMenu("Update localization")]
    public void LoadLocalization()
    {
        if(_request != null) return;

        _request = UnityWebRequest.Get(_url);
        _request.SendWebRequest().completed += OnDataLoaded;
    }

    private void OnDestroy()
    {
        _request.SendWebRequest().completed -= OnDataLoaded;
    }

    private void OnDataLoaded(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            var rows = _request.downloadHandler.text.Split('\n');
            _localizationItems.Clear();
            foreach (var row in rows)
            {
                AddlocalizationItem(row);
            }
        }
    }

    private void AddlocalizationItem(string row)
    {
        try
        {
            var parts = row.Split('\t');
            _localizationItems.Add(new LocalizationItem{Key = parts[0], Value = parts[1]});

        }
        catch (Exception e)
        {
            Debug.LogError($"Can't parse row : {row}.\n {e}");
        }
    }

    [Serializable]
    private class LocalizationItem
    {
        public string Key;
        public string Value;
    }
}
