using ElevatorSystem.Src.Data;
using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Graphics.ArrowPanel;
namespace ElevatorSystem.Src.Graphics;
public class ViewController
{
    private readonly Constants _constants;
    private readonly Graphic[] _human = new Graphic[2];
    private readonly Graphic[] _elevator = new Graphic[4];
    private readonly Graphic _title;
    private readonly ArrowPanel _arrowPanel;
    private readonly Info _info;
    private readonly ScreenBuffer _screenBuffer;
    private readonly ElevatorInputPanel _elevatorInputPanel;
    private readonly (int Row, int Col, int DoorState)[] _elevatorPositions = new (int Row, int Col, int DoorSTate)[ELEVATOR_COUNT];
    (int Row, int Col, int LegState)[] _humanPositions = Array.Empty<(int, int, int)>();
    int _openFloorInput = -1;
    void DrawElevatorInputPanel()
    {
        foreach (PicturePixel pixel in _elevatorInputPanel.GetGraphic(Console.WindowHeight / 3, Console.WindowWidth / 3))
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
    }
    void DrawElevators()
    {
        foreach (var (row, col, doorState) in _elevatorPositions)
        {
            foreach (PicturePixel pixel in _elevator[doorState].GetGraphic(row, col))
            {
                _screenBuffer.DrawToBuffer(pixel);
            }
        }
    }
    void DrawFloors()
    {
        string floorStr = new('-', _constants.SimulationArea - _constants.TotalElevatorShaftWidth);
        foreach (int floor in _constants.FloorPositions)
        {
            _screenBuffer.DrawToBuffer(floorStr, floor);
        }
    }
    void DrawShafsAndCables()
    {
        for (int i = Console.WindowHeight - 1; i >= 0; i--)
        {
            foreach (var shaft in _constants.ShaftPositions)
            {
                _screenBuffer.DrawToBuffer('|', i, shaft);
                _screenBuffer.DrawToBuffer('|', i, shaft + SHAFT_WIDTH);
            }

            for (int cable = 0; cable < _constants.CablePositions.Length; cable++)
            {
                if (i <= _elevatorPositions[cable].Row)
                {
                    _screenBuffer.DrawToBuffer('|', i, _constants.CablePositions[cable]);
                }
            }
        }
    }
    void DrawTitle()
    {
        foreach (var pixel in _title.GetGraphic(0, _constants.SimulationArea + _constants.TitlePadding))
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
    }
    void DrawInfo()
    {
        foreach (PicturePixel pixel in _info.GetGraphic(TITLE_HEIGHT, _constants.SimulationArea + 5))
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
    }
    void DrawFloorInput()
    {
        var visibleFloorInput = _info.FloorData[_openFloorInput];
        if (visibleFloorInput.UpActive)
        {
            _arrowPanel.SetPanelStatusUpActive();
        }
        else
        {
            _arrowPanel.SetPanelStatusUpInActive();
        }
        if (visibleFloorInput.DownActive)
        {
            _arrowPanel.SetPanelStatusDownActive();
        }
        else
        {
            _arrowPanel.SetPanelStatusDownInActive();
        }
        _arrowPanel.SetAvailableButtons(visibleFloorInput);
        var graphic = _arrowPanel.GetGraphic(_constants.FloorPositions[visibleFloorInput.NthFloor] - FLOOR_HEIGHT + 1, _constants.SimulationArea - _constants.TotalElevatorShaftWidth - 4);
        foreach (PicturePixel pixel in graphic)
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
    }
    void DrawBackground() 
    {
        DrawFloors();
        DrawShafsAndCables();
        DrawTitle();
        DrawInfo();
    }
    void DrawHumans()
    {
        foreach (var (row, col, legState) in _humanPositions)
        {
            foreach (PicturePixel pixel in _human[legState].GetGraphic(row, col)) 
            {
                _screenBuffer.DrawToBuffer(pixel);
            }
        }
    }
    public void Draw(bool openPanel = false)
    {
        DrawBackground();
        DrawHumans();
        DrawElevators();
        if (_openFloorInput > -1)
        {
            DrawFloorInput();
        }

        if (openPanel)
        {
            DrawElevatorInputPanel();
        }
        _screenBuffer.DrawBuffer();
    }
    public void SetHumanPositions((int Row, int Col, int LegState)[] humans)
    {
        _humanPositions = new (int Row, int Col, int LegState)[humans.Length]; 
        for (int i = 0; i < humans.Length; i++)
        {
            _humanPositions[i] = humans[i];
        }
    }
    public void SetInfoData(List<ElevatorData> elevatorData, List<FloorData> floorData)
    {
        _info.SetInfoData(elevatorData, floorData);
    }
    public void SetInputPanel(bool[] inputPanel)
    {
        _elevatorInputPanel.SetInputPanel(inputPanel);
    }
    public void SetElevatorDrawInformation((int Row, int Col, int DoorSequence)[] elevatorPositions)
    {
        for (int i = 0; i < _elevatorPositions.Length; i++)
        {
            _elevatorPositions[i].Row = elevatorPositions[i].Row - ELEVATOR_HEIGHT;
            _elevatorPositions[i].Col = elevatorPositions[i].Col + PADDING;
            _elevatorPositions[i].DoorState = OverFlowSequence(elevatorPositions[i].DoorSequence);
        }
        static int OverFlowSequence(int value)
        {
            return value switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => 3,
                5 => 2,
                6 => 1,
                7 => 0,
                _ => -1
            };
        }
    }
    public void SetOpenFloorInput(int floor) => _openFloorInput = floor;
    public ViewController()
    {
        _elevator[0] = new Graphic("../../../Assets/Elevator.txt");
        _elevator[1] = new Graphic("../../../Assets/ElevatorOpen1.txt");
        _elevator[2] = new Graphic("../../../Assets/ElevatorOpen2.txt");
        _elevator[3] = new Graphic("../../../Assets/ElevatorOpen3.txt");
        _human[0] = new Graphic("../../../Assets/Human.txt");
        _human[1] = new Graphic("../../../Assets/Human2.txt");
        _title = new Graphic("../../../Assets/Title.txt");
        _screenBuffer = ScreenBuffer.GetInstance();
        _constants = new Constants();
        _info = new Info();
        _arrowPanel = new ArrowPanel();
        _elevatorInputPanel = new ElevatorInputPanel();
    }
    public enum GraphicType
    {
        Elevator,
        Human,
    }
}
