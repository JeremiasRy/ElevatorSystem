using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorCall
{
    public int From { get; set; }
    public int To { get; set; }
    public bool FinishedFrom { get; set; } = false;
    public bool FinishedTo { get; set; } = false;
    public bool Finished => FinishedFrom && FinishedTo;
}
