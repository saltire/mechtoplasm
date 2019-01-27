using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeaponScript : WeaponScript {
    LayerMask buildingLayerMask;

    void Start() {
        GetComponent<AudioSource>().Play();

        Vector3 target = transform.position + transform.forward;
        Vector3Int targetCoords = player.GetCoords(target);
        Vector3Int otherPlayerCoords = player.otherPlayer.GetCoords();

        if (otherPlayerCoords == targetCoords) {
            Vector3 otherPlayerTarget = transform.position + transform.forward * 2;
            player.otherPlayer.Move(otherPlayerTarget);
        }

        buildingLayerMask = LayerMask.GetMask("Buildings");

        Collider[] buildings = Physics.OverlapSphere(target, .25f, buildingLayerMask);
        if (buildings.Length > 0) {
            Destroy(buildings[0].gameObject);
        }

        Destroy(gameObject, 1);
    }
}
