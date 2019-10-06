using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sends commands to the controller on behalf of the player
public class HumanShipInput : MonoBehaviour
{

    private ShipController shipController;
    private bool up;
    private bool down;
    private bool right;
    private bool left;
    private bool posRotate;
    private bool negRotate;
    private bool attack;

    // Start is called before the first frame update
    void Start() {
        shipController = GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update() {
        up = Input.GetKey(KeyCode.W);
        down = Input.GetKey(KeyCode.S);
        right = Input.GetKey(KeyCode.D);
        left = Input.GetKey(KeyCode.A);
        posRotate = Input.GetKey(KeyCode.Q);
        negRotate = Input.GetKey(KeyCode.E);

        attack = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate() {
        shipController.Move(up, down, right, left, posRotate, negRotate);
        if (attack) {
            shipController.Attack();
        }
    }
}
