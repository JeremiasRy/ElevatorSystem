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
    readonly FloorData _data = new();
    public int NthFloor { get; set; }
    public int Row { get; set; }
    public bool SetUpCallStateToIdle()
    {
        if (_upCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        _upCallState = FloorCallState.Idle;
        return true;
    }
    public bool SetUpCallStateToAssigned()
    {
        if (_upCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        _upCallState = FloorCallState.ElevatorAssigned;
        return true;
    }
    public bool SetUpCallStateToActive()
    {
        if (_upCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        _upCallState = FloorCallState.Active;
        return true;
    }
    public bool SetDownCallStateToIdle()
    {
        if (_downCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        _downCallState = FloorCallState.Idle;
        return true;
    }

    public bool SetDownCallStateToAssigned()
    {
        if (_downCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        _downCallState = FloorCallState.ElevatorAssigned;
        return true;
    }
    
    public bool SetDownCallStateToActive()
    {
        if (_downCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        _downCallState = FloorCallState.Active;
        return true;
    }
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
            _downCallState = FloorCallState.NotAvailable;
        }
        if (nthFloor == constants.FloorCount - 2)
        {
            _upCallState = FloorCallState.NotAvailable;
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
