using UnityEngine;

public static class WindowUtils
{
    private static GameObject canvas;
    public static void CreateWindow(string resourcePath)
    {
        var window = Resources.Load<GameObject>(resourcePath);
        //var canvas = Object.FindObjectOfType<Canvas>();
        canvas = GameObject.FindWithTag("Canvas");
        Object.Instantiate(window, canvas.transform);
    }
}