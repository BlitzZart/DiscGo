﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GoogleMaps.Scripts
{
    public class PointF
    {
        public PointF(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public float X { get; set; }
        public float Y { get; set; }
    }
}