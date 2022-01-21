using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] private CheckCircleOverlapComponent _interactionCheck;
    [SerializeField] private float _fallHeight;

    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _disarmed;
    [SerializeField] private int _swordCount;
    [SerializeField] private int _throwComboSwordsCount;
    [SerializeField] private float _throwComboCooldown;

    [SerializeField] private Projectile _projectile;

    private static readonly int ThrowAttackAnim = Animator.StringToHash("throw");
    
    private GameSession _session;

    private bool _allowDoubleJump;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
        var health = GetComponent<HealthComponent>();
        health.SetHealth(_session.PlayerData.Hp);
        var score = GetComponent<ScoreCounterComponent>();
        score.SetScore(_session.PlayerData.Coins);
        UpdateHeroWeaponStatus();
        if (_session.PlayerData.IsArmed)
        {
            if (_swordCount <= 0)
            {
                _swordCount = 1;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        FallHeightCheck();
    }

    public void SwordCounter(int sword)
    {
        _swordCount += sword;
        Debug.Log("player have: " + _swordCount + " sword");
    }
    
    protected override float CalculateYVelocity()
    {
        if (IsGrounded)
        {
            if (IsFalling)
            {
                _particles.Spawn("Landing");
                IsFalling = false;
            }
            _allowDoubleJump = true;
        }

        return base.CalculateYVelocity();
    }

    protected override float CalculateJumpVelocity(float yVelocity)
    {
        if (!IsGrounded && _allowDoubleJump && IsJumping)
        {
            _particles.Spawn("Jump");
            _allowDoubleJump = false;
            return _jumpImpulse;
        }
        return base.CalculateJumpVelocity(yVelocity);
    }

    private void FallHeightCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            Vector2.down, 10, _groundLayerCheck);
        if (hit.collider != null && !IsGrounded)
        {
            if (hit.distance > _fallHeight)
            {
                IsFalling = true;
            }
        }
    }

    public void OnHealthChanges(int currentHealth)
    {
        _session.PlayerData.Hp = currentHealth;
    }

    public void Interact()
    {
        _interactionCheck.Check();
    }

    public void ArmHero()
    {
        _session.PlayerData.IsArmed = true;
        UpdateHeroWeaponStatus();
    }

    private void UpdateHeroWeaponStatus()
    {
        Animator.runtimeAnimatorController = _session.PlayerData.IsArmed ? _armed : _disarmed;
    }

    public override void Attack()
    {
        if (!_session.PlayerData.IsArmed) return;
        base.Attack();
    }

    public override void ThrowAttack()
    {
        if (_swordCount > 1 && _throwCooldown.IsReady)
        {
            _projectile.SetRigidBodyToDynamic();
            base.ThrowAttack();
            SwordCounter(-1);
            _throwCooldown.Reset();
        }
    }

    private IEnumerator DoThrowComboAttack()
    {
        var throwswordcount = _throwComboSwordsCount;
        while (throwswordcount > 0)
        {
            if (_swordCount > 1)
            {
                Animator.SetTrigger(ThrowAttackAnim);
                SwordCounter(-1);
            }
            yield return new WaitForSeconds(_throwComboCooldown) ;
            throwswordcount--;
        }
    }
    
    public void ThrowComboAttack()
    {
        if (_swordCount > 1)
        {
            _projectile.SetRigidBodyToKinematic();
            StartCoroutine(DoThrowComboAttack());
        }
    }
}
