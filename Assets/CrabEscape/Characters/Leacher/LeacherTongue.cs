using System.Collections.Generic;
using UnityEngine;

public class LeacherTongue : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _tongueRoot;
    [SerializeField] private GameObject _tongueSegmentPf;
    [SerializeField] private int _tongueLenght;
    [SerializeField] private GameObject _tongueLowerSegment;
    [SerializeField] private LeacherEnemy _leacherEnemy;
    
    private List<GameObject> _tongueSegments = new List<GameObject>();
    private GameObject[] tongueGo;
    private float _sriteSizeY = 0.22f;
    private int _maxcount;
    private float _speed;

    private void Awake()
    {
        _leacherEnemy = GetComponent<LeacherEnemy>();
    }

    void Start()
    {
        _leacherEnemy = GetComponent<LeacherEnemy>();
        GenerateRope();
        //_sriteSizeY = _tongueSegmentPf.GetComponent<SpriteRenderer>().bounds.size.y;
        //Debug.Log(_sriteSizeY);
    }

    void FixedUpdate()
    {
        TongueMovement(_speed);
    }

    private void GenerateRope()
    {
        GameObject firstSegment = Instantiate(_tongueSegmentPf);
        firstSegment.transform.parent = transform.root;
        firstSegment.transform.position = transform.position;
        firstSegment.GetComponent<HingeJoint2D>().connectedBody = _tongueRoot;
        firstSegment.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        _tongueLowerSegment = firstSegment;
        _tongueSegments.Add(firstSegment);
    }

    private void AddTongueSegment()
    {
        GameObject tongueSegment = Instantiate(_tongueSegmentPf);
        tongueSegment.transform.parent = transform.root;
        tongueSegment.transform.position = transform.position;
        tongueSegment.GetComponent<HingeJoint2D>().connectedBody = _tongueRoot;
        tongueSegment.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        _tongueLowerSegment.GetComponent<HingeJoint2D>().connectedBody = tongueSegment.GetComponent<Rigidbody2D>();
        _tongueLowerSegment = tongueSegment;
        _tongueSegments.Add(tongueSegment);
    }

    private void RemoveTongueSegment()
    {
        GameObject secondTongueSegment = _tongueSegments[_maxcount - 1];
        secondTongueSegment.GetComponent<HingeJoint2D>().connectedBody = _tongueRoot;
        secondTongueSegment.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, 0);
        GameObject toDestroy = _tongueLowerSegment;
        _tongueLowerSegment = secondTongueSegment;
        _tongueSegments.Remove(toDestroy);
        Destroy(toDestroy);
    }

    public void TongueMovement(float _speed)
    {
        if (_speed < 0 && _maxcount != _tongueLenght)
        {
            _tongueLowerSegment.GetComponent<HingeJoint2D>().connectedAnchor += new Vector2(0, _speed * Time.deltaTime);
            if (_tongueLowerSegment.GetComponent<HingeJoint2D>().connectedAnchor.y * -1 >= _sriteSizeY &&
                _maxcount < _tongueLenght)
            {
                AddTongueSegment();
                _maxcount++;
            }
        }

        if (_speed > 0 && _maxcount != 0)
        {
            _tongueLowerSegment.GetComponent<HingeJoint2D>().connectedAnchor += new Vector2(0, _speed * Time.deltaTime);
            if (_tongueLowerSegment.GetComponent<HingeJoint2D>().connectedAnchor.y >= _sriteSizeY && _maxcount > 0)
            {
                RemoveTongueSegment();
                _maxcount--;
            }
        }
    }

}