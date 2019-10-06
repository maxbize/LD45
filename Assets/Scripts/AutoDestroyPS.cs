using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyPS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        float lifetime = 0;
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
            lifetime = Mathf.Max(lifetime, ps.main.startLifetime.constantMax);
            lifetime = Mathf.Max(lifetime, ps.main.duration);
        }
        Invoke("Die", lifetime);
    }

    private void Die() {
        Destroy(gameObject);
    }
}
