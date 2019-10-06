using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    // Set in editor
    // Serialized ordinals of all prefabs needed to assemble this ship.
    // Sorted top left to bottom right
    public string serialized;

    // Start is called before the first frame update
    void Start() {
        BuildShip();
    }

    // Update is called once per frame
    void Update() {

    }

    private void BuildShip() {
        if (serialized == null) {
            Debug.LogError("Enemy ship instance but no state to build from for: " + name);
        }
        Quaternion cachedRot = transform.rotation;
        transform.rotation = Quaternion.identity;
        ShipFactory sf = FindObjectOfType<ShipFactory>();
        Dictionary<string, GameObject> partMap = sf.partPrefabs.ToDictionary(p => p.name, p => p);
        int x = 0;
        int y = 0;
        string[] split = serialized.Split(',');
        for (int i = 0; i < split.Length; i += 2) {
            string partName = split[i];
            if (partName == "next") {
                x = 0;
                y++;
                continue;
            }
            if (partName != "null") {
                Quaternion partRotation = Quaternion.Euler(0, 0, int.Parse(split[i + 1]));
                Instantiate(partMap[partName], transform.position + new Vector3(x, y, 0), partRotation, transform);
            }
            x++;
        }
        transform.rotation = cachedRot;
    }
}
