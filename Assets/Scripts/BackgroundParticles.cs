using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{
    private HumanShipInput target;
    private float nextCheck;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (target == null && Time.timeSinceLevelLoad > nextCheck) {
            target = FindObjectOfType<HumanShipInput>();
            nextCheck = Time.timeSinceLevelLoad + 0.5f;
        }

        if (target != null) {
            transform.position = target.transform.position;
        }
    }
}
