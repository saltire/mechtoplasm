using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    public float moveSpeed = .07f;

    public GameObject weaponPrefab;
    public GameObject[] weaponArray;
    public bool canUseWeapon0 = false;
    public bool canUseWeapon1 = false;
    public bool canUseWeapon2 = false;
    public bool canUseWeapon3 = false;
    public float fireCooldown = .5f;
    float fireCooldownRemaining = 0;

    public float deadzone = 0.5f;

    public int playerHealthMax;
    public int playerHealthCurrent;

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

    public Vector3Int GetTargetCoords() {
        return GetCoords(targetPosition);
    }

    public Vector3Int GetCoords(Vector3 pos) {
        return new Vector3Int(Mathf.FloorToInt(pos.x), 0, Mathf.FloorToInt(pos.z));
    }

    void Update() {
        float targetDistance = Vector3.Distance(targetPosition, transform.position);
        if (targetDistance > .01f) {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref moveVelocity, moveSpeed);
        } else if (targetDistance > 0) {
            transform.position = targetPosition;

            moveVelocity = Vector3.zero;

            Vector3Int targetCoords = GetCoords();
            Square square = grid.GetSquare(targetCoords.x, targetCoords.z);
            if (square.surface != null) {
                square.surface.OnStep(GetComponent<PlayerScript>());
            }
        } else if (fireCooldownRemaining <= 0) {
            GetInput();
        }

        if (fireCooldownRemaining > 0) {
            fireCooldownRemaining -= Time.deltaTime;
        }

        if (playerHealthCurrent <= 0) {
            PlayerDeath();
        }

        if (Input.GetKeyDown("r")) {
            SceneManager.LoadScene("Scene");
        }
    }

    void GetInput() {
        float verticalInput = Input.GetAxisRaw(playerNumber + "Vertical");
        float horizontalInput = Input.GetAxisRaw(playerNumber + "Horizontal");

        if (verticalInput >= deadzone && horizontalInput >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            Move();
        } else if (verticalInput <= -deadzone && horizontalInput >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            Move();
        } else if (verticalInput <= -deadzone && horizontalInput <= -deadzone) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            Move();
        } else if (verticalInput >= deadzone && horizontalInput <= -deadzone) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Move();
        } else if (Input.GetButtonDown(playerNumber + "Fire0")) {
            if (canUseWeapon0) {
                Fire(0);
            } else {
                Fire(-1);
            }
        } else if (Input.GetButtonDown(playerNumber + "Fire1")) {
            if (canUseWeapon1) {
                Fire(1);
            } else {
                Fire(-1);
            }
        } else if (Input.GetButtonDown(playerNumber + "Fire2")) {
            if (canUseWeapon2) {
                Fire(2);
            } else {
                Fire(-1);
            }
        } else if (Input.GetButtonDown(playerNumber + "Fire3")) {
            if (canUseWeapon3) {
                Fire(3);
            } else {
                Fire(-1);
            }
        }
    }

    void Move() {
        Move(transform.position + transform.forward);
    }

    // collision check with orbs
    private void OnTriggerEnter(Collider other) {
        OrbScript orb = other.GetComponent<OrbScript>();
        if (orb != null) {
            if (orb.weaponIndex == 0) {
                canUseWeapon0 = true;
            }
            else if (orb.weaponIndex == 1) {
                canUseWeapon1 = true;
            }
            else if (orb.weaponIndex == 2) {
                canUseWeapon2 = true;
            }
            else if (orb.weaponIndex == 3) {
                canUseWeapon3 = true;
            }
            Destroy(other.gameObject);
        }
    }

    public void Move(Vector3 target) {
        Vector3Int targetCoords = GetCoords(target);
        Vector3Int otherPlayerCoords = otherPlayer.GetCoords();
        Vector3Int otherPlayerTarget = otherPlayer.GetTargetCoords();

        if (grid.SquareExists(targetCoords.x, targetCoords.z) && otherPlayerCoords != targetCoords && otherPlayerTarget != targetCoords) {
            Square square = grid.GetSquare(targetCoords.x, targetCoords.z);
            Collider[] buildings = Physics.OverlapSphere(square.top, .25f, buildingLayerMask);

            if (buildings.Length == 0) {
                targetPosition = square.top;
            }
        }
    }

    void Fire(int weaponNumber) {
        fireCooldownRemaining = fireCooldown;
        if (weaponNumber != -1) {
            GameObject weapon = Instantiate(weaponArray[weaponNumber], transform.position, transform.rotation);
            weapon.GetComponent<WeaponScript>().player = gameObject.GetComponent<PlayerScript>();
        }
        else {
            GameObject weapon = Instantiate(weaponPrefab, transform.position, transform.rotation);
            weapon.GetComponent<WeaponScript>().player = gameObject.GetComponent<PlayerScript>();
        }
    }

    void PlayerDeath() {
        ResetWeapons();// lose weapons
        // drop orbs at player position
        // move player to starting position
        // player enters flashing state and can't move until time allows
        playerHealthCurrent = playerHealthMax;// reset health
    }

    void ResetWeapons() {
        canUseWeapon0 = false;
        canUseWeapon1 = false;
        canUseWeapon2 = false;
        canUseWeapon3 = false;
    }
}
