using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour {
    public int weaponIndex;

    public float dropSpeed = .05f;
    public Vector3 dropVector = new Vector3(0, -.8f, 0);
    bool dropTriggered = false;
    Vector3 dropVelocity;
    Vector3 dropTarget;

    public void Drop() {
        dropTriggered = true;
        dropTarget = transform.position + dropVector;
    }

    void Update() {
        if (dropTriggered && transform.position != dropTarget) {
            transform.position = Vector3.SmoothDamp(transform.position, dropTarget, ref dropVelocity, dropSpeed);
        }
    }
}
