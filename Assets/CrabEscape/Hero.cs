using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpImpulse;
    [SerializeField] private float _groundCheckRadius;      
    [SerializeField] private Vector3 _groundCheckPosition;
    [SerializeField] private bool _debug = false;
    [SerializeField] private LayerCheck _groundCheck;

    private Vector2 _direction;    
    private Rigidbody2D _heroRb;

    private void Awake()
    {
        _heroRb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _heroRb.velocity = new Vector2(_direction.x * _speed, _heroRb.velocity.y);

        var isJumping = _direction.y > 0;
        if (isJumping && IsGrounded())
        {
            _heroRb.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
        }
    }    

    private bool IsGrounded()
    {
        return _groundCheck.isTouchingLayer;        
    }

    private void OnDrawGizmos()
    {
        if (_debug == true)
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPosition, _groundCheckRadius);
        }
    }
}
