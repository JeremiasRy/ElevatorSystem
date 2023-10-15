using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.GuiObjects;

namespace ElevatorSystem.Src;

public class BuildingState
{
    private const int SHAFT_PADDING = 2;
    private const int ELEVATOR_COUNT = 2;
    private const int SHAFT_WIDTH = 11;
    int _idCount = 0;
    readonly int[] _floors = new int[Console.WindowHeight / 5];
    readonly int[] _shafts = new int[4];
    readonly ScreenBuffer _buffer;
    readonly Graphic _humanGraphic;
    readonly Elevator[] _elevators = new Elevator[2];
    readonly List<Human> _humans = new();
    List<GuiObject> GraphicObjects() => _elevators.Cast<GuiObject>().Concat(_humans.Cast<GuiObject>()).ToList();
    public void Tick()
    {
        DrawBackground();
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
    void DrawBackground()
    {
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            foreach (int shaft in _shafts)
            {
                _buffer.DrawToBuffer('|', i, shaft);
            }
        }
        foreach (int floor in _floors)
        {
            var floorStringStart = new string('_', _shafts[0]);
            var floorStringEnd = new string('_', Console.WindowWidth - 1 - _shafts[^1]);
            _buffer.DrawToBuffer(floorStringStart, floor);
            _buffer.DrawToBuffer(floorStringEnd, floor, _shafts[^1] + 1);
        }
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

        int startIndex = (Console.WindowWidth - 1) / ELEVATOR_COUNT - ((SHAFT_WIDTH * 2 + SHAFT_PADDING) / 2);
        _shafts[0] = startIndex;
        _shafts[1] = startIndex + 11;
        _shafts[2] = startIndex + 11 + 2;
        _shafts[3] = startIndex + 11 + 2 + 11;
    }
}
