using System.Collections;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private LayerCheck _inAttackRange;

    [SerializeField] private float _timeToReact = 1.0f;
    [SerializeField] private float _attackCoolDown = 1.0f;

    [SerializeField] private bool _scaleCollider;

    private Coroutine _current;
    private GameObject _target;

    private SpawnPrefabListComponent _particles;
    private Character _character;
    private Animator _animator;
    private bool _isDead;
    private Patrol _patrol;
    private BoxCollider2D _collider;

    private static readonly int DeathAnim = Animator.StringToHash("isDied");

    private void Awake()
    {
        _particles = GetComponent<SpawnPrefabListComponent>();
        _character = GetComponent<Character>();
        _animator = GetComponent<Animator>();
        _patrol = GetComponent<Patrol>();
        _collider = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        Debug.Log(_current);
    }
    private void Start()
    {
        StartState(_patrol.DoPatrol());
    }

    private void StartState(IEnumerator caroutine)
    {
        _character.SetDirection(Vector2.zero);

        if (_current != null)
        {
            StopCoroutine(_current);
        }
        _current = StartCoroutine(caroutine);
    }

    public void OnTargetInVision(GameObject go)
    {
        if (_isDead) return;

        _target = go;

        Debug.Log("Go Recieved");
        StartState(AgroOnTarget());
    }

    private IEnumerator AgroOnTarget()
    {
        _particles.Spawn("Exclamation");
        yield return new WaitForSeconds(_timeToReact);

        StartState(GoToTarget());
    }

    private IEnumerator GoToTarget()
    {
        while (_vision.IsTouchingLayer)
        {
            if (_inAttackRange.IsTouchingLayer)
            {
                StartState(Attack());
            }
            else
            {
                SetDirectionToTarget();
            }

            yield return null;
        }
    }
    private IEnumerator Attack()
    {
        while (_inAttackRange.IsTouchingLayer)
        {
            _character.Attack();
            yield return new WaitForSeconds(_attackCoolDown);
        }

        StartState(GoToTarget());
    }

    private void SetDirectionToTarget()
    {
        var direction = _target.transform.position - transform.position;
        direction.y = 0;

        _character.SetDirection(direction.normalized);
    }

    public void OnDie()
    {
        _isDead = true;
        _animator.SetBool(DeathAnim, true);
        _character.SetDirection(Vector2.zero);

        if (_scaleCollider)
        {
            _collider.size = new Vector2(_collider.size.x, _collider.size.y / 2);
        }

        if (_current != null)
            StopCoroutine(_current);
    }

}
