using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onTakeHealing;
    [SerializeField] private UnityEvent _onDie;
    private int _maxHealth;

    private void OnEnable()
    {
        _maxHealth = _health;
    }

    public void ApplyDamage(int damageValue)
    {
        _health -= damageValue;
        if (_health > 0)
        {
            _onTakeDamage?.Invoke();
        }
        else
        {
            _onDie?.Invoke();
        }
    }

    public void ApplyHealing(int healValue)
    {
        _health += healValue;
        _onTakeHealing?.Invoke();
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

}
