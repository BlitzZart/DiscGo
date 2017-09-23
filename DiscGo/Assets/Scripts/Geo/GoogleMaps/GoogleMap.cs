using UnityEngine;
using System.Collections;
using System;
using Assets.GoogleMaps.Scripts;
using System.IO;

/// <summary>
/// requests the static image from google and sets it as sprite texture
/// sets style of map
/// </summary>
public class GoogleMap : MonoBehaviour
{
    public GameObject sprite;

    private float scaleFactor = 1.6f;

    [HideInInspector]
    public bool mapRendered = false;

    public enum MapType
	{
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}


	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 13; // 17 was used @ tech gate
	public MapType mapType;
	public int size = 512;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;

    [Header("SAVE IMAGE")]
    // next google image will be saved
    public bool saveNextImage = false;
    public string imageName = "";

    [Header("STATIC IMAGES")]
    // use static images to avoid unnecessary downloads
    public Texture2D TG_MM_1_tex;

    private GeoLocation geoLocation;

	void Start() {
        geoLocation = this.GetComponent<GeoLocation>();
	}

    public void InitMarkers()
    {
        markers = new GoogleMapMarker[1];
        markers[0] = new GoogleMapMarker();
        markers[0].color = GoogleMapColor.green;
        markers[0].size = GoogleMapMarker.GoogleMapMarkerSize.Mid;

        markers[0].locations = new GoogleMapLocation[2];
    }

    public void SetPlayerMarker(int n, DVector coord)
    {
        markers[0].locations[n] = new GoogleMapLocation();

        //Debug.Log("Marker: " + coord.x + " " + coord.y);
        // PIE
        //  48.36887f 48.36915
        //  14.5135f 14.51179
        markers[0].locations[n].address = "";
        markers[0].locations[n].latitude = (float)coord.x;
        markers[0].locations[n].longitude = (float)coord.y;
    }


    IEnumerator _RefreshLocation()
    {
        var url = "http://maps.googleapis.com/maps/api/staticmap";
        var qs = "";
        if (!autoLocateCenter)
        {
            if (centerLocation.address != "")
                qs += "center=" + WWW.UnEscapeURL(centerLocation.address);
            else
            {
                qs += "center=" + WWW.UnEscapeURL(string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude));
            }

            qs += "&zoom=" + zoom.ToString();
        }
        qs += "&size=" + WWW.UnEscapeURL(string.Format("{0}x{0}", size));
        qs += "&scale=" + (doubleResolution ? "2" : "1");
        qs += "&maptype=" + mapType.ToString().ToLower();
        //qs += "&color=" + "#ff2222";

        qs += "&road=" + string.Format("visibility:{0}", "off");

        var usingSensor = false;

        var req = new WWW(url + "?" + qs);
        yield return req;
    }

    /// <summary>
    /// Get new static map centered on current position
    /// </summary>
	public void Refresh() {
		if(autoLocateCenter && (markers.Length == 0 && paths.Length == 0)) {
			Debug.LogError("Auto Center will only work if paths or markers are used.");	
		}

		StartCoroutine(_Refresh());
	}
	
	IEnumerator _Refresh ()
	{
		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		if (!autoLocateCenter) {
			if (centerLocation.address != "")
				qs += "center=" + WWW.UnEscapeURL (centerLocation.address);
			else {
				qs += "center=" + WWW.UnEscapeURL (string.Format ("{0},{1}", centerLocation.latitude, centerLocation.longitude));
			}
		
			qs += "&zoom=" + zoom.ToString ();
		}
		qs += "&size=" + WWW.UnEscapeURL (string.Format ("{0}x{0}", size));
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString ().ToLower ();
		var usingSensor = false;
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
		qs += "&sensor=" + (usingSensor ? "true" : "false");
        
        SetMapStyle(ref qs);

        var req = new WWW(url + "?" + qs);

        //print(url + "?" + qs);
        yield return req;

        Texture2D nT = new Texture2D(req.texture.width, req.texture.height);
        Color[] nC = req.texture.GetPixels(0, 0, req.texture.width, req.texture.height);

        nT.SetPixels(nC);
        nT.Apply();

        sprite.GetComponent<SpriteRenderer>().sprite = Sprite.Create(nT, new Rect(0, 0, req.texture.width, req.texture.height), new Vector2(0.5f, 0.5f));

        ////this.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = req.texture; // draw on mesh (3d objects)
        sprite.GetComponent<SpriteRenderer>().gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        mapRendered = true;

        if (saveNextImage && imageName != "")
        {
            var bytes = nT.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../Assets/_StaticImages/" + imageName + ".png", bytes);
        }
	}

    void SetMapStyle(ref string qs)
    {
        // ---
        // road
        qs += "&style=feature:road%7Celement:geometry%7Clightness:+10";
        qs += "&style=feature:road%7Celement:geometry%7Cvisibility:simplified";
        qs += "&style=feature:road%7Celement:geometry.fill%7Ccolor:0xe5e9ea"; // efffee

        // labels off
        qs += "&style=feature:all%7Celement:labels%7Cvisibility:off";

        // ladscape
        qs += "&style=feature:landscape%7Celement:geometry.fill%7Ccolor:0x5a696e"; // 5599bb

        // poi
        qs += "&style=feature:poi%7Cvisibility:off"; // turn off
        // ---

        // markers
        //qs += "&markers=color:green%7Clabel:G%7C48.36887,14.5135";

        //qs += "&style=feature:poi%7Celement:geometry.fill%7Ccolor:0xbb0000";

        //qs += "&style=feature:administrative%7Cvisibility:off"; // turn off
    }


    void GetPixelOnMap()
    {
        if (!Input.GetMouseButton(1)) // right mouse button
            return;

        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Debug.Log("NoHit");
            return;
        }

        SpriteRenderer renderer = hit.collider.GetComponentInParent<SpriteRenderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (renderer == null)
            return;

        Texture2D tex = (Texture2D)renderer.sprite.texture;
        //tex.Apply();
        Vector2 pixelUV = hit.textureCoord;

        //Debug.Log("SIZE " + tex.height + " | " + tex.width);
        //Debug.Log("@" + (int)(pixelUV.x * renderer.sprite.texture.width) + " | " + (int)(pixelUV.y * renderer.sprite.texture.height));
        //print((int)(pixelUV.x * renderer.material.mainTexture.width) + "--" + (int)(pixelUV.y * renderer.material.mainTexture.height));
    }
}


