using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    [SerializeField] private Transform _destinationTransform;
    
    public void Teleportation(GameObject target)
    {
        target.transform.position = _destinationTransform.position;
    }
}
