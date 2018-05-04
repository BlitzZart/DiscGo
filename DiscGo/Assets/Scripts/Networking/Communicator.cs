using UnityEngine;
using System.Collections;

public class Communicator : MonoBehaviour {
    private static Player player;
    public static Player Player {
        get {
            if (player != null)
                return player;
            else
                player = GetPlayer();
            return player;
        }
    }

    private static Player GetPlayer() {
        Player[] nwp = FindObjectsOfType<Player>();
        foreach (Player item in nwp) {
            if (item.hasAuthority) {
                return item;
            }
        }
        return null;
    }

    void Awake() {
        GetPlayer();
    }
}