using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    // Set in editor
    public int thrustForce;


    private ParticleSystem thrustParticles;

    // Start is called before the first frame update
    void Start() {
        thrustParticles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void ActivateThruster() {
        SetThrusters(true);
    }

    public void DisableThruster() {
        SetThrusters(false);
    }

    private void SetThrusters(bool enabled) {
        ParticleSystem.EmissionModule emission = thrustParticles.emission;
        emission.enabled = enabled;
    }
}
