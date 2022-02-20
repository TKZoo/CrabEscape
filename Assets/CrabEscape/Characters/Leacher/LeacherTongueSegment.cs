using System;
using UnityEngine;
using UnityEngine.Events;

public class LeacherTongueSegment : MonoBehaviour
{
    [SerializeField] private LayerCheck _victims;
    [SerializeField] public UnityEvent _onColision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_victims.IsTouchingLayer)
        {
            _onColision?.Invoke(collision.gameObject);
        }
    }

    [Serializable]
    public class UnityEvent : UnityEvent<GameObject>
    {
    }
}