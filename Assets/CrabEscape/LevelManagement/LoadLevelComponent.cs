using UnityEngine;

public class LoadLevelComponent : MonoBehaviour
{
    [SerializeField] private string _levelToLoad;

    public void LoadLevel()
    {
        var loader = FindObjectOfType<LevelLoader>();
        loader.LoadLevel(_levelToLoad);
    }
}
