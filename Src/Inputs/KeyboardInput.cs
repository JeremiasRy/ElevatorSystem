﻿using System.Runtime.InteropServices;
using ElevatorSystem.Src.Simulation;

namespace ElevatorSystem.Src.Inputs;

public class KeyboardInput
{
    const byte _highBit = 0x1 << 7;
    [DllImport("user32.dll")]
    static extern bool GetKeyboardState(byte[] lpKeyState);
    [DllImport("user32.dll")]
    static extern short GetKeyState();
    readonly byte[] _array = new byte[256];
    public bool KeyboardKeyDown(out List<ConsoleKey> keys)
    {
        keys = new List<ConsoleKey>();
        GetKeyState();
        GetKeyboardState(_array);
        for (int i = 0; i < _array.Length; i++)
        {
            if ((_array[i] & _highBit) != 0)
            {
                keys.Add((ConsoleKey)i);
            }
        }
        return keys.Any();
    }
    public static bool ConvertConsoleKeyToInt(ConsoleKey key, out int value)
    {
        value = ConsoleKeyStringToInt(key.ToString());
        return value >= 0;
        
    }
    public static bool ConvertConsoleKeyToDirection(ConsoleKey key, out Human.Direction direction)
    {
        direction = Human.Direction.Up;
        int result = ConsoleKeyStringToDirection(key.ToString());
        if (result < 0)
        {
            return false;
        }
        direction = (Human.Direction)result;
        return true;
    }
    static int ConsoleKeyStringToDirection(string key)
    {
        return key switch
        {
            "UpArrow" => 0,
            "DownArrow" => 1,
            _ => -1
        };
    }
    static int ConsoleKeyStringToInt(string key)
    {
        return key switch
        {
            "D0" => 0,
            "D1" => 1,
            "D2" => 2,
            "D3" => 3,
            "D4" => 4,
            "D5" => 5,
            "D6" => 6,
            "D7" => 7,
            "D8" => 8,
            "D9" => 9,
            _ => -1,
        };
    }
}
