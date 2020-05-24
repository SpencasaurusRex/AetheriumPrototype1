using UnityEngine;

public class CoursePlotter : MonoBehaviour
{
    // Units per second per second per unit mass
    public float ForwardThrust = 1f;
    public float RotationalThrust = Mathf.PI / 2;
    public float BackwardThrust = -0.3f;
    public Vector2 Velocity;
    public float Mass = 1f;
    public float TargetSpeed = 5f;
    public float TimeAhead = 2f;
    public float MaxSpeed = 10;
    public float MaxAngularVelocity = Mathf.PI;

    int R;
    int T;

    void Start()
    {
        if (BackwardThrust > 0)
        {
            BackwardThrust = -BackwardThrust;
        }
    }

    void OnDrawGizmos()
    {
        const int RotationFidelity = 20;
        const int TimeFidelity = 100;
        float DeltaTime = TimeAhead / TimeFidelity;

        R = -RotationFidelity;
        T = -TimeFidelity;
        
        for (float r = -RotationFidelity; r <= RotationFidelity; r++)
        {
            PhysicsState state = new PhysicsState
            {
                Position = transform.position,
                Velocity = Velocity,
                Rotation = 0,
                AngularVelocity = 0,
                Speed = Velocity.magnitude
            };

            float maxAngularVelocity = Mathf.Abs(r) / RotationFidelity * MaxAngularVelocity; 
            float torque = r / RotationFidelity * RotationalThrust / Mass * DeltaTime;
            Vector3 lastPosition = state.Position;
            
            for (int t = 0; t < TimeFidelity; t++)
            {
                float desiredThrust = (Mathf.Min(MaxSpeed, TargetSpeed) - state.Speed) * Mass / DeltaTime;
                float thrust = Mathf.Clamp(desiredThrust, BackwardThrust, ForwardThrust) / Mass * DeltaTime;
                
                state.Apply(thrust, torque, DeltaTime, MaxSpeed, maxAngularVelocity);

                if (r == R && t == T)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(state.Position, state.Position + state.Velocity);
                }

                Gizmos.color = Color.white;
                Gizmos.DrawLine(lastPosition, state.Position);
                lastPosition = state.Position;
                
            }
        }

        T++;
        if (T >= TimeFidelity)
        {
            T = -TimeFidelity;
            R++;
            if (R >= RotationFidelity)
            {
                R = -RotationFidelity;
            }
        }
    }
    
    

    struct PhysicsState
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public float Speed;
        
        public float Rotation;
        public float AngularVelocity;

        public void Apply(float normalizedThrust, float normalizedTorque, float deltaTime, float maxSpeed, float maxAngularVelocity)
        {
            AngularVelocity = Mathf.Clamp(AngularVelocity + normalizedTorque, 
                -maxAngularVelocity, maxAngularVelocity);
            Rotation += AngularVelocity * deltaTime;

            Speed = Mathf.Clamp(Speed + normalizedThrust, -maxSpeed, maxSpeed);
            Velocity = Speed * new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
            
            Position += Velocity * deltaTime;
        }
    }
}