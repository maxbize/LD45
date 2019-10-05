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
    private bool attack;

    // Start is called before the first frame update
    void Start() {
        shipController = GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update() {
        up = Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.DownArrow);
        right = Input.GetKey(KeyCode.RightArrow);
        left = Input.GetKey(KeyCode.LeftArrow);
        attack = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate() {
        shipController.Move(up, down, right, left);
        if (attack) {
            shipController.Attack();
        }
    }
}
