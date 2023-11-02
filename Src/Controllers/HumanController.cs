using ElevatorSystem.Src.Simulation;
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
    private readonly List<Human> _userCalls = new();
    private readonly Constants _constants = new();
    public (int Floor, Human.Direction Direction)[] CallsToFire() => _userCalls
        .Where(userCall => userCall.State == Human.UserCallState.FireCall)
        .Select(userCall => (userCall.StartFloor, userCall.RequestDirection))
        .ToArray();
    public (int UserId, int StartFloor)[] HumansWaitingForElevator() => _userCalls
        .Where(userCall => userCall.State == Human.UserCallState.WaitingForElevator)
        .Select(userCall => (userCall.Id, userCall.StartFloor))
        .ToArray();
    public (int UserId, int EndFloor)[] HumansTravelling() => _userCalls
        .Where(userCall => userCall.State == Human.UserCallState.Travelling)
        .Select(userCall => (userCall.Id, userCall.EndFloor))
        .ToArray();
    public (int Row, int Col, int LegState)[] HumansToDraw()
    {
        return _userCalls
            .Where(userCall => userCall.State == Human.UserCallState.ArrivingToScene || userCall.State == Human.UserCallState.LeavingTheScene || userCall.State == Human.UserCallState.WaitingForElevator)
            .Select(userCall => 
            ( 
            userCall.State == Human.UserCallState.WaitingForElevator || userCall.State == Human.UserCallState.ArrivingToScene 
            ? _constants.FloorPositions[userCall.StartFloor] - 3 
            : _constants.FloorPositions[userCall.EndFloor] - 3,
            userCall.WalkState,
            userCall.LegState()
            ))
            .ToArray();
    }
    public void AppendHuman(int floor, Human.Direction direction)
    {
        Human newCall = new(floor, direction);
        _userCalls.Add(newCall);
    }
    public void SetCallToTravelling(int id, int elevatorId, int floor)
    {
        var call = _userCalls.First(userCall => userCall.Id == id);
        call.EndFloor = floor;
        call.InElevatorId = elevatorId;
        call.State = Human.UserCallState.Travelling;
    }
    public void SetCallToLeaving(int id)
    {
        _userCalls.First(userCall => userCall.Id == id).State = Human.UserCallState.LeavingTheScene;
    }
    public void Tick()
    {
        foreach (var userCall in _userCalls)
        {
            userCall.Walk();
        }
        _userCalls.RemoveAll(userCall => userCall.State == Human.UserCallState.Done);
    }
}
