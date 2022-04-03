using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingWindow : AnimatedWindow
{
    [SerializeField] private AudioSettingsWidget _music;
    [SerializeField] private AudioSettingsWidget _sfx;

    private Canvas _canvas;
    private GameObject window;

    protected override void Start()
    {
        base.Start();
        _canvas = FindObjectOfType<Canvas>();
        _music.SetModel(GameSettings.I.Music);
        _sfx.SetModel(GameSettings.I.Sfx);
    }

    public void OnShowLanguageMenu()
    {
        window = Resources.Load<GameObject>("UI/LocalizationWindow");
        Instantiate(window, _canvas.transform);
        Close();
    }
    
    public void OnShowMainMenu()
    {
        var curScene = SceneManager.GetActiveScene();
        if (curScene.name != "Main_menu")
        {
            window = Resources.Load<GameObject>("UI/InGameMenuWindow");
        }
        else
        {
            window = Resources.Load<GameObject>("UI/MainMenuWindow");
        }

        Instantiate(window, _canvas.transform);
        Close();
    }
}
