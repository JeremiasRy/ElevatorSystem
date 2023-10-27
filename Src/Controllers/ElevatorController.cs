using ElevatorSystem.Src.Simulation;
using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Controllers.FloorController;

namespace ElevatorSystem.Src.Controllers;
public class ElevatorController
{
    static int _idCount = 1;
    readonly Objective _primaryObjective = new ();
    int _intermediateObjective = -1;
    int _doorOpenSequence = 0;
    public readonly int Id;
    readonly Constants _constants = new();
    readonly Elevator _elevator;
    readonly FloorController[] _floorControllers; 
    /// <summary>
    /// Input panel Key is floor number Value is active
    /// </summary>
    readonly Dictionary<int, bool> _elevatorInputPanel;
    public bool IsBusy => _primaryObjective.Destination != -1;
    public bool[] InputPanelValues => _elevatorInputPanel.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToArray();
    public int Column { get; init; }
    public bool EveryoneIsBusy { get; set; }
    public (int ElevatorRow, int ElevatorColumn, int ElevatorFloor) ElevatorPosition => (_elevator.Row - ELEVATOR_HEIGHT, Column + PADDING, ElevatorIsStationaryAtAFloor());
    public FloorController? DestinationFloor => _floorControllers.Where(floorController => floorController.NthFloor == _primaryObjective.Destination).FirstOrDefault();
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
        if(_doorOpenSequence > 0)
        {
            _doorOpenSequence--;
            return;
        }
        PrioritizeRequest();
        Move();
    }
    void PrioritizeRequest()
    {
        if (DestinationFloor is not null && DestinationFloor.NthFloor == ElevatorIsStationaryAtAFloor())
        { 
            HandleCorrectFloor();
            return;
        }
       
        // keep on going forward. No need to handle more than 2 things at a time
        if (_intermediateObjective > -1)
        {
            if (ElevatorIsStationaryAtAFloor() == _intermediateObjective)
            {
                ChangeFloorCallStateToIdle(_floorControllers[_intermediateObjective]);
            }
            return;
        }
        var activeFloors = _floorControllers
            .Where(floorController => floorController.DownCallState == FloorCallState.Active || floorController.UpCallState == FloorCallState.Active)
            .OrderBy(floorController => Math.Abs(floorController.Row - _elevator.Row));

        // inputpanel is active and everyone is doing something so check if there is something to do
        if (_primaryObjective.Destination > -1 && _primaryObjective.Source == SourceOfRequest.InputPanel && EveryoneIsBusy && activeFloors.Any())
        {
            var direction = _floorControllers[_primaryObjective.Destination].Row - _elevator.Row;
            if (direction < 0)
            {
                var possiblefloorToAnswer = activeFloors.FirstOrDefault(floor => floor.UpCallState == FloorCallState.Active);
                if (possiblefloorToAnswer is not null)
                {
                    _intermediateObjective = possiblefloorToAnswer.NthFloor;
                    ChangeFloorCallStateToAssigned(_floorControllers[_intermediateObjective]);
                }
                return;
            }
            if (direction > 0)
            {
                var possiblefloorToAnswer = activeFloors.FirstOrDefault(floor => floor.DownCallState == FloorCallState.Active);
                if (possiblefloorToAnswer is not null)
                {
                    _intermediateObjective = possiblefloorToAnswer.NthFloor;
                    ChangeFloorCallStateToAssigned(_floorControllers[_intermediateObjective]);
                }
                return;
            }
        }
        if (ActiveInputPanelValue())
        {
            HandleInputPanelRequests();
            return;
        }
    }
    public void SuggestFloorCall(FloorController floorController)
    {
        var check = ElevatorIsStationaryAtAFloor();
        if (check != -1 && _primaryObjective.Destination == -1)
        {
            _primaryObjective.Destination = floorController.NthFloor;
            _primaryObjective.Source = SourceOfRequest.FloorCall;
            ChangeFloorCallStateToAssigned(floorController);
            return;
        }
    }
    void HandleInputPanelRequests()
    {
        IEnumerable<int> requestedFloors = _elevatorInputPanel.Where(request => request.Value).Select(request => request.Key);
        if (_primaryObjective.Destination == -1)
        {
            var check = ElevatorIsStationaryAtAFloor();
            if (check != -1)
            {
                var nearestDestination = requestedFloors
                    .OrderBy(request => Math.Abs(request - check))
                    .First();
                _primaryObjective.Destination = nearestDestination;
                _primaryObjective.Source = SourceOfRequest.InputPanel;
                return;
            }
            _elevatorInputPanel[check] = false;
        }
    }
    void HandleCorrectFloor()
    {
        ChangeFloorCallStateToIdle(DestinationFloor!);
        // also check input panel here
        _primaryObjective.Destination = -1;
        _doorOpenSequence = 5;
    }
    void Move()
    {
        if (_primaryObjective.Destination == -1)
        {
            return;
        }
        var destination = _intermediateObjective > -1 ? _floorControllers[_intermediateObjective] : _floorControllers[_primaryObjective.Destination];
        Elevator.Motor motor = destination.Row < _elevator.Row
            ? Elevator.Motor.Up 
            : Elevator.Motor.Down;

        _elevator.Input(motor);
    }
    static void ChangeFloorCallStateToIdle(FloorController floorController)
    {
        if (floorController.DownCallState == FloorCallState.ElevatorAssigned)
        {
            floorController.DownCallState = FloorCallState.Idle;
            return;
        } 
        if (floorController.UpCallState == FloorCallState.ElevatorAssigned)
        {
            floorController.UpCallState = FloorCallState.Idle;
            return;
        }
    }
    static void ChangeFloorCallStateToAssigned(FloorController floorController)
    {
        if (floorController.DownCallState == FloorCallState.Active)
        {
            floorController.DownCallState = FloorCallState.ElevatorAssigned;
            return;
        }
        if (floorController.UpCallState == FloorCallState.Active)
        {
            floorController.UpCallState = FloorCallState.ElevatorAssigned;
            return;
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
    internal class Objective
    {
        public int Destination { get; set; } = -1;
        public SourceOfRequest Source { get; set; }
    }
    public enum SourceOfRequest
    {
        InputPanel,
        FloorCall
    }
}
