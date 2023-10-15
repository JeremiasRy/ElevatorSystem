using ElevatorSystem.Src.GuiObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorStateHandler
{
    private int[] _shaftOneFloorPositions;
    private int[] _shaftTwoFloorPositions;
    private Elevator[] _elevators;
    public List<GuiObject> ReturnDrawableObjects => _elevators.Cast<GuiObject>().ToList();

    public ElevatorStateHandler()
    {
        
    }
}
