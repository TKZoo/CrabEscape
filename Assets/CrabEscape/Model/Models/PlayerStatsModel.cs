using System;

public class PlayerStatsModel : IDisposable
{
      private readonly PlayerData _playerData;
      public event Action OnChanged;

      public ObservableProperty<statId> InterfaceSelectedStat = new ObservableProperty<statId>();
      private readonly CompositeDisposable _trash = new CompositeDisposable();

      public PlayerStatsModel(PlayerData playerData)
      { 
            _playerData = playerData;
            InterfaceSelectedStat.Subscribe((x, y) => OnChanged?.Invoke());
      }

      public void LevelUp(statId id)
      {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;
            if (def.Levels.Length <= nextLevel) return;
            
            var price = def.Levels[nextLevel].Price;
            if(!_playerData.Inventory.IsEnough(price)) return;
            
            _playerData.Inventory.Remove(price.ItemId, price.Count);
            _playerData.PlayerLevels.LevelUp(id);
            
            OnChanged?.Invoke();
      }

      public int GetCurrentLevel(statId id) => _playerData.PlayerLevels.GetLevel(id);

      public StatLevel GetLevelDef(statId id, int level = -1)
      {
            if (level == -1)
            {
                  level = GetCurrentLevel(id);
            }
            var def = DefsFacade.I.Player.GetStat(id);
            if (def.Levels.Length > level)
            {
                  return def.Levels[level];
            }

            return default;

      }

      public float GetValue(statId id, int level = -1)
      {
            return GetLevelDef(id, level).Value;
      }

      public IDisposable Subscribe(Action call)
      {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
      }
      
      public void Dispose()
      {
            _trash.Dispose();
      }
}