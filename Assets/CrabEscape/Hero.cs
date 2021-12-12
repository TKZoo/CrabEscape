using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpImpulse;
    [SerializeField] private float _damageJumpImpulse;
    [SerializeField] private float _groundCheckRadius;      
    [SerializeField] private Vector3 _groundCheckPosition;
    [SerializeField] private bool _debug = false;
    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private LayerMask _interactLayerCheck;
    [SerializeField] private Collider2D[] _interactResult = new Collider2D[1]; 

    private Vector2 _direction;    
    private Rigidbody2D _heroRb;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private bool _isGrounded;
    private bool _allowDoubleJump;

    private static readonly int IsGroundKey = Animator.StringToHash("isGrounded");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int verticalVelocity = Animator.StringToHash("vertical_velocity");
    private static readonly int Hit = Animator.StringToHash("isHit");

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

    private void Update()
    {
        _isGrounded = IsGrounded();       
    }

    private void FixedUpdate()
    {
        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateYVelocity();

        _heroRb.velocity = new Vector2(xVelocity, yVelocity);
        
        _animator.SetBool(IsGroundKey, _isGrounded);
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

    private float CalculateYVelocity()
    {
        var yVelocity = _heroRb.velocity.y;
        var isJumpPressing = _direction.y > 0;

        if (_isGrounded)
        {
            _allowDoubleJump = true;
        }
        if (isJumpPressing)
        {
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_heroRb.velocity.y > 0)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        var isFalling = _heroRb.velocity.y <= 0.001f;
        if (!isFalling)
        {
            return yVelocity;
        }
        if (_isGrounded)
        {
            yVelocity += _jumpImpulse;
        }
        else if (_allowDoubleJump)
        {
            yVelocity = _jumpImpulse;
            _allowDoubleJump = false;
        }
        return yVelocity;
    }

    private bool IsGrounded()
    {
        return _groundCheck.isTouchingLayer;        
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(Hit);
        _heroRb.velocity = new Vector2(_heroRb.velocity.x, _damageJumpImpulse);
    }

    private void OnDrawGizmos()
    {
        if (_debug == true)
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPosition, _groundCheckRadius);
        }
    }

    public void Interact()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, 0.3f, _interactResult, _interactLayerCheck);

        for(int i = 0; i < size; i++)
        {
            var interactible = _interactResult[i].GetComponent<InteractibleComponent>();
            if(interactible != null)
            {
                interactible.Interact();
            }
        }
    }
}
