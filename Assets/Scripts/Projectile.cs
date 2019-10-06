﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Set in editor
    public float speed;
    public int damage;
    public float trackingForce;
    public float trackingRot;

    private bool alreadyDamaged = false; // Sometimes we hit two things. Only count one of them
    private Rigidbody2D rb;
    private Transform target;

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

    public void TrackTarget(Transform target) {
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        ShipPart shipPart = collider.GetComponent<ShipPart>();
        Projectile projectile = collider.GetComponent<Projectile>();
        Shields shields = collider.GetComponent<Shields>();
        bool hitShields = collider is CircleCollider2D && shields != null;
        if (hitShields) {
            if (shields.active && !alreadyDamaged) {
                shields.TakeDamage(damage);
                Vector2 toProjectile = (transform.position - shields.transform.position).normalized;
                rb.velocity = toProjectile * rb.velocity.magnitude;
                transform.up = toProjectile;
                gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(gameObject.layer) == "Friendly" ? "Hostile" : "Friendly");
                return;
            }
        }
        if (shipPart != null) {
            if (!alreadyDamaged && !hitShields) {
                Debug.LogFormat("Dealing {0} damage to {1}", damage, shipPart);
                shipPart.TakeDamage(damage);
                alreadyDamaged = true;
                Destroy(gameObject);
            }
        } else if (projectile != null) {
            // Ignore for now
        } else {
            Debug.Log("I don't know what this projectile just hit: " + collider.gameObject);
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}
