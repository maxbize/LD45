using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the behavior powering the ship parts
public class ShipPart : MonoBehaviour
{
    // Set in editor
    public GameObject destroyedParticles;
    public string partName;
    public int maxHealth;
    public int mark; // mk1, mk2, etc
    public int mass;

    private float timer;
    private GameObject cachedChildPrefab;
    private int health;
    private ParticleSystem damagedPS;

    // Start is called before the first frame update
    void Start() {
        health = maxHealth;
        damagedPS = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Instantiate(destroyedParticles, transform.position, transform.rotation);
            GetComponentInParent<ShipController>().NotifyPartDestroyed(this);
            Destroy(gameObject);
        }
        ParticleSystem.EmissionModule emission = damagedPS.emission;
        emission.rateOverTime = Mathf.Lerp(0, 50, 1f - (float)health / maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider is CircleCollider2D) {
            return; // No physical damage against shields
        }
        ShipPart otherPart = collision.collider.GetComponent<ShipPart>();
        int damage = (int)collision.relativeVelocity.magnitude * 50;
        TakeDamage(damage);
    }
}
