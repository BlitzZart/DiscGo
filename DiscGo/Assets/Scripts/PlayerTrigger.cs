using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TileDelegate(Sector tile);

public class PlayerTrigger : MonoBehaviour {
    public static TileDelegate PlayerEnteredTile, PlayerLeftTile;

    void OnTriggerEnter(Collider coll) {
        Sector t = coll.GetComponent<Sector>();
        if (t != null) {
            if (PlayerEnteredTile != null) {
                PlayerEnteredTile(t);
            }
        }
    }
    void OnTriggerExit(Collider coll) {
        Sector t = coll.GetComponent<Sector>();
        if (t != null) {
            if (PlayerLeftTile != null) {
                PlayerLeftTile(t);
            }
        }
    }
}
