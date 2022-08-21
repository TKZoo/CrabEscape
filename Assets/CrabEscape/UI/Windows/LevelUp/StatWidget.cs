using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
{
    [SerializeField] private TMP_Text _statName;
    [SerializeField] private Image _statIcon;
    [SerializeField] private TMP_Text _currentStatValue;
    [SerializeField] private TMP_Text _increaseStatValue;
    [SerializeField] private ProgressBarWidget _progressBar;
    [SerializeField] private Image _selector;

    private GameSession _session;
    private StatDef _data;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        UpdateView();
    }
 
    private void UpdateView()
    {
        var statModel = _session.StatsModel;
        
        _statIcon.sprite = _data.Icon;
        _statName.text = _data.Name; // LocalizationManager.I.Localize(_data.Name);
        var currentLevelValue = statModel.GetValue(_data.Id);
        _currentStatValue.text = _session.StatsModel.GetValue(_data.Id).ToString(CultureInfo.InvariantCulture);

        var currentLevel = statModel.GetCurrentLevel(_data.Id);
        var nextLevel = statModel.GetCurrentLevel(_data.Id) + 1;
        var nextLevelValue  = statModel.GetValue(_data.Id, nextLevel);
        var increaseValue = nextLevelValue - currentLevelValue;
        _increaseStatValue.gameObject.SetActive(increaseValue > 0);
        _increaseStatValue.text = increaseValue.ToString(CultureInfo.InvariantCulture);

        var maxLevel = DefsFacade.I.Player.GetStat(_data.Id).Levels.Length -1;
        _progressBar.SetProgress(currentLevel / (float) maxLevel);
        
        _selector.gameObject.SetActive(_session.StatsModel.InterfaceSelectedStat.Value == _data.Id);
    }

    public void SetData(StatDef data, int index)
    {
        _data = data;
        if(_session != null)
        {
            UpdateView();
        }
    }

    public void OnSelect()
    {
        _session.StatsModel.InterfaceSelectedStat.Value = _data.Id;
    }
}