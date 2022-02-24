using System.Collections.Generic;
using UnityEngine;

public class LeacherTongue : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _tongueRoot;
    [SerializeField] private LeacherTongueSegment _tongueSegmentPf;
    [SerializeField] private int _tongueLenght;
    [SerializeField] private LeacherTongueSegment _tongueLowerSegment;
    [SerializeField] private GameObject _leacherGo;
    private LeacherEnemy _leacherEnemy;
    private HingeJoint2D firstSegmentHj;

    private List<LeacherTongueSegment> _tongueSegments = new List<LeacherTongueSegment>();

    [SerializeField] private GameObject _target;
    private float _sriteSizeY = 0.2f;
    private int _maxcount;
    private float _speed;
    private HealthComponent health;

    private void Awake()
    {
        _leacherEnemy = _leacherGo.GetComponent<LeacherEnemy>();
    }

    void Start()
    {
        GenerateTongue();
        //_sriteSizeY = _tongueSegmentPf.GetComponent<SpriteRenderer>().bounds.size.y;
        //Debug.Log(_sriteSizeY);
    }

    void Update()
    {
        TongueMovement(_speed);
    }

    public void GenerateTongue()
    {
        var firstSegment = Instantiate(_tongueSegmentPf);
        firstSegment.transform.parent = gameObject.transform;
        firstSegment.transform.position = gameObject.transform.position;
        firstSegment.segmentHj.connectedBody = _tongueRoot;
        firstSegment.segmentHj.connectedAnchor = new Vector2(0, 0);
        _tongueLowerSegment = firstSegment;
        _tongueSegments.Add(firstSegment);
    }

    private void AddTongueSegment()
    {
        var tongueSegment = Instantiate(_tongueSegmentPf);
        tongueSegment.transform.parent = gameObject.transform;
        tongueSegment.transform.position = gameObject.transform.position;
        tongueSegment.segmentHj.connectedBody = _tongueRoot;
        tongueSegment.segmentHj.connectedAnchor = new Vector2(0, 0);
        _tongueLowerSegment.segmentHj.connectedBody = tongueSegment.segmentRb;
        _tongueLowerSegment = tongueSegment;
        _tongueSegments.Add(tongueSegment);
    }

    private void RemoveTongueSegment()
    {
        var secondTongueSegment = _tongueSegments[_maxcount - 1];
        secondTongueSegment.segmentHj.connectedBody = _tongueRoot;
        secondTongueSegment.segmentHj.connectedAnchor = new Vector2(0, 0);
        var toDestroy = _tongueLowerSegment;
        _tongueLowerSegment = secondTongueSegment;
        _tongueSegments.Remove(toDestroy);
        Destroy(toDestroy.gameObject);
    }

    public void TongueMovement(float _speed)
    {
        if (_speed < 0 && _maxcount != _tongueLenght)
        {
            _tongueLowerSegment.segmentHj.connectedAnchor += new Vector2(0, _speed * Time.deltaTime);
            if (_tongueLowerSegment.segmentHj.connectedAnchor.y * -1 >= _sriteSizeY &&
                _maxcount < _tongueLenght)
            {
                AddTongueSegment();
                _maxcount++;
            }
        }

        if (_speed > 0 && _maxcount != 0)
        {
            _tongueLowerSegment.segmentHj.connectedAnchor += new Vector2(0, _speed * Time.deltaTime);
            if (_tongueLowerSegment.segmentHj.connectedAnchor.y >= _sriteSizeY && _maxcount > 0)
            {
                RemoveTongueSegment();
                _maxcount--;
            }
        }
    }

    public void SetParent(GameObject target, GameObject parent)
    {
        if (target != _leacherGo && _leacherEnemy.isTraped != true)
        {
            _target = target;
            if (!target.GetComponent<HealthComponent>())
            {
                _leacherEnemy.isIndestructible = true;
            }

            _leacherEnemy.isTraped = true;
            if (!target.TryGetComponent(out HingeJoint2D targetHj)) ;
            {
                targetHj = target.AddComponent<HingeJoint2D>();
            }

            targetHj.connectedBody = parent.GetComponent<Rigidbody2D>();
            target.GetComponent<Collider2D>().isTrigger = true;
            target.transform.position = parent.transform.position;
            health = target.GetComponent<HealthComponent>();
            if (health)
            {
                health._onDie?.AddListener(delegate { OnVictimDie(targetHj); });
            }
        }
    }

    void OnVictimDie(HingeJoint2D target)
    {
        _leacherEnemy.isTraped = false;
        _leacherEnemy.ResetSpeed();
        if (target)
        {
            target.GetComponent<Collider2D>().isTrigger = false;
            target.GetComponent<Rigidbody2D>().sharedMaterial = null;
            Destroy(target);
        }
    }

    public void onDie()
    {
        onDieTarget(_target);
    }
    
    private void onDieTarget(GameObject target)
    {
        _leacherEnemy.isTraped = false;
        _leacherEnemy.PlaySound("kill");
        foreach (var segment in _tongueSegments)
        {
            segment.gameObject.layer = 10;
            Destroy(segment.segmentColC);
            _tongueRoot.gameObject.transform.DetachChildren();
            Destroy(_tongueRoot.gameObject);
        }

        if (target && target.GetComponent<HingeJoint2D>())
        {
            target.GetComponent<HingeJoint2D>().enabled = false;
            target.GetComponent<Collider2D>().isTrigger = false;
            if (!target.GetComponent<HealthComponent>())
            {
                _leacherEnemy.isIndestructible = false;
            }
        }
    }
}