using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class ElevatorInputPanel : IGraphic
{
    private readonly PicturePixelDefinition[] _picturePixelDefinitions;
    public PicturePixel[] GetGraphic(int row, int col) => _picturePixelDefinitions.Select(pixelDefinition => pixelDefinition.ReturnPixel(row, col)).ToArray();
    public ElevatorInputPanel()
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            if (i == 0 || i == 19)
            {
                stringBuilder.AppendLine(new('_', 30));
                continue;
            }
            stringBuilder.AppendLine($"|{new(' ', 28)}|");
        }
        string[] strings = stringBuilder.ToString().Split("\r\n");
        _picturePixelDefinitions = new PicturePixelDefinition[20 * 30];
        for (int row = 0; row < strings.Length; row++) 
        {
            for (int col = 0; col < strings[row].Length; col++ )
            {
                _picturePixelDefinitions[row * 30 + col] = new PicturePixelDefinition(row, col, strings[row][col]);
            }
        }
    }
}
