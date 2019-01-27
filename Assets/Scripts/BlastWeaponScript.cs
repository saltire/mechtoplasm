using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWeaponScript : WeaponScript {
    public int damage = 1;

    public IceSurfaceScript iceSurfacePrefab;

    void Start() {
        Blast(transform.forward);
        Blast(-transform.forward);
        Blast(transform.right);
        Blast(-transform.right);
        
        Vector3Int coords = player.GetCoords();
        GridScript grid = GameObject.FindObjectOfType<GridScript>();

        // 3x3 area, minus center
        for (int x = coords.x - 1; x <= coords.x + 1; x++) {
            for (int z = coords.z - 1; z <= coords.z + 1; z++) {
                if ((x != coords.x || z != coords.z) && grid.SquareExists(x, z)) {
                    grid.SetSquareSurface(x, z, iceSurfacePrefab);
                }
            }
        }

        Destroy(gameObject, 1);
    }

    void Blast(Vector3 direction) {
        Vector3 target = transform.position + direction;
        Vector3Int targetCoords = player.GetCoords(target);
        Vector3Int otherPlayerCoords = player.otherPlayer.GetCoords();

        if (otherPlayerCoords == targetCoords) {
            Vector3 otherPlayerTarget = transform.position + direction * 2;
            player.otherPlayer.Move(otherPlayerTarget);
            player.otherPlayer.Damage(damage);
        }

        LayerMask buildingLayerMask = LayerMask.GetMask("Buildings");

        Collider[] buildings = Physics.OverlapSphere(target, .25f, buildingLayerMask);
        if (buildings.Length > 0) {
            Destroy(buildings[0].gameObject);
        }
    }
}
