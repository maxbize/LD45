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
    public float moveLerpRate;
    public float sizeLerpRate;
    public float rotLerpRate;
    public float velocityWeight;
    public float secondaryWeight;
    public float maxDistance; // From primary target

    private GameObject primaryTarget;
    private List<GameObject> secondaryTargets;
    private float defaultSize;
    private Camera cam;
    private GameObject debugMarker;

    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<Camera>();

        // TODO: Get these from somewhere...
        //primaryTarget = FindObjectOfType<HumanShipInput>();
        secondaryTargets = new List<GameObject>();
        secondaryTargets.AddRange(FindObjectsOfType<EnemyShip>().Select(s => s.gameObject));

        if (debug) {
            debugMarker = new GameObject("Camera target");
        }
    }

    // Update is called once per frame
    void Update() {
        if (primaryTarget == null) {
            return;
        }

        // Define targets
        // Position
        Vector2 targetPos = primaryTarget.transform.position;
        targetPos += primaryTarget.GetComponent<Rigidbody2D>().velocity * velocityWeight;
        Vector2 secondaryPull = Vector2.zero;
        foreach (GameObject secondaryTarget in secondaryTargets) {
            secondaryPull += ((Vector2)secondaryTarget.transform.position - targetPos);
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

        // Lerp to targets
        //transform.position = Vector3.Lerp(transform.position, (Vector3)targetPos + Vector3.back * 10, moveLerpRate * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotLerpRate * Time.deltaTime);
        //cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, sizeLerpRate * Time.deltaTime);
        transform.position = (Vector3)targetPos + Vector3.back * 10;
        transform.rotation = targetRot;
        cam.orthographicSize = targetSize;
        if (debug) {
            debugMarker.transform.position = targetPos;
        }
    }


}
