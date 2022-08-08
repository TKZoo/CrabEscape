using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private string _defaultCheckPoint;

    public PlayerData PlayerData => _playerData;
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    public QuickInventoryModel QuickInventory { get; private set; }
    public PerksModel PerksModel { get; private set; }
    public PlayerStatsModel StatsModel { get; private set; }

    private readonly List<string> _checkpoints = new List<string>();

    private void Awake()
    {
        var existsSession = GetExistsSession();
        if (existsSession != null)
        {
            existsSession.StartSession(_defaultCheckPoint);
            Destroy(gameObject);
        }
        else
        {
            InitModels();
            DontDestroyOnLoad(this);
            StartSession(_defaultCheckPoint);
        }
    }

    private void StartSession(string defaultCheckPoint)
    {
        SetChecked(defaultCheckPoint);
        LoadHud();
        SpawnHero();
    }

    private void SpawnHero()
    {
        var checkpoints = FindObjectsOfType<CheckPointComponent>();
        var lastCheckPoint = _checkpoints.Last();
        foreach (var checkPoint in checkpoints)
        {
            if (checkPoint.Id == lastCheckPoint)
            {
                checkPoint.SpawnHero();
                break;
            }
        }
    }

    public bool IsChecked(string id)
    {
        return _checkpoints.Contains(id);
    }

    public void SetChecked(string id)
    {
        if (!_checkpoints.Contains(id))
        {
            _checkpoints.Add(id);
            Debug.Log(_checkpoints.Last());
        }
    }

    private void InitModels()
    {
        QuickInventory = new QuickInventoryModel(_playerData);
        _trash.Retain(QuickInventory);
        PerksModel = new PerksModel(_playerData);
        _trash.Retain(PerksModel);
        StatsModel = new PlayerStatsModel(_playerData);
        _trash.Retain(StatsModel);
    }

    private void LoadHud()
    {
        SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
    }

    private GameSession GetExistsSession()
    {
        var sessions = FindObjectsOfType<GameSession>();
        foreach (var gameSession in sessions)
        {
            if (gameSession != this)
            {
                return gameSession;
            }
        }

        return null;
    }

    public void SaveGame()
    {
    }

    public void LoadGame()
    {
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}