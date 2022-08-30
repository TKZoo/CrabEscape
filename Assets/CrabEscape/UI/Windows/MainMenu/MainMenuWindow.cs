using System;
using UnityEngine;

public class MainMenuWindow : AnimatedWindow
{
    private Action _closeAction;

    public void OnShowSettings()
    {
        var window = Resources.Load<GameObject>("UI/SettingsWindow");
        var canvas = FindObjectOfType<Canvas>();
        Instantiate(window, canvas.transform);
        Close();
    }

    public void OnStartGame()
    {
        _closeAction = () =>
        {
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel("Level_1");
        };
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