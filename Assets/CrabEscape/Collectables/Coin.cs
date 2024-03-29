﻿using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coinValue;
    private GameSession _session;
    private Rigidbody2D _coinRb;
    private Collider2D _coinCollider;

    private void Awake()
    {
        _session = FindObjectOfType<GameSession>();
        _coinRb = GetComponent<Rigidbody2D>();
        _coinCollider = GetComponent<Collider2D>();
    }
    private void Start()
    {
        if (_coinRb != null)
        {
            //_coinRb.velocity = new Vector2(Random.Range(-4f, 4f), Random.Range(5f, 6f));
            _coinRb.AddForce(new Vector2(Random.Range(-100, 100f), Random.Range(100f, 200f)), ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (_coinCollider != null && col.gameObject.CompareTag("Player"))
        {
            _coinCollider.enabled = false;
            _coinRb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void OnCoinColected()
    {
        _session.PlayerData.Inventory.Add("Coin", _coinValue);
    }
}
