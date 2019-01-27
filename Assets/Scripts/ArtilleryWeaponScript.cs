using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArtilleryWeaponScript : WeaponScript {
    public GameObject projectilePrefab;
    public float fireTime = .8f;
    public float fireOriginHeight = .6f;
    public float fireArcHeight = 6;
    public int fireLength = 3;

    public float rotateXSpeed = .3f;
    public float rotateYSpeed = .2f;

    public SlimeSurfaceScript slimeSurfacePrefab;

    GameObject projectile;
    Vector3 origin;
    Vector3 target;
    float fireTimeRemaining = 0;
    public int damage = 1;

    GridScript grid;
    MeshRenderer projectileMesh;
    LayerMask buildingLayerMask;

    void Start() {
        grid = FindObjectOfType<GridScript>();

        projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        origin = transform.position + new Vector3(0, fireOriginHeight, 0);
        target = transform.position + transform.forward * fireLength;
        if (grid.SquareExists(target.x, target.z)) {
            target = grid.GetSquare(target.x, target.z).top;
        }

        projectileMesh = projectile.GetComponentInChildren<MeshRenderer>();

        fireTimeRemaining = fireTime;

        buildingLayerMask = LayerMask.GetMask("Buildings");
    }

    void Update() {
        fireTimeRemaining -= Time.deltaTime;

        if (fireTimeRemaining > 0) {
            float normalizedFireTime = 1 - (fireTimeRemaining / fireTime);
            
            projectile.transform.position = Vector3.Lerp(origin, target, normalizedFireTime) + 
                new Vector3(0, fireArcHeight * Mathf.Sin(normalizedFireTime * Mathf.PI), 0);

            projectileMesh.transform.Rotate(new Vector3(Time.deltaTime / rotateXSpeed * 360, Time.deltaTime / rotateYSpeed * 360, 0));
        }
        else {
            projectile.GetComponent<ParticleSystem>().Play();
            Destroy(projectileMesh.gameObject);
            Destroy(projectile, 1);

            Vector3Int targetCoords = player.GetCoords(target);
            
            // 3x3 area
            for (int x = targetCoords.x - 1; x <= targetCoords.x + 1; x++) {
                for (int z = targetCoords.z - 1; z <= targetCoords.z + 1; z++) {
                    if (grid.SquareExists(x, z)) {
                        grid.SetSquareSurface(x, z, slimeSurfacePrefab);
                    }
                }
            }

            Collider[] buildings = Physics.OverlapSphere(target, .25f, buildingLayerMask);
            if (buildings.Length > 0) {
                Destroy(buildings[0].gameObject);
            }

            Vector3Int otherPlayerCoords = player.otherPlayer.GetCoords();

            if (otherPlayerCoords == targetCoords) {
                player.otherPlayer.Damage(damage);
            }

            Destroy(gameObject);
        }
    }
}
