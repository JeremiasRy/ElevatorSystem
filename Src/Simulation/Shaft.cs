using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Simulation;

public class Shaft
{
    private readonly Elevator _elevator;
    public int Column { get; init; }
    public int ElevatorHeight => _elevator.Row;
    public void InputElevator(Elevator.Motor motor)
    {
        _elevator.Input(motor);
    }
    public Shaft(Elevator elevator, int col)
    {
        _elevator = elevator;
        Column = col;
    }
}
