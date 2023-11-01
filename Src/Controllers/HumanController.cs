using ElevatorSystem.Src.Inputs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Controllers;

public class HumanController
{
    private readonly List<UserCall> _userCalls = new();
    private readonly Constants _constants = new();
    public List<UserCall> CallsToFire() => _userCalls.Where(userCall => userCall.State == UserCall.UserCallState.FireCall).ToList();
    public List<UserCall> HumansWaitingForElevator() => _userCalls.Where(userCall => userCall.State == UserCall.UserCallState.WaitingForElevator).ToList();
    public List<UserCall> HumansTravelling() => _userCalls.Where(userCall => userCall.State == UserCall.UserCallState.Travelling).ToList();
    public (int Row, int Col, int LegState)[] HumansToDraw()
    {
        return _userCalls
            .Where(userCall => userCall.State == UserCall.UserCallState.ArrivingToScene || userCall.State == UserCall.UserCallState.LeavingTheScene || userCall.State == UserCall.UserCallState.WaitingForElevator)
            .Select(userCall => 
            ( 
            userCall.State == UserCall.UserCallState.WaitingForElevator || userCall.State == UserCall.UserCallState.ArrivingToScene 
            ? _constants.FloorPositions[userCall.StartFloor] - 3 
            : _constants.FloorPositions[userCall.EndFloor] - 3,
            userCall.WalkState,
            userCall.LegState()
            ))
            .ToArray();
    }
    public void AppendHuman(int floor, UserCall.Direction direction)
    {
        UserCall newCall = new(floor, direction);
        _userCalls.Add(newCall);
    }
    public void SetCallToTravelling(int id, int elevatorId, int floor)
    {
        var call = _userCalls.First(userCall => userCall.Id == id);
        call.EndFloor = floor;
        call.InElevatorId = elevatorId;
        call.State = UserCall.UserCallState.Travelling;
    }
    public void SetCallToLeaving(int id)
    {
        _userCalls.First(userCall => userCall.Id == id).State = UserCall.UserCallState.LeavingTheScene;
    }
    public void Tick()
    {
        foreach (var userCall in _userCalls)
        {
            userCall.Walk();
        }
        _userCalls.RemoveAll(userCall => userCall.State == UserCall.UserCallState.Done);
    }
}
