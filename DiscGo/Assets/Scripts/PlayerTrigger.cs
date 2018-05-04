using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TileDelegate(Sector tile);

public class PlayerTrigger : MonoBehaviour {
    public static TileDelegate PlayerEnteredTile, PlayerLeftTile, PlayerStaysInTile;

    void OnTriggerEnter(Collider coll) {
        Sector t = coll.GetComponent<Sector>();
        if (t != null) {
            if (PlayerEnteredTile != null) {
                PlayerEnteredTile(t);

                StopAllCoroutines();
                StartCoroutine(PlayerIsInTile(t));
            }
        }
    }

    void OnTriggerExit(Collider coll) {
        Sector t = coll.GetComponent<Sector>();
        if (t != null) {
            if (PlayerLeftTile != null) {
                PlayerLeftTile(t);
                StopAllCoroutines();
            }
        }
    }

    private IEnumerator PlayerIsInTile(Sector t) {
        while (true) {
            if (PlayerStaysInTile != null) {
                PlayerStaysInTile(t);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
   
}


//void OnTriggerStay(Collider coll) {
//    Sector t = coll.GetComponent<Sector>();
//    if (t != null) {
//        if (PlayerEnteredTile != null) {
//            PlayerEnteredTile(t);
//        }
//    }
//}