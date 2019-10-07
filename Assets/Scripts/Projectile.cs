using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Set in editor
    public float speed;
    public int damage;
    public float trackingForce;
    public float trackingRot;
    public GameObject impactPS;

    private bool alreadyDamaged = false; // Sometimes we hit two things. Only count one of them
    private Rigidbody2D rb;
    private Transform target;
    private Transform originator;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        Invoke("Die", 10);
    }

    void FixedUpdate() {
        if (target != null) {
            Vector2 toTarget = (target.transform.position - transform.position).normalized;
            rb.AddForce(toTarget * trackingForce);
            transform.up = rb.velocity.normalized;
            //rb.velocity = Vector2.Lerp(rb.velocity.normalized, toTarget, trackingRot) * rb.velocity.magnitude;
        }
    }

    public void TrackTarget(Transform target, Transform spawner) {
        this.target = target;
        this.originator = spawner;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        ShipPart shipPart = collider.GetComponent<ShipPart>();
        Projectile projectile = collider.GetComponent<Projectile>();
        Shields shields = collider.GetComponent<Shields>();
        bool hitShields = collider is CircleCollider2D && shields != null;
        if (hitShields) {
            if (shields.active && !alreadyDamaged) {
                SpawnImpact(collider);
                shields.TakeDamage(damage);
                Vector2 toProjectile = (transform.position - shields.transform.position).normalized;
                rb.velocity = toProjectile * rb.velocity.magnitude;
                transform.up = toProjectile;
                gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(gameObject.layer) == "Friendly" ? "Hostile" : "Friendly");
                if (target != null) {
                    Transform tmp = target;
                    target = originator;
                    originator = tmp;
                }
                return;
            }
        }
        if (shipPart != null) {
            if (!alreadyDamaged && !hitShields) {
                shipPart.TakeDamage(damage);
                alreadyDamaged = true;
                Die(collider);
            }
        } else if (projectile != null) {
            if (target != null) {
                Die(collider); // Missiles get killed by projectiles
            }
        } else {
            Debug.Log("I don't know what this projectile just hit: " + collider.gameObject);
        }
    }

    private void SpawnImpact(Collider2D collider) {
        if (collider == null) {
            Instantiate(impactPS, transform.position, transform.rotation);
        } else {
            Vector3 pos = collider.ClosestPoint(transform.position);
            Quaternion rot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 180 + Random.Range(-30, 30));
            Instantiate(impactPS, pos, rot);
        }
    }

    private void Die(Collider2D collider) {
        SpawnImpact(collider);
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        if (systems.Length > 0) {
            foreach (ParticleSystem system in systems) {
                ParticleSystem.EmissionModule em = system.emission;
                em.enabled = false;
            }
            gameObject.AddComponent<AutoDestroyPS>();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity *= -1;
            enabled = false;
        } else {
            Destroy(gameObject);
        }
    }

    private void Die() {
        Die(null);
    }
}
