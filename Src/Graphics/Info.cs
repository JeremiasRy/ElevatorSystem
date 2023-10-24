using ElevatorSystem.Src.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class Info
{
    readonly Constants _constants;
    public PicturePixel[] ReturnInfoPixels(List<ElevatorData> elevatorData, List<FloorData> floorData)
    {
        var infoAreaSize = Console.WindowWidth - _constants.SimulationArea - 6;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{new('-', infoAreaSize / 2 - 2)}Info{new('-', infoAreaSize / 2 - 2)}");
        int count = 1;
        foreach (var elevatorDataPoint in elevatorData)
        {
            stringBuilder.AppendLine($"Elevator {count++}");
            stringBuilder.AppendLine($"Destination Row: {elevatorDataPoint.Destination}");
            stringBuilder.AppendLine($"Current Row: {elevatorDataPoint.CurrentLocation}");
            stringBuilder.AppendLine($"Destination Floor: {elevatorDataPoint.DestinationFloor}");
            stringBuilder.AppendLine("--");
        }
        foreach (var floorDataPoint in floorData)
        {
            stringBuilder.AppendLine($"Floor {floorDataPoint.NthFloor} | UP {Active(floorDataPoint.UpActive)} | DOWN {Active(floorDataPoint.DownActive)} |");
        }
        string[] lines = stringBuilder.ToString().Split("\r\n");
        List<PicturePixel> pixels = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for(int x = 0; x < lines[y].Length; x++)
            {
                pixels.Add(new PicturePixel(y, x, lines[y][x]));
            }
        }
        return pixels.ToArray();
    }
    static char Active(bool active) => active ? '█' : '-';
    public Info()
    {
        _constants = new Constants();
    }
}
