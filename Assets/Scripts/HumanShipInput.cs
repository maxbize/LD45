using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sends commands to the controller on behalf of the player
public class HumanShipInput : MonoBehaviour
{

    [HideInInspector]
    public bool control;

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
        control = true;
        shipController = GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update() {
        up = Input.GetKey(KeyCode.W) && control;
        down = Input.GetKey(KeyCode.S) && control;
        right = Input.GetKey(KeyCode.D) && control;
        left = Input.GetKey(KeyCode.A) && control;
        posRotate = Input.GetKey(KeyCode.Q) && control;
        negRotate = Input.GetKey(KeyCode.E) && control;

        attack = Input.GetKey(KeyCode.Space) && control;
    }

    private void FixedUpdate() {
        shipController.Move(up, down, right, left, posRotate, negRotate);
        if (attack) {
            shipController.Attack();
        }
    }
}
