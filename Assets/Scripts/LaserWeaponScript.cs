using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeaponScript : WeaponScript {
    public float fireTime = .5f;
    public float fireOriginHeight = .6f;

    float fireTimeRemaining = 0;

    public int damage = 1;

    void Start() {
        fireTimeRemaining = fireTime;

        GridScript grid = FindObjectOfType<GridScript>();

        int length = 0;
        while (grid.SquareExists(transform.position.x + transform.forward.x * (length + 1), transform.position.z + transform.forward.z * (length + 1))) {
            length += 1;
        }

        transform.position += new Vector3(0, fireOriginHeight, 0);
        transform.localScale = new Vector3(1, 1, (length + .5f));

        LayerMask buildingLayerMask = LayerMask.GetMask("Buildings", "Players");

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, length + .5f, buildingLayerMask);
        foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Buildings")) {
                Destroy(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject == player.otherPlayer.gameObject) {
                player.otherPlayer.Damage(damage);
            }
        }
    }

    void Update() {
        fireTimeRemaining -= Time.deltaTime;

        if (fireTimeRemaining <= 0) {
            Destroy(gameObject);
        }
    }
}
