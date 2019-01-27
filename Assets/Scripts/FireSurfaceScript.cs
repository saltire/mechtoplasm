using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSurfaceScript : SurfaceScript {
    public float duration = 4f;
    float timeRemaining = 0;

    public int damage = 1;

    public Color color = Color.red;
    public Color burntColor = Color.black;

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
        player.GetComponentInChildren<SpriteRenderer>().color = burntColor;
        player.Damage(damage);
    }
}
