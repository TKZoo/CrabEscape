using UnityEngine;

public class LeacherTongueSegment : MonoBehaviour
{
    public Rigidbody2D segmentRb;
    public HingeJoint2D segmentHj;
    public EnterCollisionComponent segmentColC;
    private void Awake()
    {
        segmentColC._action.AddListener(OnTongueCol);
    }

    public void OnTongueCol(GameObject target)
    {
        var tongue = transform.parent.GetComponent<LeacherTongue>();
        tongue.SetParent(target, gameObject);
    }
}