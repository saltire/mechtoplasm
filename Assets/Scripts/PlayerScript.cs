using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour {
    public float moveSpeed = .07f;

    public GameObject weaponPrefab;
    public float fireCooldown = .5f;
    float fireCooldownRemaining = 0;
    
    public float deadzone = 0.5f;

    public string playerNumber = "";
    public PlayerScript otherPlayer;

    Vector3 targetPosition;
    Vector3 moveVelocity;

    LayerMask buildingLayerMask;
    LayerMask floorLayerMask;

    GridScript grid;

    void Start() {
        grid = FindObjectOfType<GridScript>();

        Vector3Int coords = GetCoords();
        transform.position = grid.GetSquare(coords.x, coords.z).top;
        targetPosition = transform.position;

        buildingLayerMask = LayerMask.GetMask("Buildings");
    }

    public Vector3Int GetCoords() {
        return GetCoords(transform.position);
    }

    Vector3Int GetCoords(Vector3 pos) {
        return new Vector3Int((int)pos.x, 0, (int)pos.z);
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
        else if (fireCooldownRemaining <= 0) {
            GetInput();
        }

        if (fireCooldownRemaining > 0) {
            fireCooldownRemaining -= Time.deltaTime;
        }
    }

    void GetInput() {
        if (Input.GetAxisRaw(playerNumber + "Vertical") >= deadzone && Input.GetAxisRaw(playerNumber + "Horizontal") >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            Move();
        }
        else if (Input.GetAxisRaw(playerNumber + "Vertical") <= -deadzone && Input.GetAxisRaw(playerNumber + "Horizontal") >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            Move();
        }
        else if (Input.GetAxisRaw(playerNumber + "Vertical") <= -deadzone && Input.GetAxisRaw(playerNumber + "Horizontal") <= -deadzone) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            Move();
        }
        else if (Input.GetAxisRaw(playerNumber + "Vertical") >= deadzone && Input.GetAxisRaw(playerNumber + "Horizontal") <= -deadzone) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Move();
        }
        else if (Input.GetButtonDown(playerNumber + "Fire1")) {
            Fire();
        }
    }

    void Move() {
        Vector3 target = transform.position + transform.rotation * Vector3.forward;
        Vector3Int targetCoords = GetCoords(target);
        Vector3Int otherPlayerCoords = otherPlayer.GetCoords();

        if (grid.SquareExists(targetCoords.x, targetCoords.z) && otherPlayerCoords != targetCoords) {
            Square square = grid.GetSquare(targetCoords.x, targetCoords.z);
            Collider[] buildings = Physics.OverlapSphere(square.top, .25f, buildingLayerMask);

            if (buildings.Length == 0) {
                targetPosition = square.top;
            }
        }
    }

    void Fire() {
        fireCooldownRemaining = fireCooldown;
        Instantiate(weaponPrefab, transform.position, transform.rotation);
    }
}
