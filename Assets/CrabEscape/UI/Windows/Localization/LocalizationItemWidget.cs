using System;
using UnityEngine; 
using TMPro;
using UnityEngine.Events;

public class LocalizationItemWidget : MonoBehaviour, IItemRenderer<LocalizationItemWidget.LocaleInfo>
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _selector;
    [SerializeField] private SelectLocale _onSelected;

    private LocaleInfo _data;

    private void Start()
    {
        LocalizationManager.I.OnLocaleChanged += UpdateSelection;
    }

    public void SetData(LocaleInfo localKey, int index)
    {
        _data = localKey;
        UpdateSelection();
        _text.text = localKey.LocaleId;
    }
    
    private void UpdateSelection()
    {
        var isSelected = LocalizationManager.I.LocalKey == _data.LocaleId;
        _selector.SetActive(isSelected);
    }

    public void OnSelected()
    {
        _onSelected?.Invoke(_data.LocaleId);
    }
    
    public class LocaleInfo
    {
        public string LocaleId;
    }

    private void OnDestroy()
    {
        LocalizationManager.I.OnLocaleChanged -= UpdateSelection;
    }
}

[Serializable]
public class SelectLocale : UnityEvent<string>
{
    
}

