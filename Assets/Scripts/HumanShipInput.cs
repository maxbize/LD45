using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sends commands to the controller on behalf of the player
public class HumanShipInput : MonoBehaviour
{

    [HideInInspector]
    public bool control;

    private ShipController shipController;
    private Vector2 strafe;
    private int rotate;
    private bool attack;

    // Start is called before the first frame update
    void Start() {
        control = true;
        shipController = GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update() {

        if (control) {
            strafe = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            rotate = 0;
            if (Input.GetKey(KeyCode.Q)) {
                rotate--;
            }
            if (Input.GetKey(KeyCode.E)) {
                rotate++;
            }
        } else {
            strafe = Vector2.zero;
            rotate = 0;
            attack = false;
        }

        attack = Input.GetKey(KeyCode.Space) && control;
    }

    private void FixedUpdate() {
        shipController.Move(strafe, rotate);
        if (attack) {
            shipController.Attack();
        }
    }
}
