using UnityEngine.Events;
using UnityEngine;

public class InteractibleComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent _action;

    public void Interact()
    {
        _action?.Invoke();
    }
}
