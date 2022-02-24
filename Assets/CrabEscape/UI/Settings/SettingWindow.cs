using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingWindow : AnimatedWindow
{
    private GameObject window;
    
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
