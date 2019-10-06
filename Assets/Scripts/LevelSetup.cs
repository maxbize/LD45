using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    // Set in editor
    public int width;
    public int height;
    public int thrusters;
    public int armors;
    public int machineGuns;
    public int cannons;
    public int shields;
    public int missiles;

    public LevelsManager.LevelData ToLevelData() {
        return new LevelsManager.LevelData(
            width,
            height,
            new[] { 1, 0, 0 },
            new[] { thrusters, 0, 0 },
            new[] { armors, 0, 0 },
            new[] { machineGuns, 0, 0 },
            new[] { cannons, 0, 0 },
            new[] { shields, 0, 0 },
            new[] { missiles, 0, 0 }
            );
    }
}
