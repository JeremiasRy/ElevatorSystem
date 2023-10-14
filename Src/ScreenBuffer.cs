using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class ScreenBuffer
{
    static ScreenBuffer? _instance;
    readonly int _height;
    readonly int _width;
    readonly char[][] _buffer;
    public void DrawToBuffer(string str, int y = 0, int x = 0)
    {
        if (y >= _height || x + str.Length >= _width)
        {
            return;
        }
        for (int i = 0; i < str.Length; i++)
        {
            _buffer[y][x + i] = str[i];
        }
    }
    public void DrawToBuffer(char ch, int y = 0, int x = 0)
    {
        if (y >= _height || x >= _width)
        {
            return;
        }
        _buffer[y][x] = ch;
    }

    public void DrawBuffer()
    {
        for (int i = 0; i < _height; i++)
        {
            Console.WriteLine(_buffer[i]);
            Array.Fill(_buffer[i], ' '); // Clear buffer while we are looping throug it
        }
        Console.SetCursorPosition(0, 0);
    }

    void ClearBuffer()
    {
        for (int iy = 0; iy < _height; iy++)
        {
            Array.Fill(_buffer[iy], ' ');
        }
    }
    private ScreenBuffer()
    {
        _height = Console.WindowHeight;
        _width = Console.WindowWidth;
        _buffer = new char[_height][];
        for (int iy = 0; iy < _height; iy++)
        {
            _buffer[iy] = new char[_height];
            Array.Fill(_buffer[iy], ' ');
        }
    }
    public static ScreenBuffer GetInstance()
    {
        _instance ??= new ScreenBuffer();
        return _instance;
    }
}
