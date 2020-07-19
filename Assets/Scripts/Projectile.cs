using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    public Damage Damage;
    public float Lifetime;
    
    // TODO: Damage over distance curve?

    public Transform Target;
    public float RotationSpeed;
    public float Speed;
    
    Light2D _light;
    Rigidbody2D rb;
    Animator anim;
    static readonly int Hit = Animator.StringToHash("Hit");
    Collider2D _collider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _light = GetComponent<Light2D>();
    }

    void Update()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0)
        {
            Destroy(gameObject);
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
            Vector2 vel = rb.velocity.normalized.Rotate(rotation * Time.fixedDeltaTime) * Speed;
            rb.velocity = vel;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        anim.SetTrigger(Hit);
        
        var ship = other.gameObject.GetComponent<Ship>();
        if (ship != null) ship.ProjectileCollision(Damage);

        var bulletRb = other.otherRigidbody;

        bulletRb.velocity = other.rigidbody.velocity;
        bulletRb.angularVelocity = 0;
        
        var contactPoint = other.GetContact(0);

        transform.position = contactPoint.point;
        transform.rotation = Quaternion.Euler(0, 0, contactPoint.normal.Angle() * Mathf.Rad2Deg + 180f);
        
        _collider.enabled = false;
        StartCoroutine(DestroyAfterAnimation());
        var fadeout = gameObject.AddComponent<FadeoutLight>();
        fadeout.FadeTime = 0.2f;
        enabled = false;
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0));
        Destroy(gameObject);
    }
}
