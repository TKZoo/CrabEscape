using UnityEngine;

public class ShootingTrapAI : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private LayerCheck _vision;
    
    [Header("Ranged")]
    [SerializeField] private Cooldown _rangeCooldown;
    [SerializeField] private SpawnPrefabComponent _rangeAttack;

    [Header("Melee")]
    [SerializeField] private Cooldown _meleeCooldown;
    [SerializeField] private LayerCheck _inAttackRange;
    [SerializeField] private CheckCircleOverlapComponent _meleeAtack;

    private Animator _animator;
    
    private static readonly int Melee = Animator.StringToHash("IsMeleeAttack");
    private static readonly int Ranged = Animator.StringToHash("IsRangedAttack");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_vision.IsTouchingLayer)
        {
            if (_inAttackRange.IsTouchingLayer)
            {
                if (_meleeCooldown.IsReady)
                {
                    MeleeAttack();
                    _meleeCooldown.Reset();
                    return;
                }
            }
            if(_rangeCooldown.IsReady)
            {
                RangeAttack();
                _rangeCooldown.Reset();
            }
        }
    }

    private void RangeAttack()
    {
        _animator.SetTrigger(Ranged);
    }

    private void MeleeAttack()
    {
        _animator.SetTrigger(Melee);
    }

    public void OnMeleeAttack()
    {
        _meleeAtack.Check();
    }

    public void OnRangedAttack()
    {
        _rangeAttack.SpawnPrefab(_projectile);
    }
}
