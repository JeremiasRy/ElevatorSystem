using ElevatorSystem.Src.Simulation;
using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Controllers.FloorController;

namespace ElevatorSystem.Src.Controllers;
public class ElevatorController
{
    int _currentDestination = -1;
    int _doorOpenSequence = 0;
    public readonly int Id;
    readonly Constants _constants = new();
    readonly Elevator _elevator;
    readonly FloorController[] _floorControllers; 
    /// <summary>
    /// Input panel Key is floor number Value is active
    /// </summary>
    readonly Dictionary<int, bool> _elevatorInputPanel;
    public int Column { get; init; }
    public (int ElevatorRow, int ElevatorColumn) ElevatorPosition => (_elevator.Row - ELEVATOR_HEIGHT, Column + PADDING);
    FloorController? Destination => _floorControllers.Where(floorController => floorController.NthFloor == _currentDestination).FirstOrDefault();
    public void HandleElevatorInputPanelRequest(int requestFloor)
    {
        if (AtFloor(out int floor))
        {
            if (requestFloor == floor)
            {
                return;
            }
        }
        if (_elevatorInputPanel.TryGetValue(floor, out bool active))
        {
            _elevatorInputPanel[floor] = !active || active;
        }
    }
    public void TakeAction()
    {
        PrioritizeRequest();
        Move();
    }
    void PrioritizeRequest()
    {
        if (_doorOpenSequence-- > 0)
        {
            return;
        }
        if (Destination is not null && Destination.Row == _elevator.Row)
        {
            if (Destination.UpCallState == FloorCallState.ElevatorAssigned)
            {
                Destination.UpCallState = FloorCallState.Idle;
            }
            else if (Destination.DownCallState == FloorCallState.ElevatorAssigned)
            {
                Destination.DownCallState = FloorCallState.Idle;
            }
            _currentDestination = -1;
            _doorOpenSequence = 5;
            return;
        }

        if (_currentDestination > -1)
        {
            return;
        }

        var activeFloors = _floorControllers.Where(floorController => floorController.DownCallState == FloorCallState.Active || floorController.UpCallState == FloorCallState.Active);

        if (ActiveInputPanelValue())
        {
            IEnumerable<int> requestedFloors = _elevatorInputPanel.Where(request => request.Value).Select(request => request.Key);
            if (_currentDestination == -1)
            {
                if (AtFloor(out int floor))
                {
                    var nearestDestination = requestedFloors
                        .Select(floorRequest => new { RequestedFloor = floorRequest, DistanceFromCurrentPosition = Math.Abs(floorRequest - floor) })
                        .Min(requestObject => requestObject.DistanceFromCurrentPosition);
                    _currentDestination = nearestDestination;
                    return;
                }
            }

        }

        if (activeFloors.Any())
        {
            CheckIfCanAnswerFloorCallWithoutMoving(activeFloors);
            var destinationFloor = activeFloors.First();
            _currentDestination = destinationFloor.NthFloor;
            if (destinationFloor.DownCallState == FloorCallState.Active)
            {
                destinationFloor.DownCallState = FloorCallState.ElevatorAssigned;
            } 
            else if (destinationFloor.UpCallState == FloorCallState.Active)
            {
                destinationFloor.UpCallState = FloorCallState.ElevatorAssigned;
            }
            return;
        }
    }
    void Move()
    {
        if (Destination is null)
        {
            _elevator.Input(0);
            return;
        }
        if (Destination is not null && Destination.Row == _elevator.Row)
        {
            _elevator.Input(0);
            return;
        }

        Elevator.Motor motor = Destination!.Row - _elevator.Row < 0 
            ? Elevator.Motor.Up 
            : Elevator.Motor.Down;

        _elevator.Input(motor);
    }
    void CheckIfCanAnswerFloorCallWithoutMoving(IEnumerable<FloorController> activeFloors)
    {
        if (AtFloor(out int floor))
        {
            var floorMatch = activeFloors.FirstOrDefault(floorController => floorController.NthFloor == floor);
            if (floorMatch is not null)
            {
                // Elevator was available in the floor only take one of the calls if there are two
                if (floorMatch.UpCallState == FloorCallState.Active)
                {
                    floorMatch.UpCallState = FloorCallState.Idle;
                } 
                else if (floorMatch.DownCallState == FloorCallState.Active)
                {
                    floorMatch.DownCallState = FloorCallState.Idle;
                }
            }
        }
    }
    bool ActiveInputPanelValue()
    {
        return _elevatorInputPanel.Values.Any(value => value);
    }
    public bool AtFloor(out int floor)
    {
        floor = -1;
        var result = _floorControllers.FirstOrDefault(floorController => floorController.Row == _elevator.Row);
        if (result is not null)
        {
            floor = result.NthFloor;
        }
        return floor > -1;
    }
    static int _idCount = 1;
    public ElevatorController(Elevator elevator, int col, FloorController[] floorControllers)
    {
        Id = _idCount++;
        _elevator = elevator;
        Column = col;
        _elevatorInputPanel = new Dictionary<int, bool>();
        for (int i = 0; i < _constants.FloorCount; i++)
        {
            _elevatorInputPanel.Add(i, false);
        }
        _floorControllers = floorControllers;
    }
}
