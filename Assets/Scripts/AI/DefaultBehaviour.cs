namespace AI
{
    public class DefaultBehaviour : IBehaviour
    {
        public bool Locked => false;
        
        public void Update(WeaponControl weaponControl, Ship ship)
        {
            // Find any enemies
            
            ship.Control(0, 1);
        }
    }
}