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
    public bool BeamMode;
    
    public float MaxHeat = 1;    
    public float HeatPerShot;
    
    public bool UsesAmmo;
    public int MaxAmmo;
    public float ReloadTime;
    
    // Runtime
    float timeUntilReloaded;
    float currentCooldown;
    float heat;
    Rigidbody2D rb;
    int shotCount;
    Ship ship;
    bool beamShot;
    bool overheating;
    bool reloading;
    int ammo;
        
    public bool DebugLog;
    
    GameObject BeamProjectile;
    
    void Start()
    {
        currentCooldown = 0;
        var mainShip = transform.parent.parent; 
        rb = mainShip.GetComponent<Rigidbody2D>();
        ship = mainShip.GetComponent<Ship>();
        ammo = MaxAmmo;
    }

    void Update()
    {
        currentCooldown -= Time.deltaTime;
        heat = Mathf.Max(0, heat - Time.deltaTime);
        if (heat <= 0)
        {
            overheating = false;
        }

        if (reloading)
        {
            timeUntilReloaded = Mathf.Max(0, timeUntilReloaded - Time.deltaTime);
            if (timeUntilReloaded <= 0)
            {
                reloading = false;
                ammo = MaxAmmo;
            }
        }

        if (DebugLog)
            Debug.Log($"a:{ammo}/{MaxAmmo} h:{heat}/{MaxHeat} Rel:{reloading} Ovrht:{overheating}");
    }

    public bool TryShoot()
    {
        if (!reloading && !overheating && currentCooldown <= 0)
        {
            heat += HeatPerShot;
            if (heat > MaxHeat)
            {
                overheating = true;                
            }

            if (UsesAmmo)
            {
                ammo--;
                if (ammo == 0)
                {
                    timeUntilReloaded = ReloadTime;
                    reloading = true;
                }
            }

            if (BeamMode)
            {
                beamShot = true;
            }
            else
            {
                shotCount++;
                currentCooldown = MaxCooldown;
                return true;    
            }
        }

        return false;
    }

    public void ReleaseShot()
    {
        beamShot = false;
        if (BeamProjectile)
        {
            Destroy(BeamProjectile);
        }
    }

    void FixedUpdate()
    {
        if (BeamMode && beamShot)
        {
            if (!BeamProjectile)
            {
                BeamProjectile = Instantiate(ProjectilePrefab);
                BeamProjectile.layer = gameObject.layer;
            }

            Transform beamTransform = BeamProjectile.transform; 
            beamTransform.position = ProjectileSpawnLocation.position;
            beamTransform.rotation = ProjectileSpawnLocation.rotation;
        }
        else
        {
            while (shotCount > 0)
            {
                Shoot();
                shotCount--;
            }
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