using UnityEngine;
using Random = UnityEngine.Random;

public class DebriesRbComponent : MonoBehaviour
{
    [SerializeField] private Vector2 _forceDirection;
    [SerializeField] private float _torqueRand;
    [SerializeField] private float _forceXRandMin;
    [SerializeField] private float _forceXRandMax;
    [SerializeField] private float _forceYRandMin;
    [SerializeField] private float _forceYRandMax;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        float randomTorque = Random.Range(_torqueRand * -1, _torqueRand);

        _forceDirection.x = Random.Range(_forceXRandMin , _forceXRandMax);
        _forceDirection.y = Random.Range(_forceYRandMin , _forceYRandMax);
        
        
        _rb.AddForce(_forceDirection);
        _rb.AddTorque(randomTorque);
    }
}
