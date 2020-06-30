using UnityEngine;

public class WeaponMount : MonoBehaviour
{
    [Header("Configuration")]
    public float MinRotation;
    public float MaxRotation;
    public float RotationSpeed;

    public Transform TargetTransform;
    public Vector2 Target;
    public bool ManualTargetSet;
    
    // Runtime
    Weapon weapon;

    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        MinRotation = Util.NormalizeAngleDegrees(MinRotation, -180, 180);
        MaxRotation = Util.NormalizeAngleDegrees(MaxRotation, -180, 180);
    }

    void FixedUpdate()
    {
        if (TargetTransform)
        {
            Target = TargetTransform.position;
        }

        if (ManualTargetSet || TargetTransform)
        {
            RotateTowardTarget();
        }
    }

    void RotateTowardTarget()
    {
        // Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetVector = Target - (Vector2)transform.position;

        var currentDegrees = Util.NormalizeAngleDegrees(transform.rotation.eulerAngles.z);
        var targetRadians = Mathf.Atan2(targetVector.y, targetVector.x);
        var targetDegrees = Util.NormalizeAngleDegrees(targetRadians * Mathf.Rad2Deg);

        float newDegreesGlobal;
        if (Mathf.Abs(currentDegrees - targetDegrees) < RotationSpeed * Time.deltaTime)
        {
            // Close enough
            newDegreesGlobal = targetDegrees;
        }
        else
        {
            bool clockwise = Util.RotateTowardsAngleDegrees(currentDegrees, targetDegrees);
            newDegreesGlobal = currentDegrees + (clockwise ? -1 : 1) * RotationSpeed * Time.deltaTime;
        }
        
        float localDegrees = newDegreesGlobal - transform.parent.rotation.eulerAngles.z;
        localDegrees = Util.NormalizeAngleDegrees(localDegrees, -180, 180);
        localDegrees = Mathf.Clamp(localDegrees, MinRotation, MaxRotation);
        
        transform.localRotation = Quaternion.Euler(0, 0, localDegrees);
    }

    public void TryShoot()
    {
        weapon.TryShoot();
    }

    public void ReleaseShot()
    {
        weapon.ReleaseShot();
    }
}