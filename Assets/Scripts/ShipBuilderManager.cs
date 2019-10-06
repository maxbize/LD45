using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ShipBuilderManager : MonoBehaviour
{
    // Set in editor
    public Transform builderUI;
    public Transform nothingUI;
    public Transform gridSlotBackgroundPrefab;
    public Transform gridSlotSelectedObj; // highlights the currently selected slot
    public RectTransform partSelectPanel;
    public RectTransform[] partSelectButtons;
    public Button rotatePartButton;
    public Button removePartButton;
    // Selected Part
    public TMP_Text SP_title;
    public TMP_Text SP_mark;
    public TMP_Text SP_place;
    public Image SP_icon;

    private int partScrollIndex;
    private ShipPart inFlightPart; // The part the user has selected to place but has not yet placed
    private ShipPart[,] gridParts;
    private ShipPart selectedPart; // Selected on the right, not the left!
    private ShipFactory shipFactory;
    private List<GameObject> backgroundSprites = new List<GameObject>();
    private LevelsManager.LevelData levelData;
    private Dictionary<string, int> inventory; // [partName + mark -> num parts]. Not a dict to force an order
    private LevelsManager levelManager;

    // Start is called before the first frame update
    void Start() {
        shipFactory = FindObjectOfType<ShipFactory>();
        levelManager = FindObjectOfType<LevelsManager>();
    }

    public void Initialize(LevelsManager.LevelData levelData) {
        this.levelData = levelData;
        partScrollIndex = 0;
        gridParts = new ShipPart[levelData.shipWidth, levelData.shipHeight];
        nothingUI.gameObject.SetActive(false);
        CreateInventory();
        DisableSelectedSlotUI();
        SetupPartSelectionUI();
        SetupSelectedPartUI(0);
        SetupBuilderBackground();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && builderUI.gameObject.activeSelf) {
            HandleClick();
        }

        if (inFlightPart != null) {
            TrackPart();
        }
    }

    private void CreateInventory() {
        inventory = new Dictionary<string, int>();
        AddPartsToInventory("Cockpit", levelData.cockpits);
        AddPartsToInventory("Thrusters", levelData.thrusters);
        AddPartsToInventory("Armors", levelData.armors);
        AddPartsToInventory("Machine Gun", levelData.machineGuns);
        AddPartsToInventory("Cannon", levelData.cannons);
    }

    private void AddPartsToInventory(string partName, int[] partsArray) {
        if (partsArray[0] > 0) {
            inventory[partName + "1"] = partsArray[0];
        }
        if (partsArray[1] > 0) {
            inventory[partName + "2"] = partsArray[1];
        }
        if (partsArray[2] > 0) {
            inventory[partName + "3"] = partsArray[2];
        }
    }

    private void DisableSelectedSlotUI() {
        gridSlotSelectedObj.gameObject.SetActive(false);
        rotatePartButton.interactable = false;
        removePartButton.interactable = false;
    }

    // Bottom screen
    private void SetupPartSelectionUI() {
        List<string> invKeys = inventory.Keys.ToList();
        invKeys.Sort();
        nothingUI.gameObject.SetActive(invKeys.Count == 0);
        for (int i = 0; i < partSelectButtons.Length; i++) {
            if (i >= invKeys.Count) {
                partSelectButtons[i].gameObject.SetActive(false);
            } else {
                partSelectButtons[i].gameObject.SetActive(true);
                ShipPart part = shipFactory.GetPrefabByNameAndMark(invKeys[i + partScrollIndex]);
                partSelectButtons[i].GetComponentInChildren<TMP_Text>().text = part.partName + " MK" + part.mark;
            }
        }
    }

    // Top screen
    private void SetupSelectedPartUI(int buttonIndex) {
        List<string> invKeys = inventory.Keys.ToList();
        invKeys.Sort();
        if (invKeys.Count == 0) {
            return;
        }
        selectedPart = shipFactory.GetPrefabByNameAndMark(invKeys[partScrollIndex + buttonIndex]).GetComponent<ShipPart>();
        SP_title.text = selectedPart.partName;
        SP_mark.text = "MK " + selectedPart.mark;
        SP_place.text = "Place (" + inventory[invKeys[partScrollIndex + buttonIndex]] + ")";
        SP_icon.sprite = selectedPart.GetComponent<SpriteRenderer>().sprite;
    }

    private void TrackPart() {
        Vector3 mousePos = Utils.GetMouseWorldPos();
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width * 0.75f || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height) {
            mousePos = new Vector3(levelData.shipWidth + 0.5f, levelData.shipHeight / 2);
        }
        inFlightPart.transform.position = mousePos;
    }

    private void SetupBuilderBackground() {
        for (int x = 0; x < levelData.shipWidth; x++) {
            for (int y = 0; y < levelData.shipHeight; y++) {
                GameObject slotBackground = Instantiate(gridSlotBackgroundPrefab, new Vector3(x, y, 0), Quaternion.identity).gameObject;
                backgroundSprites.Add(slotBackground);
            }
        }

        // Center the camera
        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;
        // 0.5 = pivot of square, 2.2 = offset for UI (HACK)
        camPos.x = levelData.shipWidth / 2f - 0.5f + 2.2f;
        camPos.y = levelData.shipHeight / 2f - 0.5f;
        cam.transform.position = camPos;
    }

    private void HandleClick() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(mousePos.x);
        int y = Mathf.RoundToInt(mousePos.y);
        if (x < 0 || x >= levelData.shipWidth || y < 0 || y >= levelData.shipHeight) {
            return;
        }

        if (inFlightPart == null) {

        } else {
            RemovePartFromGrid(x, y);
            gridParts[x, y] = inFlightPart;
            inFlightPart.transform.position = new Vector2(x, y);
            string key = inFlightPart.partName + inFlightPart.mark;
            inventory[key]--;
            if (inventory[key] == 0) {
                inventory.Remove(key);
                partScrollIndex = Mathf.Min(partScrollIndex, inventory.Count - partSelectButtons.Length);
                if (partScrollIndex < 0) {
                    partScrollIndex = 0;
                }
                SetupPartSelectionUI();
                SetupSelectedPartUI(0);
            } else {
                SP_place.text = "Place (" + inventory[key] + ")";
            }
            inFlightPart = null;
        }
        if (gridParts[x, y] != null) {
            gridSlotSelectedObj.gameObject.SetActive(true);
            removePartButton.interactable = true;
            rotatePartButton.interactable = true;
            gridSlotSelectedObj.transform.position = new Vector2(x, y);
        }
    }

    private void RemovePartFromGrid(int x, int y) {
        if (gridParts[x, y] != null) {
            // TODO: Put back in inventory
            string key = GetInventoryKey(gridParts[x, y]);
            if (!inventory.ContainsKey(key)) {
                inventory[key] = 1;
                SetupPartSelectionUI();
                if (inventory.Count == 1) {
                    SetupSelectedPartUI(0);
                }
            } else {
                inventory[key]++;
            }
            Destroy(gridParts[x, y].gameObject);
        }
    }

    private string GetInventoryKey(ShipPart part) {
        return part.partName + part.mark;
    }

    public void OnPartScrollButton(bool upArrow) {
        int numParts = inventory.Count;
        int numSlots = partSelectButtons.Length;
        int newIndex = partScrollIndex + (upArrow ? -1 : 1);
        partScrollIndex = Mathf.Clamp(newIndex, 0, numParts - numSlots);
        SetupPartSelectionUI();
    }

    public void OnPartSelectButton(int buttonIndex) {
        SetupSelectedPartUI(buttonIndex);
    }

    public void OnPartPlaceButton() {
        if (inFlightPart != null) {
            Destroy(inFlightPart.gameObject);
        }
        inFlightPart = Instantiate(selectedPart, Utils.GetMouseWorldPos(), Quaternion.identity);
        DisableSelectedSlotUI();
    }

    public void OnPartRemoveButton() {
        int x = Mathf.RoundToInt(gridSlotSelectedObj.position.x);
        int y = Mathf.RoundToInt(gridSlotSelectedObj.position.y);
        RemovePartFromGrid(x, y);
        DisableSelectedSlotUI();
    }

    public void OnPartRotateButton() {
        int x = Mathf.RoundToInt(gridSlotSelectedObj.position.x);
        int y = Mathf.RoundToInt(gridSlotSelectedObj.position.y);
        if (gridParts[x, y] != null) {
            gridParts[x, y].transform.rotation = Quaternion.Euler(0, 0, gridParts[x, y].transform.rotation.eulerAngles.z + 90);
        }
    }

    public void OnPlayButton() {
        LogShipSerialized();
        if (inFlightPart != null) {
            Destroy(inFlightPart.gameObject);
        }
        shipFactory.CreateShip(gridParts);
        builderUI.gameObject.SetActive(false);
        foreach (GameObject go in backgroundSprites) {
            Destroy(go);
        }
        backgroundSprites.Clear();
        gridSlotSelectedObj.gameObject.SetActive(false);
        levelManager.StartNextLevelCombat();
    }

    private void LogShipSerialized() {
        StringBuilder sb = new StringBuilder();
        for (int x = 0; x < levelData.shipWidth; x++) {
            for (int y = 0; y < levelData.shipHeight; y++) {
                ShipPart part = gridParts[x, y];
                if (part == null) {
                    sb.Append("null,0,");
                } else {
                    string partName = part.name.Substring(0, part.name.Length - "(Clone)".Length);
                    sb.AppendFormat("{0},{1},", partName, Mathf.RoundToInt(part.transform.rotation.eulerAngles.z));
                }
            }
            sb.Append("next,next,");
        }
        sb.Length -= ",next,next,".Length; // Trim off the last bits
        Debug.Log(sb.ToString());
    }
}
 