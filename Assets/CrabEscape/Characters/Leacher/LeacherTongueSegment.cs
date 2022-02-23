using UnityEngine;

public class LeacherTongueSegment : MonoBehaviour
{
    public Rigidbody2D segmentRb;
    public HingeJoint2D segmentHj;
    public EnterCollisionComponent segmentColC;
    private void Awake()
    {
        segmentRb = GetComponent<Rigidbody2D>();
        segmentHj = GetComponent<HingeJoint2D>();
        segmentColC = GetComponent<EnterCollisionComponent>();

        segmentColC._action.AddListener(OnTongueCol);
    }

    public void OnTongueCol(GameObject target)
    {
        var tongue = transform.parent.GetComponent<LeacherTongue>();
        tongue.SetParent(target, gameObject);
    }
}