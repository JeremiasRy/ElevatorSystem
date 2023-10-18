using ElevatorSystem.Src.Graphics;

namespace ElevatorSystem.Src.GuiObjects;

public class Elevator : GuiObject
{
    public ElevatorCall? TaskAtHand { get; set; }
    public void Move()
    {
        if (TaskAtHand is not null)
        {
            if (!TaskAtHand.FromCompleted)
            {
                Row = TaskAtHand.From.Row - 5 > Row ? Row + 1 : Row - 1;
                TaskAtHand.FromCompleted = Row == TaskAtHand.From.Row - 5;
                return;
            }
            if (!TaskAtHand.ToCompleted)
            {
                Row = TaskAtHand.To.Row - 5 > Row ? Row + 1 : Row - 1;
                TaskAtHand.ToCompleted = Row == TaskAtHand.To.Row - 5;
                return;
            }
        }
    }
    public void BrutalForceMove(int row, int col)
    {
        Row = row;
        Column = col;
    }
    public Elevator(Graphic graphic) : base(graphic)
    {
    }
}
