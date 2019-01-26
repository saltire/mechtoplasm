using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {
    public GameObject cubePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float heightScale = 2;

    void Start() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        for (int x = 0; x < gridWidth; x++) {
            for (int z = 0; z < gridHeight; z++) {
                float y = Mathf.PerlinNoise((float)x / gridWidth, (float)z / gridHeight) * heightScale;
                GameObject cube = Instantiate(cubePrefab, new Vector3(x + .5f, y - 1, z + .5f), Quaternion.identity);
                cube.transform.parent = transform;
            }
        }
    }

    void Update() {
    }
}
