using ElevatorSystem.Src.Data;
using ElevatorSystem.Src.Simulation;

namespace ElevatorSystem.Src.Controllers;
public class ElevatorController
{
    static int _idCount = 1;
    readonly int _column;
    readonly ElevatorData _data = new();
    readonly Objective _primaryObjective = new();
    readonly Objective _overrideObjective = new();
    readonly Constants _constants = new();
    readonly Elevator _elevator;
    readonly Dictionary<int, bool> _elevatorInputPanel;
    int _doorOpenSequence = 0;
    public readonly int Id;
    Objective ActiveObject() => _overrideObjective.Destination > -1 
        ? _overrideObjective 
        : _primaryObjective;
    bool ClearCurrentFloorFromInputPanel()
    {
        if (ElevatorFloor() == -1)
        {
            return false;
        }
        _elevatorInputPanel[ElevatorFloor()] = false;
        return true;
    }

    bool ActiveInputPanelValue() => _elevatorInputPanel.Values.Any(value => value);
    /// <summary>
    /// Distance from floor in rows, doesn't show direction just raw distance
    /// </summary>
    /// <param name="floor">Nth floor to compare to</param>
    /// <returns>rows in int</returns>
    public int DistanceFromFloor(int floor)
    {
        return Math.Abs(_constants.FloorPositions[floor] - _elevator.Row); 
    }
    public bool IsBusy() => ActiveObject().Destination > -1;
    public bool IsIdleAtFloor() => _doorOpenSequence > 0;
    public bool ActivateInputPanelValue(int floor)
    {
        if (floor == ElevatorFloor() || floor > _constants.FloorCount - 2)
        {
            return false;
        }
        _elevatorInputPanel[floor] = true;
        return true;
    } 
    public void ReceiveFloorCall(FloorController activeFloor, bool everyoneIsBusy)
    {
        if (ActiveObject().Destination > -1 && ActiveObject().Source == SourceOfRequest.InputPanel && everyoneIsBusy)
        {
            var myDirection = _constants.FloorPositions[ActiveObject().Destination] - _elevator.Row;
            if (myDirection < 0 && activeFloor.UpCallState == FloorController.FloorCallState.Active)
            {
                _overrideObjective.Destination = activeFloor.NthFloor;
                _overrideObjective.Source = SourceOfRequest.FloorCall;
                _overrideObjective.SetClearMethod(activeFloor.SetUpCallStateToIdle);
            }
            if (myDirection > 0 && activeFloor.DownCallState == FloorController.FloorCallState.Active)
            {
                _overrideObjective.Destination = activeFloor.NthFloor;
                _overrideObjective.Source = SourceOfRequest.FloorCall;
                _overrideObjective.SetClearMethod(activeFloor.SetDownCallStateToIdle);
            }
            return;
        }
        if (ActiveObject().Destination > -1)
        {
            return;
        }
        _primaryObjective.Source = SourceOfRequest.FloorCall;
        _primaryObjective.Destination = activeFloor.NthFloor;
        
        if (activeFloor.DownCallState == FloorController.FloorCallState.Active)
        {
            activeFloor.SetDownCallStateToAssigned();
            _primaryObjective.SetClearMethod(activeFloor.SetDownCallStateToIdle);
        } else if (activeFloor.UpCallState == FloorController.FloorCallState.Active)
        {
            activeFloor.SetUpCallStateToAssigned();
            _primaryObjective.SetClearMethod(activeFloor.SetUpCallStateToIdle);
        }
    }
    public void HandleInputPanel()
    {
        if (ElevatorFloor() == -1)
        {
            return;
        }
        var panelValueToDo = _elevatorInputPanel.Where(kv => kv.Value).OrderBy(kv => Math.Abs(_constants.FloorPositions[kv.Key] - ElevatorFloor())).First().Key;
        if (_primaryObjective.Destination != -1 && _primaryObjective.Source == SourceOfRequest.FloorCall)
        {
            _overrideObjective.Destination = panelValueToDo;
            _overrideObjective.Source = SourceOfRequest.InputPanel;
            _overrideObjective.SetClearMethod(ClearCurrentFloorFromInputPanel);
            return;
        }
        _primaryObjective.SetClearMethod(ClearCurrentFloorFromInputPanel);
        _primaryObjective.Destination = panelValueToDo;
        _primaryObjective.Source = SourceOfRequest.InputPanel;
    }
    public void Tick()
    {
        if (ActiveInputPanelValue())
        {
            HandleInputPanel();
        }
        if (_doorOpenSequence > 0)
        {
            _doorOpenSequence--;
            return;
        }
        if (ActiveObject().Destination == -1)
        {
            return;
        }
        var destinationRow = _constants.FloorPositions[ActiveObject().Destination];
        if (destinationRow == _elevator.Row)
        {
            _doorOpenSequence = 7;
            ActiveObject().Complete();
            return;
        }
        _elevator.Input(destinationRow < _elevator.Row ? -1 : 1);
    }
    /// <summary>
    /// Return the current floor of the elevator
    /// </summary>
    /// <returns>Nth floor or -1 if between floors</returns>
    public int ElevatorFloor() => Array.IndexOf(_constants.FloorPositions, _elevator.Row);
    public (int Row, int Col, int DoorState) ElevatorPosition() => (_elevator.Row, _column, _doorOpenSequence);
    public ElevatorData ReturnData()
    {
        _data.DestinationRow = ActiveObject().Destination == -1 ? -1 : _constants.FloorPositions[ActiveObject().Destination];
        _data.DestinationFloor = ActiveObject().Destination;
        _data.CurrentLocation = _elevator.Row;
        _data.InputPanel = _elevatorInputPanel.Values.ToArray();
        return _data;
    }
    public ElevatorController(Elevator elevator, int col)
    {
        Id = _idCount++;
        _elevator = elevator;
        _column = col;
        _elevatorInputPanel = new Dictionary<int, bool>();
        for (int i = 0; i < _constants.FloorCount; i++)
        {
            _elevatorInputPanel.Add(i, false);
        }
    }
    internal class Objective
    {
        public delegate bool CompleteMethod();
        /// <summary>
        /// Destination floor of the objective
        /// </summary>
        public int Destination { get; set; } = -1;
        public SourceOfRequest Source { get; set; }
        CompleteMethod? _objectiveCompleteMethod;
        public void SetClearMethod(CompleteMethod clearMethod)
        {
            _objectiveCompleteMethod = clearMethod;
        }
        public void Complete()
        {
            _objectiveCompleteMethod?.Invoke();
            Destination = -1;
        }
    }
    
    public enum SourceOfRequest
    {
        InputPanel,
        FloorCall
    }
}
