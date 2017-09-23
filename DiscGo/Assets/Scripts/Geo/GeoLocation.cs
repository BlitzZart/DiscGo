using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GeoLocation : MonoBehaviour {

    public bool useHardCodedPosition = false;
    private bool loadWithoutGPS = true;

    public float latitude = 0;
    public float longitude = 0;

    private bool dataReceived = false;

    GoogleMap googleMap;
    GeoCalculations geoCalculations;

    public static GoogleMapLocation currentLocation { get; set; }

    public static DVector currentPosition = new DVector(99,99);

    void Start()
    {
#if UNITY_EDITOR
        loadWithoutGPS = true; // Test if this caues bugs on android
#endif
        


        googleMap = GetComponent<GoogleMap>();
        geoCalculations = GetComponent<GeoCalculations>();

        currentLocation = new GoogleMapLocation();
        currentLocation.address = "";

        Input.location.Start();
    }

    void Update()
    {

        if (Input.location.lastData.latitude != 0 && Input.location.lastData.longitude != 0 || loadWithoutGPS)
        { // TODO check boolen in future

            if (!loadWithoutGPS) {
                currentLocation.latitude = (float)Input.location.lastData.latitude; // TOTO change to double if accuracy is an issue
                currentLocation.longitude = (float)Input.location.lastData.longitude;
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

            currentPosition.x = (posOnImage.x - centerPoint.x) * 1280 * 10 / 3.1f;
            currentPosition.y = -(posOnImage.y - centerPoint.y) * 1280 * 10 / 3.1f;

            
            //DebugScript.SetText1("X " + currentPosition.x + "\nY " + currentPosition.y + "\nCen: " +  googleMap.centerLocation.latitude + "\nCur: " + currentLocation.latitude);
        }
    }
}