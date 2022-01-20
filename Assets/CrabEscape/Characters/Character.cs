using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Params")] 
    [SerializeField] private bool _invertSpriteScale;
    [SerializeField] private float _speed;
    [SerializeField] protected float _jumpImpulse;
    [SerializeField] private float _damageJumpImpulseY;

    [Header("Checkers")]
    [SerializeField] public LayerMask _groundLayerCheck;
    [SerializeField] public LayerCheck _groundCheck;

    [SerializeField] private CheckCircleOverlapComponent _attackRange;
    [SerializeField] protected SpawnPrefabListComponent _particles;
    [SerializeField] private Cooldown _throwCooldown;

    protected Rigidbody2D Rigidbody;
    protected Vector2 Direction;
    protected Animator Animator;
    protected bool IsGrounded;
    protected bool IsJumping;
    protected bool IsFalling;

    private static readonly int IsGroundKey = Animator.StringToHash("isGrounded");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical_velocity");
    private static readonly int Hit = Animator.StringToHash("isHit");
    private static readonly int AttackAnim = Animator.StringToHash("attack");
    private static readonly int ThrowAttackAnim = Animator.StringToHash("throw");

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction; 
    }

    protected virtual void Update()
    {
        IsGrounded = _groundCheck.IsTouchingLayer;
    }

    private void FixedUpdate()
    {
        var xVelocity = Direction.x * _speed;
        var yVelocity = CalculateYVelocity();

        Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

        Animator.SetBool(IsGroundKey, IsGrounded);
        Animator.SetBool(IsRunning, Direction.x != 0);
        Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);

        UpdateSpriteDirection(Direction);
    }

    public void UpdateSpriteDirection(Vector2 direction)
    {
        var scaleMultyplier = _invertSpriteScale ? -1 : 1;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(scaleMultyplier, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1 * scaleMultyplier, 1, 1);
        }
    }

    protected virtual float CalculateYVelocity()
    {
        var yVelocity = Rigidbody.velocity.y;
        var isJumpPressing = Direction.y > 0;

        if (IsGrounded)
        {
            IsJumping = true;
        }
        if (isJumpPressing)
        {
            IsJumping = true;

            var isFalling = Rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;
            yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
        }
        else if (Rigidbody.velocity.y > 0 && IsJumping)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    protected virtual float CalculateJumpVelocity(float yVelocity)
    {
        if (IsGrounded)
        {
            yVelocity += _jumpImpulse;
            _particles.Spawn("Jump");
        }
        return yVelocity;
    }

    public virtual void TakeDamage()
    {
        IsJumping = false;
        Animator.SetTrigger(Hit);
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpImpulseY);
    }

    public virtual void Attack()
    {
        Animator.SetTrigger(AttackAnim);
        _particles.Spawn("Attack");
    }

    public void OnAttack()
    {
        _attackRange.Check();
    }
    
    public void OnThrowAttack()
    {
        _particles.Spawn("ThrowAttack");
    }
    
    public virtual void ThrowAttack()
    {
        if (_throwCooldown.IsReady)
        {
            Animator.SetTrigger(ThrowAttackAnim);
            _throwCooldown.Reset();
        }
    }

    
}
