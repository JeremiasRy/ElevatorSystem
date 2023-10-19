using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorInstruction
{
    public int GoToRow { get; set; }
    public bool Completed { get; set; } = false;
}
