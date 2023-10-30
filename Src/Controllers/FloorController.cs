using ElevatorSystem.Src.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Controllers;

public class FloorController
{
    readonly FloorData _data = new();
    public FloorCallState DownCallState { get; private set; } = FloorCallState.Idle;
    public FloorCallState UpCallState { get; private set; } = FloorCallState.Idle;
    public bool IsActive() => DownCallState == FloorCallState.Active || UpCallState == FloorCallState.Active;
    public int NthFloor { get; set; }
    public int Row { get; set; }
    public bool SetUpCallStateToIdle()
    {
        if (UpCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        UpCallState = FloorCallState.Idle;
        return true;
    }
    public bool SetUpCallStateToAssigned()
    {
        if (UpCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        UpCallState = FloorCallState.ElevatorAssigned;
        return true;
    }
    public bool SetUpCallStateToActive()
    {
        if (UpCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        UpCallState = FloorCallState.Active;
        return true;
    }
    public bool SetDownCallStateToIdle()
    {
        if (DownCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        DownCallState = FloorCallState.Idle;
        return true;
    }

    public bool SetDownCallStateToAssigned()
    {
        if (DownCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        DownCallState = FloorCallState.ElevatorAssigned;
        return true;
    }
    
    public bool SetDownCallStateToActive()
    {
        if (DownCallState == FloorCallState.NotAvailable)
        {
            return false;
        }
        DownCallState = FloorCallState.Active;
        return true;
    }
    public FloorData ReturnData()
    {
        _data.UpActive = UpCallState == FloorCallState.Active || UpCallState == FloorCallState.ElevatorAssigned;
        _data.DownActive = DownCallState == FloorCallState.Active || DownCallState == FloorCallState.ElevatorAssigned;
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
