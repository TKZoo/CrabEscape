using System.Collections;
using UnityEngine;

public class PlatformPatrol : Patrol
{
    [SerializeField] public LayerCheck _aiDecisionPoint;
    
    private Character _character;
    private bool _isGrounded;
    private int _invertMoveDirection = 1;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    protected virtual void Update()
    {
        _isGrounded = _aiDecisionPoint.IsTouchingLayer;
    }
    public override IEnumerator DoPatrol()
    {
        while (enabled)
        {
            if (!_isGrounded)
            {
                _invertMoveDirection *= -1;
            }
            var direction = new Vector3(_invertMoveDirection, 1,1 );
            direction.y = 0;
            _character.SetDirection(direction.normalized);
            
            yield return null;
        }
    }

}
