using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphic;

class PicturePixel
{
    public int OffsetX { get; }
    public int OffsetY { get; }
    public char Ch { get; }

    public PicturePixel(int y, int x, char ch)
    {
        OffsetX = x;
        OffsetY = y;
        Ch = ch;
    }
}
