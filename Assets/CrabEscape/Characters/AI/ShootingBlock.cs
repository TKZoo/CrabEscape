using UnityEngine;

public class ShootingBlock : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;

    [SerializeField] private Cooldown _cooldown;
    [SerializeField] private SpawnPrefabComponent _rangeAttack;
    
    private Animator _animator;

    private static readonly int Ranged = Animator.StringToHash("IsRangedAttack");

    private void Awake()
    {
        _animator = GetComponent <Animator>();
    }

    public void RangeAttack()
    {
        _animator.SetTrigger(Ranged);
    }
    
    public void OnRangedAttack()
    {
        _rangeAttack.SpawnPrefab(_projectile);
    }
}
