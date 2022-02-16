using System;
using System.Collections.Generic;
using UnityEngine;

public class LeacherEnemy : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private CheckCircleOverlapComponent _meleeAtack;
    [SerializeField] private LayerCheck _inAttackRange;
    [SerializeField] private Cooldown _meleeCooldown;
    [SerializeField] private LeacherTongue _tongue;
    [SerializeField] private float _tongueSpeedIn, _tongueSpeedOut;

    public bool isTraped;

    private void Awake()
    {
        isTraped = false;
    }

    public void IsTraped()
    {
        isTraped = true;
    }
    
    private void Update()
    {
        Debug.Log(isTraped);

        if (_inAttackRange.IsTouchingLayer && isTraped)
        {
            _tongueSpeedOut = 0;
            if (_meleeCooldown.IsReady)
            {
                MeleeAttack();
                _meleeCooldown.Reset();
                return;
            }
        }
        if (_vision.IsTouchingLayer && !isTraped)
        {
            _tongue.TongueMovement(_tongueSpeedOut);
        }

        if (!_vision.IsTouchingLayer || isTraped)
        {
            _tongue.TongueMovement(_tongueSpeedIn);
        }
    }

    private void MeleeAttack()
    {
        _meleeAtack.Check();
    }
}