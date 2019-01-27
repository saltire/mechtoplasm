using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurfaceScript : MonoBehaviour {
    public GameObject cube;

    public abstract void OnStep(PlayerScript player);
}
