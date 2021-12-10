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
    private Animator _animator;
    private SpriteRenderer _sprite;

    private static readonly int IsGroundKey = Animator.StringToHash("isGrounded");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int verticalVelocity = Animator.StringToHash("vertical_velocity");

    private void Awake()
    {
        _heroRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _heroRb.velocity = new Vector2(_direction.x * _speed, _heroRb.velocity.y);

        var isGrounded = IsGrounded();
        var isJumping = _direction.y > 0;
        if (isJumping)
        {
            if (IsGrounded())
            {
                _heroRb.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
            }            
        }
        else if (_heroRb.velocity.y > 0)
        {
            _heroRb.velocity = new Vector2(_heroRb.velocity.x, _heroRb.velocity.y * 0.5f);
        }

        _animator.SetBool(IsGroundKey, isGrounded);
        _animator.SetBool(IsRunning, _direction.x != 0);
        _animator.SetFloat(verticalVelocity, _heroRb.velocity.y);

        UpdateSpriteDirection();       
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            _sprite.flipX = false;
        }
        else if (_direction.x < 0)
        {
            _sprite.flipX = true;
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
