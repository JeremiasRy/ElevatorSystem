// See https://aka.ms/new-console-template for more information
using ElevatorSystem.Src;

Console.CursorVisible = false;
var state = new GuiHandler();
state.Tick();
state.CallElevator(0, 4);
state.CallElevator(3, 0);
state.CallElevator(0, 2);
state.CallElevator(1, 2);
state.CallElevator(0, 1);
while (true)
{
    Thread.Sleep(200);
    state.Tick();
}
