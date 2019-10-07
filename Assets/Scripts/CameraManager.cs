using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Set in editor
    public bool debug;
    public float minSize;
    public float maxSize;
    public float velocityWeight;
    public float secondaryWeight;
    public float maxDistance; // From primary target
    public float screenShakeDecayRate;

    private GameObject primaryTarget;
    private List<GameObject> secondaryTargets;
    private float defaultSize;
    private Camera cam;
    private GameObject debugMarker;
    private float screenShakeAmount;

    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;

        if (debug) {
            debugMarker = new GameObject("Camera target");
        }
    }

    public void TrackTargets(GameObject primary) {
        primaryTarget = primary;
        secondaryTargets = new List<GameObject>();
        secondaryTargets.AddRange(FindObjectsOfType<EnemyShip>().Select(s => s.gameObject));
        screenShakeAmount = 0;
    }

    public void StopTracking() {
        cam.orthographicSize = defaultSize;
        transform.up = Vector2.up;
        primaryTarget = null;
        secondaryTargets = null;
    }

    // Update is called once per frame
    void Update() {
        if (primaryTarget == null) {
            cam.orthographicSize = defaultSize;
            return;
        }

        // Define targets
        // Position
        Vector2 targetPos = primaryTarget.transform.position;
        targetPos += primaryTarget.GetComponent<Rigidbody2D>().velocity * velocityWeight;
        Vector2 secondaryPull = Vector2.zero;
        foreach (GameObject secondaryTarget in secondaryTargets.Where(t => t != null)) {
            secondaryPull += ((Vector2)secondaryTarget.GetComponent<Rigidbody2D>().worldCenterOfMass - targetPos);
        }
        targetPos += secondaryPull * secondaryWeight;
        Vector2 primaryToTargetPos = targetPos - (Vector2)primaryTarget.transform.position;
        if (primaryToTargetPos.magnitude > maxDistance) {
            targetPos = (Vector2)primaryTarget.transform.position + primaryToTargetPos.normalized * maxDistance;
        }

        // Rotation
        Quaternion targetRot = primaryTarget.transform.rotation;

        // Size
        float targetSize = ((Vector2)primaryTarget.transform.position - targetPos).magnitude * 2;
        targetSize = Mathf.Clamp(targetSize, minSize, maxSize);

        // SCREEN SHAKE BABY!!!
        float shakeAmp = Mathf.Pow(Mathf.PerlinNoise(Mathf.Cos(Time.timeSinceLevelLoad), -4f) * screenShakeAmount, 2);
        Vector2 shake = Random.insideUnitCircle.normalized * shakeAmp;
        screenShakeAmount -= screenShakeDecayRate;
        if (screenShakeAmount < 0) {
            screenShakeAmount = 0;
        }

        // Snap to targets
        transform.position = (Vector3)targetPos + Vector3.back * 10 + (Vector3)shake;
        transform.rotation = targetRot;
        cam.orthographicSize = targetSize;
        if (debug) {
            debugMarker.transform.position = targetPos;
        }
    }

    public void AddScreenShake(float amount) {
        screenShakeAmount += amount;
        if (screenShakeAmount > 2.5f) {
            screenShakeAmount = 2.5f;
        }
    }

}
