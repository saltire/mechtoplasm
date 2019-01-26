using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Square {
    public GameObject cube;
    public Vector3 top;
}

public class GridScript : MonoBehaviour {
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float heightScale = 2;

    public float buildingChance = .03f;

    public GameObject[] cubePrefabs;
    public GameObject[] buildingPrefabs;

    GameObject[] cubes;

    Square[] squares;

    void Awake() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        cubes = new GameObject[gridWidth * gridHeight];
        squares = new Square[gridWidth * gridHeight];
        for (int x = 0; x < gridWidth; x++) {
            for (int z = 0; z < gridHeight; z++) {
                int i = x * gridWidth + z;
                float y = Mathf.PerlinNoise((float)x / gridWidth, (float)z / gridHeight) * heightScale;
                squares[i].cube = Instantiate(cubePrefabs[Random.Range(0, cubePrefabs.Length - 1)], new Vector3(x + .5f, y - 1, z + .5f), Quaternion.Euler(-90, Random.Range(0, 4) * 90, 0));
                squares[i].cube.transform.parent = transform;

                squares[i].top = squares[i].cube.transform.position + new Vector3(0, squares[i].cube.transform.localScale.y / 2, 0);

                if (Mathf.Abs(x - z) < Mathf.Min(gridWidth, gridHeight) - 3 && Random.value < buildingChance) {
                    Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length - 1)], new Vector3(x + .5f, y, z + .5f), Quaternion.Euler(-90, Random.Range(0, 4) * 90, 0));
                }
            }
        }
    }

    public bool SquareExists(int x, int z) {
        return !(x < 0 || x >= gridWidth || z < 0 || z >= gridWidth);
    }

    public Square GetSquare(int x, int z) {
        int i = x * gridWidth + z;
        return squares[i];
    }
}
