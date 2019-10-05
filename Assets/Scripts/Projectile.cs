using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Set in editor
    public float speed;
    public int damage;

    private bool alreadyDamaged = false; // Sometimes we hit two things. Only count one of them

    // Start is called before the first frame update
    void Start() {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collider) {
        ShipPart shipPart = collider.GetComponent<ShipPart>();
        Projectile projectile = collider.GetComponent<Projectile>();
        if (shipPart != null) {
            if (!alreadyDamaged) {
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
}
