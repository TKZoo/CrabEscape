using UnityEngine;
using UnityEngine.Events;

public class StayInTrigger : MonoBehaviour
{
    [SerializeField] private string[] _tag;
    [SerializeField] private UnityEvent _action;
    [SerializeField] private UnityEvent _actionOnExit;
    private bool _isTriggered = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isTriggered)
        {
            for(int i = 0; i < _tag.Length; i++)
            {
                if (other.gameObject.CompareTag(_tag[i]))
                {
                    _action?.Invoke();
                    _isTriggered = false;
                } 
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_isTriggered != true)
        {
            _actionOnExit?.Invoke();
            _isTriggered = true;
        }
    }
}
