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
    public bool PanelActive { get; set; }
    public static FloorData FromFloorController(FloorController floorController)
    {
        return new FloorData() 
        {
            UpActive = floorController.UpCallState == FloorController.FloorCallState.Active || floorController.UpCallState == FloorController.FloorCallState.ElevatorAssigned,
            DownActive = floorController.DownCallState == FloorController.FloorCallState.Active || floorController.DownCallState == FloorController.FloorCallState.ElevatorAssigned,
            NthFloor = floorController.NthFloor,
            PanelActive = floorController.OpenPanel,
        };
    }
}
