using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerStarter : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Network.InitializeServer(8, 25000, false);
    }

    void OnGUI() {
        GUILayout.Label("isServer " + Network.isServer);
        GUILayout.Label("isClient " + Network.isClient);
        GUILayout.Label("connections " + Network.connections.Length);
        GUILayout.Label("ip " + Network.connectionTesterIP);

        if (Network.peerType == NetworkPeerType.Disconnected)
            GUILayout.Label("Not Connected");
        else if (Network.peerType == NetworkPeerType.Connecting)
            GUILayout.Label("Connecting");
        else
            GUILayout.Label("Network started");
    }
}