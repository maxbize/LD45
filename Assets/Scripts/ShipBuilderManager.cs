using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuilderManager : MonoBehaviour
{
    // Set in editor
    public Transform gridSlotBackgroundPrefab;
    public Transform gridSlotSelectedObj; // highlights the currently selected slot
    public Transform debugShipPartPrefab;
    public RectTransform partSelectPanel;
    public RectTransform[] partSelectButtons;
    public Button rotatePartButton;
    public Button removePartButton;
    // Selected Part
    public TMP_Text SP_title;
    public TMP_Text SP_mark;
    public TMP_Text SP_quantity;
    public Image SP_icon;
    public Button SP_buy;

    private int partScrollIndex;
    private int width;
    private int height;
    private ShipPart inFlightPart; // The part the user has selected to place but has not yet placed
    private ShipPart[,] gridParts;
    private ShipPartData selectedPart; // Selected on the right, not the left!
    private ShipFactory shipFactory;

    // Start is called before the first frame update
    void Start() {
        shipFactory = FindObjectOfType<ShipFactory>();
        width = 3;
        height = 3; // TODO: Get these from somewhere
        gridParts = new ShipPart[width, height];
        DisableSelectedSlotUI();
        SetupPartSelectionUI();
        SetupSelectedPartUI(0);
        SetupBuilderBackground();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleClick();
        }

        if (inFlightPart != null) {
            TrackPart();
        }
    }

    private void DisableSelectedSlotUI() {
        gridSlotSelectedObj.gameObject.SetActive(false);
        rotatePartButton.interactable = false;
        removePartButton.interactable = false;
    }

    // Bottom screen
    private void SetupPartSelectionUI() {
        for (int i = 0; i < partSelectButtons.Length; i++) {
            ShipPartData part = ShipPartsManifest.allParts[partScrollIndex + i];
            partSelectButtons[i].GetComponentInChildren<TMP_Text>().text = part.partName + " MK" + part.mark;
        }
    }

    // Top screen
    private void SetupSelectedPartUI(int buttonIndex) {
        selectedPart = ShipPartsManifest.allParts[partScrollIndex + buttonIndex];
        SP_title.text = selectedPart.partName;
        SP_mark.text = "MK " + selectedPart.mark;
        //SP_quantity.text = "x0"; // TODO!
        SP_icon.sprite = shipFactory.GetSpriteForPart(selectedPart); // TODO!
        SP_buy.GetComponentInChildren<TMP_Text>().text = "Buy (" + selectedPart.cost + "R)";
    }

    private void TrackPart() {
        Vector3 mousePos = Utils.GetMouseWorldPos();
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width * 0.75f || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height) {
            mousePos = new Vector3(width + 0.5f, height / 2);
        }
        inFlightPart.transform.position = mousePos;
    }

    private void SetupBuilderBackground() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Instantiate(gridSlotBackgroundPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        // Center the camera
        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;
        // 0.5 = pivot of square, 2.2 = offset for UI (HACK)
        camPos.x = width / 2f - 0.5f + 2.2f;
        camPos.y = height / 2f - 0.5f;
        cam.transform.position = camPos;
    }

    private void HandleClick() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(mousePos.x);
        int y = Mathf.RoundToInt(mousePos.y);
        if (x < 0 || x >= width || y < 0 || y >= height) {
            return;
        }

        if (inFlightPart == null) {

        } else {
            RemovePartFromGrid(x, y);
            gridParts[x, y] = inFlightPart;
            inFlightPart.transform.position = new Vector2(x, y);
            inFlightPart = null;
        }
        gridSlotSelectedObj.gameObject.SetActive(true);
        if (gridParts[x, y] != null) {
            removePartButton.interactable = true;
            rotatePartButton.interactable = true;
        }
        gridSlotSelectedObj.transform.position = new Vector2(x, y);
    }

    private void RemovePartFromGrid(int x, int y) {
        if (gridParts[x, y] != null) {
            // TODO: Put back in inventory
            Destroy(gridParts[x, y]);
        }
    }

    public void OnPartScrollButton(bool upArrow) {
        int numParts = ShipPartsManifest.allParts.Count;
        int numSlots = partSelectButtons.Length;
        int newIndex = partScrollIndex + (upArrow ? -1 : 1);
        partScrollIndex = Mathf.Clamp(newIndex, 0, numParts - numSlots);
        SetupPartSelectionUI();
    }

    public void OnPartSelectButton(int buttonIndex) {
        SetupSelectedPartUI(buttonIndex);
    }

    public void OnPartBuyButton() {

    }

    public void OnPartPlaceButton() {
        inFlightPart = shipFactory.CreateShipPart(selectedPart, Utils.GetMouseWorldPos(), Quaternion.identity);
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
        shipFactory.CreateShip(gridParts);
    }
}
 