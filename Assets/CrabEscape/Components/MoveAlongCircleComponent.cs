using UnityEditor;
using UnityEngine;


public class MoveAlongCircleComponent : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;
    private Rigidbody2D[] _objects;
    private Vector2[] _objPositions;
    private float _time;

    private void Awake()
    {
        UpdateContent();
    }

    private void Update()
    {
        //transform.Rotate(new Vector3(0,0,1) , _speed);
        CalculatePositions();
        var noObjects = true;
        for (var i = 0; i < _objects.Length; i++)
        {
            if (_objects[i])
            {
                _objects[i].MovePosition(_objPositions[i]);
                noObjects = false;
            }
        }

        if (noObjects)
        {
            enabled = false;
            Destroy(gameObject, 1f);
        }
        _time += Time.deltaTime;
    }

    private void CalculatePositions()
    {
        var step = 2 * Mathf.PI / _objects.Length;
        Vector2 containerPosition = transform.position;

        for (var i = 0; i < _objects.Length; i++)
        {
            var angle = step * i;
            var pos = new Vector2(
                Mathf.Cos(angle + _time * _speed) * _radius,
                Mathf.Sin(angle + _time * _speed) * _radius);
            _objPositions[i] = containerPosition + pos;
        }
    }

    private void UpdateContent()
    {
        _objects = GetComponentsInChildren<Rigidbody2D>();
        _objPositions = new Vector2[_objects.Length];
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position , transform.forward, _radius); 
    }

    private void OnValidate()
    {
        UpdateContent();
        CalculatePositions();
        for (var i = 0; i < _objects.Length; i++)
        {
            _objects[i].transform.position = _objPositions[i];
        }
    }
#endif
}
