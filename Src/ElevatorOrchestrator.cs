using ElevatorSystem.Src.Controllers;
using ElevatorSystem.Src.Data;
using ElevatorSystem.Src.Inputs;
using ElevatorSystem.Src.Simulation;
using static ElevatorSystem.Src.Constants;

namespace ElevatorSystem.Src;

public class ElevatorOrchestrator
{
    readonly Constants _constants = new();
    readonly ElevatorController[] _elevatorControllers = new ElevatorController[ELEVATOR_COUNT];
    readonly FloorController[] _floorControllers;
    public (int Row, int Column)[] GetElevatorPositions() => _elevatorControllers.Select(controller => controller.ElevatorPosition()).ToArray();
    public ElevatorController GetElevatorControllerById(int id) => _elevatorControllers.FirstOrDefault(elevatorController => elevatorController.Id == id) ?? throw new Exception("No such elevator!!!");
    public ElevatorData GetElevatorData(int id)
    {
        var controller = _elevatorControllers.FirstOrDefault(elevatorController => elevatorController.Id == id) ?? throw new Exception("Things are exploding");
        return controller.ReturnData();
    } 
    public List<ElevatorData> GetElevatorDataPoints()
    {
        return _elevatorControllers.Select(controller =>controller.ReturnData()).ToList();
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
    }
    public void CallElevator(int floor, UserCall.Direction direction)
    {

    }
    public void CallElevatorPanelInput(int floor, int elevatorId)
    {

    }
    void HandleRequests()
    {
        // do some conducting
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
