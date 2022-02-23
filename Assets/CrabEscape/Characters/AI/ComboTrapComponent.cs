using System.Collections.Generic;
using UnityEngine;

public class ComboTrapComponent : MonoBehaviour
{
    [SerializeField] private Cooldown _cooldown;
    [SerializeField] private ShootingBlock[] _shootingBlocks;
    [SerializeField] private List<ShootingBlock> _blockList;
    private int _shootingBlock;

    private void Awake()
    {
        _shootingBlocks = GetComponentsInChildren<ShootingBlock>();
    }

    private void Start()
    {
        foreach (ShootingBlock child in _shootingBlocks)
        {
            _blockList.Add(child);
            var hp = child.GetComponentInChildren<HealthComponent>();
            hp._onDie.AddListener(() => OnObjectDestryed(child));
        }
    }
    
    private void Update()
    {
        if (_cooldown.IsReady)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_blockList.Count > 0)
        {
            _shootingBlock = (int)Mathf.Repeat(_shootingBlock + 1, _blockList.Count);
            var totem = _blockList[_shootingBlock];
            if (totem != null)
            {
                totem.RangeAttack();
                _cooldown.Reset();
            }
        }
    }

    private void OnObjectDestryed(ShootingBlock child)
    {
        var index = _blockList.IndexOf(child);
        _blockList.Remove(child);
        if (index < _shootingBlock)
        {
            _shootingBlock--;
        }
        if (_blockList.Count == 0)
        {
            Destroy(gameObject, 1f);
        }
    }
}