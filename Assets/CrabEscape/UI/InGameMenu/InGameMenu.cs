using UnityEngine;

public class InGameMenu : MainMenuWindow
{
    public void OnShowInGameMenu()
    {
        var window = Resources.Load<GameObject>("UI/InGameMenuWindow");
        var canvas = FindObjectOfType<Canvas>();
        Instantiate(window, canvas.transform);
        Close();
    }
    
    public void OnShowInGameUI()
    {
        var window = Resources.Load<GameObject>("UI/IngameMenuUI");
        var canvas = FindObjectOfType<Canvas>();
        Instantiate(window, canvas.transform);
        Close();
    }
}
