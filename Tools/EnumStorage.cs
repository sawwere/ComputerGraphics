using System;
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
    public enum classifyEnum { LEFT, RIGHT, BEYOND, BEHIND, BETWEEN, ORIGIN, DESTINATION};

    public enum Axis { AXIS_X, AXIS_Y, AXIS_Z, CUSTOM };
    public enum Projection { PERSPECTIVE = 0, ISOMETRIC, ORTHOGR_X, ORTHOGR_Y, ORTHOGR_Z };
}
