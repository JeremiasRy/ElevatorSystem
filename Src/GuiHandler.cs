using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.GuiObjects;

namespace ElevatorSystem.Src;

public class GuiHandler
{
    public const int SHAFT_PADDING = 2;
    public const int ELEVATOR_COUNT = 2;
    public const int SHAFT_WIDTH = 12;
    public const int ELEVATOR_HEIGHT = 5;
    int _idCount = 0;
    readonly int[] _shafts = new int[4];
    readonly List<Floor> _floors;
    readonly List<Elevator> _elevators;
    readonly ScreenBuffer _buffer;
    readonly Graphic _humanGraphic;
    readonly List<Human> _humans = new();

    readonly ElevatorStateHandler _elevatorHandler;
    List<GuiObject> GraphicObjects() => _elevators.Cast<GuiObject>().Concat(_humans.Cast<GuiObject>()).ToList();
    public void Tick()
    {
        DrawBackground();
        _elevatorHandler.MoveElevatorsToPlace();
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
        foreach (int floor in _floors.Select(floor => floor.Y))
        {
            var floorStringStart = new string('_', _shafts[0]);
            var floorStringEnd = new string('_', Console.WindowWidth - 1 - _shafts[^1]);
            _buffer.DrawToBuffer(floorStringStart, floor);
            _buffer.DrawToBuffer(floorStringEnd, floor, _shafts[^1] + 1);
        }
    }
    public GuiHandler()
    {
        _floors = new List<Floor>(Console.WindowHeight / 5);
        _elevators = new List<Elevator>(ELEVATOR_COUNT);
        _buffer = ScreenBuffer.GetInstance();
        var graphic = new Graphic("../../../Assets/Elevator.txt", _buffer);
        _humanGraphic = new Graphic("../../../Assets/Human.txt", _buffer);

        int elevatorShaftStartIndex = (Console.WindowWidth - 1) / ELEVATOR_COUNT - ((SHAFT_WIDTH * 2 + SHAFT_PADDING) / 2);
        _shafts[0] = elevatorShaftStartIndex;
        _shafts[1] = elevatorShaftStartIndex + SHAFT_WIDTH;
        _shafts[2] = elevatorShaftStartIndex + SHAFT_WIDTH + SHAFT_PADDING;
        _shafts[3] = elevatorShaftStartIndex + SHAFT_WIDTH * 2 + SHAFT_PADDING;

        for (int i = 0; i < _floors.Capacity; i++)
        {
            _floors.Add(new Floor(Console.WindowHeight - 1 - (5 * i), i, _shafts));
        }

        for (int i = 0; i < _elevators.Capacity; i++)
        {
            _elevators.Add(new Elevator(graphic, _idCount++, i, 0));
        }
        _elevatorHandler = new ElevatorStateHandler(_floors, _elevators);
    }
}
