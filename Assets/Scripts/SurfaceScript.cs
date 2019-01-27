using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurfaceScript : MonoBehaviour {
    public float duration = 4f;
    float timeRemaining = 0;
    public Color color = Color.green;

    public Square square;

    void Start() {
        square.cube.GetComponentInChildren<SpriteRenderer>().color = color;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) {
            ParticleSystem.MainModule psMain = ps.main;
            psMain.duration = duration;
            ps.Play();
        }

        timeRemaining = duration;
    }

    void Update() {
        timeRemaining -= Time.deltaTime;

        square.cube.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(square.color, color, EaseOutQuint(0, 1, timeRemaining / duration));

        if (timeRemaining <= 0) {
            Destroy(gameObject);
        }
    }
    
    float EaseOutQuint(float start, float end, float value) {
        value--;
        end -= start;
        return end * (value * value * value * value * value + 1) + start;
    }

    public abstract void OnStep(PlayerScript player);
}
