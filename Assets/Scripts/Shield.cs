using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Configuration")] 
    public float MaxStrength;
    public float StrengthRegen;
    public float RegenWait;
    public DamageModifier DamageModifier;
    
    public Color StrongColor;
    public Color WeakColor;
    
    [SerializeField]
    float strength;
    float lastHit;

    Collider2D _collider;
    SpriteRenderer sr;

    void Start()
    {
        strength = MaxStrength;
        _collider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (lastHit >= RegenWait)
        {
            // Regen if lastHit was long ago enough
            strength = Mathf.Min(strength + StrengthRegen * Time.deltaTime, MaxStrength);
            SetShieldActive(true);
        }
        
        sr.color = Color.Lerp(WeakColor, StrongColor, strength / MaxStrength);
        
        lastHit += Time.deltaTime;
    }

    public void TakeDamage(Damage damage)
    {
        lastHit = 0;
        
        float bulletBlocked = Mathf.Min(damage.Bullet * DamageModifier.Bullet, strength);
        strength -= bulletBlocked;
        damage.Bullet -= bulletBlocked / DamageModifier.Bullet;
        
        float laserBlocked = Mathf.Min(damage.Laser * DamageModifier.Laser, strength);
        strength -= laserBlocked;
        damage.Bullet -= laserBlocked / DamageModifier.Laser;
        
        float missileBlocked = Mathf.Min(damage.Missile * DamageModifier.Missile, strength);
        strength -= missileBlocked;
        damage.Bullet -= missileBlocked / DamageModifier.Missile;

        if (strength <= 0)
        {
            SetShieldActive(false);
        }
    }

    void SetShieldActive(bool active)
    {
        _collider.enabled = active;
        sr.enabled = active;
    }
}
