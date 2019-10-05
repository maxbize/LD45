using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartsManifest
{
    public static readonly List<ShipPartData> allParts = new List<ShipPartData>() {
        new ShipPartData("Cockpit", "Ships_0", 100, 0, 1, 100),
        new ShipPartData("Cockpit", "Ships_0", 100, 0, 2, 100),
        new ShipPartData("Cockpit", "Ships_0", 100, 0, 3, 100),
        new ShipPartData("Thrusters", "Ships_1", 100, 50, 1, 100),
        new ShipPartData("Thrusters", "Ships_1", 100, 50, 2, 100),
        new ShipPartData("Thrusters", "Ships_1", 100, 50, 3, 100),
    };
}
