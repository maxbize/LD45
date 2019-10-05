using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartsManifest
{
    public static readonly List<ShipPartData> allParts = new List<ShipPartData>() {
        new ShipPartData(ShipPartData.Type.Cockpit, "Cockpit", "Ships_0", 100, 0, 1, 100),
        new ShipPartData(ShipPartData.Type.Cockpit, "Cockpit", "Ships_0", 100, 0, 2, 100),
        new ShipPartData(ShipPartData.Type.Cockpit, "Cockpit", "Ships_0", 100, 0, 3, 100),
        new ShipPartData(ShipPartData.Type.Thruster, "Thrusters", "Ships_1", 100, 50, 1, 100, "Thrust1PS", intParam1: 100),
        new ShipPartData(ShipPartData.Type.Thruster, "Thrusters", "Ships_1", 100, 50, 2, 100, "Thrust1PS", intParam1: 250),
        new ShipPartData(ShipPartData.Type.Thruster, "Thrusters", "Ships_1", 100, 50, 3, 100, "Thrust1PS", intParam1: 1000),
        new ShipPartData(ShipPartData.Type.MachineGun, "Machine Gun", "Ships_4", 100, 0, 1, 100, "MGun1P", intParam1: 10, floatParam1: 0.2f), // 50 dps
        new ShipPartData(ShipPartData.Type.MachineGun, "Machine Gun", "Ships_4", 100, 0, 2, 100, "MGun1P", intParam1: 15, floatParam1: 0.2f), // 75 dps
        new ShipPartData(ShipPartData.Type.MachineGun, "Machine Gun", "Ships_4", 100, 0, 3, 100, "MGun1P", intParam1: 30, floatParam1: 0.2f), // 150 dps
        new ShipPartData(ShipPartData.Type.MachineGun, "Cannon", "Ships_6", 100, 0, 1, 100, "Cannon1P", intParam1: 100, floatParam1: 2f), // 50 dps
        new ShipPartData(ShipPartData.Type.MachineGun, "Cannon", "Ships_6", 100, 0, 2, 100, "Cannon1P", intParam1: 150, floatParam1: 2f), // 75 dps
        new ShipPartData(ShipPartData.Type.MachineGun, "Cannon", "Ships_6", 100, 0, 3, 100, "Cannon1P", intParam1: 300, floatParam1: 2f), // 150 dps

    };
}
