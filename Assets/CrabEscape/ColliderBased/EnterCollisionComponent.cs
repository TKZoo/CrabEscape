using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private string[] _tag;
    [SerializeField] public EnterEvent _action;

    private bool isActive = true;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive)
        {
            for(int i = 0; i < _tag.Length; i++)
            {
                if (collision.gameObject.CompareTag(_tag[i]))
                {
                    _action?.Invoke(collision.gameObject);
                }
            }  
        }
    }

    public void SetActiveFalse()
    {
        isActive = false;
    }
    
    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {

    }
}
