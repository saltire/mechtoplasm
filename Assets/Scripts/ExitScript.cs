using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExitScript : MonoBehaviour {
    public GameObject exitPrefab;

    public PlayerScript player1;
    public PlayerScript player2;

    public int minDistance = 5;

    GameObject exit;
    GridScript grid;

    bool[] orbs = new bool[4];

    void Start() {
        grid = FindObjectOfType<GridScript>();
    }

    public void UpdateOrb(int index, bool status) {
        orbs[index] = status;

        if (orbs.All(orb => orb) && exit == null) {
            SpawnExit();
        }
    }

    void Update() {
        if (Input.GetKeyDown("x")) {
            SpawnExit();
        }
    }

    void SpawnExit() {
        Vector3Int player1Coords = player1.GetCoords();
        Vector3Int player2Coords = player2.GetCoords();
        int x;
        int z;
        int player1Distance;
        int player2Distance;
        Square square;
        do {
            x = Random.Range(0, grid.gridWidth);
            z = Random.Range(0, grid.gridHeight);
            square = grid.GetSquare(x, z);
            player1Distance = Mathf.Abs(player1Coords.x - x) + Mathf.Abs(player1Coords.z - z);
            player2Distance = Mathf.Abs(player2Coords.x - x) + Mathf.Abs(player2Coords.z - z);
        }
        while (player1Distance < minDistance || player2Distance < minDistance || grid.HasBuilding(square));

        exit = Instantiate(exitPrefab, square.top, Quaternion.identity);
    }
}
