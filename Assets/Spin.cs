using UnityEngine;

public class Spin : MonoBehaviour
{
    public bool OnStart = false;
    public float SpinForce = 1;

    public bool Impulse;
    
    Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (OnStart)
        {
            rb.AddTorque(SpinForce, Impulse ? ForceMode2D.Impulse : ForceMode2D.Force);
        }
    }

    void Update()
    {
        if (!OnStart)
        {
            rb.AddTorque(SpinForce, Impulse ? ForceMode2D.Impulse : ForceMode2D.Force);
        }
    }
}
