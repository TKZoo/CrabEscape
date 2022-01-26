using UnityEngine;

public class Projectile : BaseProjectile
{
    [SerializeField] private float _ySpeedImpulse;
    [SerializeField] private bool _isDynamic;
    [SerializeField] private float _timeToDestroy;

    protected override void Start()
    {
        base.Start();
        if (_isDynamic)
        {
            Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            var force = new Vector2(Direction * _speed, _ySpeedImpulse);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _speed /= 50;
        }
    }

    private void Update()
    {
        if (_timeToDestroy > 0)
        {
            Destroy(gameObject, _timeToDestroy);
        }
    }

    private void FixedUpdate()
    {
        if (!_isDynamic)
        {
            var position = Rigidbody.position;
            position.x += Direction * _speed;
            Rigidbody.MovePosition(position);
        }
    }

    public void SetRigidBodyToKinematic()
    {
        _isDynamic = false;
    }
    
    public void SetRigidBodyToDynamic()
    {
        _isDynamic = true;
    }
}
