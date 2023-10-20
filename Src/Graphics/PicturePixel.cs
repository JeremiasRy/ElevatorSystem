using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class PicturePixel
{
    public int OffsetColumn { get; }
    public int OffsetRow { get; }
    public char Ch { get; }
    public PicturePixel(int y, int x, char ch)
    {
        OffsetColumn = x;
        OffsetRow = y;
        Ch = ch;
    }
}
