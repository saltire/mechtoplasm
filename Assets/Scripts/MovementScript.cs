using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct Fireball {
    public GameObject obj;
    public Vector3 target;
}

public class MovementScript : MonoBehaviour {
    public float moveSpeed = .5f;

    public GameObject fireballPrefab;
    public int fireballCount = 3;
    public float fireTime = .5f;
    public float fireHeight = .3f;
    Fireball[] fireballs;
    float fireTimeRemaining = 0;

    Vector3 targetPosition;
    Vector3 moveVelocity;

    LayerMask buildingLayerMask;
    LayerMask floorLayerMask;

    GridScript grid;

    void Start() {
        grid = FindObjectOfType<GridScript>();

        transform.position = grid.GetTop((int)transform.position.x, (int)transform.position.z) + new Vector3(0, .6f, 0);
        targetPosition = transform.position;

        buildingLayerMask = LayerMask.GetMask("Buildings");
        floorLayerMask = LayerMask.GetMask("Floors");
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
            GetInput();
        }

        if (fireTimeRemaining > 0) {
            fireTimeRemaining -= Time.deltaTime;
            float normalizedFireTime = 1 - (fireTimeRemaining / fireTime);

            if (fireTimeRemaining <= 0) {
                foreach (Fireball fireball in fireballs) {
                    Destroy(fireball.obj);

                    Collider[] buildings = Physics.OverlapSphere(fireball.target, .5f, buildingLayerMask);
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
            targetPosition = targetTop + new Vector3(0, .6f, 0);
        }
    }

    void Fire() {
        fireballs = new Fireball[fireballCount];
        for (int i = 0; i < fireballCount; i++) {
            fireballs[i] = new Fireball() {
                obj = Instantiate(fireballPrefab, transform.position, Quaternion.identity),
                target = transform.position - new Vector3(0, .6f, 0) + transform.rotation * Vector3.forward * (i + 1),
            };
        }
        fireTimeRemaining = fireTime;
    }
}
