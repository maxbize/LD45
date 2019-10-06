using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    // Set in editor
    public int currentLevel; // Here for debugging ;)

    private class LevelData
    {
        public readonly int shipWidth;
        public readonly int shipHeight;
        public readonly int[] cockpits;
        public readonly int[] thrusters;
        public readonly int[] armors;
        public readonly int[] machineGuns;
        public readonly int[] cannons;

        public LevelData(int shipWidth, int shipHeight, int[] cockpits, int[] thrusters, int[] armors, int[] machineGuns, int[] cannons) {
            this.shipWidth = shipWidth;
            this.shipHeight = shipHeight;
            this.cockpits = cockpits;
            this.thrusters = thrusters;
            this.armors = armors;
            this.machineGuns = machineGuns;
            this.cannons = cannons;
        }
    }

    private List<LevelData> levelData = new List<LevelData>() {
        //            w, h,   cockpits,      thrusters,     armors,        machine guns,  cannons
        new LevelData(1, 1, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(1, 2, new[] {1,0,0}, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(2, 3, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(3, 3, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(4, 4, new[] {0,1,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(5, 5, new[] {0,1,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
    };

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void StartNextLevel() {

    }
}
