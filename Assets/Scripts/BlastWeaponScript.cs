using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWeaponScript : WeaponScript {
    public int damage = 1;

    void Start() {
        Blast(transform.forward);
        Blast(-transform.forward);
        Blast(transform.right);
        Blast(-transform.right);

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
