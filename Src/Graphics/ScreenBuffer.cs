using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ScreenBuffer
{
    static ScreenBuffer? _instance;
    readonly int _rows;
    readonly int _columns;
    readonly char[] _buffer;

    int GetIndex(int row, int column) => row * _columns + column;
    public void DrawToBuffer(string str, int row = 0, int col = 0)
    {
        var startIndex = GetIndex(row, col);
        for (int i = 0; i < str.Length; i++)
        {
            _buffer[startIndex + i] = str[i];
        }
    }
    public void DrawToBuffer(char ch, int row = 0, int col = 0)
    {
        _buffer[GetIndex(row, col)] = ch;
    }

    public void DrawBuffer()
    {
        int cursorPos = 0;
        foreach (char[] row in _buffer.Chunk(_columns))
        {
            Console.SetCursorPosition(0, cursorPos++);
            Console.Write(row);
        }
        Console.SetCursorPosition(0, 0);
        ClearBuffer();
    }
    void ClearBuffer() => Array.Fill(_buffer, ' ');
    private ScreenBuffer()
    {
        _rows = Console.WindowHeight;
        _columns = Console.WindowWidth;
        _buffer = new char[_rows * _columns];
        Array.Fill(_buffer, ' ');
    }
    public static ScreenBuffer GetInstance()
    {
        _instance ??= new ScreenBuffer();
        return _instance;
    }
}
