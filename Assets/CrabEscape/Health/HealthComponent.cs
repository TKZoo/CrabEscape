using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour, ISaveable
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onTakeHealing;
    [SerializeField] private HealthChangeEvent _onHealthChange;
    [SerializeField] public UnityEvent _onDie;
    [SerializeField] private bool _immune;

    public IntProperty Hp = new IntProperty();
    public bool Immune
    {
        get => _immune;
        set => _immune = value;
    }

    private int _maxHealth;

    private void Awake()
    {
        SetHealth(_health);
        _maxHealth = _health;
    }

    public void ApplyDamage(int damageValue)
    {
        if (Immune == true) return;
        if (_health > 0)
        {
            _health -= damageValue;
            _onHealthChange?.Invoke(_health);
            _onTakeDamage?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }
    }

    public void ApplyHealing(int healValue)
    {
        _health += healValue;
        _onHealthChange?.Invoke(_health);
        _onTakeHealing?.Invoke();
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public void SetHealth(int health)
    {
        Hp.Value = health;
    }

    public int GetMaxHp()
    {
        return _maxHealth;
    }

    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {
    }
    public object SaveState()
    {
        return new SaveData()
        {
            health = _health
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        Hp.Value = saveData.health;
        Debug.Log(Hp.Value);
    }

    [Serializable]
    private struct SaveData
    {
        public int health;
    }
}
