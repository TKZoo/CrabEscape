using System.Collections;
using UnityEngine;

public class PointPatrol : Patrol
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _treshold = 1f;

    private Character _character;
    private int _destinationPointIndex;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public override IEnumerator DoPatrol()
    {
        while (enabled)
        {
            if (IsOnPoint())
            {
                _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
            }

            var direction = _points[_destinationPointIndex].position - transform.position;
            direction.y = 0;
            _character.SetDirection(direction.normalized);

            yield return null;
        }
    }

    private bool IsOnPoint()
    {
        return (_points[_destinationPointIndex].position - transform.position).magnitude < _treshold;
    }
}
