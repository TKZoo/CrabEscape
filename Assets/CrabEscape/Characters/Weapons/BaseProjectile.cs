using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] private bool _invertXDir;

    protected Rigidbody2D Rigidbody;
    protected int Direction;

    protected virtual void Start()
    {
        var mod = _invertXDir ? -1 : 1; 
        Direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
        Rigidbody = GetComponent<Rigidbody2D>();
    }
}
