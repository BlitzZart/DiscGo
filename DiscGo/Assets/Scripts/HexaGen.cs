using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexaGen : MonoBehaviour
{

    private static HexaGen instance;

    public static HexaGen Instance
    {
        get { return instance; }
    }

    private int hexRows = 10; //16
    private int hexColls = 2; // 5
    private Vector3 gridOffset = new Vector3(-3.36f, -5.64f, 0);

    public GameObject hexaStdPrefab;

    private GoogleMap googleMap;
    private List<GameObject> hexaList;
    public List<GameObject> HexaList
    {
        get { return hexaList; }
        set { hexaList = value; }
    }

    private float scale = 0.5f; // 0.4 ~25x30m (used at techgate)    0.7 is fine - 93,102,112 -> hub = 94 in hgb

    private List<int> questSectors;

    private float xOffset, yOffset;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        hexaList = new List<GameObject>();

        InitHexGrid();

        //Invoke("InitHexGrid", 0.4f);
    }

    private void InitHexGrid()
    {
        for (int i = 0; i < hexRows / scale; i++)
        {
            for (int j = 0; j < hexColls / scale; j++)
            {
                GameObject nextHex = null;
                    //if (questSectors.Contains((int)Sector.numberOfSectors + 1))
                    //{
                    //    nextHex = hexaMMPrefab;
                    //}

                if (nextHex == null)
                {
                    nextHex = hexaStdPrefab;
                }

                SpriteRenderer sprite = nextHex.GetComponent<SpriteRenderer>();
                sprite.transform.localScale = new Vector3(scale, scale, scale);
                xOffset = sprite.bounds.size.x + sprite.bounds.size.x / 2;
                yOffset = sprite.bounds.size.y / 2;

                nextHex = (GameObject)Instantiate(nextHex, new Vector3(j * xOffset + (xOffset / 2) * (i % 2), i * yOffset, 0) + gridOffset, Quaternion.identity);

                nextHex.GetComponent<Sector>().SetCoord(i, j);

                hexaList.Add(nextHex);
                nextHex.transform.parent = transform;
            }
        }
        transform.Rotate(90, 0, 0);
    }
}