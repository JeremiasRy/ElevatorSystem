using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Controllers;

public class FloorController
{
    readonly Constants _constants = new();
    FloorCallState _downCallState = FloorCallState.Idle;
    FloorCallState _upCallState = FloorCallState.Idle;
    public int NthFloor { get; set; }
    public int Row { get; set; }
    public bool OpenPanel { get; set; }
    public int Width => _constants.SimulationArea - _constants.TotalElevatorShaftWidth;
    public FloorCallState DownCallState
    {
        get => _downCallState;
        set
        {
            if (_downCallState == FloorCallState.NotAvailable)
            {
                return;
            }
            _downCallState = value;
        }
    }
    public FloorCallState UpCallState
    {
        get => _upCallState;
        set
        {
            if (_upCallState == FloorCallState.NotAvailable)
            {
                return;
            }
            _upCallState = value;
        }
    }
    public FloorController(int nthFloor, int row)
    {
        var constants = new Constants();
        if (nthFloor == 0)
        {
            DownCallState = FloorCallState.NotAvailable;
        }
        if (nthFloor == constants.FloorCount - 1)
        {
            UpCallState = FloorCallState.NotAvailable;
        }
        NthFloor = nthFloor;
        Row = row;
    }
    public enum FloorCallState
    {
        Active,
        ElevatorAssigned,
        Idle,
        NotAvailable
    };
}
