using UnityEngine;

public class LeacherEnemy : MonoBehaviour
{
    [SerializeField] private LayerCheck _vision;
    [SerializeField] private CheckCircleOverlapComponent _meleeAtack;
    [SerializeField] private LayerCheck _inAttackRange;
    [SerializeField] private Cooldown _meleeCooldown;
    [SerializeField] private LeacherTongue _tongue;
    [SerializeField] private float _tongueSpeedIn, _tongueSpeedOut;

    private GameObject[] tongueGo;

    [SerializeField] private bool _isTraped;

    private void Awake()
    {
        _tongue = transform.GetComponent<LeacherTongue>();
        _isTraped = false;
    }

    private void Update()
    {
        Debug.Log(_isTraped);
        /*if (_inAttackRange.IsTouchingLayer && _isTraped)
        {
            _tongueSpeedOut = 0;
            if (_meleeCooldown.IsReady)
            {
                MeleeAttack();
                _meleeCooldown.Reset();
                return;
            }
        }*/
        if (_vision.IsTouchingLayer && !_isTraped)
        {
            _tongue.TongueMovement(_tongueSpeedOut);
        }

        if (!_vision.IsTouchingLayer || _isTraped)
        {
            _tongue.TongueMovement(_tongueSpeedIn);
        }
    }

    public void SetParent(GameObject parent)
    {
        tongueGo = GameObject.FindGameObjectsWithTag("EnemyTongue");
        if (!_isTraped)
        {
            for (int i = 0; i < tongueGo.Length; i++)
            {
                if (tongueGo[i].GetComponent<Collider2D>().IsTouching(parent.GetComponent<Collider2D>()))
                {
                    parent.transform.SetParent(tongueGo[i].transform);
                }
            }
            _isTraped = true;
            parent.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void MeleeAttack()
    {
        _meleeAtack.Check();
    }
}