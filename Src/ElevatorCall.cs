using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorCall
{
    public static int IdCount { get; private set; } = 0;
    public int Id { get; set; }
    public Floor From { get; init; }
    public Floor To { get; init; }
    public bool IsAssigned { get; set; } = false;
    public bool FromCompleted { get; set; } = false;
    public bool ToCompleted { get; set; } = false;
    public bool Complete => FromCompleted && ToCompleted;
    public CallDirection Direction => From.NthFloor - To.NthFloor < 0 ? CallDirection.Up : CallDirection.Down;
    public ElevatorCall(Floor from, Floor to)
    {
        From = from;
        To = to;
        Id = IdCount++;
    }
    public enum CallDirection
    {
        Up,
        Down,
    }
}
