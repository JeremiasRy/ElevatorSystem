using ElevatorSystem.Src.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.GuiObjects;

public class GuiObject
{
    readonly int _id;
    protected int Y { get; set; }
    protected int X { get; set; }
    private readonly Graphic _graphic;
    public void Draw() => _graphic.Draw(Y, X);
    public GuiObject(Graphic graphic, int id)
    {
        _id = id;
        _graphic = graphic;
    }
}
