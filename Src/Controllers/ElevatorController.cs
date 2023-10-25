using ElevatorSystem.Src.Simulation;
using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Controllers.FloorController;

namespace ElevatorSystem.Src.Controllers;
public class ElevatorController
{
    static int _idCount = 1;
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
    public bool[] ReturnInputPanelValues() => _elevatorInputPanel.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToArray();
    public int Column { get; init; }
    public (int ElevatorRow, int ElevatorColumn, int ElevatorFloor) ElevatorPosition => (_elevator.Row - ELEVATOR_HEIGHT, Column + PADDING, ElevatorIsStationaryAtAFloor());
    public FloorController? Destination => _floorControllers.Where(floorController => floorController.NthFloor == _currentDestination).FirstOrDefault();
    public void HandleElevatorInputPanelRequest(int requestFloor)
    {
        var check = ElevatorIsStationaryAtAFloor();
        if (check != -1)
        {
            if (requestFloor == ElevatorIsStationaryAtAFloor())
            {
                return;
            }
        }
        if (_elevatorInputPanel.TryGetValue(check, out bool active))
        {
            _elevatorInputPanel[check] = !active || active;
        }
    }
    public void TakeAction()
    {
        if(_doorOpenSequence-- > 0)
        {
            return;
        }
        PrioritizeRequest();
        Move();
    }
    void PrioritizeRequest()
    {
        if (Destination is not null && Destination.Row == _elevator.Row)
        {
            HandleCorrectFloor();
            return;
        }
        if (_currentDestination > -1)
        {
            return;
        }
        if (ActiveInputPanelValue())
        {
            HandleInputPanelRequests();
            return;
        }
        var activeFloors = _floorControllers.Where(floorController => floorController.DownCallState == FloorCallState.Active || floorController.UpCallState == FloorCallState.Active);

        if (activeFloors.Any())
        {
            HandleFloorCallRequests(activeFloors);
        }
    }
    void HandleFloorCallRequests(IEnumerable<FloorController> activeFloors)
    {
        CheckIfCanAnswerFloorCallWithoutMoving(activeFloors);
        if (!activeFloors.Any())
        {
            return;
        }
        var check = ElevatorIsStationaryAtAFloor();
        if (check != -1)
        {
            var destinationFloor = activeFloors
                .OrderBy(floorcontroller => Math.Abs(floorcontroller.NthFloor - check))
                .First();

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
    void HandleInputPanelRequests()
    {
        IEnumerable<int> requestedFloors = _elevatorInputPanel.Where(request => request.Value).Select(request => request.Key);
        if (_currentDestination == -1)
        {
            var check = ElevatorIsStationaryAtAFloor();
            if (check != -1)
            {
                var nearestDestination = requestedFloors
                    .OrderBy(request => Math.Abs(request - check))
                    .First();
                _currentDestination = nearestDestination;
                return;
            }
        }
    }
    void HandleCorrectFloor()
    {
        if (Destination!.UpCallState == FloorCallState.ElevatorAssigned)
        {
            Destination.UpCallState = FloorCallState.Idle;
        }
        else if (Destination.DownCallState == FloorCallState.ElevatorAssigned)
        {
            Destination.DownCallState = FloorCallState.Idle;
        }
        _currentDestination = -1;
        _doorOpenSequence = 5;
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
        var check = ElevatorIsStationaryAtAFloor();
        if (check != -1)
        {
            var floorMatch = activeFloors.FirstOrDefault(floorController => floorController.NthFloor == check);
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
                _doorOpenSequence = 5;
            }
        }
    }
    bool ActiveInputPanelValue() => _elevatorInputPanel.Values.Any(value => value);
    /// <summary>
    /// Checks if elevator is at a floor
    /// </summary>
    /// <returns>The nth floor elevator is at or else -1</returns>
    public int ElevatorIsStationaryAtAFloor()
    {
        var result = _floorControllers.FirstOrDefault(floorController => floorController.Row == _elevator.Row);
        if (result is not null)
        {
            return result.NthFloor;
        }
        return -1;
    }
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
