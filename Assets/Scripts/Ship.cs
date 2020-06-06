using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float Health;

    Engine engine;
    public WeaponMount[] weapons;
    public List<Collider2D> Colliders = new List<Collider2D>();

    // Runtime variables
    float bulletCooldown;

    void Start()
    {
        engine = GetComponent<Engine>();
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
