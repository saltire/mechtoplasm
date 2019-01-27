using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleScript : MonoBehaviour {
    public GameObject rubblePrefab;
    public OrbScript orb;

    GridScript grid;
    bool isQuitting = false;

    void Start() {
        grid = FindObjectOfType<GridScript>();
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    void OnDestroy() {
        if (orb != null) {
            orb.gameObject.GetComponent<MeshRenderer>().enabled = true;
            orb.Drop();
        }

        if (!isQuitting && !grid.resetting) {
            GameObject rubble = Instantiate(rubblePrefab, transform.position, Quaternion.identity);
            rubble.GetComponentInChildren<SpriteRenderer>().color = transform.Find("Base").GetComponent<SpriteRenderer>().color;
        }
    }
}
