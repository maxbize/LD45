using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartsManifest
{
    public static readonly List<ShipPartData> allParts = new List<ShipPartData>() {
        new ShipPartData(ShipPartData.Type.Cockpit, "Cockpit", "Ships_0", 100, 0, 1, 100, 0),
        new ShipPartData(ShipPartData.Type.Cockpit, "Cockpit", "Ships_0", 100, 0, 2, 100, 0),
        new ShipPartData(ShipPartData.Type.Cockpit, "Cockpit", "Ships_0", 100, 0, 3, 100, 0),
        new ShipPartData(ShipPartData.Type.Thruster, "Thrusters", "Ships_1", 100, 50, 1, 100, 100),
        new ShipPartData(ShipPartData.Type.Thruster, "Thrusters", "Ships_1", 100, 50, 2, 100, 250),
        new ShipPartData(ShipPartData.Type.Thruster, "Thrusters", "Ships_1", 100, 50, 3, 100, 1000),
    };
}