public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
	
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}


public class GoogleMapsAPIProjection
{
    private readonly double PixelTileSize = 256d; // 256d;
    private readonly double DegreesToRadiansRatio = 180d / Math.PI;
    private readonly double RadiansToDegreesRatio = Math.PI / 180d;
    private readonly PointF PixelGlobeCenter;
    private readonly double XPixelsToDegreesRatio;
    private readonly double YPixelsToRadiansRatio;

    public GoogleMapsAPIProjection(double zoomLevel)
    {
        var pixelGlobeSize = this.PixelTileSize * Math.Pow(2d, zoomLevel);
        this.XPixelsToDegreesRatio = pixelGlobeSize / 360d;
        this.YPixelsToRadiansRatio = pixelGlobeSize / (2d * Math.PI);
        var halfPixelGlobeSize = Convert.ToSingle(pixelGlobeSize / 2d);
        this.PixelGlobeCenter = new PointF(
            halfPixelGlobeSize, halfPixelGlobeSize);
    }

    public PointF FromCoordinatesToPixel(PointF coordinates)
    {
        var x = Math.Round(this.PixelGlobeCenter.X
            + (coordinates.X * this.XPixelsToDegreesRatio));
        var f = Math.Min(
            Math.Max(
                 Math.Sin(coordinates.Y * RadiansToDegreesRatio),
                -0.9999d),
            0.9999d);
        var y = Math.Round(this.PixelGlobeCenter.Y + .5d *
            Math.Log((1d + f) / (1d - f)) * -this.YPixelsToRadiansRatio);

        return new PointF((float)x, (float)y); //        return new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
    }

    public PointF FromPixelToCoordinates(PointF pixel)
    {
        var longitude = (pixel.X - this.PixelGlobeCenter.X) /
            this.XPixelsToDegreesRatio;
        var latitude = (2 * Math.Atan(Math.Exp(
            (pixel.Y - this.PixelGlobeCenter.Y) / -this.YPixelsToRadiansRatio))
            - Math.PI / 2) * DegreesToRadiansRatio;
        return new PointF(
            Convert.ToSingle(latitude),
            Convert.ToSingle(longitude));
    }
}



//void RayCastTest() // WORKS ON A QUAD
//{
//    if (!Input.GetMouseButton(0))
//        return;

//    RaycastHit hit;
//    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
//    {
//        Debug.Log("NoHit");
//        return;
//    }

//    MeshRenderer renderer = hit.collider.GetComponentInParent<MeshRenderer>();
//    MeshCollider meshCollider = hit.collider as MeshCollider;
//    if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
//    {
//        //Debug.Log("renderer: " + renderer + " shared: " + renderer.sharedMaterial + " mainTexturer: " + renderer.sharedMaterial.mainTexture + " meshCollider: " + meshCollider);
//        Debug.Log("?? " + hit.collider.GetComponentInParent<Renderer>().GetInstanceID());
//        Debug.Log("SM " + this.GetComponent<Renderer>().GetInstanceID());
//        return;
//    }

//    Texture2D tex = (Texture2D)renderer.material.mainTexture;
//    Vector2 pixelUV = hit.textureCoord;
//    Debug.Log("X " + (int)(pixelUV.x * renderer.material.mainTexture.width) + "--" + (int)(pixelUV.y * renderer.material.mainTexture.height));
//    //print((int)(pixelUV.x * renderer.material.mainTexture.width) + "--" + (int)(pixelUV.y * renderer.material.mainTexture.height));
//}