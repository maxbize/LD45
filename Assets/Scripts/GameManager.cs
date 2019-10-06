using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Set in editor
    public GameObject mainMenuPanel;
    public GameObject shipBuilderPanel;

    // Start is called before the first frame update
    void Start() {
        shipBuilderPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnNewGameButton() {
        mainMenuPanel.SetActive(false);
        shipBuilderPanel.SetActive(true);
    }
}
