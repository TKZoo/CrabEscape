using UnityEngine;

public class ScoreCounterComponent : MonoBehaviour
{    
    private int _totalCoins = 0;

    public void CountScore (int coinvalue)
    {
        _totalCoins += coinvalue;
        Debug.Log($"Всего монет: {_totalCoins}");
    }
}
