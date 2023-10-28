using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Primitives
{
    public interface ITransformable
    {
        void Translate(Point3D vec);
        void Scale(Point3D vec);
        void Rotate(Point3D vec);
        void Draw(Graphics g, Projection pr = 0, Pen pen = null);
        ITransformable Clone();
    }
}
