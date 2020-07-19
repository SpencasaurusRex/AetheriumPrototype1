using System;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    Ship ship;
    IBehaviour currentBehaviour;
    List<IBehaviourTrigger> behaviourTriggers;
    WeaponControl weaponControl;
    ScenarioController scenario;
    Rigidbody2D rb;
    Engine engine;
    
    void OnEnable()
    {
        behaviourTriggers = new List<IBehaviourTrigger>();
        behaviourTriggers.Add(new DefaultBehaviourTrigger());
    }

    void Start()
    {
        ship = GetComponent<Ship>();
        weaponControl = GetComponent<WeaponControl>();
        rb = GetComponent<Rigidbody2D>();
        engine = GetComponent<Engine>();
        currentBehaviour = GetCurrentBehaviour();
        
        // TODO: Pass this in?
        scenario = ScenarioController.Instance;
    }
    
    void Update()
    {
        if (!currentBehaviour.Locked)
        {
            currentBehaviour = GetCurrentBehaviour();
        }
        currentBehaviour.Update(weaponControl, ship, scenario, rb, engine);
    }

    IBehaviour GetCurrentBehaviour()
    {
        foreach (var trigger in behaviourTriggers)
        {
            if (trigger.CheckConditions())
            {
                return trigger.TargetTrigger;
            }
        }
        throw new Exception("Need a default behaviour");
    }
}
