using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.Inputs;

namespace ElevatorSystem.Src;

public class MasterState
{
    int _selectedFloor = -1;
    readonly ElevatorOrchestrator _elevatorOrchestrator;
    readonly ViewController _viewController;
    readonly KeyboardInput _keyboardInput;
    readonly Constants _constants = new ();
    public void StartTick()
    {
        while (true)
        {
            Tick();
            if (_keyboardInput.KeyboardKeyDown(out List<ConsoleKey> keys))
            {
                HandleUserInput(keys);
            };
            Thread.Sleep(20);
        }
    }
    void HandleUserInput(List<ConsoleKey> keys)
    {
        foreach (ConsoleKey key in keys)
        {
            if (KeyboardInput.ConvertConsoleKeyToInt(key, out int floor))
            {
                _selectedFloor = floor;
                ActivateFloorCallPanel(floor);
                continue;
            }
            if (KeyboardInput.ConverstConsoleKeyToDirection(key, out FloorCallInput.Direction direction) && _selectedFloor != -1)
            {
                CallElevator(direction);
            }
        }
    }
    
    void ActivateFloorCallPanel(int floor)
    {
        if (floor >= _constants.FloorCount - 1)
        {
            _selectedFloor = -1;
            return;
        }
        _elevatorOrchestrator.ActivateFloorCall(floor);
    }
    void CallElevator(FloorCallInput.Direction direction)
    {
        _elevatorOrchestrator.CallElevator(_selectedFloor, direction);
        _selectedFloor = -1;
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
