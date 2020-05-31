using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Lifetime;

    public Transform Target;
    public float RotationSpeed;
    public float Speed;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (Target)
        {
            var targetVector = Target.position - transform.position;
            
            float currentAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
            float targetAngle = Mathf.Atan2(targetVector.y, targetVector.x);
            bool clockwise = Util.RotateTowardsAngleRadians(currentAngle, targetAngle);

            var rotation = (clockwise ? -1 : 1) * RotationSpeed;
            rb.velocity = rb.velocity.normalized.Rotate(rotation * Time.fixedDeltaTime) * Speed;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        var ship = other.gameObject.GetComponent<Ship>();
        if (ship != null) ship.BulletCollision(10f);
        
        Destroy(gameObject);
    }
}
