using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the behavior powering the ship parts
public class ShipPart : MonoBehaviour
{
    // Set in editor

    public ShipPartData data; // HACK! Set from factory. GameObject, so no constructor :(

    private ParticleSystem childParticles;
    private float timer;
    private GameObject cachedChildPrefab;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Initialize() {
        if (data.type == ShipPartData.Type.Thruster) {
            GameObject prefab = (GameObject)Resources.Load("Prefabs/" + data.GetExhaustPrefab());
            Instantiate(prefab, transform);
        } else if (data.type == ShipPartData.Type.MachineGun) {
            cachedChildPrefab = (GameObject)Resources.Load("Prefabs/" + data.GetProjectilePrefab());
        }
    }

    private void CheckType(ShipPartData.Type type) {
        if (data.type != type) {
            Debug.LogWarningFormat("Requested operation on type {0} but part is type {1}", type, data.type);
        }
    }

    public void ActivateThruster() {
        CheckType(ShipPartData.Type.Thruster);
        SetThrusters(true);
    }

    public void DisableThruster() {
        CheckType(ShipPartData.Type.Thruster);
        SetThrusters(false);
    }

    private void SetThrusters(bool enabled) {
        if (childParticles == null) {
            childParticles = GetComponentInChildren<ParticleSystem>();
        }
        ParticleSystem.EmissionModule emission = childParticles.emission;
        emission.enabled = enabled;
    }

    public void Attack() {
        CheckType(ShipPartData.Type.MachineGun);
        if (timer == 0) {
            // Make the first shot random so that every weapon is not in perfect sync
            timer = Time.timeSinceLevelLoad + Random.Range(0, data.GetWeaponCooldown());
        }
        if (Time.timeSinceLevelLoad > timer) {
            timer = Time.timeSinceLevelLoad + data.GetWeaponCooldown();
            GameObject projectile = Instantiate(cachedChildPrefab, transform.position, transform.rotation);
            projectile.layer = gameObject.layer;
        }
    }
}
