using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class Graphic
{
    readonly int _height;
    readonly PicturePixel[] _pixels;
    public (int Row, int Col, char Ch)[] GetGraphicInPlace(int row, int col, bool adjustHeight = false) 
    {
        if (adjustHeight)
        {
            row -= _height;
        }
        return _pixels.Select(pixel => (row + pixel.OffsetY, col + pixel.OffsetX, pixel.Ch)).ToArray(); 
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
        _height = lines.Count;
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
