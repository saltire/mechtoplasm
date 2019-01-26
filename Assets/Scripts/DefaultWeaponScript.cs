using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeaponScript : WeaponScript {
    void Start() {
        Vector3 target = transform.position + transform.rotation * Vector3.forward;
        Vector3Int targetCoords = player.GetCoords(target);
        Vector3Int otherPlayerCoords = player.otherPlayer.GetCoords();

        if (otherPlayerCoords == targetCoords) {
            Vector3 otherPlayerTarget = transform.position + transform.rotation * Vector3.forward * 2;
            player.otherPlayer.Move(otherPlayerTarget);
        }
        
        Destroy(gameObject);
    }
}
