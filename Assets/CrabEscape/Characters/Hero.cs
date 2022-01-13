﻿using UnityEditor.Animations;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] private CheckCircleOverlapComponent _interactionCheck;
    [SerializeField] private float _fallHeight;

    [SerializeField] private AnimatorController _armed;
    [SerializeField] private AnimatorController _disarmed;

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
    }

    protected override void Update()
    {
        base.Update();
        FallHeightCheck();
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
}
