using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Inputs;

public class UserCall
{
    static readonly Random _random = new ();
    static int _idCount = 1;
    static readonly Constants _constants = new();
    static readonly int _roomToWalk = _constants.SimulationArea - _constants.TotalElevatorShaftWidth - 7;
    public int WalkState { get; private set; } = 0;
    int _legState = 0;
    public int LegState()
    {
        if (State == UserCallState.WaitingForElevator)
        {
            return 0;
        }
        _legState = _legState + 1 > 1 ? 0 : 1; 
        return _legState;
    }
    public UserCallState State { get; set; }
    public int UserWeight { get; set; }
    public int Id { get; set; }
    public int InElevatorId { get; set; }
    public int StartFloor { get; set; }
    public int EndFloor { get; set; } = -1;
    public Direction RequestDirection { get; set; }
    public void Walk()
    {
        if (State == UserCallState.FireCall)
        {
            State = UserCallState.WaitingForElevator;
            return;
        }
        if (State == UserCallState.ArrivingToScene)
        {
            WalkState++;
            if (WalkState == _roomToWalk)
            {
                State = UserCallState.FireCall;
            }
            return;
        }
        if (State == UserCallState.LeavingTheScene)
        {
            WalkState--;
            if (WalkState == 0)
            {
                State = UserCallState.Done;
            }
            return;
        }
    }
    public UserCall(int floor, Direction direction)
    {
        Id = _idCount++;
        StartFloor = floor;
        RequestDirection = direction;
        UserWeight = _random.Next(50, 110);
        
    }
    public enum Direction
    {
        Up,
        Down
    }
    public enum UserCallState
    {
        ArrivingToScene,
        FireCall,
        WaitingForElevator,
        Travelling,
        LeavingTheScene,
        Done
    }
}
