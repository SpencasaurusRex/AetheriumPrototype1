using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class DefaultBehaviour : IBehaviour
    {
        public bool Locked => false;
        
        public void Update(WeaponControl weaponControl, Ship ship, ScenarioController scenario, 
            Rigidbody2D rb, Engine engine)
        {
            // TODO: Optimize enemy checks, don't do every frame
            // TODO: don't recalculate opposing ships, update it with events from scenario,
            //       when ships are removed or added to the scenario

            // Get closest enemy, if any
            Ship closestEnemy = null;
            float closestEnemyDistance = float.PositiveInfinity;
            List<Ship> opposingShips = new List<Ship>();
            
            for (int t = 0; t < scenario.TeamShips.Count; t++)
            {
                if (t == ship.Team) continue;
                for (int s = 0; s < scenario.TeamShips[t].Count; s++)
                {
                    var os = scenario.TeamShips[t][s];
                    opposingShips.Add(os);
                    var distance = (os.transform.position - ship.transform.position).sqrMagnitude;
                    if (distance < closestEnemyDistance)
                    {
                        closestEnemy = os;
                        closestEnemyDistance = distance; 
                    }
                }
            }

            if (closestEnemy == null)
            {
                Stop(ship);
                return;
            }
            
            // TODO: calculate these
            const float TooFar = 20;
            const float TooClose = 4;
            
            var targetOffset = closestEnemy.transform.position - ship.transform.position; 
            
            float thrustInput = 0;
            float rotationInput = 0;

            // Move towards enemy, until within range
            if (closestEnemyDistance > TooFar)
            {
                
            }
            
            // Too close, move away
            else if (closestEnemyDistance < TooClose)
            {
                
            }

            // Just right, keep in sweet spot
            else
            {
                
            }
            
            rotationInput = TurnTowards(ship, rb, targetOffset, engine);
            ship.Control(thrustInput, rotationInput);
        }

        void Stop(Ship ship)
        {
            // TODO
        }

        float TurnTowards(Ship ship, Rigidbody2D rb, Vector3 offset, Engine engine)
        {
            var targetAngle = Mathf.Atan2(offset.y, offset.x) + Mathf.PI;
            var currentAngle = ship.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            
            float m = rb.mass;
            float f = 1 + engine.CancelRotationBonus;
            float v_0 = rb.angularVelocity;
            var destinationAngle = -0.5f * m / f * v_0 * v_0 + currentAngle; 
            Debug.Log($"{currentAngle} {targetAngle} {destinationAngle}");
            var currentSign = Mathf.Sign(Util.SignedAngleDistanceRadians(targetAngle, currentAngle));
            var breakSign = Mathf.Sign(Util.SignedAngleDistanceRadians(targetAngle, destinationAngle));
            
            float torque;
            if (currentSign != breakSign)
            {
                // Start breaking
                torque = Util.RotateTowardsAngleRadians(currentAngle, targetAngle) ? 1 : -1;
            }
            else
            {
                // Keep accelerating
                torque = Util.RotateTowardsAngleRadians(currentAngle, targetAngle) ? -1 : 1;
            }
            
            return torque;
        }
    }
}