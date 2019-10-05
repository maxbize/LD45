using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main control and logic for all ships. Input driven by another class
public class ShipController : MonoBehaviour
{
    // Set in editor


    private ShipPart[,] parts;
    private List<ShipPart> forwardThrusters = new List<ShipPart>();
    private List<ShipPart> backThrusters = new List<ShipPart>();
    private List<ShipPart> positiveTorqueThrusters = new List<ShipPart>(); // Thrusters used in right turn - can be on both sides
    private List<ShipPart> negativeTorqueThrusters = new List<ShipPart>(); // Thrusters used in left turn - can be on both sides
    private List<ShipPart> thrusters = new List<ShipPart>();
    private List<ShipPart> weapons = new List<ShipPart>();
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
        rb = GetComponent<Rigidbody2D>(); // Might get called before Start

        foreach (ShipPart part in parts) {
            if (part == null) {
                continue;
            }

            part.transform.parent = transform;

            // Organize thrusters into categories
            if (part.data.type == ShipPartData.Type.Thruster) {
                thrusters.Add(part);
                float partZ = part.transform.rotation.eulerAngles.z;
                int dir = Mathf.RoundToInt(partZ / 90);
                if (dir == 0) {
                    forwardThrusters.Add(part);
                } else if (dir == 2 || dir == -2) {
                    backThrusters.Add(part);
                } else { 
                    if (Vector3.Cross(part.transform.up, (Vector2)part.transform.position - rb.worldCenterOfMass).z > 0) {
                        negativeTorqueThrusters.Add(part);
                    } else {
                        positiveTorqueThrusters.Add(part);
                    }
                }
            } else if (part.data.type == ShipPartData.Type.MachineGun) {
                weapons.Add(part);
            }

            // Set density. Every part has the same volume, so just use mass directly
            part.GetComponent<Collider2D>().density = part.data.mass;

            // Tag to the same layer as the parent
            part.gameObject.layer = gameObject.layer;
        }
    }

    // Each direction can be on simultaneously because they activate different thrusters
    public void Move(bool up, bool down, bool right, bool left) {
        List<ShipPart> activeThrusters = new List<ShipPart>();

        if (up) {
            activeThrusters.AddRange(forwardThrusters);
        }

        if (down) {
            activeThrusters.AddRange(backThrusters);
        }

        if (right) {
            activeThrusters.AddRange(negativeTorqueThrusters);
        }

        if (left) {
            activeThrusters.AddRange(positiveTorqueThrusters);
        }

        foreach (ShipPart thruster in thrusters) {
            if (activeThrusters.Contains(thruster)) {
                thruster.ActivateThruster();
                rb.AddForceAtPosition(thruster.transform.up * thruster.data.GetThrustForce(), thruster.transform.position);
            } else {
                thruster.DisableThruster();
            }
        }
    }

    public void Attack() {
        foreach (ShipPart weapon in weapons) {
            weapon.Attack();
        }
    }
}
