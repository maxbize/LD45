using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates the actual Ship GameObjects
public class ShipFactory : MonoBehaviour
{
    // Set in editor
    public GameObject shipPartPrefab;
    public GameObject playerShipPrefab;

    private Dictionary<string, Sprite> shipSprites;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public ShipPart CreateShipPart(ShipPartData data, Vector3 pos, Quaternion rot) {
        GameObject partObj = Instantiate(shipPartPrefab, pos, rot);
        partObj.GetComponent<ShipPart>().data = data;
        partObj.GetComponent<SpriteRenderer>().sprite = GetSpriteForPart(data);
        Debug.Log(GetSpriteForPart(data));
        return partObj.GetComponent<ShipPart>();
    }

    public GameObject CreateShip(ShipPart[,] parts) {
        // Find the center to position the ship object
        Vector3 center = Vector3.zero;
        int numParts = 0;
        foreach (ShipPart part in parts) {
            if (part != null) {
                center += part.transform.position;
                numParts++;
                
                // Load any extra prefabs the part requires
                if (part.data.childPrefabName != null) {
                    GameObject prefab = (GameObject)Resources.Load("Prefabs/" + part.data.childPrefabName);
                    Instantiate(prefab, part.transform);
                }
            }
        }
        center /= numParts;


        // Make the ship object itself
        GameObject ship = Instantiate(playerShipPrefab, center, Quaternion.identity);
        ship.GetComponent<ShipController>().RegisterParts(parts);
        return ship;
    }

    public Sprite GetSpriteForPart(ShipPartData data) {
        if (shipSprites == null) {
            shipSprites = Resources.LoadAll<Sprite>("Sprites/Ships").ToDictionary(s => s.name, s => s);
        }
        return shipSprites[data.spriteName];
    }
}
