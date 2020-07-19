using System.IO;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [Header("Controls")] public float Thrust = 1f;
    public float BackwardPenalty = 0.5f;
    public float RotationalThrust = 1;
    public float CancelThrustBonus = 1f;
    public float CancelRotationBonus = 1f;
    public float SidewaysCancel = .95f; // TODO: Change to acceleration rather than friction force
    public float RotationalFriction = .99f;

    // TODO: Implement max vel and angular vel, possibly as some multiplier of Thrust and RotationalThrust

    // Runtime
    Rigidbody2D rb;
    float thrustForce;
    float torque;

    // Radians / s
    float angularVelocity;

    // Radians
    float rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Control(float thrustInput, float rotationInput)
    {

        // Calculate thrust
        float thrustBonus = 0;
        thrustForce = thrustInput * Thrust;
        if (thrustInput < 0)
        {
            thrustForce *= BackwardPenalty;
        }
        if (Vector2.Dot(rb.velocity.normalized, transform.right * thrustInput) < 0)
        {
            thrustBonus = Vector3.Cross(rb.velocity.normalized.Rotate(90f), transform.right * thrustInput).magnitude * CancelThrustBonus;
        }
        thrustForce *= (1 + thrustBonus);
    
        // Calculate torque
        float rotationBonus = 0;
        if (-Mathf.Sign(rb.angularVelocity) != Mathf.Sign(rotationInput))
        {
            rotationBonus = CancelRotationBonus;
        }
        torque = -rotationInput * RotationalThrust * (1 + rotationBonus);
        
        // TODO: Automatically cancel rotation if not holding rotate
    }
    
    void FixedUpdate()
    {
        angularVelocity *= RotationalFriction;
        
        rb.AddForce(transform.right * thrustForce);
        angularVelocity += torque / rb.mass * Time.fixedDeltaTime;
        rotation += angularVelocity * Time.fixedDeltaTime;

        rb.MoveRotation(rotation * Mathf.Rad2Deg);
    
        // Cancel some sideways velocity
        var headingDegrees = transform.rotation.eulerAngles.z;

        var alignedVelocity = rb.velocity.Rotate(-headingDegrees);
        alignedVelocity.y *= SidewaysCancel;

        rb.velocity = alignedVelocity.Rotate(headingDegrees);
    }
}