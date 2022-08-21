using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private ProgressBarWidget _healthBar;
    [SerializeField] private UsedPerkWidget _usedPerk;
    [SerializeField] private bool _isPlayer;

    private GameSession _session;
    private HealthComponent mobHp;
    private CompositeDisposable _trash = new CompositeDisposable();

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        _trash.Retain(_session.PerksModel.Subscribe(OnPerkChanged));
        if (_isPlayer)
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.PlayerData.Hp.SubscribeAndInvoke(OnHealthChange));
        }
        if((!_isPlayer))
        {
            mobHp = gameObject.GetComponentInParent<HealthComponent>();
            _trash.Retain(mobHp.Hp.SubscribeAndInvoke(OnMobHealthChange));
        }

        OnPerkChanged();
    }

    private void OnPerkChanged()
    {
        var usedPerkId = _session.PerksModel.Used;
        var hasPerk = !string.IsNullOrEmpty(usedPerkId);
        if (hasPerk)
        {
            var perkDef = DefsFacade.I.Perks.Get(usedPerkId);
            _usedPerk.Set(perkDef);
        }
        _usedPerk.gameObject.SetActive(hasPerk);
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
    
    public void OnPerksWindow()
    {
        WindowUtils.CreateWindow("UI/ManagePerkWindow");
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}