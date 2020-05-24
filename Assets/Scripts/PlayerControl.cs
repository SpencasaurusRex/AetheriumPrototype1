using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerControl : MonoBehaviour
{
    Ship ship;
    
    void Start()
    {
        ship = GetComponent<Ship>();
    }

    void Update()
    {
        ship.Control(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        if (Input.GetKey(KeyCode.Space))
        {
            ship.TryShoot();
        }
    }
}