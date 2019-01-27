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

    public Color highColor = Color.white;
    public Color lowColor = Color.black;
    public float minHeight = .5f;

    public GameObject cubePrefab;
    public Sprite[] floorSprites;
    public GameObject[] buildingPrefabs;
    public TempleScript templePrefab;
    public OrbScript orbPrefab;

    Square[] squares;

    Color[] colors = new Color[] {
        Color.green,
        Color.red,
        Color.blue,
        Color.yellow,
    };

    void Awake() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        // random temple start positions
        temples[0] = new Vector3(Mathf.Floor(Random.Range(1, 4)), 0, Mathf.Floor(Random.Range(1, 4)));
        temples[1] = new Vector3(Mathf.Floor(Random.Range(1, 4)), 0, Mathf.Floor(Random.Range(5, 9)));
        temples[2] = new Vector3(Mathf.Floor(Random.Range(5, 9)), 0, Mathf.Floor(Random.Range(1, 4)));
        temples[3] = new Vector3(Mathf.Floor(Random.Range(5, 9)), 0, Mathf.Floor(Random.Range(5, 9)));

        int orbRandomizer = Random.Range(0, templesCount);
        float noiseOffset = Random.Range(0f, 100f);

        squares = new Square[gridWidth * gridHeight];
        for (int x = 0; x < gridWidth; x++) {
            for (int z = 0; z < gridHeight; z++) {
                int i = x * gridWidth + z;
                float y = Mathf.PerlinNoise((float)x / gridWidth + noiseOffset, (float)z / gridHeight + noiseOffset) * heightScale;

                squares[i].cube = Instantiate(cubePrefab, new Vector3(x + .5f, y - 1, z + .5f), Quaternion.identity);
                squares[i].cube.transform.parent = transform;
                squares[i].color = Color.Lerp(lowColor, highColor, y * (1 - minHeight) + minHeight);
                squares[i].top = squares[i].cube.transform.position + new Vector3(0, squares[i].cube.transform.localScale.y / 2, 0);

                SpriteRenderer cubeSprite = squares[i].cube.GetComponentInChildren<SpriteRenderer>();
                cubeSprite.sprite = floorSprites[Random.Range(0, floorSprites.Length - 1)];
                cubeSprite.color = squares[i].color;

                // place temples
                bool templePlaced = false;
                for (int j = 0; j < templesCount; j++) {
                    if (x == temples[j].x && z == temples[j].z) {
                        OrbScript orb = Instantiate<OrbScript>(orbPrefab, new Vector3(x + .5f, y + .8f, z + .5f), Quaternion.Euler(0, 0, 0));
                        orb.weaponIndex = (j + orbRandomizer) % templesCount;
                        orb.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[orb.weaponIndex]);
                        orb.GetComponent<MeshRenderer>().enabled = false;

                        TempleScript temple = Instantiate<TempleScript>(templePrefab, new Vector3(x + .5f, y, z + .5f), Quaternion.Euler(0, 0, 0));
                        templePlaced = true;
                        // temple.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[orb.weaponIndex]);
                        temple.transform.Find("Mask").GetComponent<SpriteRenderer>().color = colors[orb.weaponIndex];
                        temple.orb = orb;
                    }
                }

                // place bulidings
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
