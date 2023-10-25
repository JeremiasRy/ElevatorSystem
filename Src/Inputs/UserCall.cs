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
    public UserCallState State { get; set; }
    public int UserWeight { get; set; }
    public int Id { get; set; }
    public int Floor { get; set; }
    public Direction RequestDirection { get; set; }
    public UserCall(int floor, Direction direction)
    {
        Id = _idCount++;
        Floor = floor;
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
        WaitingForElevator,
        Travelling,
        Done
    }
}
