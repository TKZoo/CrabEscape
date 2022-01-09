using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelComponent : MonoBehaviour
{
    [SerializeField] private string _levelToLoad;

    public void LoadLevel() 
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
