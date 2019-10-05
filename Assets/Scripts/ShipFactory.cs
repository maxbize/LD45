using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates the actual Ship GameObjects
public class ShipFactory : MonoBehaviour
{
    // Set in editor
    public GameObject shipPartPrefab;

    private Dictionary<string, Sprite> shipSprites;

    // Start is called before the first frame update
    void Start() {
        shipSprites = Resources.LoadAll<Sprite>("Sprites/Ships").ToDictionary(s => s.name, s => s);
    }

    // Update is called once per frame
    void Update() {

    }

    public GameObject CreateShipPart(ShipPartData data, Vector3 pos, Quaternion rot) {
        GameObject partObj = Instantiate(shipPartPrefab, pos, rot);
        partObj.GetComponent<ShipPart>().data = data;
        partObj.GetComponent<SpriteRenderer>().sprite = GetSpriteForPart(data);
        Debug.Log(GetSpriteForPart(data));
        return partObj;
    }

    public Sprite GetSpriteForPart(ShipPartData data) {
        return shipSprites[data.spriteName];
    }
}
