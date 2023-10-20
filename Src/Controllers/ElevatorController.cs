﻿using ElevatorSystem.Src.Simulation;
using static ElevatorSystem.Src.Constants;

namespace ElevatorSystem.Src;

public class ElevatorController
{
    readonly Constants _constants;
    readonly Shaft[] _shafts = new Shaft[ELEVATOR_COUNT];
    readonly Floor[] _floors = new Floor[ELEVATOR_COUNT];
    public (int Row, int Col)[] ElevatorPositions 
    { 
        get
        {
            var result = new (int Row, int Col)[ELEVATOR_COUNT];
            for (int i = 0; i < ELEVATOR_COUNT; i++)
            {
                result[i] = (_shafts[i].ElevatorHeight, _shafts[i].Column);
            }
            return result;
        }
    }
    
    public void CallElevator()
    {

    }
    public void Tick()
    {
        MoveElevators();
    }
    void MoveElevators()
    {
        foreach (var shaft in _shafts)
        {
            shaft.InputElevator(Elevator.Motor.Up);
        }
    }
    public ElevatorController()
    {
        _constants = new Constants();
        for (int i = 0; i < _shafts.Length; i++)
        {
            _shafts[i] = new Shaft(new Elevator(), _constants.ShaftPositions[i]);
        }
    }
}
