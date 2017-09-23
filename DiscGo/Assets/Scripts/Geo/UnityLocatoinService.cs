using UnityEngine;
using System.Collections;
using System;

public class UnityLocatoinService : MonoBehaviour {

    private double latitude = 48.305990f;
    private double longitude = 14.286370;

    private float desiredAccuracy = 8.0f; // in meters
    private float desiredUpdateDistance = 1.0f; // in meters

    public void Connect() {
        StartCoroutine(StartUnityLocationService());
    }

    IEnumerator StartUnityLocationService() {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
            yield break;
        }
        // Start service before querying location
        Input.location.Start(desiredAccuracy, desiredUpdateDistance);

        // Wait until service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        // Service didn't initialize in x seconds
        if (maxWait < 1) {
            //print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
            //print("Unable to determine device location");
            yield break;
        }
        else {
            // Access granted and location value could be retrieved
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    public void Exit() {
        // not needed?
    }

    public double GetLatitude() {
        //return 48.234653d;
        return Input.location.lastData.latitude;
    }

    public double GetLongitude() {
        //return 16.413220d;
        return Input.location.lastData.longitude;
    }

    public bool HasGPS() {
        //return true;
        return Input.location.status == LocationServiceStatus.Running;
    }

    public void LocationUpdate() {
        // not needed?
    }

    public void Stop() {
        Input.location.Stop();
    }

    public void FakeHasGps(bool fake) {
        // not neede?
    }
}
