using ElevatorSystem.Src.Controllers;
using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.Inputs;
using System.Drawing;

namespace ElevatorSystem.Src;

public class MasterState
{
    int _selectedFloor = -1;
    bool _openInputPanel = false;
    int _openInputPanelElevatorId = -1;
    int _openInputPanelUserId = -1;
    readonly ElevatorOrchestrator _elevatorOrchestrator;
    readonly ViewController _viewController;
    readonly KeyboardInput _keyboardInput;
    readonly Constants _constants = new ();
    readonly List<UserCall> _userCalls = new();
    public void StartTick()
    {
        int tickCount = 0;
        while (true)
        {
            if (tickCount++ == 10)
            {
                Tick();
                tickCount = 0;
            }
            CheckKeyboardInput();
            SetupView();
            _viewController.Draw(_openInputPanel);
            Thread.Sleep(20);
        }
    }
    void CheckKeyboardInput()
    {
        if (_keyboardInput.KeyboardKeyDown(out List<ConsoleKey> keys))
        {
            foreach (var key in keys)
            {
                if (KeyboardInput.ConvertConsoleKeyToInt(key, out int floor) && !_openInputPanel)
                {
                    ActivateFloorCallPanel(floor);
                    break;
                }
                if (KeyboardInput.ConvertConsoleKeyToDirection(key, out UserCall.Direction direction) && _selectedFloor != -1)
                {
                    ActivateFloorCall(direction);
                    break;
                }
            }
        }
    }
    void SetupView()
    {
        _viewController.SetOpenFloorInput(_selectedFloor);
        _viewController.SetInfoData(_elevatorOrchestrator.GetElevatorDataPoints(), _elevatorOrchestrator.GetFloorDataPoints());
        _viewController.SetElevatorPositions(_elevatorOrchestrator.GetElevatorPositions());
        if (_openInputPanel)
        {
            _viewController.SetInputPanel(_elevatorOrchestrator.GetElevatorControllerById(_openInputPanelElevatorId).ReturnData().InputPanel);
        }
    }
    void ActivateFloorCallPanel(int floor)
    {
        if (floor >= _constants.FloorCount - 1)
        {
            _selectedFloor = -1;
            return;
        }
        _selectedFloor = floor;
    }
    void ActivateFloorCall(UserCall.Direction direction)
    {
        var result = _elevatorOrchestrator.CallElevator(_selectedFloor, direction);
        if (result)
        {
            _userCalls.Add(new UserCall(_selectedFloor, direction));
        }
        _selectedFloor = -1;
    }
    void Tick()
    {
        _elevatorOrchestrator.Tick();
        if (_openInputPanel)
        {
            bool[] inputPanel = _elevatorOrchestrator.GetElevatorData(_openInputPanelElevatorId).InputPanel;
            _viewController.SetInputPanel(inputPanel);
        }
    }
    public MasterState()
    {
        _keyboardInput = new KeyboardInput();
        _elevatorOrchestrator = new ElevatorOrchestrator();
        _viewController = new ViewController();
    }
}
