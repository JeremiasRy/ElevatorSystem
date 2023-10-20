﻿using ElevatorSystem.Src.View;
using System.Text;
using static ElevatorSystem.Src.Constants;
namespace ElevatorSystem.Src.Graphics;
public class ViewController
{
    private readonly Constants _constants;
    private readonly Graphic _human;
    private readonly Graphic _elevator;
    private readonly Graphic _title;
    private readonly ScreenBuffer _screenBuffer;
    private readonly ElevatorController _elevatorController;
    public void Draw()
    {
        DrawBackground();
        foreach (var (row, col) in _elevatorController.ElevatorPositions)
        {
            foreach(var (Row, Col, Ch) in _elevator.GetGraphicInPlace(row, col + 2, true))
            {
                _screenBuffer.DrawToBuffer(Ch, Row, Col);
            }
        }
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            foreach (var cable in _constants.CablePositions)
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
        string floorStr = new('-', _constants.SimulationArea - _constants.TotalElevatorShaftWidth);
        int floorCount = 0;
        for (int i = Console.WindowHeight - 1; i >= 0; i--)
        {
            foreach (var shaft in _constants.ShaftPositions)
            {
                _screenBuffer.DrawToBuffer('|', i, shaft);
                _screenBuffer.DrawToBuffer('|', i, shaft + SHAFT_WIDTH);
            }
            
            for (int cable = 0; cable < _constants.CablePositions.Length; cable++)
            {
                if (i + ELEVATOR_HEIGHT <= _elevatorController.ElevatorPositions[cable].Row)
                {
                    _screenBuffer.DrawToBuffer('|', i, _constants.CablePositions[cable]);
                }
            }
            if (i == Console.WindowHeight - 1 - FLOOR_HEIGHT * floorCount && floorCount <= _constants.FloorCount)
            {
                _screenBuffer.DrawToBuffer(floorStr, i);
                floorCount++;
            }
        }
        foreach (var (Row, Col, Ch) in _title.GetGraphicInPlace(0, _constants.SimulationArea + _constants.TitlePadding))
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
        _constants = new Constants();
    }
    public enum GraphicType
    {
        Elevator,
        Human,
    }
}
