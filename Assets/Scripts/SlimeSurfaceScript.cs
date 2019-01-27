using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSurfaceScript : SurfaceScript {
    public float duration = 4f;
    float timeRemaining = 0;

    public float speedMultiplier = .3f;

    public Color color = Color.green;

    void Start() {
        square.cube.GetComponentInChildren<SpriteRenderer>().color = color;
        // ParticleSystem ps = GetComponent<ParticleSystem>();
        // ParticleSystem.MainModule psMain = ps.main;
        // psMain.duration = duration;
        // ps.Play();

        timeRemaining = duration;
    }

    void Update() {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0) {
            square.cube.GetComponentInChildren<SpriteRenderer>().color = square.color;
            Destroy(gameObject);
        }
    }

    public override void OnStep(PlayerScript player) {
        // foreach (MeshRenderer meshR in player.GetComponentsInChildren<MeshRenderer>()) {
        //     meshR.material.SetColor("_Color", Color.black);
        // }
    }
}
