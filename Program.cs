// See https://aka.ms/new-console-template for more information
using ElevatorSystem.Src;

var state = new ElevatorStateHandler();
state.CreateHuman();
state.Tick();
Console.ReadKey();