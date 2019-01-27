using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurfaceScript : MonoBehaviour {
    public Square square;

    public abstract void OnStep(PlayerScript player);
}
