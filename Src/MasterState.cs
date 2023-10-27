using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.Inputs;
using System.Drawing;

namespace ElevatorSystem.Src;

public class MasterState
{
    int _selectedFloor = -1;
    bool _openInputPanel = false;
    readonly ElevatorOrchestrator _elevatorOrchestrator;
    readonly ViewController _viewController;
    readonly KeyboardInput _keyboardInput;
    readonly Constants _constants = new ();
    readonly List<UserCall> _userCalls = new();
    public void StartTick()
    {
        while (true)
        {
            Tick();
            if (_keyboardInput.KeyboardKeyDown(out List<ConsoleKey> keys))
            {
                HandleUserInput(keys);
            };
            if (_userCalls.Any() && _elevatorOrchestrator.ElevatorsAtFloor.Length > 0)
            {
                HandleElevatorsToUserCalls();
            }
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
            if (KeyboardInput.ConverstConsoleKeyToDirection(key, out UserCall.Direction direction) && _selectedFloor != -1)
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
    void CallElevator(UserCall.Direction direction)
    {
        _elevatorOrchestrator.CallElevator(_selectedFloor, direction);
        _userCalls.Add(new UserCall(_selectedFloor, direction));
        _selectedFloor = -1;
    }

    void HandleElevatorsToUserCalls()
    {
        var elevators = _elevatorOrchestrator.ElevatorsAtFloor;
        var matches = _userCalls.Where(userCall => elevators.Any(elevator => userCall.Floor == elevator.Floor)).ToList();
        if (!matches.Any())
        {
            return;
        }
        _openInputPanel = true;
        PutHumansInsideElevator(matches);
    }
    void PutHumansInsideElevator(List<UserCall> userCalls)
    {
    }
    void Tick()
    {
        _elevatorOrchestrator.Tick();
        _viewController.Draw(_openInputPanel);
    }
    public MasterState()
    {
        _keyboardInput = new KeyboardInput();
        _elevatorOrchestrator = new ElevatorOrchestrator();
        _viewController = new ViewController(_elevatorOrchestrator);
    }
}
