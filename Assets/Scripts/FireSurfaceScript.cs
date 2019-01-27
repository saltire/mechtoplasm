using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSurfaceScript : SurfaceScript {
    public float duration = 4f;
    float timeRemaining = 0;

    void Start() {
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule psMain = ps.main;
        psMain.duration = duration;
        ps.Play();

        timeRemaining = duration;
    }

    void Update() {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0) {
            cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
            Destroy(gameObject);
        }
    }

    public override void OnStep(PlayerScript player) {
        Util.Log("Ouch!");
        player.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.black);
    }
}
