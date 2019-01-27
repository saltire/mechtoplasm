using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSurfaceScript : SurfaceScript {
    public override void OnStep(PlayerScript player) {
        Util.Log("Weeee!");
        player.Ascend();
    }
}
