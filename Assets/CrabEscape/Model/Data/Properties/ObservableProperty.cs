using System;
using UnityEngine;

public class ObservableProperty<TPropertyType>
{
    [SerializeField] protected TPropertyType _value;

    public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
    
    public event OnPropertyChanged OnChanged;

    public IDisposable Subscribe(OnPropertyChanged call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }
    public IDisposable SubscribeAndInvoke(OnPropertyChanged call)
    {
        OnChanged += call;
        var dispose = new ActionDisposable(() => OnChanged -= call);
        call(_value, _value);
        return dispose;
    }
    
    public TPropertyType Value
    {
        get => _value;
        set
        {
            var isSame = _value?.Equals(value) ?? false;
            if(isSame) return;
            var oldValue = _value;

            _value = value;

            OnChanged?.Invoke(_value, oldValue);
        }
    }
}
