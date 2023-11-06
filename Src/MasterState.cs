using ElevatorSystem.Src.Controllers;
using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.Inputs;
using ElevatorSystem.Src.Simulation;
using System.Drawing;

namespace ElevatorSystem.Src;

public class MasterState
{
    int _selectedFloor = -1;
    bool _openInputPanel = false;
    int _openInputPanelElevatorId = -1;
    int _openInputPanelUserId = -1;
    int _keyPressCoolDown = -1;
    readonly ElevatorOrchestrator _elevatorOrchestrator;
    readonly ViewController _viewController;
    readonly HumanController _humanController;
    readonly KeyboardInput _keyboardInput;
    readonly Constants _constants = new ();
    public void StartTick()
    {
        int tickCount = 0;
        while (true)
        {
            if (!_openInputPanel)
            {
                Tick();
                FireCalls();
                CheckUserCalls();
            }
            if (_keyPressCoolDown == 0)
            {
                CheckKeyboardInput();
            }
            _keyPressCoolDown = _keyPressCoolDown - 1 < 0 ? 0 : _keyPressCoolDown - 1;
            SetupView();
            _viewController.Draw(_openInputPanel);
            Thread.Sleep(30);
        }
    }
    void FireCalls()
    {
        foreach (var (floor, direction) in _humanController.CallsToFire())
        {
            _elevatorOrchestrator.CallElevator(floor, direction);
        }
    }
    void CheckUserCalls()
    {
        var matchingWaiting = _humanController.HumansWaitingForElevator()
            .Where(userCall => _elevatorOrchestrator.GetElevatorsIdleAtFloor().Any(elevator => elevator.Floor == userCall.StartFloor))
            .Select(userCall => new
            {
               userCall.UserId,
               ElevatorId = _elevatorOrchestrator.GetElevatorsIdleAtFloor().First(elevator => elevator.Floor == userCall.StartFloor).Id,
           });
        if (matchingWaiting.Any())
        {
            var match = matchingWaiting.First();
            _openInputPanel = true;
            _openInputPanelElevatorId = match.ElevatorId;
            _openInputPanelUserId = match.UserId;
        }
        var matchingArrived = _humanController.HumansTravelling()
            .Where(userCall => _elevatorOrchestrator.GetElevatorsIdleAtFloor().Any(elevator => elevator.Floor == userCall.EndFloor));
        if (matchingArrived.Any())
        {
            foreach (var (id, _)in matchingArrived)
            {
                _humanController.SetCallToLeaving(id);
            }
        }
    }
    void CheckKeyboardInput()
    {
        if (_keyboardInput.KeyboardKeyDown(out List<ConsoleKey> keys))
        {
            _keyPressCoolDown = 5;
            foreach (var key in keys)
            {
                if (KeyboardInput.ConvertConsoleKeyToInt(key, out int floor))
                {
                    if (floor == -1)
                    {
                        return;
                    }
                    if (_openInputPanel)
                    {
                        ActivateElevatorInputPanel(floor);
                        return;
                    }
                    ActivateFloorCallPanel(floor);
                    return;
                } else if (KeyboardInput.ConvertConsoleKeyToDirection(key, out Human.Direction direction) && _selectedFloor != -1)
                {
                    ActivateFloorCall(direction);
                    return;
                }
            }
        }
    }
    void SetupView()
    {
        _viewController.SetHumanPositions(_humanController.HumansToDraw());
        _viewController.SetOpenFloorInput(_selectedFloor);
        _viewController.SetInfoData(_elevatorOrchestrator.GetElevatorDataPoints(), _elevatorOrchestrator.GetFloorDataPoints());
        _viewController.SetElevatorDrawInformation(_elevatorOrchestrator.GetElevatorDrawInformation());
        if (_openInputPanel)
        {
            _viewController.SetInputPanel(_elevatorOrchestrator.GetElevatorControllerById(_openInputPanelElevatorId).ReturnData().InputPanel);
        }
    }
    void ClearPanels()
    {
        _openInputPanel = false;
        _openInputPanelElevatorId = -1;
        _openInputPanelUserId = -1;
    }
    void ActivateElevatorInputPanel(int floor)
    {
        if (_elevatorOrchestrator.CallElevatorPanelInput(floor, _openInputPanelElevatorId)) 
        {
            _humanController.SetCallToTravelling(_openInputPanelUserId, _openInputPanelElevatorId, floor);
            ClearPanels();
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
    void ActivateFloorCall(Human.Direction direction)
    {
        _humanController.AppendHuman(_selectedFloor, direction);
        _selectedFloor = -1;
    }
    void Tick()
    {
        _elevatorOrchestrator.Tick();
        _humanController.Tick();
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
        _humanController = new HumanController();
    }
}
