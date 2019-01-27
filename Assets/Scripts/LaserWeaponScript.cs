using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeaponScript : WeaponScript {
    public float fireTime = .5f;
    public float fireOriginHeight = .6f;

    public float initialDiameter = .2f;

    float fireTimeRemaining = 0;

    public int damage = 1;

    Transform cylinder;
    Transform particles;

    void Start() {
        fireTimeRemaining = fireTime;

        player.Move(transform.position - transform.forward);

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
        
        cylinder = transform.Find("Cylinder");
        particles = transform.Find("Particles");
    }

    void Update() {
        fireTimeRemaining -= Time.deltaTime;

        float diameter = initialDiameter * fireTimeRemaining / fireTime;
        cylinder.localScale = new Vector3(diameter, cylinder.localScale.y, diameter);

        particles.position -= transform.forward * (Time.deltaTime / fireTime);

        if (fireTimeRemaining <= 0) {
            Destroy(gameObject);
        }
    }
}
