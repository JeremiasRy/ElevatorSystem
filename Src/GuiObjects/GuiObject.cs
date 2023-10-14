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
    protected readonly int _y = 0;
    protected readonly int _x = 0;
    private readonly Graphic _graphic;
    public void Draw() => _graphic.Draw(_y, _x);
    public GuiObject(Graphic graphic, int id)
    {
        _id = id;
        _graphic = graphic;
    }
}
