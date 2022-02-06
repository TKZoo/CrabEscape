using UnityEngine;

public class DestroyObjectComponent : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDestroy;
    [SerializeField] private float _delay = 0f;
    public void DestroyObject()
    {
        Destroy(_objectToDestroy, _delay);
    }
}
