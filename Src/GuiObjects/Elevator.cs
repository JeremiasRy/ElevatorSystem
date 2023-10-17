using ElevatorSystem.Src.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.GuiObjects;

public class Elevator : GuiObject
{
    public int ShaftIndex { get; set; }
    public int Floor { get; set; }

    public void Move(int y, int x)
    {
        Y = y;
        X = x;
    }
    public Elevator(Graphic graphic, int id, int shaft, int floor) : base(graphic, id)
    {
        ShaftIndex = shaft;
        Floor = floor;
    }
}
