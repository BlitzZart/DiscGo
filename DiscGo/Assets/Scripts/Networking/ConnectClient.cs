using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectClient : MonoBehaviour {
    // Use this for initialization
    void Start() {
        Network.Connect("192.168.1.100", 25000);
    }

    void OnGUI() {
        GUILayout.Label("isServer " + Network.isServer);
        GUILayout.Label("isClient " + Network.isClient);

        if (Network.peerType == NetworkPeerType.Disconnected)
            GUILayout.Label("Not Connected");
        else if (Network.peerType == NetworkPeerType.Connecting)
            GUILayout.Label("Connecting");
        else
            GUILayout.Label("Network started");
    }
}
