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
    public Transform debugShipPartPrefab;
    public RectTransform partSelectPanel;
    public RectTransform[] partSelectButtons;
    // Selected Part
    public TMP_Text SP_title;
    public TMP_Text SP_mark;
    public TMP_Text SP_quantity;
    public Image SP_icon;
    public Button SP_buy;

    private int partScrollIndex;
    private int width;
    private int height;
    private Transform inFlightPart; // The part the user has selected to place but has not yet placed

    // Start is called before the first frame update
    void Start() {
        width = 3;
        height = 3; // TODO: Get these from somewhere
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

    // Bottom screen
    private void SetupPartSelectionUI() {
        for (int i = 0; i < partSelectButtons.Length; i++) {
            ShipPartData part = ShipPartsManifest.allParts[partScrollIndex + i];
            partSelectButtons[i].GetComponentInChildren<TMP_Text>().text = part.partName + " MK" + part.mark;
        }
    }

    // Top screen
    private void SetupSelectedPartUI(int buttonIndex) {
        ShipPartData part = ShipPartsManifest.allParts[partScrollIndex + buttonIndex];
        SP_title.text = part.partName;
        SP_mark.text = "MK " + part.mark;
        //SP_quantity.text = "x0"; // TODO!
        //SP_icon = ; // TODO!
        SP_buy.GetComponentInChildren<TMP_Text>().text = "Buy (" + part.cost + "R)";
    }

    private void TrackPart() {
        Vector3 mousePos = Utils.GetMouseWorldPos();
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width * 0.75f || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height) {
            mousePos = new Vector3(width + 1, height / 2);
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
            inFlightPart.transform.position = new Vector2(x, y);
            inFlightPart = null;
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
        inFlightPart = Instantiate(debugShipPartPrefab, Utils.GetMouseWorldPos(), Quaternion.identity);
    }
}
 