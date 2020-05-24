using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    Ship ship;
    
    void Start()
    {
        ship = GetComponent<Ship>();
    }
    
    void Update()
    {
        ship.Control(0, 0);
        ContactFilter2D filer = new ContactFilter2D();
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, 10);

        foreach (var hit in hits)
        {
            var lookatShip = hit.collider.gameObject.GetComponent<Ship>();
            if (lookatShip && lookatShip != ship)
            {
                ship.TryShoot();
            }
        }
    }
}
