using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public static class Constants
{
    public const int FLOOR_HEIGHT = 7;
    public const int SHAFT_WIDTH = 12;
    public const int PADDING = 2;
    public const int ELEVATOR_HEIGHT = 5;
    public const int ELEVATOR_WIDTH = 9;
    public const int ELEVATOR_COUNT = 7;
    public const int TITLE_WIDTH = 35;
    public static int SimulationArea => Console.WindowWidth / 3 * 2;
    public static int FloorCount => Console.WindowHeight / FLOOR_HEIGHT;
    public static int TotalElevatorShaftWidth => SHAFT_WIDTH * ELEVATOR_COUNT + PADDING * (ELEVATOR_COUNT - 1);
    public static int TitlePadding => (Console.WindowWidth - SimulationArea - TITLE_WIDTH) / 2;
    public static int[] CablePositions 
    { 
        get
        {
            var result = new int[ELEVATOR_COUNT];
            for (int i = 0; i < ELEVATOR_COUNT; i++)
            {
                result[i] = ShaftPositions[i] + SHAFT_WIDTH / 2;
            }
            return result;
        } 
    }
    public static int[] ShaftPositions
    {
        get
        {
            var result = new int[ELEVATOR_COUNT];
            for (int i = 0; i < ELEVATOR_COUNT; i++)
            {
                result[i] = SimulationArea - TotalElevatorShaftWidth + (SHAFT_WIDTH + PADDING) * i;
            }
            return result;
        }
    }

}
