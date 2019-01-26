using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {
    bool horizDown = false;
    bool vertDown = false;

    void Start() {
        
    }

    void Update() {
        if (vertDown && Input.GetAxisRaw("Vertical") == 0) {
            vertDown = false;
        }
        else if (!vertDown && Input.GetAxisRaw("Vertical") == 1) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            vertDown = true;
            Move();
        }
        else if (!vertDown && Input.GetAxisRaw("Vertical") == -1) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            vertDown = true;
            Move();
        }

        if (horizDown && Input.GetAxisRaw("Horizontal") == 0) {
            horizDown = false;
        }
        else if (!horizDown && Input.GetAxisRaw("Horizontal") == 1) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            horizDown = true;
            Move();
        }
        else if (!horizDown && Input.GetAxisRaw("Horizontal") == -1) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            horizDown = true;
            Move();
        }
    }

    void Move() {
        transform.position += transform.rotation * Vector3.forward;
    }
}
