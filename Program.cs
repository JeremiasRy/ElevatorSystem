// See https://aka.ms/new-console-template for more information
using ElevatorSystem.Src;
Console.CursorVisible = false;
var state = new MasterState();

state.CallElevator(3, ElevatorSystem.Src.Inputs.FloorCallInput.Direction.Down);
state.CallElevator(2, ElevatorSystem.Src.Inputs.FloorCallInput.Direction.Up);
state.CallElevator(1, ElevatorSystem.Src.Inputs.FloorCallInput.Direction.Up);
state.CallElevator(4, ElevatorSystem.Src.Inputs.FloorCallInput.Direction.Down);
state.CallElevator(5, ElevatorSystem.Src.Inputs.FloorCallInput.Direction.Up);
state.CallElevator(6, ElevatorSystem.Src.Inputs.FloorCallInput.Direction.Up);
state.StartTick();

