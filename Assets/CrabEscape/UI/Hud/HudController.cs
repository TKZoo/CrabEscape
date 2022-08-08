using UnityEditor.PackageManager.UI;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private ProgressBarWidget _healthBar;
    [SerializeField] private bool _isPlayer;

    private GameSession _session;
    private HealthComponent mobHp;

    private void Start()
    {
        if (_isPlayer)
        {
            _session = FindObjectOfType<GameSession>();
            _session.PlayerData.Hp.OnChanged += OnHealthChange;
            OnHealthChange(_session.PlayerData.Hp.Value, _session.PlayerData.Hp.Value);
        }
        if((!_isPlayer))
        {
            mobHp = gameObject.GetComponentInParent<HealthComponent>();
            mobHp.Hp.OnChanged += OnMobHealthChange;
            OnMobHealthChange(mobHp.Hp.Value, mobHp.Hp.Value);
        }
    }

    private void OnMobHealthChange(int newvalue, int oldvalue)
    {
        var maxHealth = mobHp.GetMaxHp();
        var value = (float) newvalue / maxHealth;
        _healthBar.SetProgress(value);
    }

    private void OnHealthChange(int newvalue, int oldvalue)
    {
        var maxHealth = DefsFacade.I.Player.MaxHealth;
        var value = (float) newvalue / maxHealth;
        _healthBar.SetProgress(value);
    }
    
    public void OnLevelUpWindow()
    {
        WindowUtils.CreateWindow("UI/PlayerLevelUpWindow");
    }

    private void OnDestroy()
    {
        if (_isPlayer)
        {
            _session.PlayerData.Hp.OnChanged -= OnHealthChange;
        }
        else
        {
            mobHp.Hp.OnChanged -= OnMobHealthChange;
        }
    }
}