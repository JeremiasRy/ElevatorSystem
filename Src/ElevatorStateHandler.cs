using ElevatorSystem.Src.GuiObjects;

namespace ElevatorSystem.Src;

public class ElevatorStateHandler
{
    private readonly Elevator[] _elevators;
    private readonly Floor[] _floors;
    private readonly List<ElevatorInstruction> _instructions;
    private readonly List<UserInput> _inputs;
    public void CallElevator(int from)
    {
        var floor = _floors.FirstOrDefault(floor => floor.NthFloor == from);
        if (floor is null)
        {
            //Info that theres no such floor
            return;
        }

    }
    public void TakeAction()
    {
        GiveInstructionsToElevators();
        MoveElevators();
    }
    void GiveInstructionsToElevators()
    {
    }
    void MoveElevators()
    {
        foreach(var elevator in _elevators)
        {
            elevator.Move();
        }
    }
    public ElevatorStateHandler(Elevator[] elevators, Floor[] floors)
    {
        _floors = floors;
        _elevators = elevators;
    }
}
