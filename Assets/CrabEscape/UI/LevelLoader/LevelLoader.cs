using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _transitionTime;
    private AsyncOperation _asyncOperation;
 
    private static readonly int Enabled = Animator.StringToHash("Enabled");

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoad()
    {
        InitLoader();
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private static void InitLoader()
    {
        SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive);
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(StartAnimation(sceneName));
    }

    private IEnumerator StartAnimation(string sceneName)
    {
        _animator.SetBool(Enabled, true);
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneName);
        _animator.SetBool(Enabled, false);

        //_asyncOperation = SceneManager.LoadSceneAsync("");
        //_asyncOperation.completed += OnComplete;
    }

    private void OnComplete(AsyncOperation obj)
    {
        
    }

    private void Update()
    {
        //_asyncOperation.progress
    }
}