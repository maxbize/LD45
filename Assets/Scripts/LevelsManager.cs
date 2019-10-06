using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Mostly just holds data. Does what people tell it to
public class LevelsManager : MonoBehaviour
{
    // Set in editor
    public int currentLevel; // Here for debugging ;)
    public GameObject combatUI;
    public GameObject levelVictoryUI;

    private GameManager gameManager;
    private ShipBuilderManager builder;
    public List<GameObject> levelEnemies { get; set; } // HACK! Public so we can find enemies to track
    private List<GameObject> playerShip; // HACK! Don't need a list ;)

    public class LevelData
    {
        public readonly int shipWidth;
        public readonly int shipHeight;
        public readonly int[] cockpits;
        public readonly int[] thrusters;
        public readonly int[] armors;
        public readonly int[] machineGuns;
        public readonly int[] cannons;
        public readonly int[] shields;
        public readonly int[] missiles;

        public LevelData(int shipWidth, int shipHeight, int[] cockpits, int[] thrusters, int[] armors, int[] machineGuns, int[] cannons, int[] shields, int[] missiles) {
            this.shipWidth = shipWidth;
            this.shipHeight = shipHeight;
            this.cockpits = cockpits;
            this.thrusters = thrusters;
            this.armors = armors;
            this.machineGuns = machineGuns;
            this.cannons = cannons;
            this.shields = shields;
            this.missiles = missiles;
        }
    }

    private List<LevelData> levelData = new List<LevelData>() {
        //            w, h,   cockpits,      thrusters,     armors,        machine guns,  cannons,       shields,       missiles
        //new LevelData(5, 5, new[] {1,0,0}, new[] {9,9,9}, new[] {0,0,0}, new[] {9,0,0}, new[] {9,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(1, 2, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {1,0,0}, new[] {0,0,0}, new[] {1,0,0}, new[] {1,0,0}),
        new LevelData(1, 2, new[] {1,0,0}, new[] {1,0,0}, new[] {0,0,0}, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(2, 3, new[] {1,0,0}, new[] {2,0,0}, new[] {0,0,0}, new[] {2,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(3, 3, new[] {1,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(4, 4, new[] {0,1,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
        new LevelData(5, 5, new[] {0,1,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}, new[] {0,0,0}),
    };

    // Start is called before the first frame update
    void Start() {
        builder = FindObjectOfType<ShipBuilderManager>();
        gameManager = FindObjectOfType<GameManager>();
        combatUI.SetActive(false);
        levelVictoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (!levelVictoryUI.activeSelf) {
            if (levelEnemies != null && levelEnemies.All(e => e == null)) {
                Debug.Log("Detected win on level " + currentLevel);
                currentLevel++;
                levelVictoryUI.SetActive(true);
                levelVictoryUI.GetComponentInChildren<TMP_Text>().text = "Well done Captain. Your success is a personal victory.";
            } else if (playerShip != null && playerShip.All(s => s == null)) {
                Debug.Log("Detected loss on level " + currentLevel);
                levelVictoryUI.GetComponentInChildren<TMP_Text>().text = "Your failure was expected. Grab some scraps and return to base.";
                levelVictoryUI.SetActive(true);
            }
        }
    }

    public void StartNextLevelBuilder() {
        if (levelEnemies != null) {
            foreach (GameObject go in levelEnemies) {
                if (go != null) {
                    Destroy(go);
                }
            }
        }
        levelEnemies = null;
        playerShip = null;
        HumanShipInput player = FindObjectOfType<HumanShipInput>();
        if (player != null) {
            Destroy(player.gameObject);
        }
        combatUI.SetActive(false);
        levelVictoryUI.SetActive(false);
        gameManager.shipBuilderPanel.SetActive(true); // HACK!
        builder.Initialize(levelData[currentLevel]);
    }

    public void StartNextLevelCombat() {
        combatUI.SetActive(true);
        levelEnemies = new List<GameObject>();
        GameObject levelPrefab = transform.GetChild(currentLevel).gameObject;
        GameObject levelClone = Instantiate(levelPrefab);
        foreach (EnemyShip enemy in levelClone.GetComponentsInChildren<EnemyShip>()) {
            enemy.transform.parent = null;
            levelEnemies.Add(enemy.gameObject);
        }
        playerShip = new List<GameObject>();
        playerShip.Add(FindObjectOfType<HumanShipInput>().gameObject);
        Destroy(levelClone);
    }

    public void OnGiveUpButton() {
        if (!levelVictoryUI.activeSelf) {
            levelVictoryUI.SetActive(true);
            levelVictoryUI.GetComponentInChildren<TMP_Text>().text = "Your lack of courage is unwavering. Return to base for motivational seminars.";
        }
    }

    public void OnContinueButton() {
        StartNextLevelBuilder();
    }
}
