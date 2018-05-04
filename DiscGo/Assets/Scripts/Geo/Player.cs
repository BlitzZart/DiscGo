using UnityEngine;
using System.Collections;
using Assets.GoogleMaps.Scripts;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Player : MonoBehaviour {
    private float speed = 7;
    private float zPosition = 0;
    public bool debugWithMouse = false;

    public Sector currentSector;
    private ToolBox toolBox;
    public ToolBox ToolBox {
        get {
            return toolBox;
        }
    }

    public int team;

    [HideInInspector]
    public bool downOnLeftEdge, downOnRightEdge, downOnTopEdge, downOnBottomEdge = false;

    // for debugging
    float downTime = 0;
    float holdTime = 0.3f;

    // activate all colliders delayed
    private IEnumerator ActivateAllCollidersDelayed() {
        yield return new WaitForSeconds(1.5f);

        Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();

        foreach (Collider2D item in allColliders)
            item.enabled = true;
    }

    // Use this for initialization
    void Start() {
        Cursor.visible = true;

        toolBox = GetComponent<ToolBox>();

        StartCoroutine(ActivateAllCollidersDelayed());

#if UNITY_ANDROID && !UNITY_EDITOR 
        debugWithMouse = false;
#else
        debugWithMouse = true;
#endif
        Input.compass.enabled = true;

        PlayerTrigger.PlayerEnteredTile += OnPlayerEnteredTile;
        PlayerTrigger.PlayerLeftTile += OnPlayerLeftTile;
    }
    private void OnDestroy() {
        PlayerTrigger.PlayerEnteredTile -= OnPlayerEnteredTile;
        PlayerTrigger.PlayerLeftTile -= OnPlayerLeftTile;
    }

    private void OnPlayerEnteredTile(Sector tile) {
        currentSector = tile;
    }
    private void OnPlayerLeftTile(Sector tile) {
        currentSector = null;
    }

    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    float z = 0.0f;

    void LeftMouseDrag() {
        // Get direction of movement
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        // Invert direction to that terrain appears to move with the mouse
        direction = direction * -1;

        // set camera distatnce
        camera_position.z = -15;

        Camera.main.transform.position = camera_position + direction;
    }

    // Update is called once per frame
    void Update() {
        // -------------------- NW test
        if (Input.GetKeyDown(KeyCode.Space)) {
            //SyncDeliver(1);
        }
        //GameObject.Find("DebugText").GetComponent<Text>().text = "Sync " + syncInt2;
        // -------------------- NW test

        //if (currentPos != null) {
        //print("UPD + has pos");
        UpdatePlayerPosition();
        //}
    }


    private bool gotFirstPosition = false;
    void UpdatePlayerPosition() {
        if (debugWithMouse) {
            if (Input.GetMouseButton(1)) {
                Vector3 fakePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //currentPos.transform.position = fakePos;

                Vector3 newPosition = new Vector3(fakePos.x, 0, fakePos.z);

                if ((newPosition - this.transform.position).magnitude > 12)
                    if (!gotFirstPosition) {
                        gotFirstPosition = true;
                        this.transform.position = new Vector3(fakePos.x, -zPosition, fakePos.z);
                    }
                    else
                        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(fakePos.x, -zPosition, fakePos.z), speed * 2 * Time.deltaTime);
                //this.transform.position = new Vector3(fakePos.x, fakePos.y, -zPosition);
                else
                    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(fakePos.x, -zPosition, fakePos.z), speed * Time.deltaTime);
            }
        }
        else {
           
                Vector3 newPosition = new Vector3((float)GeoLocation.currentPosition.x, (float)GeoLocation.currentPosition.y, -1);

                if ((newPosition - this.transform.position).magnitude > 12)
                    if (!gotFirstPosition) {
                        gotFirstPosition = true;
                        transform.position = new Vector3((float)GeoLocation.currentPosition.x, (float)GeoLocation.currentPosition.y, -zPosition);
                    }

                    else
                        transform.position = Vector3.Lerp(this.transform.position, new Vector3((float)GeoLocation.currentPosition.x, -zPosition, (float)GeoLocation.currentPosition.y), speed * 2 * Time.deltaTime);
                else
                    transform.position = Vector3.Lerp(this.transform.position, new Vector3((float)GeoLocation.currentPosition.x, -zPosition, (float)GeoLocation.currentPosition.y), speed * Time.deltaTime);
            // }

            //Quaternion currentOrientation = Quaternion.Euler(new Vector3(0,0, -(float)AndroidCommunication.Instance.angle) * Mathf.Rad2Deg);
            //currentPos.transform.rotation = Quaternion.Lerp(currentPos.transform.rotation, currentOrientation, Time.deltaTime * 2);


            // working with unity compas
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -Input.compass.magneticHeading)),Time.deltaTime * 2); //  * Mathf.Rad2Deg

            //}
        }
    }
}