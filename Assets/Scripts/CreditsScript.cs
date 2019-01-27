using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
 

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("P1Fire0") || Input.GetButtonDown("P2Fire0")) {
            SceneManager.LoadScene("Title Screen");
        }
        if (Input.GetButtonDown("P1Fire2") || Input.GetButtonDown("P2Fire2")) {
            SceneManager.LoadScene("Title Screen");
        }
        if (Input.GetButtonDown("P1Fire1") || Input.GetButtonDown("P2Fire1")) {
            SceneManager.LoadScene("Title Screen");
        }
    }
}
