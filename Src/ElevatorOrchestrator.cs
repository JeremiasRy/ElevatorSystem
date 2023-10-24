using ElevatorSystem.Src.Controllers;
using ElevatorSystem.Src.Data;
using ElevatorSystem.Src.Inputs;
using ElevatorSystem.Src.Simulation;
using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Controllers.FloorController;

namespace ElevatorSystem.Src;

public class ElevatorOrchestrator
{
    readonly Constants _constants;
    readonly ElevatorController[] _elevatorControllers = new ElevatorController[ELEVATOR_COUNT];
    readonly FloorController[] _floorControllers;
    public (int Row, int Col)[] ElevatorPositions => _elevatorControllers.Select(shaft => shaft.ElevatorPosition).ToArray();
    public List<ElevatorData> GetElevatorDataPoints()
    {
        return _elevatorControllers.Select(controller => ElevatorData.FromElevatorController(controller)).ToList();
    }
    public List<FloorData> GetFloorDataPoints()
    {
        return _floorControllers.Select(controller => FloorData.FromFloorController(controller)).ToList();
    }
    public void Tick()
    {
        HandleRequests();
    }
    public void ActivateFloorCall(FloorCallInput request)
    {
        FloorController floor = _floorControllers.FirstOrDefault(floor => floor.NthFloor == request.Floor) ?? throw new ArgumentException($"No such floor {request.Floor}");
        if (request.RequestDirection == FloorCallInput.Direction.Up)
        {
            if (floor.UpCallState == FloorCallState.NotAvailable || floor.UpCallState == FloorCallState.ElevatorAssigned)
            {
                // Info not available
                return;
            }
            floor.UpCallState = FloorCallState.Active;
            return;
        }
        if (request.RequestDirection == FloorCallInput.Direction.Down || floor.DownCallState == FloorCallState.ElevatorAssigned)
        {
            if (floor.DownCallState == FloorCallState.NotAvailable)
            {
                // Info not available
                return;
            }
            floor.DownCallState = FloorCallState.Active;
        }
    }
    public void CallElevatorPanelInput(int floor, int elevatorIdx)
    {
        _elevatorControllers[elevatorIdx].HandleElevatorInputPanelRequest(floor);
    }
    void HandleRequests()
    {
        foreach (var elevatorController in _elevatorControllers)
        {
            elevatorController.TakeAction();
        }
    }
    public ElevatorOrchestrator()
    {
        _constants = new Constants();
        _floorControllers = new FloorController[_constants.FloorCount - 1];
        for (int i = 0; i < _constants.FloorPositions.Length - 1; i++)
        {
            _floorControllers[i] = new FloorController(i, _constants.FloorPositions[i]);
        }
        for (int i = 0; i < _elevatorControllers.Length; i++)
        {
            _elevatorControllers[i] = new ElevatorController(new Elevator(), _constants.ShaftPositions[i], _floorControllers);
        }
    }
}
