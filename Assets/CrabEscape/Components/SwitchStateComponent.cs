using UnityEngine;
using UnityEngine.Events;

public class SwitchStateComponent : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _state;
    [SerializeField] private UnityEvent _action;
    [SerializeField] private string _animationKey;

    public void SwitchState()
    {
        _state = !_state;
        _animator.SetBool(_animationKey, _state);
        _action?.Invoke();
    }

    [ContextMenu("Switch")] 
    public void SwitchIt()
    {
        SwitchState();
    }

}
