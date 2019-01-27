using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSurfaceScript : SurfaceScript {
    public float duration = 4f;

    void Start() {
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule psMain = ps.main;
        psMain.duration = duration;
        ps.Play();

        Destroy(gameObject, duration);
    }

    public override void OnStep(PlayerScript player) {
        Util.Log("Ouch!");
        player.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.black);
    }

    void OnDestroy() {
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
    }
}
