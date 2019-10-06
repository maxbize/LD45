using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    // Set in editor
    public GameObject projectilePrefab;
    public int damage;
    public float cooldown;
    public bool tracking;

    private float timer;
    private Transform target;
    private LevelsManager levelsManager;

    // Start is called before the first frame update
    void Start() {
        levelsManager = FindObjectOfType<LevelsManager>();
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
            GameObject projectile = Instantiate(projectilePrefab, transform.position, tracking ? Quaternion.Euler(0, 0, Random.Range(0, 360)) : transform.rotation);
            projectile.layer = gameObject.layer;
            if (tracking) {
                if (target == null && levelsManager.levelEnemies != null) {
                    target = PickTarget();
                }
                projectile.GetComponent<Projectile>().TrackTarget(target);
            }
            return true;
        }
        return false;
    }

    private Transform PickTarget() {
        GameObject closest = levelsManager.levelEnemies[0];
        foreach (GameObject enemy in levelsManager.levelEnemies) {
            float distToEnemy = Vector2.Distance(enemy.transform.position, transform.position);
            float distToClosest = Vector2.Distance(closest.transform.position, transform.position);
            if (distToEnemy < distToClosest) {
                closest = enemy;
            }
        }

        foreach (ShipPart part in closest.GetComponentsInChildren<ShipPart>()) {
            if (part.partName == "Cockpit") {
                return part.transform;
            }
        }
        return closest.transform;
    }
}
