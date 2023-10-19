using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;
public class Floor
{
    public int NthFloor { get; }
    public int Row { get; }
    public int RowAdjustedForElevator => Row - 4;
    public override string ToString() => $"Floor {NthFloor}";
}
