using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorInstruction
{
    public Floor GoTo { get; }
    public ElevatorInstruction(Floor floor)
    { 
        GoTo = floor;
    }
    public void Complete()
    {
    }
}
