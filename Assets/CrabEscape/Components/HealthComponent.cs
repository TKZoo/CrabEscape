using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onTakeHealing;
    [SerializeField] private UnityEvent _onDie;
    private int maxHealth;

    private void OnEnable()
    {
        maxHealth = _health;
    }

    public void ApplyDamage(int damageValue)
    {
        _health -= damageValue;
        _onTakeDamage?.Invoke();
        if(_health <= 0)
        {
            _onDie?.Invoke();
        }        
    }

    public void ApplyHealing(int healValue)
    {
        _health += healValue;
        _onTakeHealing?.Invoke();
        if (_health > maxHealth)
        {
            _health = maxHealth;
        }
    }

}
