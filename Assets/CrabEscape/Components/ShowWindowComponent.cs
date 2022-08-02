using UnityEngine;

public class ShowWindowComponent : MonoBehaviour
{
    [SerializeField] private string _patch;

    public void Show()
    {
        WindowUtils.CreateWindow(_patch);
    }
}