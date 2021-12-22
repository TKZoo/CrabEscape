using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpImpulse;
    [SerializeField] private float _damageJumpImpulseY;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private Vector3 _groundCheckPosition;
    [SerializeField] private bool _debug = false;
    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private LayerMask _interactLayerCheck;
    [SerializeField] private LayerMask _groundLayerCheck;
    [SerializeField] private Collider2D[] _interactResult = new Collider2D[1];
    [SerializeField] private float _fallHeight;

    [SerializeField] private SpawnPrefabComponent _spawnPlayerParticles;

    private Vector2 _direction;    
    private Rigidbody2D _heroRb;
    private Animator _animator;   
    private bool _isGrounded;
    private bool _allowDoubleJump;
    private bool _isJumping;
    private bool _isFalling;

    [SerializeField] private GameObject _runParticles;
    [SerializeField] private GameObject _jumpParticles;
    [SerializeField] private GameObject _landingParticles;

    private static readonly int IsGroundKey = Animator.StringToHash("isGrounded");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical_velocity");
    private static readonly int Hit = Animator.StringToHash("isHit");

    private void Awake()
    {        
        _heroRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void Update()
    {        
        _isGrounded = IsGrounded();
        FallHeightCheck();
    }

    private void FixedUpdate()
    {
        var xVelocity = _direction.x * _speed;
        var yVelocity = CalculateYVelocity();

        _heroRb.velocity = new Vector2(xVelocity, yVelocity);
        
        _animator.SetBool(IsGroundKey, _isGrounded);
        _animator.SetBool(IsRunning, _direction.x != 0);
        _animator.SetFloat(VerticalVelocity, _heroRb.velocity.y);

        UpdateSpriteDirection();       
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);            
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private float CalculateYVelocity()
    {
        var yVelocity = _heroRb.velocity.y;
        var isJumpPressing = _direction.y > 0;

        if (_isGrounded)
        {
            if (_isFalling)
            {
                _spawnPlayerParticles.SpawnPrefab(_landingParticles);
                _isFalling = false;
            }
            _allowDoubleJump = true;
            _isJumping = true;
        }
        if (isJumpPressing)
        {
            _isJumping = true;
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_heroRb.velocity.y > 0 && _isJumping)
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
        else if (_allowDoubleJump && _isJumping)
        {
            _spawnPlayerParticles.SpawnPrefab(_jumpParticles);
            yVelocity = _jumpImpulse;
            _allowDoubleJump = false;
        }
        return yVelocity;
    }

    private bool IsGrounded()
    {
        return _groundCheck._isTouchingLayer;        
    }

    private void FallHeightCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10, _groundLayerCheck);
        if (hit.collider != null && !_isGrounded)
        {
            if (hit.distance > _fallHeight)
            {
                _isFalling = true;
            }
            //Debug.Log(hit.collider.name + " hited by ray");
            //Debug.Log(hit.distance);
        }
    } 
    public void TakeDamage()
    {
        _isJumping = false;
        _animator.SetTrigger(Hit);
        _heroRb.velocity = new Vector2(_heroRb.velocity.x, _damageJumpImpulseY);
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

    public void SpawnFootDust()
    {
        //Debug.Log($"name: {_runParticles.name}");
        _spawnPlayerParticles.SpawnPrefab(_runParticles);
    }
    
    public void SpawnJumpDust()
    {
        _spawnPlayerParticles.SpawnPrefab(_jumpParticles);
    }
}
