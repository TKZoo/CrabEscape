using System;
public class PerksModel : IDisposable
{
    private readonly PlayerData _playerData;
    public readonly StringProperty  InterfaceSelection = new StringProperty();

    private readonly CompositeDisposable _trash = new CompositeDisposable();
    public event Action OnChanged;

    public readonly Cooldown Cooldown = new Cooldown();

    public PerksModel(PlayerData playerdata)
    {
        _playerData = playerdata;
        InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;
        
        _trash.Retain(_playerData.Perks.Used.Subscribe((x,y) => OnChanged?.Invoke()));
        _trash.Retain(InterfaceSelection.Subscribe((x,y) => OnChanged?.Invoke()));
    }

    public IDisposable Subscribe(Action call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }
    public bool IsSuperThrowEnabled => _playerData.Perks.Used.Value == "super_throw" && Cooldown.IsReady; //_playerData.Perks.Unlocked = if plane do all perk available in same time
    public bool IsDoubleJumpEnabled => _playerData.Perks.Used.Value == "double_jump" && Cooldown.IsReady;
    public bool IsShieldEnabled => _playerData.Perks.Used.Value == "energy_shield" && Cooldown.IsReady;
    public string Used => _playerData.Perks.Used.Value;
    
    public void Unlock(string id)
    {
        var def = DefsFacade.I.Perks.Get(id);
        var isEnoughResources = _playerData.Inventory.IsEnough(def.Price);

        if (isEnoughResources)
        {
            _playerData.Inventory.Remove(def.Price.ItemId, def.Price.Count);
            _playerData.Perks.AddPerks(id);
            OnChanged?.Invoke();
        }
    }

    public void SelectPerk(string selected)
    {
        var perkDef = DefsFacade.I.Perks.Get(selected);
        Cooldown.Value = perkDef.Cooldown;
        _playerData.Perks.Used.Value = selected;
    }

    public bool IsUsed(string perkId)
    {
        return _playerData.Perks.Used.Value == perkId;
    }
    
    public bool IsUnlocked(string perkId)
    {
        return _playerData.Perks.IsUnlocked(perkId);
    }
    
    public bool CanBuy(string perkId)
    {
        var def = DefsFacade.I.Perks.Get(perkId);
        return _playerData.Inventory.IsEnough(def.Price);
    }
    
    public void Dispose()
    {
        _trash.Dispose();
    }
}