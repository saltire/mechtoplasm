using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {
    public SpriteRenderer[] orbs;
    public Color[] colors;

    public void UpdateOrb(int index, bool status) {
        orbs[index].color = status ? colors[index] : new Color(1, 1, 1, .25f);
    }
}
