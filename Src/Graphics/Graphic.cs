using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class Graphic
{
    readonly PicturePixel[] _picture;
    readonly ScreenBuffer _screenBuffer;
    public void Draw(int y, int x)
    {
        foreach (var pixel in _picture)
        {
            _screenBuffer.DrawToBuffer(pixel.Ch, y + pixel.OffsetY, x + pixel.OffsetX);
        }
    }
    public Graphic(string filePath, ScreenBuffer buffer)
    {
        _screenBuffer = buffer;
        using var sr = new StreamReader(filePath);
        List<string> lines = new();
        int count = 0;
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine() ?? throw new Exception("Tried to read the end of the file");
            lines.Add(line);
            count += line.Length;
        }
        _picture = new PicturePixel[count];

        count = 0;
        for (int iy = 0; iy < lines.Count; iy++)
        {
            for (int ix = 0; ix < lines[iy].Length; ix++)
            {
                _picture[count++] = new PicturePixel(iy, ix, lines[iy].ElementAt(ix));
            }
        }
    }
}
