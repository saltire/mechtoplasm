﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct Fireball {
    public GameObject obj;
    public Vector3 origin;
    public Vector3 target;
}

public class FireballWeaponScript : MonoBehaviour {
    public GameObject fireballPrefab;
    public int fireballCount = 3;
    public float fireTime = .5f;
    public float fireOriginHeight = .6f;
    public float fireArcHeight = 1;

    Fireball[] fireballs;
    float fireTimeRemaining = 0;

    LayerMask buildingLayerMask;

    void Start() {
        fireballs = new Fireball[fireballCount];
        for (int i = 0; i < fireballCount; i++) {
            fireballs[i] = new Fireball() {
                obj = Instantiate(fireballPrefab, transform.position, Quaternion.identity),
                origin = transform.position + new Vector3(0, fireOriginHeight, 0),
                target = transform.position + transform.rotation * Vector3.forward * (i + 1),
            };
        }
        fireTimeRemaining = fireTime;

        buildingLayerMask = LayerMask.GetMask("Buildings");
    }

    void Update() {
        fireTimeRemaining -= Time.deltaTime;

        if (fireTimeRemaining > 0) {
            float normalizedFireTime = 1 - (fireTimeRemaining / fireTime);
            
            foreach (Fireball fireball in fireballs) {
                fireball.obj.transform.position = Vector3.Lerp(fireball.origin, fireball.target, normalizedFireTime) + 
                    new Vector3(0, fireArcHeight * Mathf.Sin(normalizedFireTime * Mathf.PI), 0);
            }
        }
        else {
            foreach (Fireball fireball in fireballs) {
                Destroy(fireball.obj);

                Collider[] buildings = Physics.OverlapSphere(fireball.target, .25f, buildingLayerMask);
                if (buildings.Length > 0) {
                    Destroy(buildings[0].gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}