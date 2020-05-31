using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject ProjectilePrefab;
    public float MaxCooldown;
    public Transform ProjectileSpawnLocation;
    public float BulletVelocity;
    public bool Homing;
    public float DetectionRange;

    // Runtime
    float currentCooldown;
    Rigidbody2D rb;
    int shotCount;
    Ship ship;
    
    void Start()
    {
        currentCooldown = 0;
        var mainShip = transform.parent.parent; 
        rb = mainShip.GetComponent<Rigidbody2D>();
        ship = mainShip.GetComponent<Ship>();
    }

    void Update()
    {
        currentCooldown -= Time.deltaTime;
    }

    public bool TryShoot()
    {
        if (currentCooldown <= 0)
        {
            shotCount++;
            currentCooldown = MaxCooldown;
            return true;
        }

        return false;
    }

    void FixedUpdate()
    {
        while (shotCount > 0)
        {
            Shoot();
            shotCount--;
        }
    }

    void Shoot()
    {
        // TODO: Maybe take into account rotational velocity? Feels weird when spinning without
        Vector2 projectileVelocity = (Vector2) transform.right.normalized * BulletVelocity + rb.velocity;

        var projectileObj = Instantiate(ProjectilePrefab);
        projectileObj.transform.position = ProjectileSpawnLocation.position;
        projectileObj.layer = gameObject.layer;

        float projectileAngle = Mathf.Atan2(projectileVelocity.y, projectileVelocity.x);
        projectileObj.transform.rotation = transform.rotation;
        
        var projectile = projectileObj.GetComponent<Projectile>();
        projectile.Lifetime = 5f;
        
        if (Homing)
        {
            var opposingTeams = Util.GetOpposingTeamLayerMask(gameObject.layer);
            Collider2D closestEnemy = Physics2D.OverlapCircle(transform.position, DetectionRange, opposingTeams);
            if (closestEnemy)
                projectile.Target = closestEnemy.transform;
        }
        
        var projectileRb = projectileObj.GetComponent<Rigidbody2D>();
        var projectileCollider = projectileObj.GetComponent<Collider2D>();

        foreach (var collider in ship.Colliders)
        {
            Physics2D.IgnoreCollision(projectileCollider, collider);   
        }

        projectileRb.velocity = projectileVelocity;
    }
}