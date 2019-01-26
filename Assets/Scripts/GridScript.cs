using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {
    public GameObject cubePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;

    void Start() {
        for (int x = 0; x < gridWidth; x++) {
            for (int z = 0; z < gridHeight; z++) {
                Instantiate(cubePrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }

    void Update() {
        
    }
}
