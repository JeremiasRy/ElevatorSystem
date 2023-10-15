// See https://aka.ms/new-console-template for more information
using ElevatorSystem.Src;

var state = new BuildingState();
state.Tick();
Console.ReadKey();