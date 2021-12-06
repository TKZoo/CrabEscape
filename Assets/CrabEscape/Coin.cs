using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coinValue;
    [SerializeField] private ScoreCounterComponent scorecounter;

    //private void Awake()
    //{
    //    scorecounter = GetComponent<ScoreCounterComponent>();
    //}

    public void OnCoinColected()
    {
        scorecounter.CountScore(_coinValue);
    }
}
