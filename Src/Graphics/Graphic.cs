using System.Text;

namespace ElevatorSystem.Src.Graphics;
public class Graphic : IGraphic
{
    readonly PicturePixelDefinition[] _pixelDefinitions;
    public PicturePixel[] GetGraphic(int row, int col) => _pixelDefinitions.Select(pixelDefinition => pixelDefinition.ReturnPixel(row, col)).ToArray();
    public Graphic(string filePath)
    {
        using var sr = new StreamReader(filePath);
        var lines = new List<string>();
        int count = 0;
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine() ?? throw new Exception("Tried to read the end of the file");
            lines.Add(line);
            count += line.Length;
        }
        _pixelDefinitions = new PicturePixelDefinition[count];
        for (int row = 0; row < lines.Count; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                _pixelDefinitions[row * lines[row].Length + col] = new PicturePixelDefinition(row, col, lines[row][col]);
            }
        }
    }
}
