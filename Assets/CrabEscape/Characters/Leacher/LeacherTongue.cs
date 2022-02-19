using System.Collections.Generic;
using UnityEngine;

public class LeacherTongue : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _tongueRoot;
    [SerializeField] private GameObject _tongueSegmentPf;
    [SerializeField] private int _tongueLenght;
    [SerializeField] private GameObject _tongueLowerSegment;
    private LeacherEnemy _leacherEnemy;
    private GameObject _leacherGo;
    
    public List<GameObject> _tongueSegments = new List<GameObject>();
    private LeacherTongue[] tongueGo;
    private GameObject _victim;
    private float _sriteSizeY = 0.2f;
    private int _maxcount;
    private float _speed;
    private HealthComponent health;
    private Rigidbody2D _victimRb;

    private void Awake()
    {
        _leacherGo = GameObject.Find("Leacher");
        _leacherEnemy = FindObjectOfType<LeacherEnemy>().GetComponent<LeacherEnemy>();
        _victimRb = gameObject.GetComponent<Rigidbody2D>();
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
    
    public void SetParent(GameObject parent)
    {
        if (parent != _leacherGo)
        {
            tongueGo = FindObjectsOfType<LeacherTongue>();
            {
                for (int i = 0; i < tongueGo.Length; i++)
                {
                    if (tongueGo[i].GetComponent<Collider2D>().IsTouching(parent.GetComponent<Collider2D>()))
                    {
                        _victim = parent;
                        health = _victim.GetComponent<HealthComponent>();
                        if (health)
                        {
                            health._onDie?.AddListener(OnVictimDie);
                        }
                        _victim.transform.SetParent(tongueGo[i].transform);
                        _leacherEnemy.isTraped = true;
                    }
                    parent.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }
        
        
    }

    void OnVictimDie()
    {
        _leacherEnemy.isTraped = false;
        _victim.gameObject.transform.parent = null;
        _victim.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        _victim.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _victim.gameObject.GetComponent<Rigidbody2D>().sharedMaterial = null;
        _leacherEnemy.PlaySound("kill");
        _leacherEnemy.ResetSpeed();
    }
    
    public void onDie()
    {
        _leacherEnemy.isTraped = false;
        _leacherEnemy.PlaySound("kill");

        
        
        _tongueRoot.GetComponent<LeacherTongue>().enabled = false;
        tongueGo = FindObjectsOfType<LeacherTongue>();
         var victimsRb = (int)Mathf.Repeat(0, tongueGo.Length);
        for (int i = 0; i < tongueGo.Length; i++)
        {
            
            if (tongueGo[i].GetComponent<HingeJoint2D>() != null)
            {
                tongueGo[i].GetComponent<HingeJoint2D>().enabled = false;
                tongueGo[i].GetComponent<Collider2D>().isTrigger = true;
                var test = tongueGo[i].transform.childCount;
                tongueGo[i].transform.DetachChildren();
                tongueGo[i].GetComponent<Rigidbody2D>().sharedMaterial = null;
            }
        }
        
        //Destroy(_tongueRoot.gameObject);
    }
}