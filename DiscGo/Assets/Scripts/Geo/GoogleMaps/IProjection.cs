﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Drawing;

using Assets.GoogleMaps.Scripts;

namespace Projection
{
    public interface IProjection
    {
        PointF FromCoordinatesToPixel(PointF coordinates);
        PointF FromPixelToCoordinates(PointF pixel);
    }
}