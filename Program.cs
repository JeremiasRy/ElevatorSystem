// See https://aka.ms/new-console-template for more information
using ElevatorSystem.Src;
using ElevatorSystem.Src.Graphic;
Console.CursorVisible = false;
ScreenBuffer buffer = ScreenBuffer.GetInstance();
var elevatorGraphic = new Graphic("../../../Assets/elevator.txt", buffer);
elevatorGraphic.Draw();
buffer.DrawBuffer();
Console.ReadKey();
