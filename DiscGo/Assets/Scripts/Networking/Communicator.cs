//using UnityEngine;
//using System.Collections;

//public class Communicator : MonoBehaviour {
//    private static NetworkPlayer player;
//    public static NetworkPlayer Player
//    {
//        get
//        {
//            if (player != null)
//                return player;
//            else
//                player = GetPlayer();
//            return player;
//        }
//    }

//    static NetworkPlayer GetPlayer() {
//        NetworkPlayer[] nwp = FindObjectsOfType<NetworkPlayer>();
//        foreach (NetworkPlayer item in nwp) {
//            if (item.hasAuthority) {
//                return item;
//            }
//        }
//        return null;
//    }

//    void Awake () {
//        GetPlayer();
//	}
//}