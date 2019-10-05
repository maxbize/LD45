using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuilderManager : MonoBehaviour
{
    // Set in editor
    public Transform debugShipPartPrefab;
    public int maxWidth;
    public int maxHeight;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandleClick();
        }
    }

    private void HandleClick() {
        Vector3 gridPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gridPos = new Vector2(Mathf.Round(gridPos.x), Mathf.Round(gridPos.y));
        Instantiate(debugShipPartPrefab, gridPos, Quaternion.identity);
    }
}
