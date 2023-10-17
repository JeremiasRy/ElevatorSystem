using ElevatorSystem.Src.GuiObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorStateHandler
{
    private readonly List<Floor> _floors;
    private readonly List<Elevator> _elevators;

    public void MoveElevatorsToPlace()
    {
        foreach (var elevator in _elevators)
        {
            var (y, x) = _floors.FirstOrDefault(floor => floor.NthFloor == elevator.Floor)!.ElevatorPositions[elevator.ShaftIndex];
            elevator.Move(y, x);
        }
    }
    public void CallElevator(int floor)
    {
        _elevators.First().Floor = floor;
    }
    public ElevatorStateHandler(List<Floor> floors, List<Elevator> elevators)
    {
        _floors = floors;
        _elevators = elevators;
    }
}
