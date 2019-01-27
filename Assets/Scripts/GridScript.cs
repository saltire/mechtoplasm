using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Square {
    public GameObject cube;
    public Vector3 top;
    public SurfaceScript surface;
    public Color color;
}

public class GridScript : MonoBehaviour {
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float heightScale = 2;

    public Vector3[] temples;
    public int templesCount = 4;

    public float buildingChance = .03f;

    public GameObject cubePrefab;
    public Sprite[] floorSprites;
    public GameObject[] buildingPrefabs;
    public GameObject[] templePrefabs;

    Square[] squares;

    void Awake() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        squares = new Square[gridWidth * gridHeight];
        for (int x = 0; x < gridWidth; x++) {
            for (int z = 0; z < gridHeight; z++) {
                int i = x * gridWidth + z;
                float y = Mathf.PerlinNoise((float)x / gridWidth, (float)z / gridHeight) * heightScale;

                squares[i].cube = Instantiate(cubePrefab, new Vector3(x + .5f, y - 1, z + .5f), Quaternion.identity);
                squares[i].cube.transform.parent = transform;
                squares[i].color = Color.Lerp(Color.black, Color.white, y + .35f);

                SpriteRenderer cubeSprite = squares[i].cube.GetComponentInChildren<SpriteRenderer>();
                cubeSprite.sprite = floorSprites[Random.Range(0, floorSprites.Length - 1)];
                cubeSprite.color = squares[i].color;

                squares[i].top = squares[i].cube.transform.position + new Vector3(0, squares[i].cube.transform.localScale.y / 2, 0);

                bool templePlaced = false;

                for (int j = 0; j < templesCount; j++) {
                    if (x == temples[j].x && z == temples[j].z) {
                        Instantiate(templePrefabs[0], new Vector3(x + .5f, y, z + .5f), Quaternion.Euler(0, 0, 0));
                        templePlaced = true;
                    }
                }

                if (!templePlaced && Mathf.Abs(x - z) < Mathf.Min(gridWidth, gridHeight) - 3 && Random.value < buildingChance) {
                    Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length - 1)], new Vector3(x + .5f, y, z + .5f), Quaternion.Euler(-90, Random.Range(0, 4) * 90, 0));
                }
            }
        }
    }

    public bool SquareExists(float x, float z) {
        return SquareExists(Mathf.FloorToInt(x), Mathf.FloorToInt(z));
    }

    public bool SquareExists(int x, int z) {
        return !(x < 0 || x >= gridWidth || z < 0 || z >= gridWidth);
    }

    public Square GetSquare(Vector3 pos) {
        return GetSquare(pos.x, pos.z);
    }

    public Square GetSquare(float x, float z) {
        return GetSquare(Mathf.FloorToInt(x), Mathf.FloorToInt(z));
    }

    public Square GetSquare(int x, int z) {
        int i = x * gridWidth + z;
        return squares[i];
    }

    public void SetSquareSurface(float x, float z, SurfaceScript surfacePrefab) {
        int i = (int)x * gridWidth + (int)z;

        if (squares[i].surface != null) {
            Destroy(squares[i].surface.gameObject);
        }

        squares[i].surface = Instantiate<SurfaceScript>(surfacePrefab, squares[i].top, Quaternion.identity);
        squares[i].surface.square = squares[i];
    }
}
