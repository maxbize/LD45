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
    public GameObject gameVictoryUI;

    private GameManager gameManager;
    private ShipBuilderManager builder;
    public List<GameObject> levelEnemies { get; private set; } // HACK! Public so we can find enemies to track
    public List<GameObject> playerShip { get; private set; } // HACK! Don't need a list ;) Also don't need public
    private bool wonLevel = false;

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

    // Start is called before the first frame update
    void Start() {
        builder = FindObjectOfType<ShipBuilderManager>();
        gameManager = FindObjectOfType<GameManager>();
        combatUI.SetActive(false);
        levelVictoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (!levelVictoryUI.activeSelf && !gameVictoryUI.activeSelf) {
            if (playerShip != null && playerShip.All(s => s == null)) {
                Debug.Log("Detected loss on level " + currentLevel);
                ActivateLevelDoneScreen("Your failure was expected. Grab some scraps and return to base.");
            } else if (levelEnemies != null && levelEnemies.All(e => e == null)) {
                Debug.Log("Detected win on level " + currentLevel);
                currentLevel++;
                wonLevel = true;
                if (currentLevel == transform.childCount) {
                    gameVictoryUI.SetActive(true);
                    combatUI.SetActive(false);
                    foreach (EnemyShip enemy in levelEnemies.Where(e => e != null).Select(e => e.GetComponent<EnemyShip>())) {
                        enemy.control = false;
                    }
                    foreach (HumanShipInput human in playerShip.Where(e => e != null).Select(e => e.GetComponent<HumanShipInput>())) {
                        human.control = false;
                    }

                } else {
                    ActivateLevelDoneScreen("Well done Captain. Your success is a personal victory.");
                }
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
        foreach (Projectile proj in FindObjectsOfType<Projectile>()) {
            Destroy(proj.gameObject);
        }
        combatUI.SetActive(false);
        levelVictoryUI.SetActive(false);
        gameManager.shipBuilderPanel.SetActive(true); // HACK!
        builder.Initialize(transform.GetChild(currentLevel).GetComponent<LevelSetup>().ToLevelData(), !wonLevel);
        wonLevel = false;
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

    private void ActivateLevelDoneScreen(string text) {
        levelVictoryUI.SetActive(true);
        levelVictoryUI.GetComponentInChildren<TMP_Text>().text = text;
        foreach (EnemyShip enemy in levelEnemies.Where(e => e != null).Select(e => e.GetComponent<EnemyShip>())) {
            enemy.control = false;
        }
        foreach (HumanShipInput human in playerShip.Where(e => e != null).Select(e => e.GetComponent<HumanShipInput>())) {
            human.control = false;
        }
    }

    public void OnGiveUpButton() {
        if (!levelVictoryUI.activeSelf) {
            ActivateLevelDoneScreen("Your lack of courage is unwavering. Return to base for motivational disciplinary action.");
        }
    }

    public void OnContinueButton() {
        StartNextLevelBuilder();
    }
}
