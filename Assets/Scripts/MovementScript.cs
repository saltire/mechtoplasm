using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {
    public float moveSpeed = .5f;

    Vector3 targetPosition;
    Vector3 moveVelocity;

    void Start() {
        targetPosition = transform.position;
    }

    void Update() {
        float targetDistance = Vector3.Distance(targetPosition, transform.position);
        if (targetDistance > .01f) {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref moveVelocity, moveSpeed);
        }
        else if (targetDistance > 0) {
            transform.position = targetPosition;
            moveVelocity = Vector3.zero;
        }
        else {
            if (Input.GetAxisRaw("Vertical") == 1) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Move();
            }
            else if (Input.GetAxisRaw("Vertical") == -1) {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                Move();
            }
            else if (Input.GetAxisRaw("Horizontal") == 1) {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                Move();
            }
            else if (Input.GetAxisRaw("Horizontal") == -1) {
                transform.rotation = Quaternion.Euler(0, 270, 0);
                Move();
            }
        }
    }

    void Move() {
        targetPosition = transform.position + transform.rotation * Vector3.forward;
    }
}
