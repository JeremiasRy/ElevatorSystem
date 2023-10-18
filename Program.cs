// See https://aka.ms/new-console-template for more information
using ElevatorSystem.Src;
Console.CursorVisible = false;
var state = new MasterState();
state.CallElevator(7, 0);
state.CallElevator(6, 0);
state.CallElevator(5, 0);
state.CallElevator(4, 0);
state.CallElevator(3, 0);
state.CallElevator(2, 0);
state.CallElevator(1, 0);
state.CallElevator(0, 7);
state.CallElevator(0, 6);
state.CallElevator(0, 5);
state.CallElevator(0, 4);
state.CallElevator(0, 3);
state.CallElevator(0, 2);
state.CallElevator(0, 1);

state.StartTick();

