using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSurfaceScript : SurfaceScript {
    public float speedMultiplier = 2f;

    public override void OnStep(PlayerScript player) {
        player.Move();
    }
}
