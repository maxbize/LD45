using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main control and logic for all ships. Input driven by another class
public class ShipController : MonoBehaviour
{
    // Set in editor


    private ShipPart[,] parts;
    private List<ShipPart> forwardThrusters;
    private List<ShipPart> backThrusters;
    private List<ShipPart> rightThrusters; // Thrusters used in right turn - can be on both sides
    private List<ShipPart> leftThrusters; // Thrusters used in left turn - can be on both sides
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void RegisterParts(ShipPart[,] parts) {
        this.parts = parts;

        foreach (ShipPart part in parts) {
            if (part == null) {
                continue;
            }

            part.transform.parent = transform;

            // Organize thrusters into categories
            if (part.data.type == ShipPartData.Type.Thruster) {
                float partZ = part.transform.rotation.eulerAngles.z;
                float shipZ = transform.rotation.eulerAngles.z;
            }
        }
    }

    // Each direction can be on simultaneously because they activate different thrusters
    public void Move(bool up, bool down, bool right, bool left) {

        if (up) {
            foreach (ShipPart thruster in parts) {
                if (thruster == null || !(thruster.data.type == ShipPartData.Type.Thruster)) {
                    continue;
                }
                rb.AddForceAtPosition(thruster.transform.up * thruster.data.GetThrustForce(), thruster.transform.position);
            }
        }
    }
}
