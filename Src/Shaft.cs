using ElevatorSystem.Src.GuiObjects;

namespace ElevatorSystem.Src;

public class Shaft
{
    private readonly Elevator _elevator;
    public int Column { get; init; }
    public (int Row, int Col) ElevatorPosition => (_elevator.Row, Column + 2);
    public Shaft(Elevator elevator)
    {
        _elevator = elevator;
    }
}
