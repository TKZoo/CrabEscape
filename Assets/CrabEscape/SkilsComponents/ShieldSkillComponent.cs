using UnityEngine;

public class ShieldSkillComponent : MonoBehaviour
{
    [SerializeField] private Cooldown _cooldown;
    [SerializeField] private HealthComponent _healthComponent;

    public void UseShield()
    {
        _healthComponent.Immune = true;
        _cooldown.Reset();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_cooldown.IsReady)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _healthComponent.Immune = false;
    }
}