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
            SetupView();
            _viewController.Draw(_openInputPanel);
            if (tickCount == 10)
            {
                Tick();
                tickCount = 0;
            }

            tickCount++;
            Thread.Sleep(20);
        }
    }
    void SetupView()
    {
        _viewController.SetOpenFLoorInput(_selectedFloor);
        _viewController.SetInfoData(_elevatorOrchestrator.GetElevatorDataPoints(), _elevatorOrchestrator.GetFloorDataPoints());
        _viewController.SetElevatorPositions(_elevatorOrchestrator.GetElevatorPositions());
    }
    void HandleUserInput(List<ConsoleKey> keys)
    {
    }
    void HandleUserCallsElevatorInputPanel(int floor)
    {
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
    void CallElevator(UserCall.Direction direction)
    {

    }

    void HandleElevatorsToUserCalls()
    {

    }
    void PutHumansInsideElevator(IEnumerable<UserCall> userCalls, ElevatorController elevator)
    {

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
