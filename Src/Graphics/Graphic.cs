namespace ElevatorSystem.Src.Graphics;
public class Graphic
{
    readonly PicturePixel[] _pixels;
    public (int Row, int Col, char Ch)[] GetGraphicInPlace(int row, int col) 
    {
        return _pixels.Select(pixel => (row + pixel.OffsetRow, col + pixel.OffsetColumn, pixel.Ch)).ToArray(); 
    }
    public Graphic(string filePath)
    {
        using var sr = new StreamReader(filePath);
        List<string> lines = new();
        int count = 0;
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine() ?? throw new Exception("Tried to read the end of the file");
            lines.Add(line);
            count += line.Length;
        }
        _pixels = new PicturePixel[count];
        count = 0;
        for (int iy = 0; iy < lines.Count; iy++)
        {
            for (int ix = 0; ix < lines[iy].Length; ix++)
            {
                _pixels[count++] = new PicturePixel(iy, ix, lines[iy].ElementAt(ix));
            }
        }
    }
}
