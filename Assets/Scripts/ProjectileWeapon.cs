using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    // Set in editor
    public GameObject projectilePrefab;
    public int damage;
    public float cooldown;

    private float timer;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public bool Attack() {
        if (timer == 0) {
            // Make the first shot random so that every weapon is not in perfect sync
            timer = Time.timeSinceLevelLoad + Random.Range(0, cooldown);
        }
        if (Time.timeSinceLevelLoad > timer) {
            timer = Time.timeSinceLevelLoad + cooldown;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.layer = gameObject.layer;
            return true;
        }
        return false;
    }
}
