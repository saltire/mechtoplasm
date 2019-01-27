using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour {
    public GameObject eggBurstPrefab;

    SpriteRenderer top;
    SpriteRenderer bottom;
    Color[] colors = new Color[] { 
        new Color(0.678f, 0.780f, 0.796f), 
        new Color(0.717f, 0.796f, 0.729f), 
        new Color(0.898f, 0.898f, 0.403f),
        new Color(0.462f, 0.847f, 0.654f), 
        new Color(0.898f, 0.749f, 0.490f), 
        new Color(0.674f, 0.447f, 0.898f),
    };

    bool isQuitting = false;
    
    void Start() {
        top = transform.Find("top").GetComponent<SpriteRenderer>();
        bottom = transform.Find("bottom").GetComponent<SpriteRenderer>();
    }

    void Update() {
        float time = Time.time % 6;
        int color = Mathf.FloorToInt(time);
        float interval = time % 1;
        top.color = Color.Lerp(colors[color], colors[(color + 1) % 6], interval);
        bottom.color = Color.Lerp(colors[(color + 1) % 6], colors[(color + 2) % 6], interval);
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    public void DestroyQuietly() {
        isQuitting = true;
        Destroy(gameObject);
    }

    void OnDestroy() {
        if (!isQuitting) {
            Destroy(Instantiate(eggBurstPrefab, transform.position, Quaternion.identity), 5f);
        }
    }
}
