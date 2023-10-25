using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public static class ArrowPanel
{
    public static PicturePixel[] ReturnPanel(bool upActive, bool downActive, int row, int col, bool upDisabled = false, bool downDisabled = false)
    {
        var result = new List<PicturePixel>();
        if (!upDisabled)
        {
            result.AddRange(upActive ? ArrowUp.ArrowActive(row, col) : ArrowUp.ArrowInActive(row, col));
        }
        if (!downDisabled)
        {
            result.AddRange(downActive ? ArrowDown.ArrowActive(row + 4, col) : ArrowDown.ArrowInActive(row + 4, col));
        }
        return result.ToArray();
    }
    internal static class ArrowUp
    {
        public static PicturePixel[] ArrowActive(int row, int col)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(" ^ ");
            stringBuilder.AppendLine("/█\\");
            string[] lines = stringBuilder.ToString().Split("\r\n");
            List<PicturePixel> result = new();
            for (int offsetRow = 0; offsetRow < lines.Length; offsetRow++)
            {
                for (int offsetCol = 0; offsetCol < lines[offsetRow].Length; offsetCol++)
                {
                    result.Add(new PicturePixel(row + offsetRow, col + offsetCol, lines[offsetRow][offsetCol]));
                }
            }
            return result.ToArray();
        }
        public static PicturePixel[] ArrowInActive(int row, int col) 
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" ^ ");
            stringBuilder.AppendLine("/ \\");
            string[] lines = stringBuilder.ToString().Split("\r\n");
            List<PicturePixel> result = new();
            for (int offsetRow = 0; offsetRow < lines.Length; offsetRow++)
            {
                for (int offsetCol = 0; offsetCol < lines[offsetRow].Length; offsetCol++)
                {
                    result.Add(new PicturePixel(row + offsetRow, col + offsetCol, lines[offsetRow][offsetCol]));
                }
            }
            return result.ToArray();
        }
    }
    internal static class ArrowDown
    {
        public static PicturePixel[] ArrowActive(int row, int col)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("\\█/");
            stringBuilder.AppendLine(" v ");
            string[] lines = stringBuilder.ToString().Split("\r\n");
            List<PicturePixel> result = new();
            for (int offsetRow = 0; offsetRow < lines.Length; offsetRow++)
            {
                for (int offsetCol = 0; offsetCol < lines[offsetRow].Length; offsetCol++)
                {
                    result.Add(new PicturePixel(row + offsetRow, col + offsetCol, lines[offsetRow][offsetCol]));
                }
            }
            return result.ToArray();
        }
        public static PicturePixel[] ArrowInActive(int row, int col)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\\ /");
            stringBuilder.AppendLine(" v ");
            string[] lines = stringBuilder.ToString().Split("\r\n");
            List<PicturePixel> result = new();
            for (int offsetRow = 0; offsetRow < lines.Length; offsetRow++)
            {
                for (int offsetCol = 0; offsetCol < lines[offsetRow].Length; offsetCol++)
                {
                    result.Add(new PicturePixel(row + offsetRow, col + offsetCol, lines[offsetRow][offsetCol]));
                }
            }
            return result.ToArray();
        }
    }
}
