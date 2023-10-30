﻿using ElevatorSystem.Src.Data;
using ElevatorSystem.Src.Simulation;

namespace ElevatorSystem.Src.Controllers;
public class ElevatorController
{
    static int _idCount = 1;
    readonly ElevatorData _data = new();
    readonly Objective _primaryObjective = new();
    readonly Objective _intermediateObjective = new();
    Objective ActiveObject() => _intermediateObjective.Destination > -1 ? _intermediateObjective : _primaryObjective;
    readonly int _column;
    int _doorOpenSequence = 0;
    public readonly int Id;
    readonly Constants _constants = new();
    readonly Elevator _elevator;
    /// <summary>
    /// Input panel Key is floor number Value is active
    /// </summary>
    readonly Dictionary<int, bool> _elevatorInputPanel;
    bool ActiveInputPanelValue() => _elevatorInputPanel.Values.Any(value => value);
    public (int Row, int Col) ElevatorPosition() => (_elevator.Row, _column);
    public ElevatorData ReturnData()
    {
        _data.DestinationRow = ActiveObject().Destination == -1 ? -1 : _constants.FloorPositions[ActiveObject().Destination];
        _data.DestinationFloor = ActiveObject().Destination;
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
        /// <summary>
        /// Destination floor of the objective
        /// </summary>
        public int Destination { get; set; } = -1;
        public SourceOfRequest Source { get; set; }
    }
    public enum SourceOfRequest
    {
        InputPanel,
        FloorCall
    }
}
