using System.Collections;
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
    [SerializeField] private Material _hitBlinkMat;
    [SerializeField] private Color _hitBlinkColor;
    [SerializeField] private float _hitBlinkDuration;
    private SpriteRenderer _spriteRenderer;

    public bool isTraped;
    public bool isIndestructible;

    private void Awake()
    {
        isTraped = false;
        _tongueSpeedReset = _tongueSpeedIn;
        Sound = GetComponent<PlaySoundComponent>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
            _tongueSpeedIn = 0f;
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
        if (!isIndestructible)
        {
            Sound.Play("kill");
        }
    }

    public void GetHit()
    {
        Sound.Play("hurt");
        StartCoroutine(GetHitBlink());
    }

    private IEnumerator GetHitBlink()
    {
        Material defaultMaterial = _spriteRenderer.material;
        _hitBlinkMat.color = _hitBlinkColor;
        _spriteRenderer.material = _hitBlinkMat;

        yield return new WaitForSeconds(_hitBlinkDuration);

        _spriteRenderer.material = defaultMaterial;
        
        StopCoroutine(GetHitBlink());
    }
}