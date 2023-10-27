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
    public (int Row, int Col)[] ElevatorPositions => _elevatorControllers
        .Select(shaft => (shaft.ElevatorPosition.ElevatorRow, shaft.ElevatorPosition.ElevatorColumn))
        .ToArray();
    public (int Id, int Floor)[] ElevatorsAtFloor => _elevatorControllers
        .Where(shaft => shaft.ElevatorPosition.ElevatorRow != -1)
        .Select(elevatorController => (elevatorController.Id, elevatorController.ElevatorPosition.ElevatorFloor)).ToArray();
    public ElevatorData GetElevatorData(int id)
    {
        var controller = _elevatorControllers.FirstOrDefault(elevatorController => elevatorController.Id == id) ?? throw new Exception("Things are exploding");
        return ElevatorData.FromElevatorController(controller);
    } 
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
    public void ActivateFloorCall(int floor) 
    {
        foreach (var floorController in _floorControllers)
        {
            floorController.OpenPanel = floorController.NthFloor == floor;
        }
    }
    public void CallElevator(int floor, UserCall.Direction direction)
    {
        var floorController = _floorControllers.FirstOrDefault(floorController => floorController.NthFloor == floor) ?? throw new ArgumentException("Things shouldn't explode here");
        if (direction == UserCall.Direction.Up && floorController.UpCallState == FloorCallState.Idle)
        {
            floorController.UpCallState = FloorCallState.Active;
        } else if (direction == UserCall.Direction.Down && floorController.DownCallState == FloorCallState.Idle)
        {
            floorController.DownCallState = FloorCallState.Active;
        }
        floorController.OpenPanel = false;
    }
    public void CallElevatorPanelInput(int floor, int elevatorIdx)
    {
        _elevatorControllers[elevatorIdx].HandleElevatorInputPanelRequest(floor);
    }
    void HandleRequests()
    {
        var activeFloors = _floorControllers.Where(floorController => floorController.DownCallState == FloorCallState.Active || floorController.UpCallState == FloorCallState.Active);

        if (activeFloors.Any())
        {
            foreach (var floorController in activeFloors)
            {
                var freeElevatorsNearby = _elevatorControllers
                    .Where(elevator => !elevator.IsBusy)
                    .OrderBy(elevator => Math.Abs(elevator.ElevatorPosition.ElevatorFloor - floorController.NthFloor));

                if (!freeElevatorsNearby.Any())
                {
                    break;
                }
                freeElevatorsNearby.First().SuggestFloorCall(floorController);
            }
        }
        foreach (var elevatorController in _elevatorControllers)
        {
            elevatorController.EveryoneIsBusy = _elevatorControllers.All(elevator => elevator.IsBusy);
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
