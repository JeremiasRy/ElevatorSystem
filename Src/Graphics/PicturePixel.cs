using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class PicturePixelDefinition
{
    readonly int _offsetColumn;
    readonly int _offsetRow;
    readonly char _ch;
    public PicturePixel ReturnPixel(int row, int col) => new(row + _offsetRow, col + _offsetColumn, _ch);
    public PicturePixelDefinition(int row, int col, char ch)
    {
        _offsetColumn = col;
        _offsetRow = row;
        _ch = ch;
    }
}

public readonly struct PicturePixel
{
    public readonly int Row;
    public readonly int Column;
    public readonly char Ch;
    public PicturePixel(int row, int col, char ch)
    {
        Row = row;
        Column = col;
        Ch = ch;
    }
}