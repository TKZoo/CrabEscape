using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounterComponent : MonoBehaviour
{    
    private int _totalCoins = 0;

    public void CountScore (int cointvalue)
    {
        _totalCoins += cointvalue;
        Debug.Log($"Всего монет: {_totalCoins}");
    }
}
