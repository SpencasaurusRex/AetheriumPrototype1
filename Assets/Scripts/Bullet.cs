using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Lifetime;

    void Update()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var ship = other.gameObject.GetComponent<Ship>();
        if (ship != null) ship.BulletCollision(10f);
        
        Destroy(gameObject);
    }
}
