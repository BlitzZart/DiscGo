using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GeoLocation : MonoBehaviour {

    public bool useHardCodedPosition = false;
    private bool loadWithoutGPS = false;

    public float latitude = 0;
    public float longitude = 0;

    private bool locationAcive = false;

    GoogleMap googleMap;
    GeoCalculations geoCalculations;

    public static GoogleMapLocation currentLocation { get; set; }

    public static DVector currentPosition = new DVector(99,99);

    void Start() {
#if UNITY_EDITOR
        loadWithoutGPS = true; // Test if this caues bugs on android
#endif
        googleMap = GetComponent<GoogleMap>();
        geoCalculations = GetComponent<GeoCalculations>();

        currentLocation = new GoogleMapLocation();
        currentLocation.address = "";

        if (loadWithoutGPS)
            Debug.LogError("Not using GPS");
        else
            StartCoroutine(StartLocation());
    }

    void Update()
    {
        if (locationAcive || loadWithoutGPS)
        { // TODO check boolen in future
            if (!loadWithoutGPS) {
                currentLocation.latitude = (float)Input.location.lastData.latitude; // TOTO change to double if accuracy is an issue
                currentLocation.longitude = (float)Input.location.lastData.longitude;
            } else {
                currentLocation.latitude = latitude;
                currentLocation.longitude = longitude;
            }

            if (!googleMap.mapRendered)
            {
                googleMap.mapRendered = true;
                if (useHardCodedPosition)
                {
                    googleMap.centerLocation.latitude = latitude; // 48.36743 - wiese hgb
                    googleMap.centerLocation.longitude = longitude; // 14.51568
                }
                else
                {
                    googleMap.centerLocation.latitude = currentLocation.latitude;
                    googleMap.centerLocation.longitude = currentLocation.longitude;
                }

                // calculte corners of image
                geoCalculations.CalcCorners(googleMap.centerLocation.latitude, googleMap.centerLocation.longitude);

                // TODO only 4 debugging (markers)
                googleMap.InitMarkers();
                googleMap.SetPlayerMarker(0, geoCalculations.SWCorner);
                googleMap.SetPlayerMarker(1, geoCalculations.NECorner);

                googleMap.Refresh();
            }

            DVector posOnImage = geoCalculations.GetPixelPos(new DVector(currentLocation.latitude, currentLocation.longitude));
            DVector centerPoint = geoCalculations.centerPoint;

            currentPosition.x = (posOnImage.x - centerPoint.x) * 1280 * 6.6f; // No idea where those 6.6f came from!!
            currentPosition.y = -(posOnImage.y - centerPoint.y) * 1280 * 6.6f;

            //print("Lat: " + currentLocation.latitude);
            //DebugScript.SetText1("X " + currentPosition.x + "\nY " + currentPosition.y + "\nCen: " +  googleMap.centerLocation.latitude + "\nCur: " + currentLocation.latitude);
        }
    }

    private IEnumerator StartLocation() {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
            Debug.LogError("Location service disabled");
            yield break;
        }

        // Start service before querying location
        Input.location.Start(2.0f, 2.0f);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
            Debug.LogError("Timed out - no Location after " + maxWait + " seconds");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else {
            // Access granted and location value could be retrieved
            locationAcive = true;
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
    }
}