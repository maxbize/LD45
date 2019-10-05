using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipBuilderManager : MonoBehaviour
{
    // Set in editor
    public Transform debugShipPartPrefab;
    public RectTransform partSelectPanel;
    public RectTransform[] partSelectButtons;
    public int maxWidth;
    public int maxHeight;
    // Selected Part
    public TMP_Text SP_title;
    public TMP_Text SP_mark;
    public TMP_Text SP_quantity;
    public Image SP_icon;
    public Button SP_buy;

    private int partScrollIndex;

    // Start is called before the first frame update
    void Start() {
        SetupPartSelectionUI();
        SetupSelectedPartUI(0);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //HandleClick();
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

    }

    private void HandleClick() {
        Vector3 gridPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gridPos = new Vector2(Mathf.Round(gridPos.x), Mathf.Round(gridPos.y));
        Instantiate(debugShipPartPrefab, gridPos, Quaternion.identity);
    }

    public void OnPartScrollButton(bool upArrow) {
        int numParts = ShipPartsManifest.allParts.Count;
        int numSlots = partSelectButtons.Length;
        int newIndex = partScrollIndex + (upArrow ? -1 : 1);
        partScrollIndex = Mathf.Clamp(newIndex, 0, numParts - numSlots);
        SetupPartSelectionUI();
    }

    /*
     *     public TMP_Text SP_title;
    public TMP_Text SP_mark;
    public TMP_Text SP_quantity;
    public Image SP_icon;
    public Button SP_buy;
*/
    public void OnPartSelectButton(int buttonIndex) {
        ShipPartData part = ShipPartsManifest.allParts[partScrollIndex + buttonIndex];
        SP_title.text = part.partName;
        SP_mark.text = "MK " + part.mark;
        SP_quantity.text = "x0"; // TODO!
        //SP_icon = ; // TODO!
        SP_buy.GetComponentInChildren<TMP_Text>().text = "Buy (" + part.cost + "R)";
    }

    public void OnPartBuy() {

    }

}
 