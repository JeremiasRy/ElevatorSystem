using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.GuiObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ElevatorStateHandler
{
    readonly int _floors;
    int _idCount = 0;
    readonly ScreenBuffer _buffer;
    readonly Graphic _humanGraphic;
    readonly Elevator[] _elevators = new Elevator[2];
    readonly List<Human> _humans = new();
    List<GuiObject> GraphicObjects() => _elevators.Cast<GuiObject>().Concat(_humans.Cast<GuiObject>()).ToList();
    public void Tick()
    {
        foreach (var graphicObj in GraphicObjects())
        {
            graphicObj.Draw();
        }
        _buffer.DrawBuffer();
    }
    public void CreateHuman()
    {
        _humans.Add(new Human(_humanGraphic, _idCount++));
    }
    public ElevatorStateHandler()
    {
        _buffer = ScreenBuffer.GetInstance();
        var graphic = new Graphic("../../../Assets/Elevator.txt", _buffer);
        _humanGraphic = new Graphic("../../../Assets/Human.txt", _buffer);
        for (int i = 0; i < _elevators.Length; i++)
        {
            _elevators[i] = new Elevator(graphic, _idCount++);
        }
    }
}
