using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class Constants
{
    public const int FLOOR_HEIGHT = 7;
    public const int SHAFT_WIDTH = 12;
    public const int PADDING = 2;
    public const int ELEVATOR_HEIGHT = 5;
    public const int ELEVATOR_WIDTH = 9;
    public const int ELEVATOR_COUNT = 6;
    public const int TITLE_WIDTH = 35;
    public int[] CablePositions;
    public int[] ShaftPositions;
    public int[] FloorPositions;
    public int SimulationArea = Console.WindowWidth / 3 * 2;
    public int FloorCount = Console.WindowHeight / FLOOR_HEIGHT;
    public int TotalElevatorShaftWidth = SHAFT_WIDTH * ELEVATOR_COUNT + PADDING * (ELEVATOR_COUNT - 1);
    public int TitlePadding;
    public Constants()
    {
        ShaftPositions = new int[ELEVATOR_COUNT];
        CablePositions = new int[ELEVATOR_COUNT];
        for (int i = 0; i < ELEVATOR_COUNT; i++)
        {
            ShaftPositions[i] = SimulationArea - TotalElevatorShaftWidth + (SHAFT_WIDTH + PADDING) * i;
            CablePositions[i] = ShaftPositions[i] + SHAFT_WIDTH / 2;
        }
        TitlePadding = (Console.WindowWidth - SimulationArea - TITLE_WIDTH) / 2;
        FloorPositions = new int[FloorCount];
        for (int i = 0; i < FloorCount; i++)
        {
            FloorPositions[i] = Console.WindowHeight - 1 - FLOOR_HEIGHT * i;
        }
    }
}
