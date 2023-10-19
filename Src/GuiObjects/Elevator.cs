using ElevatorSystem.Src.Graphics;

namespace ElevatorSystem.Src.GuiObjects;

public class Elevator : GuiObject
{
    public const int FORCE_TO_MOVE = 50;
    public const int DISTANCE_TO_START_DECELARATE = 30;
    public const int MAX_FORCE = 200;
    int _force = 0;
    int _forceIncrement = 0;

    public void ReceiveInstruction(ElevatorInstruction instruction) => throw new NotImplementedException();
    public void Move()
    {
        
    }
    void Accelerate() => _forceIncrement += 10;
    void Decelerate() => _forceIncrement -= 20;

    public void BrutalForceMove(int row, int col)
    {
        Row = row;
        Column = col;
    }
    public Elevator(Graphic graphic) : base(graphic)
    {
    }
    public enum Direction
    {
        Up,
        Down
    }
}
