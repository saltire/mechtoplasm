using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSurfaceScript : SurfaceScript {
    public float duration = 4f;
    float timeRemaining = 0;

    public float speedMultiplier = 2f;

    public Color color = Color.blue;

    void Start() {
        square.cube.GetComponentInChildren<SpriteRenderer>().color = color;
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule psMain = ps.main;
        psMain.duration = duration;
        ps.Play();

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
        player.Move();
    }
}
