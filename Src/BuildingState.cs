using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.GuiObjects;

namespace ElevatorSystem.Src;

public class BuildingState
{
    int _idCount = 0;
    int[] _floors = new int[Console.WindowHeight / 5];
    readonly ScreenBuffer _buffer;
    readonly Graphic _humanGraphic;
    readonly Elevator[] _elevators = new Elevator[2];
    readonly List<Human> _humans = new();
    List<GuiObject> GraphicObjects() => _elevators.Cast<GuiObject>().Concat(_humans.Cast<GuiObject>()).ToList();
    public void Tick()
    {
        foreach (int floor in _floors)
        {
            _buffer.DrawToBuffer(new string('_', Console.WindowWidth), floor);
        }
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
    public BuildingState()
    {
        _buffer = ScreenBuffer.GetInstance();
        var graphic = new Graphic("../../../Assets/Elevator.txt", _buffer);
        _humanGraphic = new Graphic("../../../Assets/Human.txt", _buffer);
        for (int i = 0; i < _elevators.Length; i++)
        {
            _elevators[i] = new Elevator(graphic, _idCount++);
        }

        for (int i = 0; i < _floors.Length; i++)
        {
            _floors[i] = Console.WindowHeight - 1 - (5 * i);
        }
    }
}
