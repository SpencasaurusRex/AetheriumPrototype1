using System.Xml;
using DefaultNamespace;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Rigidbody2D rb;
    
    [Header("Controls")]
    public float Thrust = 1f;
    public float BackwardPenalty = 0.5f;
    public float RotationalThrust = 1;
    public float CancelThrustBonus = 1f;
    public float CancelRotationBonus = 1f;
    public float SidewaysCancel = .95f;
    
    [Header("Health")]
    public float Health;

    [Header("Weapons")]
    public float BulletCoolDown;
    public int BulletSpawnIndex;
    public Transform[] BulletSpawns;
    public GameObject BulletPrefab;
    public float BulletVelocity;
    
    // Runtime variables
    float thrustForce;
    float torque;
    float bulletCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TryShoot()
    {
        bulletCooldown = Mathf.Max(0, bulletCooldown - Time.deltaTime);
        if (bulletCooldown <= 0)
        {
            Shoot(BulletSpawns[BulletSpawnIndex++]);
            BulletSpawnIndex %= BulletSpawns.Length;
            bulletCooldown = BulletCoolDown;
        }
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
        torque = rotationInput * RotationalThrust * (1 + rotationBonus);
    }

    void Shoot(Transform bulletSpawnLoc)
    {
        var bulletObj = Instantiate(BulletPrefab);
        bulletObj.transform.position = bulletSpawnLoc.position;
        var bullet = bulletObj.GetComponent<Bullet>();
        bullet.Lifetime = 5f;
        
        var bulletRb = bulletObj.GetComponent<Rigidbody2D>();
        var bulletCollider = bulletObj.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(bulletCollider, GetComponent<BoxCollider2D>());
        bulletRb.velocity = (Vector2)transform.right.normalized * BulletVelocity + rb.velocity;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.right * thrustForce);
        rb.AddTorque(-torque);
        
        // Cancel some sideways velocity
        var headingDegrees = transform.rotation.eulerAngles.z;

        var alignedVelocity = rb.velocity.Rotate(-headingDegrees);
        alignedVelocity.y *= SidewaysCancel;

        rb.velocity = alignedVelocity.Rotate(headingDegrees);
    }

    public void BulletCollision(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
