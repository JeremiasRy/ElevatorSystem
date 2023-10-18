using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorCall
{
    public Floor From { get; init; }
    public Floor To { get; init; }
    public bool FromCompleted { get; set; } = false;
    public bool ToCompleted { get; set; } = false;
    public bool Complete => FromCompleted && ToCompleted;

    public ElevatorCall(Floor from, Floor to)
    {
        From = from;
        To = to;
    }
}
