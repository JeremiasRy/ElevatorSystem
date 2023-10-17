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
    private readonly Queue<ElevatorCall> _taskQueue = new();

    public void GiveTaskToElevator()
    {
        if (_taskQueue.Count == 0)
        {
            return;
        }

        foreach (var elevator in _elevators)
        {
            if (elevator.TaskAtHand.Finished)
            {
                elevator.TaskAtHand = _taskQueue.Dequeue();
            }
            if (_taskQueue.Count == 0)
            {
                break;
            }
        }
    }
    public void MoveElevatorsTowardsTheirTask()
    {
        foreach (var elevator in _elevators)
        {
            elevator.Move();
        }
    }
    public void CallElevator(int from, int to)
    {
        _taskQueue.Enqueue(new ElevatorCall()
        {
            From = _floors[from].ElevatorPositions.First().y,
            To = _floors[to].ElevatorPositions.First().y
        });
    }
    public ElevatorStateHandler(List<Floor> floors, List<Elevator> elevators)
    {
        _floors = floors;
        _elevators = elevators;
    }
}
