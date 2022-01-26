using UnityEngine;

public class SinusoidalProjectiles : BaseProjectile
{
    [SerializeField] private float _sinFrequency;
    [SerializeField] private float _sinAmplitude;
    private float _originalY;
    private float _time;
    
    protected override void Start()
    {
        base.Start();
        _originalY = Rigidbody.position.y;
    }

    private void FixedUpdate()
    {
        var position = Rigidbody.position;
        position.x += Direction * _speed;
        position.y = _originalY + Mathf.Sin(_time * _sinFrequency) * _sinAmplitude;
        Rigidbody.MovePosition(position);
        _time += Time.fixedDeltaTime;
    }
}
