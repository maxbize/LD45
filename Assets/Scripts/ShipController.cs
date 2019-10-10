using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main control and logic for all ships. Input driven by another class
public class ShipController : MonoBehaviour
{
    // Set in editor
    // Add some drags and frictions to make control easier
    public float notRotatingRotationalDrag;
    public float rotatingRotationalDrag; // Drag amounts for when we are / aren't trying to rotate
    public float correctionDrag; // Apply a drag to nullify velocity not in our forward direction

    private List<ShipPart> parts;
    private List<Thrusters> thrusters = new List<Thrusters>();
    private List<ProjectileWeapon> weapons = new List<ProjectileWeapon>();
    private Rigidbody2D rb;
    private CameraManager cameraManager;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        cameraManager = FindObjectOfType<CameraManager>();
        /*if (parts == null) { // Happens in debug mode
            List<ShipPart> asList = new List<ShipPart>();
            foreach (ShipPart part in GetComponentsInChildren<ShipPart>()) {
                asList.Add(part);
            }
            RegisterParts(asList);
        }*/
    }

    // Update is called once per frame
    void Update() {

    }

    public void RegisterParts(ShipPart[,] parts) {

        List<ShipPart> asList = new List<ShipPart>();
        foreach (ShipPart part in parts) {
            asList.Add(part);
        }
        RegisterParts(asList);
    }

    public void RegisterParts(List<ShipPart> parts) {
        Debug.Log(string.Join(", ", parts.Where(p => p != null).Select(p => p.name)));
        this.parts = parts.Distinct().ToList(); // Some bug with dupes somewhere... HACK!
        rb = GetComponent<Rigidbody2D>(); // Might get called before Start

        foreach (ShipPart part in parts) {
            if (part == null) {
                continue;
            }

            part.transform.parent = transform;

            // Organize thrusters into categories
            Thrusters partThrusters = part.GetComponent<Thrusters>();
            if (partThrusters != null) {
                thrusters.Add(partThrusters);
            } else if (part.GetComponent<ProjectileWeapon>() != null) {
                weapons.Add(part.GetComponent<ProjectileWeapon>());
            }

            // Set density. Every part has the same volume, so just use mass directly
            part.GetComponent<Collider2D>().density = part.mass;

            // Tag to the same layer as the parent
            part.gameObject.layer = gameObject.layer;
        }
    }

    // Strafe and rotation independent. Input will be normalized. Rotate should be negative, zero, or positive
    // Strafe is given priority over rotate? Can we do both?
    public void Move(Vector2 strafe, int rotate) {
        strafe.Normalize();
        rb.angularDrag = rotate == 0 ? notRotatingRotationalDrag : rotatingRotationalDrag;

        Vector3 cm = rb.worldCenterOfMass;
        foreach (Thrusters thruster in thrusters) {
            if (strafe.magnitude > 0 && rotate != 0) {
                Quaternion strafeRot = Quaternion.LookRotation(Vector3.forward, strafe);

                Vector3 toCm = cm - thruster.transform.position;
                Vector3 orthoCm = Vector3.Cross(toCm, rotate > 0 ? Vector3.back : Vector3.forward);
                Quaternion rotateRot = Quaternion.LookRotation(Vector3.forward, orthoCm);

                thruster.ActivateThruster(Quaternion.Lerp(strafeRot, rotateRot, 0.5f));
                rb.AddForceAtPosition(thruster.childSprite.transform.up * thruster.thrustForce, thruster.transform.position);
            } else if (strafe.magnitude > 0) {
                thruster.ActivateThruster(Quaternion.LookRotation(Vector3.forward, strafe));
                rb.AddForceAtPosition(thruster.childSprite.transform.up * thruster.thrustForce, thruster.transform.position);
            } else if (rotate != 0) {
                Vector3 toCm = cm - thruster.transform.position;
                Vector3 orthoCm = Vector3.Cross(toCm, rotate > 0 ? Vector3.back : Vector3.forward);
                thruster.ActivateThruster(Quaternion.LookRotation(Vector3.forward, orthoCm));
                rb.AddForceAtPosition(thruster.childSprite.transform.up * thruster.thrustForce, thruster.transform.position);
            } else {
                thruster.DisableThruster();
            }
        }

        Vector2 correctionForce = -Vector2.Dot(transform.right, rb.velocity) * transform.right * correctionDrag;
        rb.AddForce(correctionForce);
    }

    public void Attack() {
        bool attacked = false;
        foreach (ProjectileWeapon weapon in weapons) {
            attacked |= weapon.Attack();
        }
        if (attacked) {
            cameraManager.AddScreenShake(0.2f);
        }
    }

    public void NotifyPartDestroyed(ShipPart part) {
        parts.Remove(part);
        parts = parts.Where(p => p != null).ToList(); // HACK! Don't know why I need this. Single cockpit throws errors otherwise
        cameraManager.AddScreenShake(1.5f);

        Thrusters thruster = part.GetComponent<Thrusters>();
        if (thruster != null) {
            thrusters.Remove(thruster);
        }

        ProjectileWeapon weapon = part.GetComponent<ProjectileWeapon>();
        if (weapon != null) {
            weapons.Remove(weapon);
        }

        if (part.partName == "Cockpit") {
            // Inverse for loop because each death will modify parts with the callback
            Debug.LogFormat("Killing remaining {0} parts", parts.Count);
            for (int i = parts.Count - 1; i >=0; i--) {
                // TODO: It would be really cool if we could show each part blowing up in succession
                parts[i].TakeDamage(parts[i].maxHealth);
            }
            Destroy(gameObject);
        }
    }
}
