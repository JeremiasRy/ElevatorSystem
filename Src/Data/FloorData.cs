using ElevatorSystem.Src.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Data;

public class FloorData
{
    public bool UpActive { get; set; }
    public bool DownActive { get; set; }
    public int NthFloor { get; set; }
}
