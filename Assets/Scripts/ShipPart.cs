﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the behavior powering the ship parts
public class ShipPart : MonoBehaviour
{
    // Set in editor
    public string partName;
    public int maxHealth;
    public int mark; // mk1, mk2, etc
    public int mass;

    private ParticleSystem childParticles;
    private float timer;
    private GameObject cachedChildPrefab;
    private int health;
    private ShipController controller;

    // Start is called before the first frame update
    void Start() {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update() {

    }

    public void Initialize() {
        controller = GetComponentInParent<ShipController>();
    }

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.LogFormat("{0} took {1} damage. {2} health left", gameObject.name, damage, health);
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ShipPart otherPart = collision.collider.GetComponent<ShipPart>();
        int damage = (int)collision.relativeVelocity.magnitude * 10;
        TakeDamage(damage);
    }
}
