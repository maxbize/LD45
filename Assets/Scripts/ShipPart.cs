using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All the behavior powering the ship parts
public class ShipPart : MonoBehaviour
{
    // Set in editor

    public ShipPartData data; // HACK! Set from factory. GameObject, so no constructor :(

    private ParticleSystem childParticles;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

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
}
