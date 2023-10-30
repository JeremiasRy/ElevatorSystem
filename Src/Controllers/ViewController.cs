using ElevatorSystem.Src.Data;
using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Graphics.ArrowPanel;
namespace ElevatorSystem.Src.Graphics;
public class ViewController
{
    private readonly Constants _constants;
    private readonly Graphic _human;
    private readonly Graphic _elevator;
    private readonly Graphic _title;
    private readonly ArrowPanel _arrowPanel;
    private readonly Info _info;
    private readonly ScreenBuffer _screenBuffer;
    private readonly ElevatorInputPanel _elevatorInputPanel;
    private readonly (int Row, int Col)[] _elevatorPositions = new (int Row, int Col)[ELEVATOR_COUNT];
    int _openFloorInput = -1;
    public void SetInputPanel(bool[] inputPanel)
    {
        _elevatorInputPanel.SetInputPanel(inputPanel);
    }
    public void Draw(bool openPanel = false)
    {
        DrawBackground();
        foreach (var (row, col) in _elevatorPositions)
        {
            foreach(PicturePixel pixel in _elevator.GetGraphic(row, col))
            {
                _screenBuffer.DrawToBuffer(pixel);
            }
        }
        if (openPanel)
        {
            foreach(PicturePixel pixel in _elevatorInputPanel.GetGraphic(Console.WindowHeight / 3, Console.WindowWidth / 3))
            {
                _screenBuffer.DrawToBuffer(pixel);
            }
        }
        _screenBuffer.DrawBuffer();
    }
    public void SetInfoData(List<ElevatorData> elevatorData, List<FloorData> floorData)
    {
        _info.SetInfoData(elevatorData, floorData);
    }
    public void DrawBackground() 
    {
        string floorStr = new('-', _constants.SimulationArea - _constants.TotalElevatorShaftWidth);
        foreach (int floor in _constants.FloorPositions)
        {
            _screenBuffer.DrawToBuffer(floorStr, floor);
        }
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
        foreach (var pixel in _title.GetGraphic(0, _constants.SimulationArea + _constants.TitlePadding))
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
        
        foreach (PicturePixel pixel in _info.GetGraphic(TITLE_HEIGHT, _constants.SimulationArea + 5))
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
        if (_openFloorInput > -1)
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
    }
    public void SetElevatorPositions((int Row, int Col)[] elevatorPositions)
    {
        for (int i = 0; i < _elevatorPositions.Length; i++)
        {
            _elevatorPositions[i].Row = elevatorPositions[i].Row - ELEVATOR_HEIGHT;
            _elevatorPositions[i].Col = elevatorPositions[i].Col + PADDING;
        }
    }
    public void SetOpenFLoorInput(int floor) => _openFloorInput = floor;
    public ViewController()
    {
        _elevator = new Graphic("../../../Assets/Elevator.txt");
        _human = new Graphic("../../../Assets/Human.txt");
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
