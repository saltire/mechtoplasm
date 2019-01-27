using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {
    public GameObject rubblePrefab;
    public Sprite[] rubbleSprites;

    bool isQuitting = false;

    void OnApplicationQuit() {
        isQuitting = true;
    }

    void OnDestroy() {
        if (!isQuitting) {
            GameObject rubble = Instantiate(rubblePrefab, transform.position, Quaternion.identity);
            rubble.GetComponentInChildren<SpriteRenderer>().sprite = rubbleSprites[Random.Range(0, rubbleSprites.Length)];
            rubble.GetComponentInChildren<SpriteRenderer>().color = GetComponentInChildren<SpriteRenderer>().color;
        }
    }
}
