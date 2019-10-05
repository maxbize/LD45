using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartData
{
    public readonly Type type;
    public readonly string partName;
    public readonly string spriteName;
    public readonly int maxHealth;
    public readonly int cost;
    public readonly int mark; // mk1, mk2, etc
    public readonly int mass;
    public readonly int intParam1; // Thrust force, 

    public enum Type
    {
        Cockpit,
        Thruster
    }

    public ShipPartData(Type type, string partName, string spriteName, int maxHealth, int cost, int mark, int mass, int intParam1) {
        this.partName = partName;
        this.spriteName = spriteName;
        this.maxHealth = maxHealth;
        this.cost = cost;
        this.mark = mark;
        this.mass = mass;
        this.type = type;
        this.intParam1 = intParam1;
    }

    public int GetThrustForce() {
        if (type != Type.Thruster) {
            Debug.LogWarning("Asked for Thrust force but type is " + type);
        }
        return intParam1;
    }
}
