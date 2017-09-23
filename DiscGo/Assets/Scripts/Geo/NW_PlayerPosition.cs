using UnityEngine;
using System.Collections;
using Assets.GoogleMaps.Scripts;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NW_PlayerPosition : MonoBehaviour
{
    
    private float speed = 7;

    private PolygonCollider2D boundsCollider;

    GoogleMapsAPIProjection mapsProjection;
    PointF pixelPosition;

    private float zPosition = 0;
    private bool debugWithMouse = false;
    private bool debugWithTouch = false;

    [HideInInspector]
    public bool downOnLeftEdge, downOnRightEdge, downOnTopEdge, downOnBottomEdge = false;


    // activate all colliders delayed
    private IEnumerator ActivateAllCollidersDelayed() {
        yield return new WaitForSeconds(1.5f);

        Collider2D[] allColliders = GetComponentsInChildren<Collider2D>();

        foreach (Collider2D item in allColliders)
            item.enabled = true;
    }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = true;

        StartCoroutine(ActivateAllCollidersDelayed());

#if UNITY_ANDROID && !UNITY_EDITOR 
        debugWithMouse = false;

#else
        //debugWithMouse = true;
        //debugWithTouch = true;
#endif
        Input.compass.enabled = true;
        //InvokeRepeating("PrintNetworkDebugInformation", 1, 0.5f);
    }

    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    float z = 0.0f;

    void LeftMouseDrag()
    {
        // Get direction of movement
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        // Invert direction to that terrain appears to move with the mouse
        direction = direction * -1;

        // set camera distatnce
        camera_position.z = -15;

        Camera.main.transform.position = camera_position + direction;
    }

    void OnMouseDrag() {

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

    public Vector3 fakePos = new Vector3(100, 0, 0);
    private void FakeCurrenPos()
    {
        fakePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 1);
    }

    private bool gotFirstPosition = false;
    void UpdatePlayerPosition()
    {

        if (debugWithTouch)
            FakeCurrenPos();
        /*
        if (debugWithMouse)
        {
            if (Input.GetMouseButton(1))
            {
                //currentPos.transform.position = fakePos;
                
                Vector3 newPosition = new Vector3(fakePos.x, fakePos.y, -1);

                if ((newPosition - this.transform.position).magnitude > 12)
                    if (!gotFirstPosition)
                    {
                        gotFirstPosition = true;
                        this.transform.position = new Vector3(fakePos.x, fakePos.y, -zPosition);
                    }
                    else
                        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(fakePos.x, fakePos.y, -zPosition), speed * 2 * Time.deltaTime);
                //this.transform.position = new Vector3(fakePos.x, fakePos.y, -zPosition);
                else
                    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(fakePos.x, fakePos.y, -zPosition), speed * Time.deltaTime);
            }
        }
        else
        {
            if (debugWithTouch)
            {
                Vector3 newPosition;
                if (Input.GetMouseButton(0))
                {
                    downTime += Time.deltaTime;
                }
                else
                {
                    downTime = 0;
                }

                if (downTime > holdTime)
                {
                    if (!gotFirstPosition)
                    {
                        gotFirstPosition = true;
                        newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        newPosition.z = -zPosition;
                        this.transform.position = newPosition;
                    }
                    else
                    {
                        newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        newPosition.z = -zPosition;

                        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, speed * Time.deltaTime);
                    }
                       

                }

            }
            else
            {*/
            Vector3 newPosition = new Vector3((float)GeoLocation.currentPosition.x, (float)GeoLocation.currentPosition.y, -1);

            if ((newPosition - this.transform.position).magnitude > 12)
                if (!gotFirstPosition)
                {
                    gotFirstPosition = true;
                    this.transform.position = new Vector3((float)GeoLocation.currentPosition.x, (float)GeoLocation.currentPosition.y, -zPosition);
                }

                else
                    this.transform.position = Vector3.Lerp(this.transform.position, new Vector3((float)GeoLocation.currentPosition.x, (float)GeoLocation.currentPosition.y, -zPosition), speed * 2 * Time.deltaTime);
            else
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3((float)GeoLocation.currentPosition.x, (float)GeoLocation.currentPosition.y, -zPosition), speed * Time.deltaTime);
            }
       // }

        //Quaternion currentOrientation = Quaternion.Euler(new Vector3(0,0, -(float)AndroidCommunication.Instance.angle) * Mathf.Rad2Deg);
        //currentPos.transform.rotation = Quaternion.Lerp(currentPos.transform.rotation, currentOrientation, Time.deltaTime * 2);


        // working with unity compas
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -Input.compass.magneticHeading)),Time.deltaTime * 2); //  * Mathf.Rad2Deg

    //}

    float downTime = 0;
    float holdTime = 0.3f;
}