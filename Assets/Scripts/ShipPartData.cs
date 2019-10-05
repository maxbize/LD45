using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartData
{
    // Required params
    public readonly Type type;
    public readonly string partName;
    public readonly string spriteName;
    public readonly int maxHealth;
    public readonly int cost;
    public readonly int mark; // mk1, mk2, etc
    public readonly int mass;

    // Optional params
    private readonly string childPrefabName;
    private readonly int intParam1; // Thrust force, damage per hit/tick
    private readonly float floatParam1; // Cooldown

    public enum Type
    {
        Cockpit,
        Thruster,
        MachineGun
    }

    public ShipPartData(Type type, string partName, string spriteName, int maxHealth, int cost, int mark, int mass,
            string childPrefabName = null, int intParam1 = 0, float floatParam1 = 0f) {
        this.partName = partName;
        this.spriteName = spriteName;
        this.maxHealth = maxHealth;
        this.cost = cost;
        this.mark = mark;
        this.mass = mass;
        this.type = type;
        this.childPrefabName = childPrefabName;
        this.intParam1 = intParam1;
        this.floatParam1 = floatParam1;
    }

    public int GetThrustForce() {
        CheckType(Type.Thruster);
        return intParam1;
    }

    public float GetWeaponCooldown() {
        CheckType(Type.MachineGun);
        return floatParam1;
    }

    public string GetExhaustPrefab() {
        CheckType(Type.Thruster);
        return childPrefabName;
    }

    public string GetProjectilePrefab() {
        CheckType(Type.MachineGun);
        return childPrefabName;
    }

    private void CheckType(Type type) {
        if (this.type != type) {
            Debug.LogWarningFormat("Requested operation on type {0} but part is type {1}", type, this.type);
        }
    }
}
