using System;
using UnityEngine;

[Serializable]
public abstract class PersistantProperty<TPropertyType>
{
    [SerializeField] protected TPropertyType _value;
    private TPropertyType _defValue;
    protected TPropertyType _stored;

    public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

    public event OnPropertyChanged OnChanged;

    public IDisposable Subscribe(OnPropertyChanged call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }
    
    public PersistantProperty(TPropertyType defValue)
    {
        _defValue = defValue;
    }

    public TPropertyType Value
    {
        get => _stored;
        set
        {
            var oldValue = _stored;
            var isEquals = _stored.Equals(value);
            if (isEquals) return;

            Write(value);
            _stored = _value = value;

            OnChanged?.Invoke(value, oldValue);
        }
    }

    protected void Init()
    {
        _stored = _value = Read(_defValue);
    }

    protected abstract void Write(TPropertyType value);
    protected abstract TPropertyType Read(TPropertyType defValue);
    
    public void Validate()
    {
        if (!_stored.Equals(_value))
        {
            Value = _value;
        }
    }
}