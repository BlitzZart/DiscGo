using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public struct HexCoord
{
    public int x, y;
    public HexCoord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
public delegate void SectorInteraction(Sector sector);

public class Sector : MonoBehaviour
{
    public static event SectorInteraction Entered;
    public static event SectorInteraction Interacted;
    public virtual event SectorInteraction Used;

    public bool fadeOutOnEnter = false;

    private static float fadeInDuration = 0.4f;
    private static float fadeOutDuration = 0.4f;
    private static float checkVisibilityRate = 0.2f;
    private static TimeSpan stayInvisTimeSpan;
    private static int stayInvisHours = 0;
    private static int stayInvisMinutes = 0;
    private static int stayInvisSeconds = 4;

    float timeToNextcheck = 0;

    public uint sectorID = 0;
    public static uint numberOfSectors;

    protected SpriteRenderer spriteRenderer;

    public HexCoord coord;

    //[HideInInspector]
    public bool activated = false;

    protected bool visible = false;

    public bool fadeOut { get; set; }
    public bool fadeIn { get; set; }

    protected Color sectorColor;
    protected DateTime fadeOutTime;

    #region unity callbacks
    protected virtual void Awake()
    {
        numberOfSectors++;
        sectorID = numberOfSectors;
        coord = new HexCoord();
    }
    protected virtual void Start()
    {
        stayInvisTimeSpan = new TimeSpan(stayInvisHours, stayInvisMinutes, stayInvisSeconds); // TODO 6s for debugging - use stayInvisHours instead

        fadeIn = true;

        spriteRenderer = this.GetComponent<SpriteRenderer>();

        sectorColor = spriteRenderer.color;
        
        //spriteRenderer.color = new Color(sectorColor.r, sectorColor.g, sectorColor.b, sectorColor.a);
    }
    protected virtual void Update() {
        if (fadeOut) {
            FadeOut();
        }
        else if (!visible) {
            if (fadeIn)
                FadeIn();
            else {
                AutoFadeInAfterTime();
            }
        }
    }


    void OnTriggerEnter(Collider coll) {
        //Debug.Log(this.sectorID);
        DoOnEnter(coll);
    }
    void OnTriggerStay(Collider coll) {
        DoOnStay(coll);
    }
    void OnTriggerExit(Collider coll) {
        DoOnExit(coll);
    }
    #endregion
    #region private
    private void FadeIn()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + Time.deltaTime / fadeInDuration);

        if (spriteRenderer.color.a > 1)
        {
            visible = true;
            fadeIn = false;
        }
    }
    private void FadeOut()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - Time.deltaTime / fadeOutDuration);

        if (spriteRenderer.color.a <= 0)
        {
            visible = fadeOut = false;
            fadeOutTime = DateTime.Now;
        }
    }
    private void AutoFadeInAfterTime() {
        timeToNextcheck += Time.deltaTime;
        if (timeToNextcheck > checkVisibilityRate)
            if (DateTime.Now.Subtract(fadeOutTime).CompareTo(stayInvisTimeSpan) == 1) {
                fadeIn = true;
                timeToNextcheck = 0;
            }
    }
    #endregion
    #region public virtual
    public virtual void SetCoord(int x, int y) {
        coord.x = x;
        coord.y = y;
    }
    public virtual void Interact() {
        //if (!this.activated)
        //    fadeOut = true;

        if (Used != null)
            Used(this);

        if (Interacted != null)
            Interacted(this);
    }
    public virtual void SetColor(Color color)
    {
        sectorColor = color;
        GetComponent<SpriteRenderer>().color = color;
    }
    public virtual void TriggerExternalCollision(Collider coll)
    {
        DoOnEnter(coll);
    }
    #endregion
    #region protected virtual
    protected virtual void DoOnEnter(Collider coll) {
        // fade out all inactive sectors
        if (!this.activated && fadeOutOnEnter)
            fadeOut = true;

        this.Interact();

        if (Entered != null)
            Entered(this);
        //Debug.Log("ISIN " + sectorID + " " + this.GetType());
    }
    protected virtual void DoOnStay(Collider coll)
    {
        fadeOutTime = DateTime.Now; // don't fade out while in sector
                                    //print("ID: " + sectorID);
    }
    protected virtual void DoOnExit(Collider coll)
    {
    }
    #endregion
}