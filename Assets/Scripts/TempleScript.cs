using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleScript : MonoBehaviour {
    public OrbScript orb;

    void OnDestroy() {
        if (orb != null) {
            orb.gameObject.GetComponent<MeshRenderer>().enabled = true;
            orb.Drop();
        }
    }
}
