using ElevatorSystem.Src.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.GuiObjects;

public class GuiObject
{
    public int Row { get; protected set; }
    public int Column { get; protected set; }
    private readonly Graphic _graphic;
    public PicturePixel[] Pixels => _graphic.GetGraphicInPlace(Row, Column);
    public GuiObject(Graphic graphic)
    {
        _graphic = graphic;
    }
}
