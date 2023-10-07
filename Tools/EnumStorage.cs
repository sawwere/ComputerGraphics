﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    /// <summary>
    /// Тип положения ребра для определения принадлежности точки полигону
    /// КАСАТЕЛbНОЕ, ПЕРЕСЕКАЮЩЕЕ, НЕСУЩЕСТВЕННОЕ
    /// </summary>
    public enum MyEdge { TOUCHING, CROSSING, INESSENTIAL };
    public enum EdgeIntersectionType { PARRALEL, INTERSECT, NOT_INTERSECT, COLLINEAR };
    public enum PointRealtiveToPolygon { INSIDE, OUTSIDE, BOUNDARY };
    public enum classifyEnum { LEFT, RIGHT, BEYOUND, BEHIND, BETWEEN, ORIGIN, DESTINATION};
}