using ElevatorSystem.Src.View;
using System.Text;
using static ElevatorSystem.Src.Constants;
namespace ElevatorSystem.Src.Graphics;
public class ViewController
{
    private readonly Graphic _human;
    private readonly Graphic _elevator;
    private readonly Graphic _title;
    private readonly ScreenBuffer _screenBuffer;
    private readonly ElevatorController _elevatorController;
    public void Draw()
    {
        foreach (var (row, col) in _elevatorController.ElevatorPositions)
        {
            foreach(var (Row, Col, Ch) in _elevator.GetGraphicInPlace(row, col + 2, true))
            {
                _screenBuffer.DrawToBuffer(Ch, Row, Col);
            }
        }
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            foreach (var cable in CablePositions)
            {
                int count = 0;
                if (i < _elevatorController.ElevatorPositions[count++].Row - ELEVATOR_HEIGHT)
                {
                    _screenBuffer.DrawToBuffer('|', i, cable);
                }
            }
        }
        _screenBuffer.DrawBuffer();
    }
    public void DrawBackground() 
    {
        string floorStr = new('-', SimulationArea - TotalElevatorShaftWidth);
        int floorCount = 0;
        for (int i = Console.WindowHeight - 1; i >= 0; i--)
        {
            foreach (var shaft in ShaftPositions)
            {
                _screenBuffer.DrawToBuffer('|', i, shaft);
                _screenBuffer.DrawToBuffer('|', i, shaft + SHAFT_WIDTH);
            }
            
            for (int cable = 0; cable < CablePositions.Length; cable++)
            {
                if (i + ELEVATOR_HEIGHT <= _elevatorController.ElevatorPositions[cable].Row)
                {
                    _screenBuffer.DrawToBuffer('|', i, CablePositions[cable]);
                }
            }
            if (i == Console.WindowHeight - 1 - FLOOR_HEIGHT * floorCount && floorCount <= FloorCount)
            {
                _screenBuffer.DrawToBuffer(floorStr, i);
                floorCount++;
            }
        }
        foreach (var (Row, Col, Ch) in _title.GetGraphicInPlace(0, SimulationArea + TitlePadding))
        {
            _screenBuffer.DrawToBuffer(Ch, Row, Col);
        }
    }
    public ViewController(ElevatorController elevatorController)
    {
        _elevatorController = elevatorController;
        _elevator = new Graphic("../../../Assets/Elevator.txt");
        _human = new Graphic("../../../Assets/Human.txt");
        _title = new Graphic("../../../Assets/Title.txt");
        _screenBuffer = ScreenBuffer.GetInstance();
        DrawBackground();
    }
    public enum GraphicType
    {
        Elevator,
        Human,
    }
}
