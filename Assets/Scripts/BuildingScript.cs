using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {
    public GameObject rubblePrefab;
    public Sprite[] rubbleSprites;

    GridScript grid;
    bool isQuitting = false;

    void Start() {
        grid = FindObjectOfType<GridScript>();
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    void OnDestroy() {
        if (!isQuitting && !grid.resetting) {
            GameObject rubble = Instantiate(rubblePrefab, transform.position, Quaternion.identity);
            rubble.GetComponentInChildren<SpriteRenderer>().sprite = rubbleSprites[Random.Range(0, rubbleSprites.Length)];
            rubble.GetComponentInChildren<SpriteRenderer>().color = GetComponentInChildren<SpriteRenderer>().color;
        }
    }
}
