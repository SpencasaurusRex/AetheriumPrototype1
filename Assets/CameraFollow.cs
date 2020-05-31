using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;

    void Start()
    {
        if (Target)
        {
            SetTarget(Target);
        }
    }

    void LateUpdate()
    {
        if (Target)
        {
            transform.position = Target.position + Offset;   
        }
    }
    
    public void SetTarget(Transform target)
    {
        Target = target;
        Offset = transform.position - target.position;
    }
}
