namespace AI
{
    public interface IBehaviour
    {
        bool Locked { get; }

        void Update(WeaponControl weaponControl, Ship ship);
    }
}

