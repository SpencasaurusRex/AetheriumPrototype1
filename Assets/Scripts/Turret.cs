using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform TurretMount;
    public float RotationRate;
    public float ShootCooldown;
    public float BulletShootSpeed;
    public float BulletCreationRadius = .4f;
    public float Health = 50f;
    
    float rotation;
    float shootCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        shootCooldown = ShootCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        rotation += RotationRate * Time.deltaTime;
        TurretMount.rotation = Quaternion.AngleAxis(rotation * Mathf.Rad2Deg, Vector3.forward);

        shootCooldown -= Time.deltaTime;
        if (shootCooldown <= 0)
        {
            Shoot();
            shootCooldown = ShootCooldown;
        }
    }

    void Shoot()
    {
        var rotationVector = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
        
        var bulletObj = Instantiate(BulletPrefab);
        bulletObj.transform.position = rotationVector * BulletCreationRadius + (Vector2)transform.position;
        var bullet = bulletObj.GetComponent<Projectile>();
        bullet.Lifetime = 5f;
        
        var bulletRb = bulletObj.GetComponent<Rigidbody2D>();
        var bulletCollider = bulletObj.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(bulletCollider, GetComponent<BoxCollider2D>());
        bulletRb.velocity = rotationVector * BulletShootSpeed;
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
