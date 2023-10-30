using ElevatorSystem.Src.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Controllers;

public class FloorController
{
    FloorCallState _downCallState = FloorCallState.Idle;
    FloorCallState _upCallState = FloorCallState.Idle;
    FloorData _data = new();
    public int NthFloor { get; set; }
    public int Row { get; set; }
    public FloorCallState DownCallState
    {
        get => _downCallState;
        private set
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
        private set
        {
            if (_upCallState == FloorCallState.NotAvailable)
            {
                return;
            }
            _upCallState = value;
        }
    }
    public void SetUpCallStateToIdle() => _upCallState = FloorCallState.Idle;
    public void SetDownCallStateToIdle() => _downCallState = FloorCallState.Idle;
    public void SetUpCallStateToAssigned() => _upCallState = FloorCallState.ElevatorAssigned;
    public void SetDownCallStateToAssigned() => _downCallState = FloorCallState.ElevatorAssigned;
    public FloorData ReturnData()
    {
        _data.UpActive = _upCallState == FloorCallState.Active || _upCallState == FloorCallState.ElevatorAssigned;
        _data.DownActive = _downCallState == FloorCallState.Active || _downCallState == FloorCallState.ElevatorAssigned;
        return _data;
    }
    public FloorController(int nthFloor, int row)
    {
        var constants = new Constants();
        if (nthFloor == 0)
        {
            DownCallState = FloorCallState.NotAvailable;
        }
        if (nthFloor == constants.FloorCount - 2)
        {
            UpCallState = FloorCallState.NotAvailable;
        }
        NthFloor = nthFloor;
        Row = row;
        _data.NthFloor = NthFloor;
    }
    public enum FloorCallState
    {
        Active,
        ElevatorAssigned,
        Idle,
        NotAvailable
    };
}
