using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct Fireball {
    public GameObject obj;
    public Vector3 target;
}

public class PlayerScript : MonoBehaviour {
    public float moveSpeed = .07f;

    public GameObject fireballPrefab;
    public int fireballCount = 3;
    public float fireTime = .5f;
    public float fireHeight = 1;
    Fireball[] fireballs;
    float fireTimeRemaining = 0;

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
        transform.position = grid.GetTop(x, z);
        targetPosition = transform.position;

        buildingLayerMask = LayerMask.GetMask("Buildings");
        floorLayerMask = LayerMask.GetMask("Floors");
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

        if (fireTimeRemaining > 0) {
            fireTimeRemaining -= Time.deltaTime;
            float normalizedFireTime = 1 - (fireTimeRemaining / fireTime);

            if (fireTimeRemaining <= 0) {
                foreach (Fireball fireball in fireballs) {
                    Destroy(fireball.obj);

                    Collider[] buildings = Physics.OverlapSphere(fireball.target, .25f, buildingLayerMask);
                    if (buildings.Length > 0) {
                        Destroy(buildings[0].gameObject);
                    }
                }
            }
            else {
                foreach (Fireball fireball in fireballs) {
                    fireball.obj.transform.position = Vector3.Lerp(transform.position, fireball.target, normalizedFireTime) + new Vector3(0, fireHeight * Mathf.Sin(normalizedFireTime * Mathf.PI), 0);
                }
            }
        }
    }

    void GetInput() {
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
        else if (Input.GetButtonDown("Fire1") && fireTimeRemaining <= 0) {
            Fire();
        }
    }

    void Move() {
        Vector3 target = transform.position + transform.rotation * Vector3.forward;
        Vector3 targetTop = grid.GetTop((int)target.x, (int)target.z);
        Collider[] floors = Physics.OverlapSphere(targetTop, .25f, floorLayerMask);
        Collider[] buildings = Physics.OverlapSphere(targetTop, .25f, buildingLayerMask);

        if (floors.Length == 1 && buildings.Length == 0) {
            targetPosition = targetTop;
        }
    }

    void Fire() {
        fireballs = new Fireball[fireballCount];
        for (int i = 0; i < fireballCount; i++) {
            fireballs[i] = new Fireball() {
                obj = Instantiate(fireballPrefab, transform.position, Quaternion.identity),
                target = transform.position + transform.rotation * Vector3.forward * (i + 1),
            };
        }
        fireTimeRemaining = fireTime;
    }
}
