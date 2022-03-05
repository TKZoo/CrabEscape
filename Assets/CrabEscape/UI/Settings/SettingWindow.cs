using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingWindow : AnimatedWindow
{
    [SerializeField] private AudioSettingsWidget _music;
    [SerializeField] private AudioSettingsWidget _sfx;
    
    
    private GameObject window;

    protected override void Start()
    {
        base.Start();
        
        _music.SetModel(GameSettings.I.Music);
        _sfx.SetModel(GameSettings.I.Sfx);
    }
    
    public void OnShowMainMenu()
    {
        var curScene = SceneManager.GetActiveScene();
        var canvas = FindObjectOfType<Canvas>();
        if (curScene.name != "Main_menu")
        {
            window = Resources.Load<GameObject>("UI/InGameMenuWindow");
        }
        else
        {
            window = Resources.Load<GameObject>("UI/MainMenuWindow");
        }

        Instantiate(window, canvas.transform);
        Close();
    }
}
