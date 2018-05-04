using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

public class NetworkClient : NetworkManager {

    public void ClientStuff(string netID) {
        NetworkID id = (NetworkID)Int64.Parse(netID);
        NetworkManager.singleton.StartMatchMaker();
        NetworkManager.singleton.matchMaker.JoinMatch(id, "", "", "", 0, 0, OnMatchJoined);
    }
}
