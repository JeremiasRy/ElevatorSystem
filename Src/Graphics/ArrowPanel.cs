using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class ArrowPanel : IGraphic
{
    const string ARROW_UP_ACTIVE = " ^ \r\n/█\\";
    const string ARROW_UP = " ^ \r\n/ \\";
    const string ARROW_DOWN_ACTIVE = "\\█/\r\n v ";
    const string ARROW_DOWN = "\\ /\r\n v ";
    
    Active _panelStatus = 0;
    readonly PicturePixelDefinition[] _arrowUp;
    readonly PicturePixelDefinition[] _arrowUpActive;
    readonly PicturePixelDefinition[] _arrowDown;
    readonly PicturePixelDefinition[] _arrowDownActive;

    public void SetPanelStatusUpActive()
    {
        if ((_panelStatus & Active.Up) == Active.Up)
        {
            return;
        }
        _panelStatus |= Active.Up;
    }
    public void SetPanelStatusDownActive()
    {
        if ((_panelStatus & Active.Down) == Active.Down)
        {
            return;
        }
        _panelStatus |= Active.Down;
    }
    public void SetPanelStatusUpInActive()
    {
        if ((_panelStatus & Active.Up) != Active.Up)
        {
            return;
        }
        _panelStatus ^= Active.Up;
    }
    public void SetPanelStatusDownInActive()
    {
        if ((_panelStatus & Active.Down) != Active.Down)
        {
            return;
        }
        _panelStatus ^= Active.Down;
    }
    public PicturePixel[] GetGraphic(int row, int col)
    {
        List<PicturePixel> panel = new(_arrowUp.Length * 2);
        
        panel.AddRange((_panelStatus & Active.Up) == Active.Up 
            ? _arrowUpActive.Select(pixelDefinition => pixelDefinition.ReturnPixel(row, col)) 
            : _arrowUp.Select(pixelDefinition => pixelDefinition.ReturnPixel(row, col)));

        panel.AddRange((_panelStatus & Active.Down) == Active.Down
            ? _arrowDownActive.Select(pixelDefinition => pixelDefinition.ReturnPixel(row + 4, col))
            : _arrowDown.Select(pixelDefinition => pixelDefinition.ReturnPixel(row + 4, col)));

        return panel.ToArray();
    }
    public ArrowPanel()
    {
        _arrowUp = ArrowBuilder(ARROW_UP);
        _arrowUpActive = ArrowBuilder(ARROW_UP_ACTIVE);
        _arrowDown = ArrowBuilder(ARROW_DOWN);
        _arrowDownActive = ArrowBuilder(ARROW_DOWN_ACTIVE);
    }
    static PicturePixelDefinition[] ArrowBuilder(string arrow)
    {
        string[] lines = arrow.Split("\r\n");
        List<PicturePixelDefinition> result = new();
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                result.Add(new PicturePixelDefinition(row, col, lines[row][col]));
            }
        }
        return result.ToArray();
    }
    [Flags]
    enum Active
    {
        None = 0,
        Up = 1,
        Down = 1 << 1,
    }
}
