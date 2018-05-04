using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class NetworkServer : NetworkManager {
    public static string SEPERATOR = "|";
    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        //print(matchName + matchInfo.id);


        GoogleMap gMap = FindObjectOfType<GoogleMap>();
        string qrInfo = matchInfo.networkId.ToString();// + SEPERATOR + gMap.centerLocation.latitude + SEPERATOR + gMap.centerLocation.longitude;

        QRGenerator qrg = FindObjectOfType<QRGenerator>();
        qrg.GenerateQR(qrInfo);
    }
}
