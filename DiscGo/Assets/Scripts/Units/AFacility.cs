using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AFacility :MonoBehaviour {
    public abstract void Install(int team);
    public void Dismount(int team) {
        Destroy(gameObject);
    }
}
