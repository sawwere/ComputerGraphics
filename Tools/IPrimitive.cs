using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public interface IPrimitive
    {
        System.Drawing.Color Color { get; set; }

        float X { get; }
        float Y { get; }

        void Draw(System.Drawing.Graphics g); 
    }
}
