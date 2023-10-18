using ElevatorSystem.Src.GuiObjects;

namespace ElevatorSystem.Src;

public class ElevatorStateHandler
{
    private readonly Elevator[] _elevators;
    private readonly Queue<ElevatorCall> _callQueue = new();

    public void CallElevator(Floor from, Floor to)
    {
        _callQueue.Enqueue(new ElevatorCall(from, to));
    }
    public void GiveTasksToElevators()
    {
        if (!_callQueue.Any())
        {
            return;
        }
        var freeElevator = _elevators.FirstOrDefault(elevator => elevator.TaskAtHand is null || elevator.TaskAtHand.Complete);
        if (freeElevator is not null)
        {
            freeElevator.TaskAtHand = _callQueue.Dequeue();
        }
    }
    public void MoveElevators()
    {
        foreach(var elevator in _elevators)
        {
            elevator.Move();
        }
    }
    public ElevatorStateHandler(Elevator[] elevators)
    {
        _elevators = elevators;
    }
}
