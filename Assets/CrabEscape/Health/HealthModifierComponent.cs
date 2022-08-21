using UnityEngine;

public class HealthModifierComponent : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private int _healing;

    private int additionalValue;
    
    public int SetAdditionalValue(int value)
    {
        additionalValue = value;
        return additionalValue;
    }
    
    public void DoDamage(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.ApplyDamage(_damage + additionalValue);
        }
    }

    public void DoHealing(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.ApplyHealing(_healing + additionalValue);
        }
    }
}
