using UnityEngine;
using System.Collections;
using System;

public struct DVector
{
    public double x, y;
    public DVector(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}

/// <summary>
/// calculates the actual player position on the map via mercator projection
/// </summary>
public class GeoCalculations : MonoBehaviour
{
    public DVector SWCorner, NECorner, centerPoint;

    double width = 1280;
    double height = 1280;

    public void CalcCorners(double x, double y)
    {
        var centerPoint = new DVector(x, y);
        var zoom = 16f;
        MercatorProjection();
        GetCorners(centerPoint, zoom, width, height);
    }

    double MERCATOR_RANGE = 256;

    double bound(double value, double opt_min, double opt_max)
    {
        if (opt_min != null) value = Math.Max(value, opt_min);
        if (opt_max != null) value = Math.Min(value, opt_max);
        return value;
    }

    double degreesToRadians(double deg)
    {
        return deg * (Math.PI / 180);
    }

    double radiansToDegrees(double rad)
    {
        return rad / (Math.PI / 180);
    }

    DVector pixelOrigin_;
    double pixelsPerLonDegree_;
    double pixelsPerLonRadian_; 

    void MercatorProjection()
    {
        pixelOrigin_ = new DVector(MERCATOR_RANGE / 2, MERCATOR_RANGE / 2);
        pixelsPerLonDegree_ = MERCATOR_RANGE / 360;
        pixelsPerLonRadian_ = MERCATOR_RANGE / (2 * Math.PI);
    }

    DVector FromLatLngToPoint(DVector latLng)
    {
        var me = this;
        var point = new DVector(0, 0);

        var origin = me.pixelOrigin_;
        point.x = origin.x + latLng.y * me.pixelsPerLonDegree_;
        // NOTE(appleton): Truncating to 0.9999 effectively limits latitude to
        // 89.189.  This is about a third of a tile past the edge of the world tile.
        var siny = bound(Math.Sin(degreesToRadians(latLng.x)), -0.9999d, 0.9999d);
        point.y = origin.y + 0.5 * Math.Log((1 + siny) / (1 - siny)) * -me.pixelsPerLonRadian_;
        return point;
    }

    DVector FromPointToLatLng(DVector point)
    {
        var me = this;

        var origin = me.pixelOrigin_;
        var lng = (point.x - origin.x) / me.pixelsPerLonDegree_;
        var latRadians = (point.y - origin.y) / -me.pixelsPerLonRadian_;
        var lat = radiansToDegrees(2 * Math.Atan(Math.Exp(latRadians)) - Math.PI / 2);
        return new DVector(lat, lng);
    }

    public void GetCorners(DVector center, double zoom, double mapWidth, double mapHeight)
    {
        var scale = Math.Pow(2, zoom);
        var centerPx = FromLatLngToPoint(center);
        centerPoint = centerPx;
        var SWPoint = new DVector((centerPx.x - (mapWidth / 2) / scale), (centerPx.y + (mapHeight / 2) / scale));
        var SWLatLon = FromPointToLatLng(SWPoint);
        SWCorner = SWLatLon;

        //Debug.Log("SW: " + SWLatLon.x + " " + SWLatLon.y);
        var NEPoint = new DVector((centerPx.x + (mapWidth / 2) / scale), (centerPx.y - (mapHeight / 2) / scale));
        var NELatLon = FromPointToLatLng(NEPoint);
        NECorner = NELatLon;
        //Debug.Log(" NE: " + NELatLon.x + " " + NELatLon.y);
    }

    public DVector GetPixelPos(DVector center)
    {
        var centerPx = FromLatLngToPoint(center);

        return centerPx;
    }
}