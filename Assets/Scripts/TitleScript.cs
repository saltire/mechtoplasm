using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour {
    public Sprite titleSprite;
    public Sprite creditsSprite;

    void Update() {
        if (Input.GetButtonDown("P1Fire0") || Input.GetButtonDown("P2Fire0")) {
            SceneManager.LoadScene("Game");
        }
        else if (Input.GetButtonDown("P1Fire2") || Input.GetButtonDown("P2Fire2")) {
            GetComponent<SpriteRenderer>().sprite = creditsSprite;
        }
        else if (Input.GetButtonDown("P1Fire1") || Input.GetButtonDown("P2Fire1")) {
            GetComponent<SpriteRenderer>().sprite = titleSprite;
        }
        else if (Input.GetButtonDown("Quit")) {
            Application.Quit();
        }
    }
}
