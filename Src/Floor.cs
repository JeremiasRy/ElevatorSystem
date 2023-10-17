using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class Floor
{
    public int Y { get; }
    public int NthFloor { get; }
    public (int y, int x)[] ElevatorPositions;

    public Floor(int y, int nthFloor, int[] shafts)
    {
        Y = y; 
        NthFloor = nthFloor;
        ElevatorPositions = new (int, int)[shafts.Length / 2];
        for (int i = 0; i < shafts.Length; i += 2)
        {
            ElevatorPositions[i >= 2 ? i - 1 : i] = (y - 4, shafts[i] + 2);
        }
    }
}
