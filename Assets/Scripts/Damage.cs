using System;

[Serializable]
public class Damage
{
    public float Bullet;
    public float Laser;
    public float Missile;

    public float Total(DamageModifier dm)
    {
        return Bullet * dm.Bullet + 
               Laser * dm.Laser +
               Missile * dm.Missile;
    }
}

[Serializable]
public class DamageModifier
{
    public float Bullet = 1;
    public float Laser = 1;
    public float Missile = 1;
}