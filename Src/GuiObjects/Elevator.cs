using ElevatorSystem.Src.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.GuiObjects;

public class Elevator : GuiObject
{
    public int Floor { get; set; }
    public Elevator(Graphic graphic, int id) : base(graphic, id)
    {
    }
}
