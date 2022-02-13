using UnityEngine;

public class LeacherEnemy : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private CheckCircleOverlapComponent _meleeAtack;
    [SerializeField] private LayerCheck _inAttackRange;
    [SerializeField] private Cooldown _meleeCooldown;
    [SerializeField] private LeacherTongue _tongue;
    [SerializeField] private float _tongueSpeedIn, _tongueSpeedOut;
    
    public bool _isTraped = false;

    private void Awake()
    {
        //_isTraped = false;
        _tongue = transform.GetComponent<LeacherTongue>();
    }

    public void SetTraped()
    {
        _isTraped = true;
    }
    private void Update()
    {
        Debug.Log("_isTraped " + _isTraped);
        /*if (_inAttackRange.IsTouchingLayer && _isTraped)
        {
            _tongueSpeedOut = 0;
            if (_meleeCooldown.IsReady)
            {
                MeleeAttack();
                _meleeCooldown.Reset();
                return;
            }
        }*/
        if (_vision.IsTouchingLayer && !_isTraped)
        {
            _tongue.TongueMovement(_tongueSpeedOut);
        }
        if(!_vision.IsTouchingLayer || _isTraped)
        {
            _tongue.TongueMovement(_tongueSpeedIn);
        }
    }

    private void MeleeAttack()
    {
        _meleeAtack.Check();
    }
}