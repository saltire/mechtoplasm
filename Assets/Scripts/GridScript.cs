using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {
    public GameObject cubePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float heightScale = 2;

    GameObject[] cubes;

    void Awake() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        cubes = new GameObject[gridWidth * gridHeight];
        for (int x = 0; x < gridWidth; x++) {
            for (int z = 0; z < gridHeight; z++) {
                int i = x * gridWidth + z;
                float y = Mathf.PerlinNoise((float)x / gridWidth, (float)z / gridHeight) * heightScale;
                cubes[i] = Instantiate(cubePrefab, new Vector3(x + .5f, y - 1, z + .5f), Quaternion.identity);
                cubes[i].transform.parent = transform;
            }
        }
    }
    
    public Vector3 GetTop(int x, int z) {
        if (x < 0 || x >= gridWidth || z < 0 || z >= gridWidth) {
            return Vector3.zero;
        }
        int i = x * gridWidth + z;
        return cubes[i].transform.position + new Vector3(0, .5f, 0);
    }
}
