using UnityEngine;

public class InvertHpBarScaleComponent : MonoBehaviour
{
    [SerializeField] private GameObject _go;
    [SerializeField] private RectTransform _rectTransform;

    void Update()
    {
        if (_go.transform.localScale.x < 0)
        {
            _rectTransform.localScale = new Vector3(-1, 1);
        }
        else
        {
            _rectTransform.localScale = new Vector3(1, 1);
        }
    }
}
