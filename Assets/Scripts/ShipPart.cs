using System.Collections;
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

    }

    // Update is called once per frame
    void Update() {

    }

    public void Initialize() {
        health = maxHealth;
        controller = GetComponentInParent<ShipController>();
    }

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.LogFormat("{0} took {1} damage. {2} health left", gameObject.name, damage, health);
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
