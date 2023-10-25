using static ElevatorSystem.Src.Constants;
using static ElevatorSystem.Src.Graphics.ArrowPanel;
namespace ElevatorSystem.Src.Graphics;
public class ViewController
{
    private readonly Constants _constants;
    private readonly Graphic _human;
    private readonly Graphic _elevator;
    private readonly Graphic _title;
    private readonly Info _info;
    private readonly ScreenBuffer _screenBuffer;
    private readonly ElevatorOrchestrator _elevatorOrchestrator;
    public void Draw()
    {
        DrawBackground();
        foreach (var (row, col) in _elevatorOrchestrator.ElevatorPositions)
        {
            foreach(var (elevatorRow, elevatorCol, ch) in _elevator.GetGraphicInPlace(row, col))
            {
                _screenBuffer.DrawToBuffer(ch, elevatorRow, elevatorCol);
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
        foreach (var (Row, Col, Ch) in _title.GetGraphicInPlace(0, _constants.SimulationArea + _constants.TitlePadding))
        {
            _screenBuffer.DrawToBuffer(Ch, Row, Col);
        }
        var elevatorDataPoints = _elevatorOrchestrator.GetElevatorDataPoints();
        var floorDataPoints = _elevatorOrchestrator.GetFloorDataPoints();
        foreach (PicturePixel pixel in _info.ReturnInfoPixels(elevatorDataPoints, floorDataPoints))
        {
            _screenBuffer.DrawToBuffer(pixel.Ch, TITLE_HEIGHT + pixel.OffsetRow, _constants.SimulationArea + 5 + pixel.OffsetColumn);
        }
        foreach(var floorData in floorDataPoints.Where(dataObj => dataObj.PanelActive))
        {
            var panel = ReturnPanel(
                floorData.UpActive,
                floorData.DownActive,
                _constants.FloorPositions[floorData.NthFloor] - FLOOR_HEIGHT + 1,
                _constants.SimulationArea - _constants.TotalElevatorShaftWidth - 4,
                floorData.NthFloor == _constants.FloorPositions.Length - 2,
                floorData.NthFloor == 0);
            foreach (PicturePixel pixel in panel)
            {
                _screenBuffer.DrawToBuffer(pixel.Ch, pixel.OffsetRow, pixel.OffsetColumn);
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
    }
    public enum GraphicType
    {
        Elevator,
        Human,
    }
}
