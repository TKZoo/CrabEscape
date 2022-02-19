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
    [SerializeField] protected Cooldown _throwCooldown;
    [SerializeField] protected PlaySoundComponent Sound;

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
    private static readonly int Died = Animator.StringToHash("isDied");

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Sound = GetComponent<PlaySoundComponent>();
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

    public void ChangeSpeedTo(float speed)
    {
        _speed = speed;
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
            DoJumpVfx();

        }
        return yVelocity;
    }

    protected virtual void FootstepVfx()
    {
       _particles.Spawn("Run");
       Sound.Play("footstep");
    }
    
    protected virtual void DoJumpVfx()
    {
        _particles.Spawn("Jump");
        Sound.Play("jump");
    }
    
    public virtual void TakeDamage()
    {
        IsJumping = false;
        Animator.SetTrigger(Hit);
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpImpulseY);
    }

    public virtual void Die()
    {
        Animator.SetTrigger(Died);
        gameObject.tag = "Untagged";
    }
    
    public virtual void Attack()
    {
        Animator.SetTrigger(AttackAnim);
        _particles.Spawn("Attack");
    }

    public void OnAttack()
    {
        _attackRange.Check();
        Sound.Play("mele");
    }
    
    public void OnThrowAttack()
    {
        _particles.Spawn("ThrowAttack");
        Sound.Play("range");
    }
    
    public virtual void ThrowAttack()
    {
        Animator.SetTrigger(ThrowAttackAnim);
    }

    
}
