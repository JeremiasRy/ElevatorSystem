using ElevatorSystem.Src.GuiObjects;
using System.Xml.Serialization;

namespace ElevatorSystem.Src;

public class ElevatorStateHandler
{
    private readonly Elevator[] _elevators;
    private readonly List<ElevatorCall> _callTable = new();
    public void CallElevator(Floor from, Floor to)
    {
        _callTable.Add(new ElevatorCall(from, to));
    }
    public void GiveInstructionsToElevators()
    {
        var calls = _callTable.Where(call => !call.IsAssigned).OrderBy(call => call.Id).ToList();
        if (!calls.Any())
        {
            return;
        }
        
        foreach (var elevator in _elevators)
        {
            if (CheckIfCallMatchesElevatorLocation(calls, elevator, out ElevatorCall? call))
            {
                CheckWhichCallsCanBeDoneWithinCall(calls, call ?? throw new Exception("Call shouldn't be null?"));
            }
        }
    }
    static bool CheckIfCallMatchesElevatorLocation(List<ElevatorCall> calls, Elevator elevator, out ElevatorCall? call) 
    {
        call = calls.FirstOrDefault(call => call.From.RowAdjustedForElevator == elevator.Row);
        if (call is not null) 
        {
            return calls.Remove(call);
        };
        return false;
    }
    static void CheckWhichCallsCanBeDoneWithinCall(List<ElevatorCall> calls, ElevatorCall call)
    {
        calls = calls.Where(c => c.Direction == call.Direction).ToList();
        if (call.Direction == ElevatorCall.CallDirection.Up)
        {
            calls = calls.Where(c => c.From.NthFloor >= call.From.NthFloor && c.To.NthFloor <= call.To.NthFloor).ToList();
        } else
        {
            calls = calls.Where(c => c.From.NthFloor <= call.From.NthFloor && c.To.NthFloor >= call.To.NthFloor).ToList();
        }
    }
    static List<ElevatorInstruction> CreateInstructions(List<ElevatorCall> calls)
    {

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
