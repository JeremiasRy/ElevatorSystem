using ElevatorSystem.Src.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.GuiObjects;

public class Elevator : GuiObject
{   
    public ElevatorCall TaskAtHand { get; set; } = new();

    public void Move()
    {
        if (!TaskAtHand.FinishedFrom)
        {
            Y = TaskAtHand.From > Y ? Y + 1 : Y - 1;
            TaskAtHand.FinishedFrom = Y == TaskAtHand.From;
            return;
        }
        if (!TaskAtHand.FinishedTo)
        {
            Y = TaskAtHand.To > Y ? Y + 1 : Y - 1;
            TaskAtHand.FinishedTo = Y == TaskAtHand.To;
            return;
        }
    }
    public Elevator(Graphic graphic, int id, int y, int x) : base(graphic, id)
    {
        X = x;
        Y = y;
        TaskAtHand = new()
        {
            FinishedFrom = true,
            FinishedTo = true,
        }; 
    }
}
