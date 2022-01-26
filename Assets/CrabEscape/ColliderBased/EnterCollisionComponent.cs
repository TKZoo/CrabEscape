using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private string[] _tag;
    [SerializeField] private EnterEvent _action;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < _tag.Length; i++)
        {
            if (collision.gameObject.CompareTag(_tag[i]))
            {
                _action?.Invoke(collision.gameObject);
            }
        }        
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {

    }
}
