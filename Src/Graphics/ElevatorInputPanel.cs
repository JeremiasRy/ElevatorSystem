using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src.Graphics;

public class ElevatorInputPanel : IGraphic
{
    const int CENTER = 12;
    private readonly PicturePixelDefinition[] _button;
    private readonly PicturePixelDefinition[] _buttonActive;
    private readonly PicturePixelDefinition[] _border;
    private readonly bool[] _inputPanel;
    public void SetInputPanel(bool[] inputPanel)
    {
        for (int i = 0; i < _inputPanel.Length; i++)
        {
            _inputPanel[i] = inputPanel[i];
        }
    }
    public PicturePixel[] GetGraphic(int row, int col)
    {
        List<PicturePixel> result = new ();
        result.AddRange(_border.Select(pixelDefinition => pixelDefinition.ReturnPixel(row, col)));
        col += 3;
        row += 1;
        for (int i = _inputPanel.Length - 1; i >= 0; i--)
        {
            if (_inputPanel[i])
            {
                for (int j = 0; j < _buttonActive.Length; j++)
                {
                    if (j == CENTER)
                    {
                        result.Add(_buttonActive[j].ReturnPixel(row, col, char.Parse(i.ToString())));
                        continue;
                    }
                    result.Add(_buttonActive[j].ReturnPixel(row, col));
                }
            } else
            {
                for (int j = 0; j < _button.Length; j++)
                {
                    if (j == CENTER)
                    {
                        result.Add(_button[j].ReturnPixel(row, col, char.Parse(i.ToString())));
                        continue;
                    }
                    result.Add(_button[j].ReturnPixel(row, col));
                }
            }
            row += 4;
        }
        return result.ToArray();
    }
    public ElevatorInputPanel()
    {
        var constants = new Constants();
        _inputPanel = new bool[constants.FloorCount - 1];
        var lines = new List<string>();
        int count = 0;
        using (var sr = new StreamReader("../../../Assets/Button.txt"))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine() ?? throw new Exception("Is everything in place?");
                lines.Add(line);
                count += line.Length;
            }
        }
        _button = new PicturePixelDefinition[count];
        for (int row = 0; row < lines.Count; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                _button[row * lines[row].Length + col] = new PicturePixelDefinition(row, col, lines[row][col]);
            }
        }
        lines = new List<string>();
        count = 0;
        using (var sr = new StreamReader("../../../Assets/ButtonActive.txt"))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine() ?? throw new Exception("Is everything in place?");
                lines.Add(line);
                count += line.Length;
            }
        }
        _buttonActive = new PicturePixelDefinition[count];
        for (int row = 0; row < lines.Count; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                _buttonActive[row * lines[row].Length + col] = new PicturePixelDefinition(row, col, lines[row][col]);
            }
        }
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(new string('-', 11));
        for (int i = 0; i < (constants.FloorCount - 1) * 4; i++)
        {
            stringBuilder.AppendLine("|         |");
        }
        stringBuilder.AppendLine("|         |");
        stringBuilder.AppendLine(new string('-', 11));
        string[] strings = stringBuilder.ToString().Split("\r\n");
        var result = new List<PicturePixelDefinition>();
        
        for (int row = 0; row < strings.Length; row++)
        {
            for (int col = 0; col < strings[row].Length; col++)
            {
                result.Add(new PicturePixelDefinition(row, col, strings[row][col]));
            }
        }
        _border = result.ToArray(); 
    }
}
