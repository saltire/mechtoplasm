using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour {
    public float moveSpeed = .07f;

    public GameObject weaponPrefab;
    public float fireCooldown = .5f;
    float fireCooldownRemaining = 0;
    
    public float deadzone = 0.5f;

    int x;
    int z;

    Vector3 targetPosition;
    Vector3 moveVelocity;

    LayerMask buildingLayerMask;
    LayerMask floorLayerMask;

    GridScript grid;

    void Start() {
        grid = FindObjectOfType<GridScript>();

        ResetXZ();
        transform.position = grid.GetSquare(x, z).top;
        targetPosition = transform.position;

        buildingLayerMask = LayerMask.GetMask("Buildings");
    }

    void ResetXZ() {
        x = (int)transform.position.x;
        z = (int)transform.position.z;
    }

    void Update() {
        float targetDistance = Vector3.Distance(targetPosition, transform.position);
        if (targetDistance > .01f) {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref moveVelocity, moveSpeed);
            ResetXZ();
        }
        else if (targetDistance > 0) {
            transform.position = targetPosition;
            ResetXZ();

            moveVelocity = Vector3.zero;
        }
        else {
            GetInput();
        }

        if (fireCooldownRemaining > 0) {
            fireCooldownRemaining -= Time.deltaTime;
        }
    }

    void GetInput() {
        //Util.Log(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetAxisRaw("Vertical") >= deadzone && Input.GetAxisRaw("Horizontal") >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            Move();
        }
        else if (Input.GetAxisRaw("Vertical") <= -deadzone && Input.GetAxisRaw("Horizontal") >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            Move();
        }
        else if (Input.GetAxisRaw("Vertical") <= -deadzone && Input.GetAxisRaw("Horizontal") <= -deadzone) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            Move();
        }
        else if (Input.GetAxisRaw("Vertical") >= deadzone && Input.GetAxisRaw("Horizontal") <= deadzone) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Move();
        }
        else if (Input.GetButtonDown("Fire1") && fireCooldownRemaining <= 0) {
            Fire();
        }
    }

    void Move() {
        Vector3 target = transform.position + transform.rotation * Vector3.forward;
        if (grid.SquareExists((int)target.x, (int)target.z)) {
            Square square = grid.GetSquare((int)target.x, (int)target.z);
            Collider[] buildings = Physics.OverlapSphere(square.top, .25f, buildingLayerMask);

            if (buildings.Length == 0) {
                targetPosition = square.top;
            }
        }
    }

    void Fire() {
        Instantiate(weaponPrefab, transform.position, transform.rotation);
    }
}
