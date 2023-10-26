using ElevatorSystem.Src.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class Info : IGraphic
{
    readonly Constants _constants;
    public ElevatorData[] ElevatorData { get; private set; } = Array.Empty<ElevatorData>();
    public FloorData[] FloorData { get; private set; } = Array.Empty<FloorData>();
    static char Active(bool active) => active ? '█' : '-';
    public void SetInfoData(List<ElevatorData> elevatorDatas, List<FloorData> floorDatas)
    {
        ElevatorData = elevatorDatas.ToArray();
        FloorData = floorDatas.ToArray();
    }
    public PicturePixel[] GetGraphic(int row, int col)
    {
        var infoAreaSize = Console.WindowWidth - _constants.SimulationArea - 6;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{new('-', infoAreaSize / 2 - 2)}Info{new('-', infoAreaSize / 2 - 2)}");
        int count = 1;
        foreach (var elevatorDataPoint in ElevatorData)
        {
            stringBuilder.AppendLine($"Elevator {count++}");
            stringBuilder.AppendLine($"Destination Row: {elevatorDataPoint.Destination}");
            stringBuilder.AppendLine($"Current Row: {elevatorDataPoint.CurrentLocation}");
            stringBuilder.AppendLine($"Destination Floor: {elevatorDataPoint.DestinationFloor}");
            stringBuilder.AppendLine("--");
        }
        foreach (var floorDataPoint in FloorData)
        {
            stringBuilder.AppendLine($"Floor {floorDataPoint.NthFloor} | UP {Active(floorDataPoint.UpActive)} | DOWN {Active(floorDataPoint.DownActive)} |");
        }
        string[] lines = stringBuilder.ToString().Split("\r\n");
        List<PicturePixel> pixels = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                pixels.Add(new PicturePixel(y + row, x + col, lines[y][x]));
            }
        }
        return pixels.ToArray();
    }

    public Info()
    {
        _constants = new Constants();
    }
}
