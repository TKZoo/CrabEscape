using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] private string[] _tag;
    [SerializeField] private LayerMask _layer = ~0;
    [SerializeField] private UnityEvent _action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 0; i < _tag.Length; i++)
        {
            if (collision.gameObject.CompareTag(_tag[i]))
            {
                _action?.Invoke(collision.gameObject);
            }
        }
        if(!collision.gameObject.IsInLayer(_layer)) return;
                
        _action?.Invoke(collision.gameObject);
    }
    
    [Serializable]
    public class UnityEvent : UnityEvent<GameObject>
    {

    }
}
