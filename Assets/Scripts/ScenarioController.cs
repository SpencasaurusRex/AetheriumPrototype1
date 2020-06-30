using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public static ScenarioController Instance;
    
    public List<List<Ship>> TeamShips = new List<List<Ship>>();

    void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        // Initialize data from ships existing in the scene already
        // This is for custom created scenarios
        {
            for (int i = 0; i < Util.NumberOfTeams; i++)
            {
                TeamShips.Add(new List<Ship>());
            }

            var ships = FindObjectsOfType<Ship>();
            foreach (var ship in ships)
            {
                int team = Util.GetTeamIndexFromLayer(ship.gameObject.layer);
                TeamShips[team].Add(ship);
                ship.Team = team;
            }
        }
        
        // TODO
        // Construct ships / scene from passed in data
        // This should be pulled from some sort of initialization object that is placed into the scene
        // by the previous scene 
        {
            
        }
    }
}