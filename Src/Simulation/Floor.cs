using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Simulation;

public class Floor
{
    public int NthFloor { get; set; }
    public int Row { get; set; }
    public FloorCallState DownCallActive { get; set; } = FloorCallState.Idle;
    public FloorCallState UpCallActive { get; set; } = FloorCallState.Idle;
    public enum FloorCallState
    {
        Active,
        ElevatorAssigned,
        Idle,
    };
}
