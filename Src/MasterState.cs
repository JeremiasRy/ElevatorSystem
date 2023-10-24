using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.Inputs;

namespace ElevatorSystem.Src;

public class MasterState
{
    readonly ElevatorOrchestrator _elevatorOrchestrator;
    readonly ViewController _viewController;
    readonly KeyboardInput _keyboardInput;
    readonly Constants _constants = new Constants();
    public void StartTick()
    {
        while (true)
        {
            Tick();
            if (_keyboardInput.KeyboardKeyDown(out List<ConsoleKey> keys))
            {
                if (KeyboardInput.ConvertConsoleKeyToInt(keys.First(), out int floor))
                {
                    CallElevator(floor, FloorCallInput.Direction.Up);
                }
            };
            Thread.Sleep(20);
        }
    }
    void CallElevator(int floor, FloorCallInput.Direction dir)
    {
        if (floor >= _constants.FloorCount)
        {
            return;
        }
        _elevatorOrchestrator.ActivateFloorCall(new FloorCallInput() { Floor = floor, RequestDirection = dir });
    }
    void CallElevatorPanelInput(int floor, int elevatorIdx)
    {

    }
    void Tick()
    {
        _elevatorOrchestrator.Tick();
        _viewController.Draw();
    }
    public MasterState()
    {
        _keyboardInput = new KeyboardInput();
        _elevatorOrchestrator = new ElevatorOrchestrator();
        _viewController = new ViewController(_elevatorOrchestrator);
    }
}
