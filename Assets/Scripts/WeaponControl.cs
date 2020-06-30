using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public List<WeaponGroup> Groups;
    int AssignedGroupIndex;
    
    void Start()
    {
        // TODO: Assignment?
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}


[Serializable]
public class WeaponGroup
{
    public Weapon[] Weapons;
    public FiringType FiringType;
}

public enum FiringType
{
    AtWill,
    Alternating,
    Synchronous
}