using UnityEngine;

public class LeacherEnemy : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private CheckCircleOverlapComponent _meleeAtack;
    [SerializeField] private LayerCheck _inAttackRange;
    [SerializeField] private Cooldown _meleeCooldown;
    [SerializeField] private LeacherTongue _tongue;
    [SerializeField] private float _tongueSpeedIn, _tongueSpeedOut, _tongueSpeedReset;
    [SerializeField] protected PlaySoundComponent Sound;

    public bool isTraped;

    private void Awake()
    {
        isTraped = false;
        _tongueSpeedReset = _tongueSpeedIn;
        Sound = GetComponent<PlaySoundComponent>();
    }

    public void PlaySound(string sound)
    {
        Sound.Play(sound);
    }

    public void ResetSpeed()
    {
        _tongueSpeedIn = _tongueSpeedReset;
    }

    private void Update()
    {
        Debug.Log(isTraped);

        if (_inAttackRange.IsTouchingLayer && isTraped)
        {
            _tongueSpeedIn = 0.1f;
            if (_meleeCooldown.IsReady)
            {
                MeleeAttack();
                _meleeCooldown.Reset();
                //return;
            }
        }
        if (!isTraped)
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