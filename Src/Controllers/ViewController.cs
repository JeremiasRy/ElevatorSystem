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
    private readonly ElevatorOrchestrator _elevatorOrchestrator;
    public void Draw(bool openPanel = false)
    {
        DrawBackground();
        foreach (var (row, col) in _elevatorOrchestrator.ElevatorPositions)
        {
            foreach(PicturePixel pixel in _elevator.GetGraphic(row, col))
            {
                _screenBuffer.DrawToBuffer(pixel);
            }
        }
        if (openPanel)
        {
            foreach(PicturePixel pixel in _elevatorInputPanel.GetGraphic(Console.WindowHeight / 10, Console.WindowWidth / 8))
            {
                _screenBuffer.DrawToBuffer(pixel);
            }
        }
        _screenBuffer.DrawBuffer();
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
                if (i <= _elevatorOrchestrator.ElevatorPositions[cable].Row)
                {
                    _screenBuffer.DrawToBuffer('|', i, _constants.CablePositions[cable]);
                }
            }
        }
        foreach (var pixel in _title.GetGraphic(0, _constants.SimulationArea + _constants.TitlePadding))
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
        var elevatorDataPoints = _elevatorOrchestrator.GetElevatorDataPoints();
        var floorDataPoints = _elevatorOrchestrator.GetFloorDataPoints();
        _info.SetInfoData(elevatorDataPoints, floorDataPoints);
        foreach (PicturePixel pixel in _info.GetGraphic(TITLE_HEIGHT, _constants.SimulationArea + 5))
        {
            _screenBuffer.DrawToBuffer(pixel);
        }
        foreach(var floorData in floorDataPoints.Where(dataObj => dataObj.PanelActive))
        {
            if (floorData.UpActive)
            {
                _arrowPanel.SetPanelStatusUpActive();
            } else
            {
                _arrowPanel.SetPanelStatusUpInActive();
            }

            if (floorData.DownActive)
            {
                _arrowPanel.SetPanelStatusDownActive();
            } else
            {
                _arrowPanel.SetPanelStatusDownInActive();
            }
            _arrowPanel.SetAvailableButtons(floorData);
            var graphic = _arrowPanel.GetGraphic(_constants.FloorPositions[floorData.NthFloor] - FLOOR_HEIGHT + 1, _constants.SimulationArea - _constants.TotalElevatorShaftWidth - 4);
            foreach (PicturePixel pixel in graphic)
            {
                _screenBuffer.DrawToBuffer(pixel);
            }
        }
    }
    public ViewController(ElevatorOrchestrator elevatorController)
    {
        _elevatorOrchestrator = elevatorController;
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
