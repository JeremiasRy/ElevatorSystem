using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.GuiObjects;

namespace ElevatorSystem.Src;

public class MasterState
{
    public const int ELEVATOR_COUNT = 2;
    public const int FLOOR_HEIGHT = 7;
    public const int SHAFT_WIDTH = 11;
    public const int PADDING = 2;
    public const int ELEVATOR_HEIGHT = 5;
    public const int ELEVATOR_WIDTH = 9;
    readonly int _floorCount = Console.WindowHeight / FLOOR_HEIGHT;
    int _playArea;
    int _idCount;
    readonly Graphic _title;
    readonly Graphic _elevator;
    readonly Graphic _human;
    List<GuiObject> _guiObjects;
    readonly Shaft[] _shafts = new Shaft[2];
    readonly ScreenBuffer _screenBuffer;
    readonly ElevatorStateHandler _elevatorStateHandler;
    readonly List<Floor> _floors = new();

    static int TotalShaftWidth() => SHAFT_WIDTH * 2 + PADDING + 4;
    public void StartTick()
    {
        while (true)
        {
            Tick();
            Thread.Sleep(20);
        }
    }
    public void CallElevator(int from, int to)
    {
        Floor? fromFloor = _floors.FirstOrDefault(floor => floor.NthFloor == from);
        Floor? toFloor = _floors.FirstOrDefault(floor => floor.NthFloor == to);
        if (fromFloor is null ||  toFloor is null)
        {
            return;
        }
        _elevatorStateHandler.CallElevator(fromFloor, toFloor);
    }
    void Tick()
    {
        _elevatorStateHandler.GiveTasksToElevators();
        _elevatorStateHandler.MoveElevators();
        DrawBackground();
        DrawToBuffer();
        _screenBuffer.DrawBuffer();
    }
    void DrawToBuffer()
    {
        PicturePixel[] thingsToDraw = _guiObjects.SelectMany(gObj => gObj.Pixels).ToArray();
        foreach (var pixel in thingsToDraw)
        {
            _screenBuffer.DrawToBuffer(pixel.Ch, pixel.OffsetY, pixel.OffsetX);
        }
    }
    void DrawBackground()
    {
        string floorStr = new('-', _playArea);
        string shaftStr = $"|{new(' ', SHAFT_WIDTH)}|{new(' ', PADDING)}|{new(' ', SHAFT_WIDTH)}|";
        foreach (var floor in _floors)
        {
            _screenBuffer.DrawToBuffer(floorStr, row: floor.Row);
        }
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            _screenBuffer.DrawToBuffer(shaftStr, i, _playArea - TotalShaftWidth());
            foreach (var shaft in _shafts)
            {
                if (i <= shaft.ElevatorPosition.Row)
                {
                    _screenBuffer.DrawToBuffer('|', i, shaft.ElevatorPosition.Col + ELEVATOR_WIDTH / 2); // Cable for elevators
                }
            }
        }
        foreach(var pixel in _title.GetGraphicInPlace(0, _playArea + PADDING))
        {
            _screenBuffer.DrawToBuffer(pixel.Ch, pixel.OffsetY, pixel.OffsetX);
        }
    }
    public MasterState()
    {
        _screenBuffer = ScreenBuffer.GetInstance();
        _guiObjects = new List<GuiObject>();
        _playArea = Console.WindowWidth / 3 * 2;
        _elevator = new Graphic("../../../Assets/Elevator.txt");
        _human = new Graphic("../../../Assets/Human.txt");
        _title = new Graphic("../../../Assets/Title.txt");
        Elevator[] elevators = new Elevator[ELEVATOR_COUNT];
        for (int i = 0; i < _floorCount; i++)
        {
            _floors.Add(new Floor()
            {
                NthFloor = i,
                Row = Console.WindowHeight - 1 - FLOOR_HEIGHT * i,
            });
        }
        for (int i = 0; i < ELEVATOR_COUNT; i++)
        {
            elevators[i] = new Elevator(_elevator);
            _shafts[i] = new Shaft(elevators[i])
            {
                Column = _playArea - TotalShaftWidth() + (SHAFT_WIDTH + PADDING + 2) * i,
            };
            _guiObjects.Add(elevators[i]);
            elevators[i].BrutalForceMove(Console.WindowHeight - ELEVATOR_HEIGHT, _shafts[i].ElevatorPosition.Col);
        }
        _elevatorStateHandler = new ElevatorStateHandler(elevators);
    }
}
