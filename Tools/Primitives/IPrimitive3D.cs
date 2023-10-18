using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    interface IPrimitive3D
    {
        System.Drawing.Color Color { get; set; }

        float X { get; }
        float Y { get; }
        float Z { get; }
    }
}
