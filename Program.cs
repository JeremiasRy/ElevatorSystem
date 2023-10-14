// See https://aka.ms/new-console-template for more information
using ElevatorSystem;

ScreenBuffer buffer = ScreenBuffer.GetInstance();
var elevatorGraphic = new Graphic("../../../elevator.txt", buffer);
elevatorGraphic.Draw();
buffer.DrawBuffer();
