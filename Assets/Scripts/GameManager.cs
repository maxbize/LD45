using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles game state transitions
public class GameManager : MonoBehaviour
{
    // Set in editor
    public GameObject mainMenuPanel;
    public GameObject shipBuilderPanel;

    private LevelsManager levelsManager;

    // Start is called before the first frame update
    void Start() {
        levelsManager = FindObjectOfType<LevelsManager>();
        //mainMenuPanel.SetActive(true); // Re-enable before shipping :)
        shipBuilderPanel.SetActive(false);
        if (!mainMenuPanel.activeSelf) {
            Invoke("OnNewGameButton", 0); // Auto-press if we've hidden main menu to enter the game faster. Using Invoke to let other scripts Start
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnNewGameButton() {
        mainMenuPanel.SetActive(false);
        shipBuilderPanel.SetActive(true);
        levelsManager.StartNextLevelBuilder();
    }
}
