using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerControl : MonoBehaviour
{
    Ship ship;
    WeaponMount assignedWeapon;
    int assignedIndex;

    void Start()
    {
        ship = GetComponent<Ship>();
        ship.GetWeapons();
        SwapWeapon(assignedIndex);
    }

    void SwapWeapon(int index)
    {
        if (assignedWeapon) assignedWeapon.ReleaseShot();
        assignedWeapon = ship.weapons[index];
    }

    void FixedUpdate()
    {
        assignedWeapon.Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        ship.Control(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        
        
        if (Input.GetKey(KeyCode.Space))
        {
            assignedWeapon.TryShoot();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            assignedWeapon.ReleaseShot();
        }

        assignedWeapon.ManualTargetSet = true;
        
        
        // TODO: Assign targets for manual weapons, even if not assigned?
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (assignedWeapon)
            {
                assignedWeapon.ManualTargetSet = false;
            }

            assignedIndex++;
            assignedIndex %= ship.weapons.Length;
            SwapWeapon(assignedIndex);
        }
    }
}