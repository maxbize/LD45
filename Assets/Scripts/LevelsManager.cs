using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mostly just holds data. Does what people tell it to
public class LevelsManager : MonoBehaviour
{
    // Set in editor
    public int currentLevel; // Here for debugging ;)

    private GameManager gameManager;
    private ShipBuilderManager builder;
    private List<GameObject> levelEnemies;

    public class LevelData
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
        new LevelData(1, 1, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {1,0,0}, new[] {0,0,0}),
        new LevelData(1, 2, new[] {1,0,0}, new[] {1,0,0}, new[] {0,0,0}, new[] {1,0,0}, new[] {0,0,0}),
        new LevelData(2, 3, new[] {1,0,0}, new[] {2,0,0}, new[] {0,0,0}, new[] {2,0,0}, new[] {0,0,0}),
        new LevelData(3, 3, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(4, 4, new[] {0,1,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(5, 5, new[] {0,1,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
    };

    // Start is called before the first frame update
    void Start() {
        builder = FindObjectOfType<ShipBuilderManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (levelEnemies != null && levelEnemies.All(e => e == null)) {
            currentLevel++;
            StartNextLevelBuilder();
            levelEnemies = null;
            gameManager.shipBuilderPanel.SetActive(true); // HACK!
            Destroy(FindObjectOfType<HumanShipInput>().gameObject);
        }
    }

    public void StartNextLevelBuilder() {
        builder.Initialize(levelData[currentLevel]);
    }

    public void StartNextLevelCombat() {
        levelEnemies = new List<GameObject>();
        GameObject levelPrefab = transform.GetChild(currentLevel).gameObject;
        GameObject levelClone = Instantiate(levelPrefab);
        foreach (EnemyShip enemy in levelClone.GetComponentsInChildren<EnemyShip>()) {
            enemy.transform.parent = null;
            levelEnemies.Add(enemy.gameObject);
        }
        Destroy(levelClone);
    }
}
