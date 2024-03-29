﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    // Set in editor
    // Serialized ordinals of all prefabs needed to assemble this ship.
    // Sorted top left to bottom right
    public string serialized;
    public MoveMode moveMode;
    public AttackMode attackMode;
    public float attackPeriod; // On / off this period

    private float periodAttackOnTime;
    private float periodAttackOffTime;

    public enum MoveMode
    {
        Static,
        AlwaysForward,
        AlwaysRotate,
        FacePlayer,
        SeekPlayer,
        Dogfight // I hope I have time to implement this...
    }

    public enum AttackMode
    {
        Never,
        Always,
        Periodic,
        WhenPlayerInRange
    }

    [HideInInspector]
    public bool control;

    private ShipController shipController;
    private Transform playerShip;
    private Transform cockpit;

    // Start is called before the first frame update
    void Start() { 
        control = true;
        shipController = GetComponent<ShipController>();
        BuildShip();
        shipController.RegisterParts(GetComponentsInChildren<ShipPart>().ToList());
        playerShip = FindObjectOfType<HumanShipInput>().gameObject.transform;
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        if (!control || playerShip == null) {
            shipController.Move(Vector2.zero, 0);
            return;
        }

        // Handle movement
        if (moveMode == MoveMode.Static) {

        } else if (moveMode == MoveMode.AlwaysForward) {
            Debug.LogWarning("Deprecated");
        } else if (moveMode == MoveMode.AlwaysRotate) {
            shipController.Move(Vector2.zero, 1);
        } else if (moveMode == MoveMode.FacePlayer) {
            RotateTowardsPlayer(Vector2.zero);
        } else if (moveMode == MoveMode.SeekPlayer) {
            RotateTowardsPlayer(playerShip.transform.position - transform.position);
        }

        // Handle attack
        if (attackMode == AttackMode.Never) {

        } else if (attackMode == AttackMode.Always) {
            shipController.Attack();
        } else if (attackMode == AttackMode.Periodic) {
            if (Time.timeSinceLevelLoad > periodAttackOffTime) {
                periodAttackOnTime = Time.timeSinceLevelLoad + attackPeriod;
                periodAttackOffTime = periodAttackOnTime + attackPeriod;
            }
            if (Time.timeSinceLevelLoad > periodAttackOnTime) {
                shipController.Attack();
            }
        }
    }

    private void RotateTowardsPlayer(Vector2 strafe) {
        Vector2 toPlayer = (playerShip.position - cockpit.position).normalized;
        shipController.Move(strafe, Vector3.Cross(toPlayer, cockpit.up).z < 0 ? -1 : 1);
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
                y = 0;
                x++;
                continue;
            }
            if (partName != "null") {
                Quaternion partRotation = Quaternion.Euler(0, 0, int.Parse(split[i + 1]));
                GameObject partObj = Instantiate(partMap[partName], transform.position + new Vector3(x, y, 0), partRotation, transform);
                if (partObj.GetComponent<ShipPart>().partName == "Cockpit") {
                    cockpit = partObj.transform;
                }
            }
            y++;
        }
        transform.rotation = cachedRot;
    }
}
