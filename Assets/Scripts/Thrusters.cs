using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    // Set in editor
    public int thrustForce;
    public ParticleSystem thrustParticles;
    public Transform childSprite;
    public float rotationRate;

    private Quaternion targetAngle;

    // Start is called before the first frame update
    void Start() {
        targetAngle = transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        childSprite.rotation = Quaternion.Lerp(childSprite.rotation, targetAngle, rotationRate);
    }

    public void ActivateThruster(Quaternion targetAngle) {
        this.targetAngle = targetAngle;
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
