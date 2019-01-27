using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSurfaceScript : SurfaceScript {
    public int damage = 1;

    public Color burntColor = Color.black;

    public override void OnStep(PlayerScript player) {
        player.GetComponentInChildren<SpriteRenderer>().color = burntColor;
        player.Damage(damage);
    }
}
