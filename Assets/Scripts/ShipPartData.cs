using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartData
{
    public readonly string partName;
    public readonly string spriteName;
    public readonly int maxHealth;
    public readonly int cost;
    public readonly int mark; // mk1, mk2, etc
    public readonly int mass;

    public ShipPartData(string partName, string spriteName, int maxHealth, int cost, int mark, int mass) {
        this.partName = partName;
        this.spriteName = spriteName;
        this.maxHealth = maxHealth;
        this.cost = cost;
        this.mark = mark;
        this.mass = mass;
    }
}
