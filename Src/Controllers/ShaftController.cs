using ElevatorSystem.Src.Inputs;
using ElevatorSystem.Src.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Simulation.Floor;

namespace ElevatorSystem.Src.Controllers;

public class ShaftController
{
    readonly Constants _constants = new();
    readonly Elevator _elevator;
    readonly List<ElevatorFloorCall> _callTable;
    int _currentDestination = -1;
    public int Column { get; init; }
    public (int ElevatorRow, int ElevatorColumn) ElevatorPosition => (_elevator.Row - ELEVATOR_HEIGHT, Column + PADDING);
    public void MoveElevator()
    {

    }

    public ShaftController(Elevator elevator, int col)
    {
        _elevator = elevator;
        Column = col;
        _callTable = new List<ElevatorFloorCall>(_constants.FloorCount);
        for (int i = 0; i < _constants.FloorCount; i++)
        {
            _callTable.Add(new ElevatorFloorCall()
            {
                Floor = i,
                Active = false,
            });
        }
    }
}
