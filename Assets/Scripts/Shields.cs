using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    // Set in editor
    public SpriteRenderer shieldSprite;
    public float radius;
    public int maxHealth;
    public float regenTime;

    public bool active { get; private set; }

    private CircleCollider2D shieldCollider;
    private int health;
    private float timeForRegen;

    // Start is called before the first frame update
    void Start() {
        shieldCollider = GetComponent<CircleCollider2D>();
        shieldCollider.radius = radius;
        shieldSprite.gameObject.transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        Activate();
    }

    // Update is called once per frame
    void Update() {
        if (timeForRegen != 0 && Time.timeSinceLevelLoad > timeForRegen) {
            Activate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Handled in projectile code?
    }

    public void TakeDamage(int amount) {
        Debug.LogFormat("Took {0} damage. Remaining shields: {1}", amount, health);
        health -= amount;
        if (health <= 0) {
            Deactivate();
        }
    }

    private void Activate() {
        health = maxHealth;
        shieldSprite.enabled = true;
        active = true;
        timeForRegen = 0;
    }

    private void Deactivate() {
        shieldSprite.enabled = false;
        active = false;
        timeForRegen = Time.timeSinceLevelLoad + regenTime;
    }
}
