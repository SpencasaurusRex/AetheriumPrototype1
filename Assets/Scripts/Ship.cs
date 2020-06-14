using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("Configuration")]
    public float Health;
    public DamageModifier DamageModifier;
    
    Engine engine;
    Shield shield;
    
    [HideInInspector]
    public WeaponMount[] weapons;
    [HideInInspector]
    public List<Collider2D> Colliders = new List<Collider2D>();

    // Runtime variables
    float bulletCooldown;

    void Start()
    {
        engine = GetComponent<Engine>();
        shield = GetComponentInChildren<Shield>();
        GetWeapons();
        
        Colliders.AddRange(GetComponentsInChildren<Collider2D>());
    }

    public void GetWeapons()
    {
        weapons = GetComponentsInChildren<WeaponMount>();
    }

    public void Control(float thrustInput, float rotationInput)
    {
        engine.Control(thrustInput, rotationInput);
    }

    public void ProjectileCollision(Damage damage)
    {
        shield.TakeDamage(damage);
        Health -= damage.Total(DamageModifier);

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
