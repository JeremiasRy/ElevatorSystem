using ElevatorSystem.Src.Controllers;
using ElevatorSystem.Src.Data;
using ElevatorSystem.Src.Inputs;
using ElevatorSystem.Src.Simulation;
using System.Linq;
using static ElevatorSystem.Src.Constants;

namespace ElevatorSystem.Src;

public class ElevatorOrchestrator
{
    readonly Constants _constants = new();
    readonly ElevatorController[] _elevatorControllers = new ElevatorController[ELEVATOR_COUNT];
    readonly FloorController[] _floorControllers;
    IEnumerable<FloorController> ActiveFloors() => _floorControllers.Where(floorController => floorController.IsActive());
    public (int Floor, int Id)[] GetElevatorsIdleAtFloor() => _elevatorControllers
        .Where(elevatorController => elevatorController.IsIdleAtFloor())
        .Select(elevatorController => (elevatorController.ElevatorFloor(), elevatorController.Id))
        .ToArray();
    public (int Row, int Column)[] GetElevatorPositions() => _elevatorControllers.Select(controller => controller.ElevatorPosition()).ToArray();
    public ElevatorController GetElevatorControllerById(int id) => _elevatorControllers.FirstOrDefault(elevatorController => elevatorController.Id == id) ?? throw new Exception("No such elevator!!!");
    public ElevatorData GetElevatorData(int id)
    {
        var controller = _elevatorControllers.FirstOrDefault(elevatorController => elevatorController.Id == id) ?? throw new Exception("Things are exploding");
        return controller.ReturnData();
    } 
    public List<ElevatorData> GetElevatorDataPoints()
    {
        return _elevatorControllers.Select(controller => controller.ReturnData()).ToList();
    }
    public FloorData GetFloorDataByFloor(int floor)
    {
        var controller = _floorControllers.FirstOrDefault(floorController => floorController.NthFloor == floor) ?? throw new Exception("Things are exploding");
        return controller.ReturnData();
    }
    public List<FloorData> GetFloorDataPoints()
    {
        return _floorControllers.Select(controller => controller.ReturnData()).ToList();
    }
    public void Tick()
    {
        HandleRequests();
        foreach (var elevatorController in _elevatorControllers)
        {
            elevatorController.Tick();
        }
    }
    public bool CallElevator(int floor, UserCall.Direction direction)
    {
        if (floor > _constants.FloorCount - 2)
        {
            return false;
        }
        var controller = _floorControllers[floor];
        if (direction == UserCall.Direction.Up)
        {
            return controller.SetUpCallStateToActive();
        } else
        {
            return controller.SetDownCallStateToActive();
        }
    }
    public bool CallElevatorPanelInput(int floor, int elevatorId)
    {
        var elevatorController = GetElevatorControllerById(elevatorId);
        return elevatorController.ActivateInputPanelValue(floor);
    }
    void HandleRequests()
    {
        if (!ActiveFloors().Any())
        {
            return;
        }

        var elevatorFloorPairs = _elevatorControllers
            .Select(elevatorController => new
            {
                ElevatorController = elevatorController,
                FloorControllerSortObject = ActiveFloors()
                .Select(controller => new
                {
                    controller,
                    distance = elevatorController.DistanceFromFloor(controller.NthFloor)
                })
                .OrderBy(comparisonObject => comparisonObject.distance)
                .First()
            })
            .OrderBy(monster => monster.FloorControllerSortObject.distance)
            .Select(monster => new 
            { 
                monster.ElevatorController, 
                FloorController = monster.FloorControllerSortObject.controller
            });

        var everyoneIsBusy = _elevatorControllers.All(elevatorController => elevatorController.IsBusy());

        foreach (var elevatorFloorPair in elevatorFloorPairs)
        {
            elevatorFloorPair.ElevatorController.ReceiveFloorCall(elevatorFloorPair.FloorController, everyoneIsBusy);
            if (!ActiveFloors().Any())
            {
                break;
            }
        }
    }
    public ElevatorOrchestrator()
    {
        _floorControllers = new FloorController[_constants.FloorCount - 1];
        for (int i = 0; i < _constants.FloorPositions.Length - 1; i++)
        {
            _floorControllers[i] = new FloorController(i, _constants.FloorPositions[i]);
        }
        for (int i = 0; i < _elevatorControllers.Length; i++)
        {
            _elevatorControllers[i] = new ElevatorController(new Elevator(), _constants.ShaftPositions[i]);
        }
    }
}
