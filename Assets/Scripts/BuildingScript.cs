using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {
    public GameObject rubblePrefab;

    bool isQuitting = false;

    void OnApplicationQuit() {
        isQuitting = true;
    }

    void OnDestroy() {
        if (!isQuitting) {
            GameObject rubble = Instantiate(rubblePrefab, transform.position, Quaternion.identity);
            rubble.GetComponentInChildren<SpriteRenderer>().color = GetComponentInChildren<SpriteRenderer>().color;
        }
    }
}
