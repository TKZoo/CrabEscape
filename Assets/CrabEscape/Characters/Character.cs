using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Params")] 
    [SerializeField] private bool _invertSpriteScale;
    [SerializeField] private float _speed;
    [SerializeField] protected float _jumpImpulse;
    [SerializeField] private float _damageJumpImpulseY;
    public float additionalSpeedValue = 0;

    [Header("Checkers")] 
    public LayerMask _groundLayerCheck;
    public LayerCheck _groundCheck;

    [SerializeField] private CheckCircleOverlapComponent _attackRange;
    [SerializeField] protected SpawnPrefabListComponent _spawnPf;
    [SerializeField] protected Cooldown _throwCooldown;
    [SerializeField] protected PlaySoundComponent Sound;
    [SerializeField] protected SpawnPrefabComponent _throwSpawner;

    private protected GameSession _session;
    protected Rigidbody2D Rigidbody;
    protected Vector2 Direction;
    protected Animator Animator;
    protected HealthComponent Health;
    protected bool IsGrounded;
    protected bool IsJumping;
    protected bool IsFalling;
    protected float _aditionalsSpeedValue;


    private static readonly int IsGroundKey = Animator.StringToHash("isGrounded");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int VerticalVelocity = Animator.StringToHash("vertical_velocity");
    private static readonly int Hit = Animator.StringToHash("isHit");
    private static readonly int AttackAnim = Animator.StringToHash("attack");
    private static readonly int ThrowAttackAnim = Animator.StringToHash("throw");
    private static readonly int Died = Animator.StringToHash("isDied");

    protected virtual void Awake()
    {
        _session = FindObjectOfType<GameSession>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Health = GetComponent<HealthComponent>();
        Sound = GetComponent<PlaySoundComponent>();
    }

    private void Start()
    {
        Health.SetHealth(Health.Hp.Value);
    }

    public virtual void OnHealthChanges(int currentHealth)
    {
        Health.Hp.Value = currentHealth;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    public Vector2 GetDirection()
    {
        return Direction;
    }

    protected virtual void Update()
    {
        IsGrounded = _groundCheck.IsTouchingLayer;
    }

    private void FixedUpdate()
    {
        var xVelocity = CalculateXVelocity();
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

    protected virtual float CalculateXVelocity()
    {
        return Direction.x * CalculateSpeed();
    }
    
    protected virtual float CalculateSpeed()
    {
        return _speed + additionalSpeedValue;
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
        _spawnPf.Spawn("Run");
        Sound.Play("footstep");
    }

    protected virtual void DoJumpVfx()
    {
        _spawnPf.Spawn("Jump");
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
        _spawnPf.Spawn("Attack");
    }

    public void OnAttack()
    {
        _attackRange.Check();
        Sound.Play("mele");
    }

    public void OnThrowAttack()
    {
        _throwSpawner.SpawnPrefab();
        //_spawnPf.Spawn("ThrowAttack");
        Sound.Play("range");
    }

    public virtual void ThrowAttack(GameObject pf)
    {
        _throwSpawner.SetPrefab(pf);
        Animator.SetTrigger(ThrowAttackAnim);
    }
}