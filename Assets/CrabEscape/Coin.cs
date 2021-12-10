using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coinValue;
    [SerializeField] private ScoreCounterComponent scorecounter;

    public void OnCoinColected()
    {
        scorecounter.CountScore(_coinValue);
    }
}
