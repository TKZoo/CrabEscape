using UnityEngine;
using UnityEngine.Serialization;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coinValue;
    [FormerlySerializedAs("scorecounter")] [SerializeField] private ScoreCounterComponent _scorecounter;

    public void OnCoinColected()
    {
        _scorecounter.CountScore(_coinValue);
    }
}
