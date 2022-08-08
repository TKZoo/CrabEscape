using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUpWindow : AnimatedWindow
{
    [SerializeField] private Transform _statContainer;
    [SerializeField] private StatWidget _prefab;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private ItemWidget _price;

    private DataGroup<StatDef, StatWidget> _dataGroup;

    private GameSession _session;
    private CompositeDisposable _trash = new CompositeDisposable();

    protected override void Start()
    {
        base.Start();
        
        _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _statContainer);
        
        _session = FindObjectOfType<GameSession>();
        _session.StatsModel.InterfaceSelectedStat.Value = DefsFacade.I.Player.Stats[0].Id;

        _trash.Retain(_session.StatsModel.Subscribe(OnLevelChanged));
        _trash.Retain(_upgradeButton.onClick.Subscribe(OnUpgrade));
        
        OnLevelChanged();
    }

    private void OnUpgrade()
    {
        var selected = _session.StatsModel.InterfaceSelectedStat.Value;
        _session.StatsModel.LevelUp(selected);
    }

    private void OnLevelChanged()
    {
        var stats = DefsFacade.I.Player.Stats;
        _dataGroup.SetData(stats);

        var selected = _session.StatsModel.InterfaceSelectedStat.Value;
        var nextLevel = _session.StatsModel.GetCurrentLevel(selected) + 1;
        var def = _session.StatsModel.GetLevelDef(selected, nextLevel);
        _price.SetData(def.Price);
        
        _price.gameObject.SetActive(def.Price.Count !=0);
        _upgradeButton.gameObject.SetActive(def.Price.Count !=0);
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}