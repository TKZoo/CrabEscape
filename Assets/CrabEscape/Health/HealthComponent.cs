using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onTakeHealing;
    [SerializeField] public UnityEvent _onDie;
    [SerializeField] private HealthChangeEvent _onHealthChange;

   public IntProperty Hp = new IntProperty();
    
    private int _maxHealth;

    private void Awake()
    {
        SetHealth(_health);
        _maxHealth = _health;
    }

    public void ApplyDamage(int damageValue)
    {
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
}
