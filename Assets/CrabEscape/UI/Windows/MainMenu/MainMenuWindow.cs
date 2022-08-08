using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : AnimatedWindow
{
    private Action _closeAction;

    public void OnShowSettings()
    {
        var window = Resources.Load<GameObject>("UI/ManagePerkWindow"); //"UI/SettingsWindow"
        var canvas = FindObjectOfType<Canvas>();
        Instantiate(window, canvas.transform);
        Close();
    }

    public void OnStartGame()
    {
        _closeAction = () => { SceneManager.LoadScene("Level_3"); };
        Close();
    }

    public void OnQuitGame()
    {
        _closeAction = () =>
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };
        Close();
    }

    public override void OnCloseAnimationComplete()
    {
        _closeAction?.Invoke();
        base.OnCloseAnimationComplete();
    }
}