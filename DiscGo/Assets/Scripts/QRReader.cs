using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ZXing;
using ZXing.QrCode;

public class QRReader : MonoBehaviour {

    private WebCamTexture camTexture;
    private Rect screenRect;
    private bool CameraAvailable = false;

    private bool done = false;

    private void Start() {
        StartScanning();
    }

    void OnGUI() {
        // no cam - do nothing
        //if (!CameraAvailable)
        //    return;
        // drawing the camera on screen
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        // do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
        try {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
            if (result != null && !done) {
                Debug.Log("DECODED TEXT FROM QR: " + result.Text);
                FindObjectOfType<NetworkClient>().ClientStuff(result.Text);
                done = true;
            }
        }
        catch (Exception ex) {
            Debug.LogWarning(ex.Message);
        }
    }

    public void StartScanning() {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null) {
            camTexture.Play();
        }
    }
}