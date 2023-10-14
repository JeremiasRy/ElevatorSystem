// See https://aka.ms/new-console-template for more information
using ElevatorSystem.Src;

var state = new BuildingState();
state.CreateHuman();
state.Tick();
Console.ReadKey();