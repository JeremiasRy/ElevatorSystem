using ElevatorSystem.Src.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Simulation;

public class Elevator
{ 
    public int Row { get; private set; } = Console.WindowHeight - 1;
    public void Input(int motor)
    {
        if (motor > 1 || motor < -1)
        {
            throw new ArgumentException($"Input out of motors range: {motor}");
        }
        if (Row + motor < Constants.ELEVATOR_HEIGHT ||  Row + motor > Console.WindowHeight - 1)
        {
            throw new Exception($"Elevator was at: {Row} motor input was {motor}. Crash!");
        }
        Row += motor;
    }
    public void Input(Motor motor)
    {
        if (Row + (int)motor < Constants.ELEVATOR_HEIGHT || Row + (int)motor > Console.WindowHeight - 1)
        {
            throw new Exception($"Elevator was at: {Row} motor input was {motor}. Crash!");
        }
        Row += (int)motor;
    }
    public enum Motor
    {
        Up = -1,
        Idle = 0,
        Down = 1
    }
}
