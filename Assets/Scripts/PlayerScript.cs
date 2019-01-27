using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float respawnTimer = 0f;
    public float respawnTimerMax = 5f;

    public float sineIndex = 0f;
    public float sineAmplitude = 1.0f;
    public float sineOmega = 10.0f;

    public string playerNumber = "";
    public PlayerScript otherPlayer;

    public OrbScript orbPrefab;

    public UIScript ui;

    Vector3 startingPosition;
    Quaternion startingRotation;
    Vector3 targetPosition;
    Vector3 moveVelocity;

    LayerMask buildingLayerMask;
    LayerMask floorLayerMask;

    GridScript grid;
    ExitScript exit;

    Animator animator;

    void Start() {
        grid = FindObjectOfType<GridScript>();
        exit = FindObjectOfType<ExitScript>();

        Vector3Int coords = GetCoords();
        transform.position = grid.GetSquare(coords.x, coords.z).top;
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        targetPosition = transform.position;

        buildingLayerMask = LayerMask.GetMask("Buildings");

        animator = GetComponentInChildren<Animator>();
        SetAnimatorRotation();
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
            Square square = grid.GetSquare(transform.position);
            float currentSpeed = moveSpeed;
            if (square.surface != null) {
                SlimeSurfaceScript slime = square.surface.GetComponent<SlimeSurfaceScript>();
                if (slime != null) {
                    currentSpeed /= slime.speedMultiplier;
                }
                else {
                    IceSurfaceScript ice = square.surface.GetComponent<IceSurfaceScript>();
                    if (ice != null) {
                        currentSpeed /= ice.speedMultiplier;
                    }
                }
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref moveVelocity, currentSpeed);
        } 
        else if (targetDistance > 0) {
            transform.position = targetPosition;

            moveVelocity = Vector3.zero;

            Square square = grid.GetSquare(transform.position);
            if (square.surface != null) {
                square.surface.OnStep(GetComponent<PlayerScript>());
            }
        } 
        else if (fireCooldownRemaining <= 0 && respawnTimer <= 0) {
            GetInput();
        }

        if (fireCooldownRemaining > 0) {
            fireCooldownRemaining -= Time.deltaTime;
        }

        if (playerHealthCurrent <= 0) {
            PlayerDeath();
        }

        // handle respawn timer and flicker
        if (respawnTimer > 0) {
            respawnTimer -= Time.deltaTime;

            sineIndex += Time.deltaTime;
            float sineFlicker = Mathf.Abs(sineAmplitude * Mathf.Sin(sineOmega * sineIndex));

            Color tmp = GetComponentInChildren<SpriteRenderer>().color;
            tmp.a = sineFlicker;
            GetComponentInChildren<SpriteRenderer>().color = tmp;
        }
        else {
            Color tmp = GetComponentInChildren<SpriteRenderer>().color;
            tmp.a = 1f;
            GetComponentInChildren<SpriteRenderer>().color = tmp;
        }
    }

    void GetInput() {
        float verticalInput = Input.GetAxisRaw(playerNumber + "Vertical");
        float horizontalInput = Input.GetAxisRaw(playerNumber + "Horizontal");

        if (verticalInput >= deadzone && horizontalInput >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            SetAnimatorRotation();
            Move();
        } 
        else if (verticalInput <= -deadzone && horizontalInput >= deadzone) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            SetAnimatorRotation();
            Move();
        } 
        else if (verticalInput <= -deadzone && horizontalInput <= -deadzone) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            SetAnimatorRotation();
            Move();
        } 
        else if (verticalInput >= deadzone && horizontalInput <= -deadzone) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            SetAnimatorRotation();
            Move();
        } 
        else if (Input.GetButtonDown(playerNumber + "Fire0")) {
            Fire(canUseWeapon0 ? 0 : -1);
        } 
        else if (Input.GetButtonDown(playerNumber + "Fire1")) {
            Fire(canUseWeapon1 ? 1 : -1);
        } 
        else if (Input.GetButtonDown(playerNumber + "Fire2")) {
            Fire(canUseWeapon2 ? 2 : -1);
        } 
        else if (Input.GetButtonDown(playerNumber + "Fire3")) {
            Fire(canUseWeapon3 ? 3 : -1);
        }
    }

    void SetAnimatorRotation() {
        animator.transform.parent.rotation = Quaternion.identity;
        animator.SetInteger("facingDirection", (int)Mathf.Round(transform.rotation.eulerAngles.y / 90));
    }

    // collision check with orbs
    private void OnTriggerEnter(Collider other) {
        OrbScript orb = other.GetComponent<OrbScript>();
        if (orb != null) {
            if (orb.weaponIndex == 0) {
                canUseWeapon0 = true;
                exit.UpdateOrb(0, true);
                ui.UpdateOrb(0, true);
            }
            else if (orb.weaponIndex == 1) {
                canUseWeapon1 = true;
                exit.UpdateOrb(1, true);
                ui.UpdateOrb(1, true);
            }
            else if (orb.weaponIndex == 2) {
                canUseWeapon2 = true;
                exit.UpdateOrb(2, true);
                ui.UpdateOrb(2, true);
            }
            else if (orb.weaponIndex == 3) {
                canUseWeapon3 = true;
                exit.UpdateOrb(3, true);
                ui.UpdateOrb(3, true);
            }
            Destroy(other.gameObject);
        }
    }

    public void Move() {
        Move(transform.position + transform.forward);
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

    public void Damage(int damage) {
        if (respawnTimer <= 0) {
            playerHealthCurrent -= damage;
            if (playerHealthCurrent < 0) { playerHealthCurrent = 0; }
        }
    }

    void PlayerDeath() {
        DropOrbs();
        ResetWeapons();
        transform.position = startingPosition;
        targetPosition = transform.position;
        transform.rotation = startingRotation;
        SetAnimatorRotation();
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        respawnTimer = respawnTimerMax;
        playerHealthCurrent = playerHealthMax;
    }

    void DropOrbs() {
        if (canUseWeapon0) {
            OrbScript orb = Instantiate<OrbScript>(orbPrefab, transform.position + new Vector3(Random.Range(0f, 0.5f), 0.8f, Random.Range(0f, 0.5f)), Quaternion.identity);
            orb.weaponIndex = 0;
            orb.GetComponent<MeshRenderer>().material.SetColor("_Color", grid.colors[0]);
            orb.Drop();
        }
        if (canUseWeapon1) {
            OrbScript orb = Instantiate<OrbScript>(orbPrefab, transform.position + new Vector3(Random.Range(0f, 0.5f), 0.8f, Random.Range(0f, 0.5f)), Quaternion.identity);
            orb.weaponIndex = 1;
            orb.GetComponent<MeshRenderer>().material.SetColor("_Color", grid.colors[1]);
            orb.Drop();
        }
        if (canUseWeapon2) {
            OrbScript orb = Instantiate<OrbScript>(orbPrefab, transform.position + new Vector3(Random.Range(0f, 0.5f), 0.8f, Random.Range(0f, 0.5f)), Quaternion.identity);
            orb.weaponIndex = 2;
            orb.GetComponent<MeshRenderer>().material.SetColor("_Color", grid.colors[2]);
            orb.Drop();
        }
        if (canUseWeapon3) {
            OrbScript orb = Instantiate<OrbScript>(orbPrefab, transform.position + new Vector3(Random.Range(0f, 0.5f), 0.8f, Random.Range(0f, 0.5f)), Quaternion.identity);
            orb.weaponIndex = 3;
            orb.GetComponent<MeshRenderer>().material.SetColor("_Color", grid.colors[3]);
            orb.Drop();
        }
    }

    void ResetWeapons() {
        canUseWeapon0 = false;
        canUseWeapon1 = false;
        canUseWeapon2 = false;
        canUseWeapon3 = false;

        ui.UpdateOrb(0, false);
        ui.UpdateOrb(1, false);
        ui.UpdateOrb(2, false);
        ui.UpdateOrb(3, false);
    }
}
