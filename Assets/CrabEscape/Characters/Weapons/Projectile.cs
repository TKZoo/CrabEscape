using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _ySpeedImpulse;
    [SerializeField] private bool _isDynamic;

    private Rigidbody2D _rigidbody;
    private int _direction;

    private void Start()
    {
        _direction = transform.lossyScale.x > 0 ? 1 : -1;
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_isDynamic)
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            var force = new Vector2(_direction * _speed, _ySpeedImpulse);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _speed /= 50;
        }
    }

    private void FixedUpdate()
    {
        if (!_isDynamic)
        {
            var position = _rigidbody.position;
            position.x += _direction * _speed;
            _rigidbody.MovePosition(position);
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
