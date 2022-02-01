using UnityEngine;

public class ScoreCounterComponent : MonoBehaviour
{    
    private int _totalCoins = 0;
    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }
    /*public void CountScore (int coinvalue)
    {
        _totalCoins += coinvalue;
        _session.PlayerData.Coins = _totalCoins;
        Debug.Log($"Всего монет: {_totalCoins}");
    }*/

    public void SetScore(int coins)
    {
        _totalCoins = coins;
    }
    
}
