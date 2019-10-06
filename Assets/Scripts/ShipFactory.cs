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
    public GameObject[] partPrefabs;

    private Dictionary<string, ShipPart> nameMarksToParts; // name + mark -> part

    // Start is called before the first frame update
    void Start() {
        nameMarksToParts = partPrefabs
            .Select(go => go.GetComponent<ShipPart>())
            .ToDictionary(p => p.partName + p.mark, p => p);
    }

    // Update is called once per frame
    void Update() {

    }

    public GameObject CreateShip(ShipPart[,] parts) {
        // Find the center to position the ship object
        Vector3 center = Vector3.zero;
        int numParts = 0;
        foreach (ShipPart part in parts) {
            if (part != null) {
                part.Initialize();

                center += part.transform.position;
                numParts++;
            }
        }
        center /= numParts;


        // Make the ship object itself
        GameObject ship = Instantiate(playerShipPrefab, center, Quaternion.identity);
        ship.GetComponent<ShipController>().RegisterParts(parts);
        return ship;
    }

    public ShipPart GetPrefabByNameAndMark(string nameAndMark) {
        if (!nameMarksToParts.ContainsKey(nameAndMark)) {
            Debug.LogErrorFormat("Requested {0} but no prefab", nameAndMark);
        }
        return nameMarksToParts[nameAndMark];
    }
}
