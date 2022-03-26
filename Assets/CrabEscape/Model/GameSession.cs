using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    public PlayerData PlayerData => _playerData;
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    public QuickInventoryModel QuickInventory { get; private set; }

    private void Awake()
    {
        LoadHud();
        
        if (IsSessionExist())
        {
            Destroy(gameObject);
        }
        else
        {
            InitModels();
            DontDestroyOnLoad(this);
        }
    }

    private void InitModels()
    {
        QuickInventory = new QuickInventoryModel(PlayerData);
        _trash.Retain(QuickInventory);
    }

    private void LoadHud()
    {
        SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
    }

    private bool IsSessionExist()
    {
        var sessions = FindObjectsOfType<GameSession>();
        foreach (var gameSession in sessions)
        {
            if (gameSession != this)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
